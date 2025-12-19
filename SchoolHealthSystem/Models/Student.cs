namespace SchoolHealthSystem.Models
{
    public class Student
    {
        public Guid Id { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public Guid ParentId { get; set; }
        public User? Parent { get; set; }

        public string FullName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public string? Note { get; set; }
        public ICollection<HealthRecord> HealthRecords { get; set; }
            = new List<HealthRecord>();

        public ICollection<MedicationRequest> MedicationRequests { get; set; }
            = new List<MedicationRequest>();

        public ICollection<VaccinationRecord> VaccinationRecords { get; set; }
            = new List<VaccinationRecord>();
    }
}