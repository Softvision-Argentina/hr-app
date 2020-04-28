using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.Reservation;

namespace Domain.Services.Impl.Profiles
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            CreateMap<Reservation, ReadedReservationContract>().ForMember(x => x.User, opt => opt.MapFrom(r => r.User.Id));
            CreateMap<CreateReservationContract, Reservation>().ForMember(x => x.User, opt => opt.Ignore());
            CreateMap<Reservation, CreatedReservationContract>();
            CreateMap<UpdateReservationContract, Reservation>().ForMember(x => x.User, opt => opt.Ignore());
        }
    }
}
