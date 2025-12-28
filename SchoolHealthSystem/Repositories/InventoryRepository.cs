using Microsoft.EntityFrameworkCore;
using SchoolHealthSystem.Data;
using SchoolHealthSystem.Models;

namespace SchoolHealthSystem.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly AppDbContext _context;

        public InventoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(MedicineInventory inventory)
        {
            await _context.MedicineInventories.AddAsync(inventory);
        }

        public void Delete(MedicineInventory inventory)
        {
            _context.MedicineInventories.Remove(inventory);
        }

        public async Task<IEnumerable<MedicineInventory>> GetAllAsync()
        {
            return await _context.MedicineInventories
                .OrderBy(mi => mi.MedicineName)
                .ToListAsync();
        }

        public Task<MedicineInventory?> GetByIdAsync(Guid id)
        {
            return _context.MedicineInventories
                .FirstOrDefaultAsync(mi => mi.Id == id);
        }

        public Task<MedicineInventory?> GetByMedicineNameAsync(string medicineName)
        {
            return _context.MedicineInventories
                .FirstOrDefaultAsync(mi => mi.MedicineName.ToLower() == medicineName.ToLower());
        }

        public async Task<IEnumerable<MedicineInventory>> GetExpiredAsync()
        {
            var today = DateTime.UtcNow.Date;
            return await _context.MedicineInventories
                .Where(m => m.ExpiryDate < today)
                .OrderBy(m => m.ExpiryDate)
                .ToListAsync();            
        }

        public async Task<IEnumerable<MedicineInventory>> GetExpiringSoonAsync(int days = 30)
        {
            var today = DateTime.UtcNow.Date;
            var futureDate = today.AddDays(days);

            return await _context.MedicineInventories
                .Where(m => m.ExpiryDate.Date >= today && m.ExpiryDate.Date <= futureDate)
                .OrderBy(m => m.ExpiryDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicineInventory>> GetLowStockAsync(int threshold = 10)
        {
            return await _context.MedicineInventories
                .Where(m => m.Quantity <= threshold)
                .OrderBy(m => m.Quantity)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MedicineInventory>> SearchAsync(string keyword)
        {
            return await _context.MedicineInventories
                .Where(m => m.MedicineName.Contains(keyword))
                .OrderBy(m => m.MedicineName)
                .ToListAsync();
        }

        public void Update(MedicineInventory inventory)
        {
            _context.MedicineInventories.Update(inventory);
        }
    }
}