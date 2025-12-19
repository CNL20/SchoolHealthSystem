using AutoMapper;
using SchoolHealthSystem.Data;
using SchoolHealthSystem.DTOs.Students;
using SchoolHealthSystem.Models;
using SchoolHealthSystem.Repositories;

namespace SchoolHealthSystem.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repo;
        private readonly IMapper _mapper;
        public StudentService(IStudentRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task CreateAsync(CreateStudentRequest request)
        {
            var student = _mapper.Map<Student>(request);

            student.Id = Guid.NewGuid();
            student.StudentCode = await GenerateStudentCodeAsync();
            student.IsActive = true;

            await _repo.AddAsync(student);
            await _repo.SaveChangesAsync();
        }

        // Soft Delete (thường dùng)
        public async Task DeactivateAsync(Guid id)
        {
            var student = await _repo.GetByIdAsync(id);
            if (student == null)
                throw new Exception("Không có học sinh này");

            student.IsActive = false;
            _repo.Update(student);
            await _repo.SaveChangesAsync();
        }

        // Hard Delete (admin only)
        public async Task DeletePermanentlyAsync(Guid id)
        {
            var student = await _repo.GetByIdAsync(id);
            if (student == null)
                throw new Exception("Không có học sinh này");

            _repo.Delete(student);
            await _repo.SaveChangesAsync();
        }

        public async Task<string> GenerateStudentCodeAsync()
        {
           var count = await _repo.GetStudentCountAsync();
            return $"HS{(count + 1):D3}";
        }

        public async Task<IEnumerable<StudentResponse>> GetAllAsync()
        {
            var student = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<StudentResponse>>(student);
        }

        public async Task<StudentResponse> GetByIdAsync(Guid id)
        {
            var student = await _repo.GetByIdAsync(id);
            if(student == null) 
                throw new Exception("Không tìm thấy học sinh này");

            return _mapper.Map<StudentResponse>(student);
        }

        public async Task<IEnumerable<StudentResponse>> SearchAsync(string keyword)
        {
            var student = await _repo.SearchAsync(keyword);
            return _mapper.Map<IEnumerable<StudentResponse>>(student);
        }

        public async Task UpdateAsync(Guid id, UpdateStudentRequest request)
        {
            var student = await _repo.GetByIdAsync(id);

            if(student == null) 
                throw new Exception("Không có học sinh này");

            _mapper.Map(request, student);
            _repo.Update(student);
            await _repo.SaveChangesAsync();
        }
    }
}