using SchoolHealthSystem.Models;

namespace SchoolHealthSystem.DTOs.Medications
{
    public class MedicationResponse
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public Guid RequestedByParentId { get; set; }
        public string RequestedByParentName { get; set; } = string.Empty;
        public Guid? ApprovedByNurseId { get; set; }
        public string? ApprovedByNurseName { get; set; }
        public string MedicineName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string? Instruction { get; set; }
        public MedicationStatus Status { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
