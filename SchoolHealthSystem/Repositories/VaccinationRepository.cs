using Microsoft.EntityFrameworkCore;
using SchoolHealthSystem.Data;
using SchoolHealthSystem.Models;

namespace SchoolHealthSystem.Repositories
{
    public class VaccinationRepository : IVaccinationRepository
    {
        private readonly AppDbContext _context;
        public VaccinationRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(VaccinationRecord record)
        {
            await _context.VaccinationRecords.AddAsync(record);
        }

        public void Delete(VaccinationRecord record)
        {
            _context.VaccinationRecords.Remove(record);
        }

        public async Task<IEnumerable<VaccinationRecord>> GetAllAsync()
        {        return await _context.VaccinationRecords
                .Include(s => s.Student)
                .Include(n => n.AdministeredByNurse)
                .OrderByDescending(v => v.ScheduledDate)
                .ToListAsync();
        }

        public async Task<VaccinationRecord?> GetByIdAsync(Guid id)
        {
            return await _context.VaccinationRecords
                .Include(s => s.Student)
                .Include(n => n.AdministeredByNurse)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<VaccinationRecord?> GetByStudentAndVaccineAsync(Guid studentId, string vaccineName)
        {
            return await _context.VaccinationRecords
                .Include(s => s.Student)
                .Include(n => n.AdministeredByNurse)
                .FirstOrDefaultAsync(v => v.StudentId == studentId && v.VaccineName == vaccineName);
        }        public async Task<IEnumerable<VaccinationRecord>> GetByStudentIdAsync(Guid studentId)
        {
            return await _context.VaccinationRecords
                .Include(s => s.Student)
                .Include(n => n.AdministeredByNurse)
                .Where(v => v.StudentId == studentId)
                .OrderByDescending(v => v.ScheduledDate)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }        public async Task<IEnumerable<VaccinationRecord>> SearchAsync(string keyword)
        {
            return await _context.VaccinationRecords
                .Include(s => s.Student)
                .Include(n => n.AdministeredByNurse)
                .Where(v => v.VaccineName.Contains(keyword) ||
                            v.Student!.FullName.Contains(keyword) ||
                            (v.Note != null && v.Note.Contains(keyword)) ||
                            (v.BatchNumber != null && v.BatchNumber.Contains(keyword)))
                .OrderByDescending(v => v.ScheduledDate)
                .ToListAsync();
        }

        public void Update(VaccinationRecord record)
        {
            _context.VaccinationRecords.Update(record);
        }

        // New methods for enhanced functionality
        public async Task<IEnumerable<VaccinationRecord>> GetByStatusAsync(VaccinationStatus status)
        {
            return await _context.VaccinationRecords
                .Include(s => s.Student)
                .Include(n => n.AdministeredByNurse)
                .Where(v => v.Status == status)
                .OrderByDescending(v => v.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<VaccinationRecord>> GetScheduledVaccinationsAsync()
        {
            return await _context.VaccinationRecords
                .Include(s => s.Student)
                .Include(n => n.AdministeredByNurse)
                .Where(v => v.Status == VaccinationStatus.Scheduled)
                .OrderBy(v => v.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<VaccinationRecord>> GetOverdueVaccinationsAsync()
        {
            var today = DateTime.UtcNow.Date;
            return await _context.VaccinationRecords
                .Include(s => s.Student)
                .Include(n => n.AdministeredByNurse)
                .Where(v => v.Status == VaccinationStatus.Scheduled && v.ScheduledDate.Date < today)
                .OrderBy(v => v.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<VaccinationRecord>> GetUpcomingVaccinationsAsync(int days = 7)
        {
            var today = DateTime.UtcNow.Date;
            var endDate = today.AddDays(days);
            return await _context.VaccinationRecords
                .Include(s => s.Student)
                .Include(n => n.AdministeredByNurse)
                .Where(v => v.Status == VaccinationStatus.Scheduled && 
                           v.ScheduledDate.Date >= today && 
                           v.ScheduledDate.Date <= endDate)
                .OrderBy(v => v.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<VaccinationRecord>> GetByBatchNumberAsync(string batchNumber)
        {
            return await _context.VaccinationRecords
                .Include(s => s.Student)
                .Include(n => n.AdministeredByNurse)
                .Where(v => v.BatchNumber == batchNumber)
                .OrderByDescending(v => v.ActualDate ?? v.ScheduledDate)
                .ToListAsync();
        }
    }
}