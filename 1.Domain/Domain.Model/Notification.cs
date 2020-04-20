namespace Domain.Model
{
    public class Notification
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int ApplicationUserId { get; set; }
        public User ApplicationUser { get; set; }
        public bool IsRead { get; set; } = false;
        public string ReferredBy { get; set; }
    }
}
