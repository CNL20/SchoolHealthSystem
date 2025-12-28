using SchoolHealthSystem.DTOs.Medications;

namespace SchoolHealthSystem.Services
{
    public interface IMedicationService
    {
        Task CreateAsync(CreateMedicationRequest request, Guid parentId);
        Task<MedicationResponse> GetByIdAsync(Guid id);
        Task<IEnumerable<MedicationResponse>> GetAllAsync();
        Task<IEnumerable<MedicationResponse>> GetByStudentIdAsync(Guid studentId);
        Task<IEnumerable<MedicationResponse>> GetByParentIdAsync(Guid parentId);
        Task<IEnumerable<MedicationResponse>> SearchAsync(string keyword);
        Task UpdateAsync(Guid id, UpdateMedicationRequest request);
        Task DeleteAsync(Guid id);
        
        // Workflow methods for approval
        Task ApproveRequestAsync(Guid id, Guid nurseId);
        Task RejectRequestAsync(Guid id, Guid nurseId);
    }
}