using DataAccess;
using DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;
using TestTaskServices.Interfaces;
using TestTaskServices.Models;

namespace TestTaskServices.Services
{
    public class CodeService: ICodeService
    {
        #region private members

        private readonly IUnitOfWork uow;

        private readonly MapperService mapper;

        #endregion

        public CodeService(IUnitOfWork uow)
        {
            this.uow = uow;
            mapper = new MapperService();
        }

        public IEnumerable<CreateCodeModel> GetAll()
        {
            return mapper.Map<IQueryable<Code>, IEnumerable<CreateCodeModel>>(uow.Codes.GetAll()).ToList();
        }

        public CreateCodeModel Get(int id)
        {
            return mapper.Map<Code, CreateCodeModel>(uow.Codes.Get(id));
        }

        public void Create(CreateCodeModel createCodeModel)
        {
            var code = mapper.Map<CreateCodeModel, Code>(createCodeModel);
            uow.Codes.Create(code);
            uow.Save();
        }

        public void Update(UpdateCodeModel updateCodeModel)
        {
            var code = mapper.Map<UpdateCodeModel, Code>(updateCodeModel);
            uow.Codes.Update(code);
            uow.Save();
        }

    }

}
