namespace ClockIn.Models
{
    public class Approval
    {
        public Guid Id { get; set; }
        public Guid TimeEntryId { get; set; }
        public string? Status { get; set; }
        public string? Comment { get; set; }
        public Guid? ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
