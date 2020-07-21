using Core;

namespace Domain.Model
{
    public class EmployeeCasualty : Entity<int>
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int Value { get; set; }
    }
}
