namespace ClockIn.Models
{
    public class TimeEntry
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TaskId { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsManual { get; set; }
        public string? Description { get; set; }
        public bool IsApproved { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public Guid? ApprovedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
