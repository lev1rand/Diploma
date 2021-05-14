using DiplomaServices.Models;
using System.Collections.Generic;

namespace DiplomaServices.Interfaces
{
    public interface IQuestionService
    {
        public void CreateQuestion(CreateQuestionModel model);
        public List<GetQuestionModel> GetQuestionsForTheTest(int testId);
    }
}
