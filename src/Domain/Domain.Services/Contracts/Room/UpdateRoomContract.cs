using Domain.Services.Contracts.Office;
using Domain.Services.Contracts.Reservation;
using System.Collections.Generic;

namespace Domain.Services.Contracts.Room
{
    public class UpdateRoomContract
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OfficeId { get; set; }
        public UpdateOfficeContract Office { get; set; }
        public ICollection<CreateReservationContract> ReservationItems { get; set; }
    }
}
