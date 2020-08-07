// <copyright file="Office.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    using System.Collections.Generic;
    using Core;

    public class Office : DescriptiveEntity<int>
    {
        public IList<Room> RoomItems { get; set; }
    }
}
