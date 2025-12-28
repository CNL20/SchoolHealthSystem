using AutoMapper;
using SchoolHealthSystem.DTOs.Inventories;
using SchoolHealthSystem.Models;
using SchoolHealthSystem.Repositories;

namespace SchoolHealthSystem.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _repo;
        private readonly IMapper _mapper;

        public InventoryService(IInventoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task AdjustStockAsync(Guid id, AdjustStockRequest request)
        {
            var inventory = await _repo.GetByIdAsync(id);
            if (inventory == null)
                throw new Exception("Không tìm thấy thuốc này trong kho");
            
            int newQuantity = inventory.Quantity + request.AdjustmentQuantity;

            if(newQuantity < 0)
                throw new Exception($"Không thể điều chỉnh. Số lượng hiện tại: {inventory.Quantity}, yêu cầu điều chỉnh: {request.AdjustmentQuantity}");
            
            inventory.Quantity = newQuantity;
            _repo.Update(inventory);
            await _repo.SaveChangesAsync();
        }

        public async Task<InventoryResponse?> CheckMedicineExistsAsync(string medicineName)
        {
            if (string.IsNullOrWhiteSpace(medicineName))
                return null;

            var medicine = await _repo.GetByMedicineNameAsync(medicineName);
            if (medicine == null)
                return null;

            return _mapper.Map<InventoryResponse>(medicine);
        }

        public async Task CreateAsync(CreateInventoryRequest request)
        {
            // Validate expiry date
            if (request.ExpiryDate.Date <= DateTime.UtcNow.Date)
            {
                throw new Exception("Hạn sử dụng phải lớn hơn ngày hiện tại");
            }

            // Validate quantity
            if (request.Quantity <= 0)
            {
                throw new Exception("Số lượng phải lớn hơn 0");
            }

            // Check if medicine already exists
            var existingMedicine = await _repo.GetByMedicineNameAsync(request.MedicineName);

            if (existingMedicine != null)
            {
                // If exists, add to existing quantity
                existingMedicine.Quantity += request.Quantity;

                // Update expiry date to the later date
                if (request.ExpiryDate > existingMedicine.ExpiryDate)
                {
                    existingMedicine.ExpiryDate = request.ExpiryDate;
                }

                _repo.Update(existingMedicine);
            }
            else
            {
                // Create new medicine inventory
                var newInventory = _mapper.Map<MedicineInventory>(request);
                await _repo.AddAsync(newInventory);
            }

            await _repo.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var inventory = await _repo.GetByIdAsync(id);
            if (inventory == null)
                throw new Exception("Thuốc không tồn tại");
            _repo.Delete(inventory);
            await _repo.SaveChangesAsync(); 
        }

        public async Task<IEnumerable<InventoryResponse>> GetAllAsync()
        {
            var inventory = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<InventoryResponse>>(inventory);
        }

        public async Task<InventoryResponse> GetByIdAsync(Guid id)
        {
            var inventory = await _repo.GetByIdAsync(id);
            return _mapper.Map<InventoryResponse>(inventory);
        }

        public async Task<IEnumerable<InventoryResponse>> GetExpiredAsync()
        {
            var inventory = await _repo.GetExpiredAsync();
            return _mapper.Map<IEnumerable<InventoryResponse>>(inventory);
        }

        public async Task<IEnumerable<InventoryResponse>> GetExpiringSoonAsync(int days = 30)
        {
            var inventory = await _repo.GetExpiringSoonAsync();
            return _mapper.Map<IEnumerable<InventoryResponse>>(inventory);
        }

        public async Task<IEnumerable<InventoryResponse>> GetLowStockAsync(int threshold = 10)
        {
            var inventory = await _repo.GetLowStockAsync();
            return _mapper.Map<IEnumerable<InventoryResponse>>(inventory);
        }

        public async Task<IEnumerable<InventoryResponse>> SearchAsync(string keyword)
        {
            var inventory = await _repo.SearchAsync(keyword);
            return _mapper.Map<IEnumerable<InventoryResponse>>(inventory);
        }

        public async Task UpdateAsync(Guid id, UpdateInventoryRequest request)
        {
            var inventory = await _repo.GetByIdAsync(id);
            if (inventory == null)
                throw new Exception("Thuốc không tồn tại");

            _mapper.Map(request, inventory);
            _repo.Update(inventory);
            await _repo.SaveChangesAsync();
        }
    }
}