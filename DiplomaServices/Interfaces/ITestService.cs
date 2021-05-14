using DataAccess.Entities.TestEntities;
using DiplomaServices.Models;
using System.Collections.Generic;

namespace DiplomaServices.Interfaces
{
    public interface ITestService
    {
        public CreateTestModel CreateTest(CreateTestModel model);
        public IEnumerable<Test> GetAll();
    }
}
