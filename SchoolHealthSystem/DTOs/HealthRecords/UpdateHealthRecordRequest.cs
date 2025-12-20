using SchoolHealthSystem.Models;

namespace SchoolHealthSystem.DTOs.HealthRecords
{
    public class UpdateHealthRecordRequest
    {
        public string? Name { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public string? Note { get; set; }
    }
}
