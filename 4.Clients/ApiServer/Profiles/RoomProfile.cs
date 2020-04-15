using ApiServer.Contracts.Room;
using Domain.Services.Contracts.Room;
using AutoMapper;

namespace ApiServer.Profiles
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<CreateRoomViewModel, CreateRoomContract>();
            CreateMap<CreatedRoomContract, CreatedRoomViewModel>();
            CreateMap<ReadedRoomContract, ReadedRoomViewModel>();
            CreateMap<UpdateRoomViewModel, UpdateRoomContract>();
        }
    }
}
