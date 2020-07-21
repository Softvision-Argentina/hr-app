using ApiServer.Contracts.Room;
using System.Collections.Generic;

namespace ApiServer.Contracts.Office
{
    public class UpdateOfficeViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<CreateRoomViewModel> RoomItems { get; set; }
    }
}
