namespace Domain.Services.Contracts.EmployeeCasualty
{
    public class UpdateEmployeeCasualtyContract
    {
        public int Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int Value { get; set; }
    }
}
