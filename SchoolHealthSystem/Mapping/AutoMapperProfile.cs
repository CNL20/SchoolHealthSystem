using AutoMapper;
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

        }
    }
}