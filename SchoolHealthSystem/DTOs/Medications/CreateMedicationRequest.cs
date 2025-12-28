namespace SchoolHealthSystem.DTOs.Medications
{
    public class CreateMedicationRequest
    {
        public Guid StudentId { get; set; }
        public string MedicineName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string? Instruction { get; set; }
    }
}
