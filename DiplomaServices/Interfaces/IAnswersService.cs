using DiplomaServices.Models;
using System.Collections.Generic;

namespace DiplomaServices.Interfaces
{
    public interface IAnswersService
    {
        public void SaveAnswers(List<SaveUserAnswersModel> answers, int userId);
        public void EvaluateAnswers(List<SaveUserAnswersModel> answers, int userId);
    }
}
