using DataAccess;
using DataAccess.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TestTaskServices.Interfaces;
using TestTaskServices.Models;
using TestTaskServices.Validation;

namespace TestTaskServices.Services
{
    public class CodeService: ICodeService
    {
        #region private members

        private readonly IUnitOfWork uow;

        private readonly MapperService mapper;

        private readonly CreateCodeValidator createValidator;

        private readonly UpdateCodeValidator updateValidator;

        #endregion

        public CodeService(IUnitOfWork uow)
        {
            this.uow = uow;
            mapper = new MapperService();
            createValidator = new CreateCodeValidator();
            updateValidator = new UpdateCodeValidator();
        }

        public IEnumerable<CodeModel> GetAll()
        {
            return mapper.Map<IQueryable<Code>, IEnumerable<CodeModel>>(uow.Codes.GetAll()).ToList();
        }

        public CodeModel Get(int id)
        {
            return mapper.Map<Code, CodeModel>(uow.Codes.Get(id));
        }

        public void Create(CreateCodeModel createCodeModel)
        {
            if (!createValidator.Validate(createCodeModel).IsValid)
            {
                throw new ArgumentException(createValidator
                    .Validate(createCodeModel)
                    .Errors
                    .First()
                    .ErrorMessage);
            }
            
            var code = mapper.Map<CreateCodeModel, Code>(createCodeModel);
            uow.Codes.Create(code);
            uow.Save();
        }

        public void UpdatePatch(int id, [FromBody] JsonPatchDocument<UpdateCodeModel> updateCodeModel)
        {
            var code = uow.Codes.Get(id);

            if(code == null)
            {
                throw new KeyNotFoundException();
            }

            var codeToPatch = mapper.Map<Code, UpdateCodeModel>(code);
            updateCodeModel.ApplyTo(codeToPatch);

            if (!updateValidator.Validate(codeToPatch).IsValid)
            {
                throw new ArgumentException(updateValidator
                    .Validate(codeToPatch)
                    .Errors
                    .First()
                    .ErrorMessage);
            }
            /*string nameChecked = codeToPatch.Name;
            nameChecked = codeToPatch..Replace(" ", null);
            if (s == null)
            {
                valContext.AddFailure("String should have text!");
            }*/
            uow.Codes.Update(mapper.Map<UpdateCodeModel, Code>(codeToPatch));
            uow.Save();
        }

    }

}
