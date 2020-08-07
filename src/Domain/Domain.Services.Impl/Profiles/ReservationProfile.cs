// <copyright file="ReservationProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.Reservation;

    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            this.CreateMap<Reservation, ReadedReservationContract>().ForMember(x => x.User, opt => opt.MapFrom(r => r.User.Id));
            this.CreateMap<CreateReservationContract, Reservation>().ForMember(x => x.User, opt => opt.Ignore());
            this.CreateMap<Reservation, CreatedReservationContract>();
            this.CreateMap<UpdateReservationContract, Reservation>().ForMember(x => x.User, opt => opt.Ignore());
        }
    }
}
