using AutoMapper;
using DataAccess.Entities;
using TestTaskServices.Models;

namespace TestTaskServices.Mapping
{
    public class CodeMapperProfile : Profile
    {
        public CodeMapperProfile()
        {
            CreateMap<Code, CreateCodeModel>()
                .ReverseMap();

            CreateMap<Code, UpdateCodeModel>()
                .ReverseMap();

            CreateMap<Code, CodeModel>()
                .ReverseMap();

            CreateMap<SignInModel, CreateTokenModel>()
                    .ReverseMap();

            CreateMap<CreateAccountModel, User>()
                .ForMember(u=>u.Login, cam=>cam.MapFrom(src=>src.Login))
                .ForMember(u => u.Password, cam => cam.MapFrom(src => src.Password))
                .ForMember(u => u.Name, cam => cam.MapFrom(src => src.Name))
                .ForMember(u => u.Fathername, cam => cam.MapFrom(src => src.Fathername))
                .ForMember(u => u.Surname, cam => cam.MapFrom(src => src.Surname))
                .ForMember(u => u.Role, cam => cam.MapFrom(src => src.Role))
                .ReverseMap();
        }
    }
}
