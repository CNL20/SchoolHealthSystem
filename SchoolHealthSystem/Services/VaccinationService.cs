using AutoMapper;
using SchoolHealthSystem.DTOs.Vaccinations;
using SchoolHealthSystem.Models;
using SchoolHealthSystem.Repositories;

namespace SchoolHealthSystem.Services
{
    public class VaccinationService : IVaccinationService
    {
        private readonly IVaccinationRepository _repo;
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepo;

        public VaccinationService(IVaccinationRepository repo, IStudentRepository studentRepo, IMapper mapper)
        {
            _repo = repo;
            _studentRepo = studentRepo;
            _mapper = mapper;
        }

        public async Task<VaccinationResponse?> CheckExistingVaccinationAsync(Guid studentId, string vaccineName)
        {
            var vac = await _repo.GetByStudentAndVaccineAsync(studentId, vaccineName);
            if (vac == null)
                return null;
            return _mapper.Map<VaccinationResponse>(vac);
        }

        public async Task CreateAsync(CreateVaccinationRequest request, Guid nurseId)
        {
            var vac = _mapper.Map<VaccinationRecord>(request);
            var student = await _studentRepo.GetByIdAsync(request.StudentId);
            if (student == null)
                throw new Exception("Không có học sinh này");
            var ext = await _repo.GetByStudentAndVaccineAsync(request.StudentId, request.VaccineName);            if (ext != null)
                throw new Exception($"Học sinh đã được tiêm loại vaccine {request.VaccineName} vào {ext.ScheduledDate:dd/MM/yyyy}");

            vac.Id = Guid.NewGuid();
            vac.AdministeredByNurseId = nurseId;

            await _repo.AddAsync(vac);
            await _repo.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var vac = await _repo.GetByIdAsync(id);
            if (vac == null)
                throw new Exception("Không tìm thấy sự kiện tiêm chủng này");
            _repo.Delete(vac);
            await _repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<VaccinationResponse>> GetAllAsync()
        {
            var vac = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<VaccinationResponse>>(vac);
        }

        public async Task<VaccinationResponse> GetByIdAsync(Guid id)
        {
            var vac = await _repo.GetByIdAsync(id);
            if (vac == null)
                throw new Exception("Không tìm thấy sự kiện tiêm chủng này");

            return _mapper.Map<VaccinationResponse>(vac);
        }

        public async Task<IEnumerable<VaccinationResponse>> GetByStudentIdAsync(Guid studentId)
        {
            var vac = await _repo.GetByStudentIdAsync(studentId);
            return _mapper.Map<IEnumerable<VaccinationResponse>>(vac);
        }

        public async Task<IEnumerable<VaccinationResponse>> SearchAsync(string keyword)
        {
            var vac = await _repo.SearchAsync(keyword);
            return _mapper.Map<IEnumerable<VaccinationResponse>>(vac);
        }

        public async Task UpdateAsync(Guid id, UpdateVaccinationRequest request)
        {
            var vac = await _repo.GetByIdAsync(id);
            if (vac == null)
                throw new Exception("Không tìm thấy sự kiện tiêm chủng này");
            _mapper.Map(request, vac);
            _repo.Update(vac);
            await _repo.SaveChangesAsync();
        }

        // New methods for enhanced functionality
        public async Task CompleteVaccinationAsync(Guid id, CompleteVaccinationRequest request, Guid nurseId)
        {
            var vaccination = await _repo.GetByIdAsync(id);
            if (vaccination == null)
                throw new Exception("Không tìm thấy sự kiện tiêm chủng này");

            if (vaccination.Status == VaccinationStatus.Completed)
                throw new Exception("Sự kiện tiêm chủng này đã được hoàn thành");

            if (vaccination.Status == VaccinationStatus.Cancelled)
                throw new Exception("Không thể hoàn thành sự kiện tiêm chủng đã bị hủy");

            // Update vaccination record
            _mapper.Map(request, vaccination);
            vaccination.AdministeredByNurseId = nurseId;
            
            _repo.Update(vaccination);
            await _repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<VaccinationResponse>> GetByStatusAsync(VaccinationStatus status)
        {
            var vaccinations = await _repo.GetByStatusAsync(status);
            return _mapper.Map<IEnumerable<VaccinationResponse>>(vaccinations);
        }

        public async Task<IEnumerable<VaccinationResponse>> GetScheduledVaccinationsAsync()
        {
            var vaccinations = await _repo.GetScheduledVaccinationsAsync();
            return _mapper.Map<IEnumerable<VaccinationResponse>>(vaccinations);
        }

        public async Task<IEnumerable<VaccinationResponse>> GetOverdueVaccinationsAsync()
        {
            var vaccinations = await _repo.GetOverdueVaccinationsAsync();
            return _mapper.Map<IEnumerable<VaccinationResponse>>(vaccinations);
        }

        public async Task<IEnumerable<VaccinationResponse>> GetUpcomingVaccinationsAsync(int days = 7)
        {
            var vaccinations = await _repo.GetUpcomingVaccinationsAsync(days);
            return _mapper.Map<IEnumerable<VaccinationResponse>>(vaccinations);
        }

        public async Task<IEnumerable<VaccinationResponse>> GetByBatchNumberAsync(string batchNumber)
        {
            var vaccinations = await _repo.GetByBatchNumberAsync(batchNumber);
            return _mapper.Map<IEnumerable<VaccinationResponse>>(vaccinations);
        }

        public async Task CancelVaccinationAsync(Guid id, string reason)
        {
            var vaccination = await _repo.GetByIdAsync(id);
            if (vaccination == null)
                throw new Exception("Không tìm thấy sự kiện tiêm chủng này");

            if (vaccination.Status == VaccinationStatus.Completed)
                throw new Exception("Không thể hủy sự kiện tiêm chủng đã hoàn thành");

            vaccination.Status = VaccinationStatus.Cancelled;
            vaccination.Note = $"{vaccination.Note}\nLý do hủy: {reason}".Trim();

            _repo.Update(vaccination);
            await _repo.SaveChangesAsync();
        }

        public async Task PostponeVaccinationAsync(Guid id, DateTime newDate, string reason)
        {
            var vaccination = await _repo.GetByIdAsync(id);
            if (vaccination == null)
                throw new Exception("Không tìm thấy sự kiện tiêm chủng này");

            if (vaccination.Status == VaccinationStatus.Completed)
                throw new Exception("Không thể hoãn sự kiện tiêm chủng đã hoàn thành");

            if (vaccination.Status == VaccinationStatus.Cancelled)
                throw new Exception("Không thể hoãn sự kiện tiêm chủng đã bị hủy");

            vaccination.ScheduledDate = newDate;
            vaccination.Status = VaccinationStatus.Postponed;
            vaccination.Note = $"{vaccination.Note}\nLý do hoãn: {reason}".Trim();

            _repo.Update(vaccination);
            await _repo.SaveChangesAsync();
        }
    }
}