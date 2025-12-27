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
        {
            return await _context.VaccinationRecords
                .Include(s => s.Student)
                .Include(n => n.AdministeredByNurse)
                .OrderByDescending(v => v.VaccinationDate)
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
        }

        public async Task<IEnumerable<VaccinationRecord>> GetByStudentIdAsync(Guid studentId)
        {
            return await _context.VaccinationRecords
                .Include(s => s.Student)
                .Include(n => n.AdministeredByNurse)
                .Where(v => v.StudentId == studentId)
                .OrderByDescending(v => v.VaccinationDate)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<VaccinationRecord>> SearchAsync(string keyword)
        {
            return await _context.VaccinationRecords
                .Include(s => s.Student)
                .Include(n => n.AdministeredByNurse)
                .Where(v => v.VaccineName.Contains(keyword) ||
                            v.Student!.FullName.Contains(keyword) ||
                            (v.Note != null && v.Note.Contains(keyword)))
                .OrderByDescending(v => v.VaccinationDate)
                .ToListAsync();
        }

        public void Update(VaccinationRecord record)
        {
            _context.VaccinationRecords.Update(record);
        }
    }
}