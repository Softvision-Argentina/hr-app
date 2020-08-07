// <copyright file="CreateOfficeContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.Office
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.Room;

    public class CreateOfficeContract
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<CreateRoomContract> RoomItems { get; set; }
    }
}
