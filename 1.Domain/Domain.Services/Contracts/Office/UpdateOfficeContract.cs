using Domain.Services.Contracts.Room;
using System.Collections.Generic;

namespace Domain.Services.Contracts.Office
{
    public class UpdateOfficeContract
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<CreateRoomContract> RoomItems { get; set; }
    }
}
