using SchoolHealthSystem.Models;

namespace SchoolHealthSystem.Repositories
{
    public interface IVaccinationRepository
    {
        Task<IEnumerable<VaccinationRecord>> GetAllAsync();
        Task<VaccinationRecord?> GetByIdAsync(Guid id);
        Task AddAsync(VaccinationRecord record);
        void Update(VaccinationRecord record);
        void Delete(VaccinationRecord record);
        Task SaveChangesAsync();
        Task<VaccinationRecord?> GetByStudentAndVaccineAsync(Guid studentId, string vaccineName);
        Task<IEnumerable<VaccinationRecord>> SearchAsync(string keyword);
        Task<IEnumerable<VaccinationRecord>> GetByStudentIdAsync(Guid studentId);
    }
}