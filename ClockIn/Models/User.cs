namespace ClockIn.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public Guid? TeamId { get; set; }
        public decimal? HourlyRate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
