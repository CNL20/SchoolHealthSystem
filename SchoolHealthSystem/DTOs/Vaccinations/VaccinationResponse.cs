using SchoolHealthSystem.Models;

namespace SchoolHealthSystem.DTOs.Vaccinations
{
    public class VaccinationResponse
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string AdministeredByNurseName { get; set; } = string.Empty;
        
        public string VaccineName { get; set; } = string.Empty;
        
        public DateTime ScheduledDate { get; set; }
        public DateTime? ActualDate { get; set; }
        public VaccinationStatus Status { get; set; }
        public string StatusDisplay => GetStatusDisplay();
        
        public int DoseNumber { get; set; }
        public DateTime? NextDoseDate { get; set; }
        
        public string? BatchNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }
        
        public string? SideEffects { get; set; }
        public string? Note { get; set; }

        private string GetStatusDisplay()
        {
            return Status switch
            {
                VaccinationStatus.Scheduled => "Đã lên lịch",
                VaccinationStatus.Completed => "Đã hoàn thành",
                VaccinationStatus.Cancelled => "Đã hủy",
                VaccinationStatus.Postponed => "Hoãn lại",
                VaccinationStatus.InProgress => "Đang thực hiện",
                _ => "Không xác định"
            };
        }
    }
}
