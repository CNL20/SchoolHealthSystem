using Microsoft.EntityFrameworkCore;
using SchoolHealthSystem.Data;
using SchoolHealthSystem.Models;

namespace SchoolHealthSystem.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Student student)
        {
            await _context.Students.AddAsync(student);
        }

        public void Delete(Student student)
        {
             _context.Students.Remove(student);
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students.Include(p => p.Parent).ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(Guid id)
        {
            return await _context.Students
                .Include(p => p.Parent)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<int> GetStudentCountAsync()
        {
            return await _context.Students.CountAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Student>> SearchAsync(string keyword)
        {
           return await _context.Students
                .Include(p => p.Parent)
                .Where(s => s.IsActive &&(
                    s.StudentCode.Contains(keyword) ||
                    s.FullName.Contains(keyword) ||
                    s.ClassName.Contains(keyword) 
                ))
                .OrderBy(s => s.StudentCode)
                .ToListAsync();
        }

        public void Update(Student student)
        {
             _context.Students.Update(student);
        }
    }
}