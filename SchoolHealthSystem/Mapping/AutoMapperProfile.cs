using AutoMapper;
using SchoolHealthSystem.DTOs.HealthRecords;
using SchoolHealthSystem.DTOs.Medications;
using SchoolHealthSystem.DTOs.Students;
using SchoolHealthSystem.Models;

namespace SchoolHealthSystem.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Student
            // 1. Map từ Model sang Response (Dùng để trả dữ liệu về Client)
            CreateMap<Student, StudentResponse>();

            CreateMap<CreateStudentRequest, Student>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())              
                .ForMember(dest => dest.StudentCode, opt => opt.Ignore())       
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Parent, opt => opt.Ignore())          
                .ForMember(dest => dest.HealthRecords, opt => opt.Ignore())   
                .ForMember(dest => dest.MedicationRequests, opt => opt.Ignore())
                .ForMember(dest => dest.VaccinationRecords, opt => opt.Ignore());

            // 3. Map từ Request sang Model (Dùng để cập nhật)
            // Lưu ý: Dùng .ForAllMembers để bỏ qua các trường null nếu bạn chỉ muốn cập nhật một phần
            CreateMap<UpdateStudentRequest, Student>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            //HealthRecord
            CreateMap<HealthRecord, HealthRecordResponse>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student!.FullName))
                .ForMember(dest => dest.NurseName, opt => opt.MapFrom(src => src.Nurse!.FullName));

            CreateMap<CreateHealthRecordRequest, HealthRecord>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.NurseId, opt => opt.Ignore())
                .ForMember(dest => dest.RecordDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<UpdateHealthRecordRequest, HealthRecord>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            //MedicationRequest
            CreateMap<MedicationRequest, MedicationResponse>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student!.FullName))
                .ForMember(dest => dest.RequestedByParentName, opt => opt.MapFrom(src => src.RequestedByParent!.FullName))
                .ForMember(dest => dest.ApprovedByNurseName, opt => opt.MapFrom(src => src.ApprovedByNurse!.FullName));

            CreateMap<CreateMedicationRequest, MedicationRequest>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RequestedByParentId, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovedByNurseId, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => MedicationStatus.Pending))
                .ForMember(dest => dest.RequestDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<UpdateMedicationRequest, MedicationRequest>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}