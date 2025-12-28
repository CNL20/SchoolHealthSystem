using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolHealthSystem.DTOs.Inventories;
using SchoolHealthSystem.Services;

namespace SchoolHealthSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _service;

        public InventoryController(IInventoryService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<IActionResult> CreateInventory([FromBody] CreateInventoryRequest request)
        {
            try
            {
                await _service.CreateAsync(request);
                return Ok(new { message = "Thêm thuốc vào kho thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllInventories()
        {
            try
            {
                var inventories = await _service.GetAllAsync();
                return Ok(inventories);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetInventoryById(Guid id)
        {
            try
            {
                var inventory = await _service.GetByIdAsync(id);
                return Ok(inventory);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<IActionResult> UpdateInventory(Guid id, [FromBody] UpdateInventoryRequest request)
        {
            try
            {
                await _service.UpdateAsync(id, request);
                return Ok(new { message = "Cập nhật thông tin thuốc thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteInventory(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new { message = "Xóa thuốc khỏi kho thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("search")]
        [Authorize]
        public async Task<IActionResult> SearchInventories([FromQuery] string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return BadRequest(new { message = "Từ khóa tìm kiếm không được để trống" });
                }

                var inventories = await _service.SearchAsync(keyword);
                return Ok(inventories);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/adjust-stock")]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<IActionResult> AdjustStock(Guid id, [FromBody] AdjustStockRequest request)
        {
            try
            {
                await _service.AdjustStockAsync(id, request);
                string action = request.AdjustmentQuantity > 0 ? "nhập" : "xuất";
                return Ok(new { message = $"Điều chỉnh kho ({action}) thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("low-stock")]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<IActionResult> GetLowStockInventories([FromQuery] int threshold = 10)
        {
            try
            {
                var inventories = await _service.GetLowStockAsync(threshold);
                return Ok(inventories);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("expired")]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<IActionResult> GetExpiredInventories()
        {
            try
            {
                var inventories = await _service.GetExpiredAsync();
                return Ok(inventories);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("expiring-soon")]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<IActionResult> GetExpiringSoonInventories([FromQuery] int days = 30)
        {
            try
            {
                var inventories = await _service.GetExpiringSoonAsync(days);
                return Ok(inventories);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("check-medicine")]
        [Authorize]
        public async Task<IActionResult> CheckMedicineExists([FromQuery] string medicineName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(medicineName))
                {
                    return BadRequest(new { message = "Tên thuốc không được để trống" });
                }

                var medicine = await _service.CheckMedicineExistsAsync(medicineName);
                if (medicine == null)
                {
                    return Ok(new { exists = false, message = "Thuốc chưa có trong kho" });
                }

                return Ok(new 
                { 
                    exists = true, 
                    message = $"Thuốc {medicineName} có trong kho với số lượng {medicine.Quantity}",
                    medicine = medicine
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}