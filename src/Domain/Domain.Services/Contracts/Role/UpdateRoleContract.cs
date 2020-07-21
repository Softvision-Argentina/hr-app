namespace Domain.Services.Contracts.Role
{
    public class UpdateRoleContract
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool isActive { get; set; }
    }
}
