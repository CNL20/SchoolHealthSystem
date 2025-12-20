using AutoMapper;
using SchoolHealthSystem.DTOs.HealthRecords;
using SchoolHealthSystem.Repositories;
using SchoolHealthSystem.Models;

namespace SchoolHealthSystem.Services
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IHealthRecordRepository _repo;
        private readonly IStudentRepository _studentRepo;
        private readonly IMapper _mapper;
        public HealthRecordService(IHealthRecordRepository repo, IStudentRepository studentRepo ,IMapper mapper)
        {
            _repo = repo;
            _studentRepo = studentRepo;
            _mapper = mapper;
        }

        public async Task CreateAsync(CreateHealthRecordRequest request, Guid nurseId)
        {
            var record = _mapper.Map<HealthRecord>(request);
            var student = await _studentRepo.GetByIdAsync(request.StudentId);
            if (student == null)
                throw new Exception("Không có học sinh này");

            record.Id = Guid.NewGuid();
            record.NurseId = nurseId;
            record.RecordDate = DateTime.UtcNow;

            await _repo.AddAsync(record);
            await _repo.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var record = await _repo.GetByIdAsync(id);
            if (record == null)
                throw new Exception("Không có hồ sơ sức khỏe này");

            _repo.Delete(record);
            await _repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<HealthRecordResponse>> GetAllAsync()
        {
            var record = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<HealthRecordResponse>>(record);
        }

        public async Task<HealthRecordResponse> GetByIdAsync(Guid id)
        {
            var record = await _repo.GetByIdAsync(id);
            if (record == null)
                throw new Exception("Không có hồ sơ sức khỏe này");

            return _mapper.Map<HealthRecordResponse>(record);
        }

        public async Task<IEnumerable<HealthRecordResponse>> GetByStudentIdAsync(Guid studentId)
        {
            var record = await _repo.GetByStudentIdAsync(studentId);
            return _mapper.Map<IEnumerable<HealthRecordResponse>>(record);
        }

        public async Task<IEnumerable<HealthRecordResponse>> SearchAsync(string keyword)
        {
            var record = await _repo.SearchAsync(keyword);
            return _mapper.Map<IEnumerable<HealthRecordResponse>>(record);
        }

        public async Task UpdateAsync(Guid id, UpdateHealthRecordRequest request)
        {
            var record = await _repo.GetByIdAsync(id);
            if (record == null)
                throw new Exception("Không có hồ sơ sức khỏe này");

            _mapper.Map(request, record);
            _repo.Update(record);
            await _repo.SaveChangesAsync();
        }
    }
}