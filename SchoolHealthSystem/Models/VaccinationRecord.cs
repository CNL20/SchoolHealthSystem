namespace SchoolHealthSystem.Models
{
    public class VaccinationRecord
    {
        public Guid Id { get; set; }

        public Guid StudentId { get; set; }
        public Guid AdministeredByNurseId { get; set; }

        public string VaccineName { get; set; } = string.Empty;

        public DateTime ScheduledDate { get; set; }
        public DateTime? ActualDate { get; set; }
        public VaccinationStatus Status { get; set; } = VaccinationStatus.Scheduled;

        public int DoseNumber { get; set; } = 1;
        public DateTime? NextDoseDate { get; set; }

        public string? BatchNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public string? SideEffects { get; set; }
        public string? Note { get; set; }

        public Student? Student { get; set; }
        public User? AdministeredByNurse { get; set; }
    }
}