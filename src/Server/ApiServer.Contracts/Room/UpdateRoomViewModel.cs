using ApiServer.Contracts.Office;
using ApiServer.Contracts.Reservation;
using System.Collections.Generic;

namespace ApiServer.Contracts.Room
{
    public class UpdateRoomViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int OfficeId { get; set; }
        public UpdateOfficeViewModel Office { get; set; }
        public ICollection<CreateReservationViewModel> ReservationItems { get; set; }
    }
}
