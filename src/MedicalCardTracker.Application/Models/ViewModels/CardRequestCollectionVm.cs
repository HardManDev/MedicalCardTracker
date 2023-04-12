// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using AutoMapper;
using MedicalCardTracker.Application.Interfaces;
using MedicalCardTracker.Domain.Entities;

namespace MedicalCardTracker.Application.Models.ViewModels;

public class CardRequestCollectionVm : IMapWith<VmCollection<CardRequest>>, INotifyPropertyChanged
{
    public uint TotalCount { get; set; }
    public ObservableCollection<CardRequestVm> CardRequests { get; set; } = null!;

    public void Mapping(Profile profile)
        => profile.CreateMap<VmCollection<CardRequest>, CardRequestCollectionVm>()
            .ForMember(dest => dest.CardRequests,
                opt =>
                    opt.MapFrom(src => src.Collection));

    [field: NotMapped] public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
