using Microsoft.EntityFrameworkCore;
using SchoolHealthSystem.Data;
using SchoolHealthSystem.Models;

namespace SchoolHealthSystem.Repositories
{
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly AppDbContext _context;
        public HealthRecordRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(HealthRecord healthRecord)
        {
            await _context.HealthRecords.AddAsync(healthRecord);
        }

        public void Delete(HealthRecord healthRecord)
        {
            _context.HealthRecords.Remove(healthRecord);
        }

        public async Task<IEnumerable<HealthRecord>> GetAllAsync()
        {
            return await _context.HealthRecords
                .Include(s => s.Student)
                .Include(n => n.Nurse)
                .ToListAsync();
        }

        public async Task<HealthRecord?> GetByIdAsync(Guid id)
        {
            return await _context.HealthRecords
                .Include(s => s.Student)
                .Include(n => n.Nurse)
                .OrderByDescending(hr => hr.RecordDate)
                .FirstOrDefaultAsync(hr => hr.Id == id);
        }

        public async Task<IEnumerable<HealthRecord>> GetByStudentIdAsync(Guid studentId)
        {
            return await _context.HealthRecords
                .Include(s => s.Student)
                .Include(n => n.Nurse)
                .Where(hr => hr.StudentId == studentId)
                .OrderByDescending(hr => hr.RecordDate)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<HealthRecord>> SearchAsync(string keyword)
        {
           return await _context.HealthRecords
                .Include(s => s.Student)
                .Include(n => n.Nurse)
                .Where(h => h.Name.Contains(keyword)||
                            h.Student!.FullName.Contains(keyword)||
                            h.Nurse!.FullName.Contains(keyword))
                .OrderByDescending(h => h.RecordDate)
                .ToListAsync();
        }

        public void Update(HealthRecord healthRecord)
        {
            _context.HealthRecords.Update(healthRecord);
        }
    }
}