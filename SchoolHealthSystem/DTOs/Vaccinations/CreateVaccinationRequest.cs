using SchoolHealthSystem.Models;

namespace SchoolHealthSystem.DTOs.Vaccinations
{
    public class CreateVaccinationRequest
    {
        public Guid StudentId { get; set; }
        public string VaccineName { get; set; } = string.Empty;
        
        public DateTime ScheduledDate { get; set; } = DateTime.UtcNow;
        public DateTime? ActualDate { get; set; }    // null if just scheduling, set if actually administered
        public VaccinationStatus Status { get; set; } = VaccinationStatus.Scheduled;
        
        public int DoseNumber { get; set; } = 1;
        public DateTime? NextDoseDate { get; set; }
        
        public string? BatchNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }
        
        public string? SideEffects { get; set; }
        public string? Note { get; set; }
    }
}
