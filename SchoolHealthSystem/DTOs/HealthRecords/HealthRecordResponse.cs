using System.ComponentModel.DataAnnotations;
using SchoolHealthSystem.Models;

namespace SchoolHealthSystem.DTOs.HealthRecords
{
    public class HealthRecordResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string NurseName { get; set; } = string.Empty;      
        public Guid StudentId { get; set; }
        public Guid NurseId { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public string? Note { get; set; }
        public DateTime RecordDate { get; set; }
    }
}
