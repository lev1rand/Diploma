using DiplomaServices.Models;
using System.Collections.Generic;

namespace DiplomaServices.Interfaces
{
    public interface ITestService
    {
        public CreateTestModel CreateTest(CreateTestModel model);
        public IEnumerable<GetTestSimpleModel> GetAll();
        public void ProcessTestResultSaving(SavePassedTestResultsModel testResult);
        public GetTestForStudentModel GetTestForStudentPassing(string sessionId, int testId);
        public GetTestForEvaluationModel GetTestForEvaluation (string sessionId, int testId, int studentId);
        public void SaveTestAfterEvaluation(SaveTestAfterEvaluationModel model);
        public List<GetDetailedTestModel> GetDetailedTestsListByStudentId(int studentId);
        public GetTestResultFullyEvaluatedModel GetTestResultFullyEvaluated(int studentId, int testId);
    }
}
