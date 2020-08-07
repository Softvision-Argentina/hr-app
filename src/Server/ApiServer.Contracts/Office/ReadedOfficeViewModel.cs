// <copyright file="ReadedOfficeViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Office
{
    using System.Collections.Generic;
    using ApiServer.Contracts.Room;

    public class ReadedOfficeViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<ReadedRoomViewModel> RoomItems { get; set; }
    }
}
