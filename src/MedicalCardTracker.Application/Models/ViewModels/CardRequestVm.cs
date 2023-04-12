// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using AutoMapper;
using MedicalCardTracker.Application.Interfaces;
using MedicalCardTracker.Domain.Entities;
using MedicalCardTracker.Domain.Enums;
using DateOnlyConverter = MedicalCardTracker.Application.Convertors.DateOnlyConverter;

namespace MedicalCardTracker.Application.Models.ViewModels;

public class CardRequestVm : IMapWith<CardRequest>, INotifyPropertyChanged
{
    public Guid Id { get; set; }

    public string CustomerName { get; set; } = null!;
    public string TargetAddress { get; set; } = null!;

    public string PatientFullName { get; set; } = null!;

    [JsonConverter(typeof(DateOnlyConverter))]
    public DateOnly? PatientBirthDate { get; set; }

    public string? Description { get; set; }

    public CardRequestStatus Status { get; set; }
    public CardRequestPriority Priority { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<CardRequest, CardRequestVm>();

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
