using SchoolHealthSystem.DTOs.HealthRecords;

namespace SchoolHealthSystem.Services
{
    public interface IHealthRecordService
    {
        Task CreateAsync(CreateHealthRecordRequest request, Guid nurseId);          
        Task<HealthRecordResponse> GetByIdAsync(Guid id);                             
        Task<IEnumerable<HealthRecordResponse>> GetAllAsync();                      
        Task<IEnumerable<HealthRecordResponse>> GetByStudentIdAsync(Guid studentId);
        Task<IEnumerable<HealthRecordResponse>> SearchAsync(string keyword);       
        Task UpdateAsync(Guid id, UpdateHealthRecordRequest request);              
        Task DeleteAsync(Guid id);
    }
}