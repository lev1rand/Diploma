using DiplomaServices.Models;
using System.Collections.Generic;

namespace DiplomaServices.Interfaces
{
    public interface ITestService
    {
        public CreateTestModel CreateTest(CreateTestModel model);
        public IEnumerable<GetTestSimpleModel> GetAll();
        public IEnumerable<GetTestDetailsModel> GetTestDetailsByStudentId(int userId);
        public IEnumerable<decimal> ProcessTestResultSaving(SavePassedTestResultsModel testResult);
    }
}
