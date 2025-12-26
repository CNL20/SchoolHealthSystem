using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata;
using SchoolHealthSystem.DTOs.Medications;
using SchoolHealthSystem.Models;
using SchoolHealthSystem.Repositories;

namespace SchoolHealthSystem.Services
{
    public class MedicationService : IMedicationService
    {
        private readonly IMedicationRepository _repo;
        private readonly IStudentRepository _studentRepo;
        private IMapper _mapper;

        public MedicationService(IMedicationRepository repo, IStudentRepository studentRepo, IMapper mapper)
        {
            _repo = repo;
            _studentRepo = studentRepo;
            _mapper = mapper;
        }
        public async Task ApproveRequestAsync(Guid id, Guid nurseId)
        {
            var medi = await _repo.GetByIdAsync(id);
            if (medi == null)
                throw new Exception("không tìm thấy yêu cầu thuốc này");
            if (medi.Status != MedicationStatus.Pending)
                throw new Exception("Không phê duyệt yêu cầu đã xử lí");
            medi.Status = MedicationStatus.Approved;
            medi.ApprovedByNurseId = nurseId;
            _repo.Update(medi);
            await _repo.SaveChangesAsync();
        }

        public async Task CreateAsync(CreateMedicationRequest request, Guid parentId)
        {
            var medi = _mapper.Map<MedicationRequest>(request);
            var student = await _studentRepo.GetByIdAsync(request.StudentId);
            if (student == null)
                throw new Exception("Không có học sinh này");

            medi.Id = Guid.NewGuid();
            medi.RequestedByParentId = parentId;     

            await _repo.AddAsync(medi);
            await _repo.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var medi = await _repo.GetByIdAsync(id);
            if (medi == null)
                throw new Exception("Không tìm thấy yêu cầu thuốc này");

            if (medi.Status != MedicationStatus.Pending)
                throw new Exception("Chỉ có thể xóa yêu cầu đang chờ duyệt");

            _repo.Delete(medi);
            await _repo.SaveChangesAsync() ;

        }

        public async Task<IEnumerable<MedicationResponse>> GetAllAsync()
        {
            var medi = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<MedicationResponse>>(medi);
        }

        public async Task<MedicationResponse> GetByIdAsync(Guid id)
        {
            var medi = await _repo.GetByIdAsync(id);
            if (medi == null)
                throw new Exception("Không tìm thấy yêu cầu thuốc này");

            return _mapper.Map<MedicationResponse>(medi);
        }

        public async Task<IEnumerable<MedicationResponse>> GetByParentIdAsync(Guid parentId)
        {
            var medi = await _repo.GetByParentIdAsync(parentId);
            return _mapper.Map<IEnumerable<MedicationResponse>>(medi);
        }

        public async Task<IEnumerable<MedicationResponse>> GetByStudentIdAsync(Guid studentId)
        {
            var medi = await _repo.GetByStudentIdAsync(studentId);
            return _mapper.Map<IEnumerable<MedicationResponse>>(medi);
        }

        public async Task RejectRequestAsync(Guid id, Guid nurseId)
        {
            var medi = await _repo.GetByIdAsync(id);
            if (medi == null)
                throw new Exception("Không tìm thấy yêu cầu thuốc này");
            if (medi.Status != MedicationStatus.Pending)
                throw new Exception("Không từ chối yêu cầu đã được xử lí");
            medi.Status = MedicationStatus.Rejected;
            medi.ApprovedByNurseId = nurseId;
            _repo.Update(medi);
            await _repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<MedicationResponse>> SearchAsync(string keyword)
        {
            var medi = await _repo.SearchAsync(keyword);
            return _mapper.Map<IEnumerable<MedicationResponse>>(medi);
        }

        public async Task UpdateAsync(Guid id, UpdateMedicationRequest request)
        {
            var medi = await _repo.GetByIdAsync(id);
            if (medi == null)
                throw new Exception("Không tìm thấy yêu cầu thuốc này");

            if (medi.Status != MedicationStatus.Pending)
                throw new Exception("Chỉ có thể sửa yêu cầu đang chờ duyệt");

            _mapper.Map(request, medi);
            _repo.Update(medi);
            await _repo.SaveChangesAsync();
        }
    }
}