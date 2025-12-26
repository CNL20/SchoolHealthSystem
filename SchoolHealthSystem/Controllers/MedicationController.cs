using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolHealthSystem.DTOs.Medications;
using SchoolHealthSystem.Services;

namespace SchoolHealthSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MedicationController : ControllerBase
    {
        private readonly IMedicationService _service;

        public MedicationController(IMedicationService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var medi = await _service.GetAllAsync();
                return Ok(medi);
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
                var medi = await _service.GetByIdAsync(id);
                return Ok(medi);
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

        [HttpGet("parent/{parentId}")]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<ActionResult> GetByParentId(Guid parentId)
        {
            try
            {
                var records = await _service.GetByParentIdAsync(parentId);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("my-requests")]
        [Authorize(Roles = "Parent")]
        public async Task<ActionResult> GetMyRequests()
        {
            try
            {
                // Lấy parentId từ JWT token
                var parentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(parentIdClaim) || !Guid.TryParse(parentIdClaim, out Guid parentId))
                    return BadRequest(new { message = "Invalid parent ID" });

                var records = await _service.GetByParentIdAsync(parentId);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles ="Parent")]
        public async Task<ActionResult> Create([FromBody] CreateMedicationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var parentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(parentIdClaim) || !Guid.TryParse(parentIdClaim, out Guid parentId))
                    return BadRequest(new { message = "Invalid parent ID" });
                
                await _service.CreateAsync(request, parentId);
                return Ok(new { message = "Tạo đơn yêu cầu thuốc thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles ="Parent")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateMedicationRequest request)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _service.UpdateAsync(id, request);
                return Ok(new { message = "Cập nhật yêu cầu thuốc thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles ="Parent")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new { message = "Xóa yêu cầu thuốc thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<ActionResult> Search([FromQuery]string keyword)
        {
            try
            {
                var medi = await _service.SearchAsync(keyword);
                return Ok(medi);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<ActionResult> Approve(Guid id)
        {
            try
            {
                var nurseIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(nurseIdClaim) || !Guid.TryParse(nurseIdClaim, out Guid nurseId))
                    return BadRequest(new { message = "Invalid nurse ID" });

                await _service.ApproveRequestAsync(id, nurseId);
                return Ok(new { message = "Phê duyệt yêu cầu thuốc thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<ActionResult> Reject(Guid id)
        {
            try
            {
                var nurseIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(nurseIdClaim) || !Guid.TryParse(nurseIdClaim, out Guid nurseId))
                    return BadRequest(new { message = "Invalid nurse ID" });

                await _service.RejectRequestAsync(id, nurseId);
                return Ok(new { message = "Từ chối yêu cầu thuốc thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}