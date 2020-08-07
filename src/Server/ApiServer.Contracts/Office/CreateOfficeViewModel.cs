// <copyright file="CreateOfficeViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Office
{
    using System.Collections.Generic;
    using ApiServer.Contracts.Room;

    public class CreateOfficeViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<CreateRoomViewModel> RoomItems { get; set; }
    }
}
