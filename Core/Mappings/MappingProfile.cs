using AutoMapper;
using Core.Entities;
using Core.Models.Task;
using Core.Models.TeamMember;

namespace Core.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // TaskItem Mappings
            CreateMap<TaskItem, TaskResponse>()
                .ForMember(dest => dest.MemberName,
                           opt => opt.MapFrom(src => src.Member != null ? $"{src.Member.Name} {src.Member.Surname}".Trim() : null));

            CreateMap<CreateTaskRequest, TaskItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UpdateTaskRequest, TaskItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
                               srcMember != null && (!(srcMember is string s) || !string.IsNullOrWhiteSpace(s))));

            // TeamMember Mappings
            CreateMap<TeamMember, TeamMemberResponse>()
                .ReverseMap();

            CreateMap<TeamMemberRequest, TeamMember>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}