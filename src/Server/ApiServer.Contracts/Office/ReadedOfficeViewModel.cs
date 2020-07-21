using ApiServer.Contracts.Room;
using System.Collections.Generic;

namespace ApiServer.Contracts.Office
{
    public class ReadedOfficeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<ReadedRoomViewModel> RoomItems { get; set; }
    }
}
