using SchoolHealthSystem.Models;

namespace SchoolHealthSystem.Repositories
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<MedicineInventory>> GetAllAsync();
        Task<MedicineInventory?> GetByIdAsync(Guid id);
        Task<MedicineInventory?> GetByMedicineNameAsync(string medicineName);
        Task AddAsync(MedicineInventory inventory);
        void Update(MedicineInventory inventory);
        void Delete(MedicineInventory inventory);
        Task SaveChangesAsync();
        
        Task<IEnumerable<MedicineInventory>> SearchAsync(string keyword);
        Task<IEnumerable<MedicineInventory>> GetExpiredAsync();
        Task<IEnumerable<MedicineInventory>> GetExpiringSoonAsync(int days = 30);
        Task<IEnumerable<MedicineInventory>> GetLowStockAsync(int threshold = 10);
    }
}