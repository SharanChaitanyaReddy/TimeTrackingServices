namespace ClockIn.Models
{
    public class AuditLog
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Action { get; set; }
        public string ResourceType { get; set; }
        public Guid ResourceId { get; set; }
        public object Details { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
