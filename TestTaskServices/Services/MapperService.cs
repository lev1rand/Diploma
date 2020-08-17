using AutoMapper;
using System.Linq;
using TestTaskServices.Mapping;

namespace TestTaskServices.Services
{
    public class MapperService
    {
        private readonly IMapper mapper;

        public MapperService()
        {
            mapper = ConfigureMapper();
        }

        private IMapper ConfigureMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CodeMapperProfile>();
            });

            return config.CreateMapper();
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return mapper.Map<TSource, TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return mapper.Map(source, destination);
        }

        public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source)
        {
            return mapper.ProjectTo<TDestination>(source);
        }
    }
}
