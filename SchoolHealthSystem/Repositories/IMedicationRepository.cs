using SchoolHealthSystem.Models;

namespace SchoolHealthSystem.Repositories
{
    public interface IMedicationRepository
    {
        Task<IEnumerable<MedicationRequest>> GetAllAsync();
        Task<MedicationRequest?> GetByIdAsync(Guid id);
        Task AddAsync(MedicationRequest request);
        void Update(MedicationRequest request);
        void Delete(MedicationRequest request);
        Task SaveChangesAsync();
        Task<IEnumerable<MedicationRequest>> SearchAsync(string keyword);
        Task<IEnumerable<MedicationRequest>> GetByStudentIdAsync(Guid studentId);
        Task<IEnumerable<MedicationRequest>> GetByParentIdAsync(Guid parentId);
    }
}