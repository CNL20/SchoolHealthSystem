namespace SchoolHealthSystem.Models
{
    public class HealthRecord
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid StudentId { get; set; }
        public Guid NurseId { get; set; }

        public double? Height { get; set; }
        public double? Weight { get; set; }
        public string? Note { get; set; }

        public DateTime RecordDate { get; set; } = DateTime.UtcNow;

        public Student? Student { get; set; }
        public User? Nurse { get; set; }
    }
}