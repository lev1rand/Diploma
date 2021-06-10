using System.Collections.Generic;

namespace DiplomaServices.Models
{
    //Test model
    public class SavePassedTestResultsModel: AuthTemplateModel
    {
        public SavePassedTestResultsModel()
        {
            Questions = new List<SavePassedTestResultQuestionModel>();
        }
        public int TestId { get; set; }
        public List<SavePassedTestResultQuestionModel> Questions { get; set; }
    }

    //Question model
    public class SavePassedTestResultQuestionModel
    {
        public SavePassedTestResultQuestionModel()
        {
            ChosenResponseOptionIds = new List<int>();
        }
        public int QuestionId { get; set; }
        public string FileName { get; set; }
        public string OpenOrFileAnswerValue{ get; set; }
        public List<int> ChosenResponseOptionIds { get; set; }
    }
}
