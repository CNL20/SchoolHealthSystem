using System.Threading.Tasks;
using SchoolHealthSystem.DTOs.Students;

namespace SchoolHealthSystem.Services
{
    public interface IStudentService
    {
        Task<StudentResponse> GetByIdAsync(Guid id);
        Task<IEnumerable<StudentResponse>> GetAllAsync();
        Task CreateAsync(CreateStudentRequest request);
        Task UpdateAsync(Guid id,UpdateStudentRequest request);
        Task DeactivateAsync(Guid id);           
        Task DeletePermanentlyAsync(Guid id);
        Task<string> GenerateStudentCodeAsync();
        Task<IEnumerable<StudentResponse>> SearchAsync(string keyword);
    }
}