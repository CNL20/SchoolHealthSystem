using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolHealthSystem.DTOs.Students;
using SchoolHealthSystem.Services;

namespace SchoolHealthSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _service;

        public StudentsController(IStudentService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<ActionResult<IEnumerable<StudentResponse>>> GetAllAsync()
        {
            try
            {
                var student = await _service.GetAllAsync();
                return Ok(student);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Manager,Nurse,Parent")]
        public async Task<ActionResult<StudentResponse>> GetByIdAsync(Guid id)
        {
            try
            {
                var student = await _service.GetByIdAsync(id);

                return Ok(student);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> CreateStudent([FromBody] CreateStudentRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _service.CreateAsync(request);
                return Ok(new { message = "Tạo học sinh mới thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> UpdateStudent(Guid id, [FromBody] UpdateStudentRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _service.UpdateAsync(id, request);
                return Ok(new { message = "Cập nhật học sinh thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Soft Delete - Manager có thể dùng
        [HttpPatch("{id}/deactivate")]
        [Authorize(Roles = "Admin,Manager,nurse")]
        public async Task<ActionResult> DeactivateStudent(Guid id)
        {
            try
            {
                await _service.DeactivateAsync(id);
                return Ok(new { message = "Vô hiệu hóa học sinh thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Hard Delete - Chỉ Admin
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteStudent(Guid id)
        {
            try
            {
                await _service.DeletePermanentlyAsync(id);
                return Ok(new { message = "Xóa học sinh vĩnh viễn thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin,Manager,Nurse")]
        public async Task<ActionResult<IEnumerable<StudentResponse>>> SearchStudents([FromQuery] string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return BadRequest(new { message = "Từ khóa tìm kiếm không được để trống" });

                var student = await _service.SearchAsync(keyword);
                return Ok(student);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}