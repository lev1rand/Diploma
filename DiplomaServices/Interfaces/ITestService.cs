using DataAccess.Entities.TestEntities;
using DiplomaServices.Models;
using System.Collections.Generic;

namespace DiplomaServices.Interfaces
{
    public interface ITestService
    {
        public CreateTestModel CreateTest(CreateTestModel model);
        public IEnumerable<Test> GetAll();
        public IEnumerable<GetTestDetailsModel> GetTestDetailsByStudentId(int userId);
        public void ProcessAnswers(List<SaveUserAnswersModel> answers, int userId);
    }
}
