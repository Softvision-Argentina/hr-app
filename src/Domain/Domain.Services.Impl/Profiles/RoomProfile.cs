using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.Room;

namespace Domain.Services.Impl.Profiles
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<Room, ReadedRoomContract>();
            CreateMap<CreateRoomContract, Room>();
            CreateMap<Room, CreatedRoomContract>();
            CreateMap<UpdateRoomContract, Room>();
        }
    }
}
