using ApiServer.Contracts.Room;
using Domain.Services.Contracts.Room;
using System.Collections.Generic;
using System.Collections;
using AutoMapper;
using ApiServer.Profiles;

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
