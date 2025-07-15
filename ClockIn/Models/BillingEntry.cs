namespace ClockIn.Models
{
    public class BillingEntry
    {
        public Guid Id { get; set; }
        public Guid TimeEntryId { get; set; }
        public Guid UserId { get; set; }
        public decimal HourlyRate { get; set; }
        public bool Billable { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
