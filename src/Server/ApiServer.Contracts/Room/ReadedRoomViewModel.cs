using ApiServer.Contracts.Office;
using ApiServer.Contracts.Reservation;
using System.Collections.Generic;

namespace ApiServer.Contracts.Room
{
    public class ReadedRoomViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OfficeId { get; set; }
        public ReadedOfficeViewModel Office { get; set; }
        public ICollection<ReadedReservationViewModel> ReservationItems { get; set; }
    }
}
