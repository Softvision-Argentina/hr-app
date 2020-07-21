using ApiServer.Contracts.Reservation;
using AutoMapper;
using Domain.Services.Contracts.Reservation;

namespace ApiServer.Profiles
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            CreateMap<CreateReservationViewModel, CreateReservationContract>();
            CreateMap<CreatedReservationContract, CreatedReservationViewModel>();
            CreateMap<ReadedReservationContract, ReadedReservationViewModel>();
            CreateMap<UpdateReservationViewModel, UpdateReservationContract>();
        }
    }
}
