using AutoMapper;
using DataAccess.Entities;
using DataAccess.Entities.Answers;
using DataAccess.Entities.TestEntities;
using DiplomaServices.Models;

namespace DiplomaServices.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<SignInModel, CreateTokenModel>()
                    .ReverseMap();

            CreateMap<SignOutModel, RemoveTokenModel>()
                    .ReverseMap();

            CreateMap<CreateAccountModel, User>()
                .ForMember(u => u.Login, cam => cam.MapFrom(src => src.Login))
                .ForMember(u => u.Password, cam => cam.MapFrom(src => src.Password))
                .ForMember(u => u.Name, cam => cam.MapFrom(src => src.Name))
                .ForMember(u => u.Fathername, cam => cam.MapFrom(src => src.Fathername))
                .ForMember(u => u.Surname, cam => cam.MapFrom(src => src.Surname))
                .ForMember(u => u.Role, cam => cam.MapFrom(src => src.Role))
                .ReverseMap();

            CreateMap<CreateCourseModel, Course>()
                .ReverseMap();

            CreateMap<CreateQuestionModel, Question>()
                .ReverseMap();

            CreateMap<CreateResponseOptionModel, ResponseOption>()
                .ReverseMap();

            CreateMap<CreateRightAnswerModel, RightSimpleAnswer>()
                .ReverseMap();
        }
    }
}
