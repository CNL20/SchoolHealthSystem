using Microsoft.EntityFrameworkCore;
using SchoolHealthSystem.Data;
using SchoolHealthSystem.Models;

namespace SchoolHealthSystem.Repositories
{
    public class MedicationRepository : IMedicationRepository
    {
        private readonly AppDbContext _context;

        public MedicationRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(MedicationRequest request)
        {
            await _context.MedicationRequests.AddAsync(request);
        }

        public void Delete(MedicationRequest request)
        {
            _context.MedicationRequests.Remove(request);
        }

        public async Task<IEnumerable<MedicationRequest>> GetAllAsync()
        {
            return await _context.MedicationRequests
                .Include(s => s.Student)
                .Include(p => p.RequestedByParent)
                .Include(n => n.ApprovedByNurse)
                .ToListAsync();
        }

        public async Task<MedicationRequest?> GetByIdAsync(Guid id)
        {
            return await _context.MedicationRequests
                .Include(s => s.Student)
                .Include(p => p.RequestedByParent)
                .Include(n => n.ApprovedByNurse)
                .FirstOrDefaultAsync(mr => mr.Id == id);
        }

        public async Task<IEnumerable<MedicationRequest>> GetByParentIdAsync(Guid parentId)
        {
            return await _context.MedicationRequests
                .Include(s => s.Student)
                .Include(p => p.RequestedByParent)
                .Include(n => n.ApprovedByNurse)
                .Where(mr => mr.RequestedByParentId == parentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicationRequest>> GetByStudentIdAsync(Guid studentId)
        {
            return await _context.MedicationRequests
                .Include(s => s.Student)
                .Include(p => p.RequestedByParent)
                .Include(n => n.ApprovedByNurse)
                .Where(mr => mr.StudentId == studentId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MedicationRequest>> SearchAsync(string keyword)
        {
            return await _context.MedicationRequests
                .Include(s => s.Student)
                .Include(p => p.RequestedByParent)
                .Include(n => n.ApprovedByNurse)
                .Where(mr => mr.MedicineName.Contains(keyword)||
                             mr.Dosage.Contains(keyword) ||
                             mr.Student!.FullName.Contains(keyword))
                .OrderByDescending(h => h.RequestDate)
                .ToListAsync();
        }

        public void Update(MedicationRequest request)
        {
            _context.MedicationRequests.Update(request);
        }
    }
}