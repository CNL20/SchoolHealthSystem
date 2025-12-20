using SchoolHealthSystem.Models;

namespace SchoolHealthSystem.DTOs.HealthRecords
{
    public class CreateHealthRecordRequest
    {
        public string Name { get; set; } = string.Empty;    
        public Guid StudentId { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public string? Note { get; set; }
    }
}
