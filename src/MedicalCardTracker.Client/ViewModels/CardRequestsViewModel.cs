// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MedicalCardTracker.Application.Client.Configuration;
using MedicalCardTracker.Application.Models.Enums;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Requests.Commands.CardRequests.CreateCardRequest;
using MedicalCardTracker.Application.Requests.Commands.CardRequests.UpdateCardRequest;
using MedicalCardTracker.Application.Requests.Queries.CardRequests.GetCardRequestCollection;
using MedicalCardTracker.Client.Models.Enums;
using MedicalCardTracker.Client.Utils;
using MedicalCardTracker.Domain.Enums;
using Microsoft.AspNetCore.SignalR.Client;
using Serilog;

namespace MedicalCardTracker.Client.ViewModels;

public class CardRequestsViewModel : BaseViewModel
{
    private ObservableCollection<CardRequestVm> _cardRequests = new();
    private HubConnectionHelper _hubConnectionHelper;
    private int _itemsPerPage = 100;
    private int _itemsTotalCount;
    private int _pageCount;
    private int _pageIndex = 1;

    public CardRequestsViewModel(IMapper mapper,
        IMediator mediator,
        ApplicationConfiguration configuration,
        HubConnectionHelper hubConnectionHelper)
        : base(mapper, mediator, configuration)
    {
        _hubConnectionHelper = hubConnectionHelper;
        _hubConnectionHelper.PropertyChanged += HubConnectionHelperOnPropertyChanged;

        _hubConnectionHelper.NotificationHubConnection.On<string, CardRequestVm>(
            $"On{nameof(CreateCardRequestCommand)}", OnCreateCardRequestCommandHandler);
        _hubConnectionHelper.NotificationHubConnection.On<string, CardRequestVm>(
            $"On{nameof(UpdateCardRequestCommand)}", OnUpdateCardRequestCommandHandler);

        MarkCardRequestAsCompletedCommand = new RelayCommand(MarkCardRequestAsCompletedCommand_Execute,
            o => true);
        MarkCardRequestAsNotCompletedCommand = new RelayCommand(MarkCardRequestAsNotCompletedCommand_Execute,
            o => true);
        MarkCardRequestAsCanceledCommand = new RelayCommand(MarkCardRequestAsCanceledCommand_Execute,
            o => true);
        NextPageCommand = new RelayCommand(NextPageCommand_Execute,
            o => PageIndex + 1 <= PageCount);
        PrevPageCommand = new RelayCommand(PrevPageCommand_Execute,
            o => PageIndex - 1 >= 1);
    }

    public RelayCommand MarkCardRequestAsCompletedCommand { get; }
    public RelayCommand MarkCardRequestAsNotCompletedCommand { get; }
    public RelayCommand MarkCardRequestAsCanceledCommand { get; }
    public RelayCommand NextPageCommand { get; }
    public RelayCommand PrevPageCommand { get; }

    public int PageIndex
    {
        get => _pageIndex;
        set
        {
            if (value == _pageIndex) return;
            _pageIndex = value;
            OnPropertyChanged();
        }
    }

    public int PageCount
    {
        get => _pageCount;
        set
        {
            if (value == _pageCount) return;
            _pageCount = value;
            OnPropertyChanged();
        }
    }

    public int ItemsPerPage
    {
        get => _itemsPerPage;
        set
        {
            if (value == _itemsPerPage) return;
            _itemsPerPage = value;
            OnPropertyChanged();
        }
    }

    public int ItemsTotalCount
    {
        get => _itemsTotalCount;
        set
        {
            if (value == _itemsTotalCount) return;
            _itemsTotalCount = value;
            OnPropertyChanged();
        }
    }

    public HubConnectionHelper HubConnectionHelper
    {
        get => _hubConnectionHelper;
        set
        {
            if (Equals(value, _hubConnectionHelper)) return;
            _hubConnectionHelper = value ?? throw new ArgumentNullException(nameof(value));
            OnPropertyChanged();
        }
    }

