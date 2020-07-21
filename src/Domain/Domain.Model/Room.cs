using Core;
using System.Collections.Generic;

namespace Domain.Model
{
    public class Room : DescriptiveEntity<int>
    {
        public int OfficeId { get; set; }
        public Office Office { get; set; }
        public IList<Reservation> ReservationItems { get; set; }
    }
}
