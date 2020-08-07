// <copyright file="RoomProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.Room;

    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            this.CreateMap<Room, ReadedRoomContract>();
            this.CreateMap<CreateRoomContract, Room>();
            this.CreateMap<Room, CreatedRoomContract>();
            this.CreateMap<UpdateRoomContract, Room>();
        }
    }
}
