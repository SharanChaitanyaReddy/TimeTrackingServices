namespace ClockIn.Models
{
    public class ProductivityBenchmark
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime WeekStart { get; set; }
        public int ExpectedHours { get; set; }
        public int ActualHours { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
