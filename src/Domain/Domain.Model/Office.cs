using Core;
using System.Collections.Generic;

namespace Domain.Model
{
    public class Office : DescriptiveEntity<int>
    {
        public IList<Room> RoomItems { get; set; }
    }
}
