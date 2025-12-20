using SchoolHealthSystem.Models; 

namespace SchoolHealthSystem.Repositories
{
    public interface IHealthRecordRepository
    {
        Task<HealthRecord?> GetByIdAsync(Guid id);
        Task<IEnumerable<HealthRecord>> GetAllAsync();
        Task<IEnumerable<HealthRecord>> GetByStudentIdAsync(Guid studentId);
        Task<IEnumerable<HealthRecord>> SearchAsync(string keyword);
        Task AddAsync(HealthRecord healthRecord);
        void Update(HealthRecord healthRecord);
        void Delete(HealthRecord healthRecord);
        Task SaveChangesAsync();
    }
}