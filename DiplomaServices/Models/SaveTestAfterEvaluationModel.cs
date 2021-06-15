using System.Collections.Generic;

namespace DiplomaServices.Models
{
    public class SaveTestAfterEvaluationModel : AuthTemplateModel
    {
        public SaveTestAfterEvaluationModel()
        {
            QuestionResults = new List<QuestionResult>();
        }
        public int TestId { get; set; }
        public int StudentId { get; set; }
        public List<QuestionResult> QuestionResults { get; set; }
    }

    public class QuestionResult
    {
        public int QuestionId { get; set; }
        public decimal Grade { get; set; }
    }
}