    public ObservableCollection<CardRequestVm> CardRequests
    {
        get => _cardRequests;
        set
        {
            if (Equals(value, _cardRequests)) return;
            _cardRequests = value ?? throw new ArgumentNullException(nameof(value));
            OnPropertyChanged();
        }
    }

    private async void HubConnectionHelperOnPropertyChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != nameof(_hubConnectionHelper.HubConnectionStatus)) return;

        if (_hubConnectionHelper.HubConnectionStatus != HubConnectionStatus.Connected) return;

        await LoadCardRequestCollection();
    }

    private async Task LoadCardRequestCollection()
    {
        try
        {
            GetCardRequestCollectionQuery? query = null;

            if (Configuration.IsRegistrar)
                query = new GetCardRequestCollectionQuery
                {
                    Page = (uint)PageIndex - 1,
                    Count = (uint)ItemsPerPage,
                    OrderBy = OrderBy.Descending
                };
            else
                query = new GetCardRequestCollectionQuery
                {
                    Page = (uint)PageIndex - 1,
                    Count = (uint)ItemsPerPage,
                    SearchIn = nameof(CardRequestVm.TargetAddress),
                    SearchQuery = Configuration.TargetAddress,
                    OrderBy = OrderBy.Descending
                };

            var result = await Mediator.Send(query, CancellationToken.None);

            CardRequests.Clear();
            CardRequests = result.CardRequests;
            ItemsTotalCount = (int)result.TotalCount;
            PageCount = (int)Math.Ceiling(ItemsTotalCount / (decimal)ItemsPerPage);
        }
        catch (Exception e)
        {
            Log.Error("Failed to fetch CardRequest collection", e);
        }
    }

    private async void OnCreateCardRequestCommandHandler(string user, CardRequestVm payload)
    {
        await System.Windows.Application.Current.Dispatcher
            .Invoke(async () =>
            {
                ItemsTotalCount += 1;
                PageCount = (int)Math.Ceiling(ItemsTotalCount / (decimal)ItemsPerPage);

                if (PageIndex == 1)
                {
                    CardRequests.Insert(0, payload);

                    if (CardRequests.Count > ItemsPerPage)
                        CardRequests.RemoveAt(CardRequests.Count - 1);
                }
            });
    }

    private void OnUpdateCardRequestCommandHandler(string user, CardRequestVm payload)
    {
        System.Windows.Application.Current.Dispatcher
            .Invoke(() =>
            {
                var targetCardRequest = CardRequests
                    .FirstOrDefault(item => item.Id == payload.Id);

                if (targetCardRequest != null)
                    CardRequests[CardRequests.IndexOf(targetCardRequest)] = payload;
            });
    }

    private async void MarkCardRequestAsCompletedCommand_Execute(object obj)
    {
        if (obj is not Guid id) return;

        try
        {
            await Mediator.Send(
                new UpdateCardRequestCommand
                {
                    Id = id,
                    Status = CardRequestStatus.Completed
                }, CancellationToken.None);
        }
        catch (Exception e)
        {
            Log.Error("Failed to update CardRequest status", e);
        }
    }

    private async void MarkCardRequestAsNotCompletedCommand_Execute(object obj)
    {
        if (obj is not Guid id) return;

        try
        {
            await Mediator.Send(
                new UpdateCardRequestCommand
                {
                    Id = id,
                    Status = CardRequestStatus.NotCompleted
                }, CancellationToken.None);
        }
        catch (Exception e)
        {
            Log.Error("Failed to update CardRequest status", e);
        }
    }

    private async void MarkCardRequestAsCanceledCommand_Execute(object obj)
    {
        if (obj is not Guid id) return;

        try
        {
            await Mediator.Send(
                new UpdateCardRequestCommand
                {
                    Id = id,
                    Status = CardRequestStatus.Canceled
                }, CancellationToken.None);
        }
        catch (Exception e)
        {
            Log.Error("Failed to update CardRequest status", e);
        }
    }

    private async void PrevPageCommand_Execute(object obj)
    {
        PageIndex -= 1;
        await LoadCardRequestCollection();
    }

    private async void NextPageCommand_Execute(object obj)
    {
        PageIndex += 1;
        await LoadCardRequestCollection();
    }
}
