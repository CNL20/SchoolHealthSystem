using SchoolHealthSystem.Models;

namespace SchoolHealthSystem.DTOs.Vaccinations
{
    public class UpdateVaccinationRequest
    {
        public string? VaccineName { get; set; }
        
        public DateTime? ScheduledDate { get; set; }
        public DateTime? ActualDate { get; set; }
        public VaccinationStatus? Status { get; set; }
        
        public int? DoseNumber { get; set; }
        public DateTime? NextDoseDate { get; set; }
        
        public string? BatchNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }
        
        public string? SideEffects { get; set; }
        public string? Note { get; set; }
    }
}
