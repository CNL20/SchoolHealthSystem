using SchoolHealthSystem.Models;

namespace SchoolHealthSystem.DTOs.Vaccinations
{
    public class CompleteVaccinationRequest
    {
        public DateTime ActualDate { get; set; } = DateTime.UtcNow;
        public string? BatchNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? SideEffects { get; set; }
        public string? Note { get; set; }
        public DateTime? NextDoseDate { get; set; }
    }
}
