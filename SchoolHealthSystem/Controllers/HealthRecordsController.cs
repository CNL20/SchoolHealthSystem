using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolHealthSystem.DTOs.HealthRecords;
using SchoolHealthSystem.Helpers;
using SchoolHealthSystem.Services;

namespace SchoolHealthSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HealthRecordsController : ControllerBase
    {
        private readonly IHealthRecordService _service;

        public HealthRecordsController(IHealthRecordService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var records = await _service.GetAllAsync();
                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Manager,Nurse,Parent")]
        public async Task<ActionResult> GetById(Guid id)
        {
            try
            {
                var record = await _service.GetByIdAsync(id);
                return Ok(record);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("student/{studentId}")]
        [Authorize(Roles = "Admin,Manager,Nurse,Parent")]
        public async Task<ActionResult> GetByStudentId(Guid studentId)
        {
            try
            {
                var records = await _service.GetByStudentIdAsync(studentId);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<ActionResult> Create([FromBody] CreateHealthRecordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var nurseIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(string.IsNullOrEmpty(nurseIdClaim) || !Guid.TryParse(nurseIdClaim, out Guid nurseId))
                    return BadRequest(new { message = "Invalid nurse ID" });

                await _service.CreateAsync(request, nurseId);
                return Ok(new { message = "Tạo hồ sơ sức khỏe thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new { message = "Xóa hồ sơ sức khỏe thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateHealthRecordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                await _service.UpdateAsync(id, request);
                return Ok(new { message = "Cập nhật hồ sơ sức khỏe thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin,Manager,Nurse,Parent")]
        public async Task<ActionResult> Search([FromQuery] string keyword)
        {
            try
            {
                var records = await _service.SearchAsync(keyword);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}