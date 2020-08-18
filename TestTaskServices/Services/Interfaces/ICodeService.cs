using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Web.Http;
using TestTaskServices.Models;

namespace TestTaskServices.Interfaces
{
    public interface ICodeService
    {
        public IEnumerable<CodeModel> GetAll();

        public CodeModel Get(int id);

        public void Create(CreateCodeModel createCodeModel);

        public void UpdatePatch(int id, [FromBody] JsonPatchDocument<UpdateCodeModel> updateCodeModel);
    }
}
