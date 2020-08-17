using DataAccess.Models;
using System.Linq;

namespace DataAccess.Repositories
{
    public class CodeRepository
    {
        #region 

        private readonly TestContext context;

        #endregion

        public CodeRepository(TestContext context)
        {
            this.context = context;
        }

        public void Create(Code item)
        {
            context.Codes.Add(item);
        }

        public void Update(Code item)
        {
            context.Codes.Update(item);
        }

        public Code Get(int id)
        {
            return context.Codes
                .FirstOrDefault(p => p.Id == id);
        }

        public IQueryable<Code> GetAll()
        {
            return context.Codes;
        }
    }
}
