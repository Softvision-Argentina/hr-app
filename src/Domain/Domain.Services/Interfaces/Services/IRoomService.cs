// <copyright file="IRoomService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.Room;

    public interface IRoomService
    {
        CreatedRoomContract Create(CreateRoomContract contract);

        ReadedRoomContract Read(int id);

        void Update(UpdateRoomContract contract);

        void Delete(int id);

        IEnumerable<ReadedRoomContract> List();
    }
}
