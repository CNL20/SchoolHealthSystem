using SchoolHealthSystem.DTOs.Vaccinations;
using SchoolHealthSystem.Models;

namespace SchoolHealthSystem.Services
{
    public interface IVaccinationService
    {
        Task CreateAsync(CreateVaccinationRequest request, Guid nurseId);
        Task<VaccinationResponse> GetByIdAsync(Guid id);
        Task<IEnumerable<VaccinationResponse>> GetAllAsync();
        Task<IEnumerable<VaccinationResponse>> GetByStudentIdAsync(Guid studentId);
        Task<IEnumerable<VaccinationResponse>> SearchAsync(string keyword);
        Task UpdateAsync(Guid id, UpdateVaccinationRequest request);
        Task DeleteAsync(Guid id);

        // Duplicate prevention method
        Task<VaccinationResponse?> CheckExistingVaccinationAsync(Guid studentId, string vaccineName);

        // New methods for enhanced functionality
        Task CompleteVaccinationAsync(Guid id, CompleteVaccinationRequest request, Guid nurseId);
        Task<IEnumerable<VaccinationResponse>> GetByStatusAsync(VaccinationStatus status);
        Task<IEnumerable<VaccinationResponse>> GetScheduledVaccinationsAsync();
        Task<IEnumerable<VaccinationResponse>> GetOverdueVaccinationsAsync();
        Task<IEnumerable<VaccinationResponse>> GetUpcomingVaccinationsAsync(int days = 7);
        Task<IEnumerable<VaccinationResponse>> GetByBatchNumberAsync(string batchNumber);        Task CancelVaccinationAsync(Guid id, string reason);
        Task PostponeVaccinationAsync(Guid id, DateTime newDate, string reason);
    }
}