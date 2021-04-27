using AutoMapper;
using DataAccess.Entities;
using TestTaskServices.Models;
using TestTaskAPI.Models;

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

            CreateMap<TestTaskAPI.Models.SignInModel, CreateTokenModel>()
                    .ReverseMap();
        }
    }
}
