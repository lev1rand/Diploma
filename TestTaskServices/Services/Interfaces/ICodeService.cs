using System;
using System.Collections.Generic;
using System.Text;
using TestTaskServices.Models;

namespace TestTaskServices.Interfaces
{
    public interface ICodeService
    {
        public IEnumerable<CreateCodeModel> GetAll();

        public CreateCodeModel Get(int id);

        public void Create(CreateCodeModel createCodeModel);

        public void Update(UpdateCodeModel updateCodeModel);
    }
}
