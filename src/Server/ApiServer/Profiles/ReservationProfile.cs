// <copyright file="ReservationProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.Reservation;
    using AutoMapper;
    using Domain.Services.Contracts.Reservation;

    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            this.CreateMap<CreateReservationViewModel, CreateReservationContract>();
            this.CreateMap<CreatedReservationContract, CreatedReservationViewModel>();
            this.CreateMap<ReadedReservationContract, ReadedReservationViewModel>();
            this.CreateMap<UpdateReservationViewModel, UpdateReservationContract>();
        }
    }
}
