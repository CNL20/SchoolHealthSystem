using SchoolHealthSystem.Models;

namespace SchoolHealthSystem.Repositories
{
    public interface IStudentRepository
    {
        Task<Student?> GetByIdAsync(Guid id);
        Task<IEnumerable<Student>> GetAllAsync();
        Task<int> GetStudentCountAsync();
        Task AddAsync(Student student);
        void Update(Student student);
        void Delete(Student student);
        Task<IEnumerable<Student>> SearchAsync(string keyword);
        Task SaveChangesAsync();
    }
}