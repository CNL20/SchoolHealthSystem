using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolHealthSystem.DTOs.Vaccinations;
using SchoolHealthSystem.Models;
using SchoolHealthSystem.Services;
using System.Security.Claims;

namespace SchoolHealthSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VaccinationController : ControllerBase
    {
        private readonly IVaccinationService _service;

        public VaccinationController(IVaccinationService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<IActionResult> CreateVaccination([FromBody] CreateVaccinationRequest request)
        {
            try
            {
                var nurseId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
                await _service.CreateAsync(request, nurseId);
                return Ok(new { message = "Thêm sự kiện tiêm chủng thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllVaccinations()
        {
            try
            {
                var vaccinations = await _service.GetAllAsync();
                return Ok(vaccinations);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetVaccinationById(Guid id)
        {
            try
            {
                var vaccination = await _service.GetByIdAsync(id);
                return Ok(vaccination);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("student/{studentId}")]
        [Authorize]
        public async Task<IActionResult> GetVaccinationsByStudent(Guid studentId)
        {
            try
            {
                var vaccinations = await _service.GetByStudentIdAsync(studentId);
                return Ok(vaccinations);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<IActionResult> UpdateVaccination(Guid id, [FromBody] UpdateVaccinationRequest request)
        {
            try
            {
                await _service.UpdateAsync(id, request);
                return Ok(new { message = "Cập nhật sự kiện tiêm chủng thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<IActionResult> DeleteVaccination(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new { message = "Xóa sự kiện tiêm chủng thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("search")]
        [Authorize]
        public async Task<IActionResult> SearchVaccinations([FromQuery] string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return BadRequest(new { message = "Từ khóa tìm kiếm không được để trống" });
                }

                var vaccinations = await _service.SearchAsync(keyword);
                return Ok(vaccinations);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("check-existing")]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<IActionResult> CheckExistingVaccination([FromQuery] Guid studentId, [FromQuery] string vaccineName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(vaccineName))
                {
                    return BadRequest(new { message = "Tên vaccine không được để trống" });
                }

                var existingVaccination = await _service.CheckExistingVaccinationAsync(studentId, vaccineName);
                if (existingVaccination == null)
                {
                    return Ok(new { exists = false, message = "Học sinh chưa được tiêm vaccine này" });
                }

                return Ok(new 
                { 
                    exists = true, 
                    message = $"Học sinh đã được tiêm vaccine {vaccineName} vào {existingVaccination.ScheduledDate:dd/MM/yyyy}",
                    vaccination = existingVaccination
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/complete")]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<IActionResult> CompleteVaccination(Guid id, [FromBody] CompleteVaccinationRequest request)
        {
            try
            {
                var nurseId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
                await _service.CompleteVaccinationAsync(id, request, nurseId);
                return Ok(new { message = "Hoàn thành tiêm chủng thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/cancel")]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<IActionResult> CancelVaccination(Guid id, [FromBody] CancelVaccinationRequest request)
        {
            try
            {
                await _service.CancelVaccinationAsync(id, request.Reason);
                return Ok(new { message = "Hủy lịch tiêm chủng thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/postpone")]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<IActionResult> PostponeVaccination(Guid id, [FromBody] PostponeVaccinationRequest request)
        {
            try
            {
                await _service.PostponeVaccinationAsync(id, request.NewDate, request.Reason);
                return Ok(new { message = "Hoãn lịch tiêm chủng thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("status/{status}")]
        [Authorize]
        public async Task<IActionResult> GetVaccinationsByStatus(VaccinationStatus status)
        {
            try
            {
                var vaccinations = await _service.GetByStatusAsync(status);
                return Ok(vaccinations);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("scheduled")]
        [Authorize]
        public async Task<IActionResult> GetScheduledVaccinations()
        {
            try
            {
                var vaccinations = await _service.GetScheduledVaccinationsAsync();
                return Ok(vaccinations);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("overdue")]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<IActionResult> GetOverdueVaccinations()
        {
            try
            {
                var vaccinations = await _service.GetOverdueVaccinationsAsync();
                return Ok(vaccinations);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("upcoming")]
        [Authorize]
        public async Task<IActionResult> GetUpcomingVaccinations([FromQuery] int days = 7)
        {
            try
            {
                var vaccinations = await _service.GetUpcomingVaccinationsAsync(days);
                return Ok(vaccinations);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("batch/{batchNumber}")]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<IActionResult> GetVaccinationsByBatchNumber(string batchNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(batchNumber))
                {
                    return BadRequest(new { message = "Số lô vaccine không được để trống" });
                }

                var vaccinations = await _service.GetByBatchNumberAsync(batchNumber);
                return Ok(vaccinations);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}