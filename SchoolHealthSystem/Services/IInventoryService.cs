using SchoolHealthSystem.DTOs.Inventories;

namespace SchoolHealthSystem.Services
{
    public interface IInventoryService
    {
        // CRUD Operations
        Task CreateAsync(CreateInventoryRequest request);
        Task<InventoryResponse> GetByIdAsync(Guid id);
        Task<IEnumerable<InventoryResponse>> GetAllAsync();
        Task UpdateAsync(Guid id, UpdateInventoryRequest request);
        Task DeleteAsync(Guid id);

        // Search & Filter
        Task<IEnumerable<InventoryResponse>> SearchAsync(string keyword);

        // Stock Management
        Task AdjustStockAsync(Guid id, AdjustStockRequest request);
        Task<IEnumerable<InventoryResponse>> GetLowStockAsync(int threshold = 10);

        // Expiry Management  
        Task<IEnumerable<InventoryResponse>> GetExpiredAsync();
        Task<IEnumerable<InventoryResponse>> GetExpiringSoonAsync(int days = 30);

        // Business Logic
        Task<InventoryResponse?> CheckMedicineExistsAsync(string medicineName);
    }
}