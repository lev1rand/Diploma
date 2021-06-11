using DiplomaServices.Models;
using System.Collections.Generic;

namespace DiplomaServices.Interfaces
{
    public interface ITestService
    {
        public CreateTestModel CreateTest(CreateTestModel model);
        public IEnumerable<GetTestSimpleModel> GetAll();
        public IEnumerable<decimal> ProcessTestResultSaving(SavePassedTestResultsModel testResult);
        public GetTestForStudentModel GetTestForStudentPassing(string sessionId, int testId);
    }
}
