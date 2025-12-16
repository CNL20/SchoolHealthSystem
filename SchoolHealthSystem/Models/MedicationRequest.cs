namespace SchoolHealthSystem.Models
{
    public class MedicationRequest
    {
        public Guid Id { get; set; }

        public Guid StudentId { get; set; }
        public Student? Student { get; set; }

        // PHỤ HUYNH GỬI
        public Guid RequestedByParentId { get; set; }
        public User? RequestedByParent { get; set; }

        // Y TÁ DUYỆT
        public Guid? ApprovedByNurseId { get; set; }
        public User? ApprovedByNurse { get; set; }

        public string MedicineName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string? Instruction { get; set; }

        public MedicationStatus Status { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.UtcNow;
    }
}