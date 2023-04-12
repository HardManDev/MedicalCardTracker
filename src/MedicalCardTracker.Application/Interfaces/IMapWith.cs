// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;

namespace MedicalCardTracker.Application.Interfaces;

public interface IMapWith<TSource>
{
    public void Mapping(Profile profile)
        => profile.CreateMap(typeof(TSource), GetType());
}
