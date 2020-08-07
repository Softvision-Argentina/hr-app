// <copyright file="RoomProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.Room;
    using AutoMapper;
    using Domain.Services.Contracts.Room;

    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            this.CreateMap<CreateRoomViewModel, CreateRoomContract>();
            this.CreateMap<CreatedRoomContract, CreatedRoomViewModel>();
            this.CreateMap<ReadedRoomContract, ReadedRoomViewModel>();
            this.CreateMap<UpdateRoomViewModel, UpdateRoomContract>();
        }
    }
}
