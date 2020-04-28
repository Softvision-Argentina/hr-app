using ApiServer.Contracts.Room;
using AutoMapper;
using Domain.Services.Contracts.Room;

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
