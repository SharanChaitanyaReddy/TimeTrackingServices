namespace ClockIn.Models
{
    public class Holiday
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime HolidayDate { get; set; }
        public string? Description { get; set; }
        public string? Region { get; set; }
    }
}
