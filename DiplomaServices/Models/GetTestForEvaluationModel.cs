using System.Collections.Generic;

namespace DiplomaServices.Models
{
    public class GetTestForEvaluationModel
    {
        public GetTestForEvaluationModel()
        {

        }
        public int TestId { get; set; }
        public int StudentId { get; set; }
        public string StudentLogin { get; set; }
        public string Theme { get; set; }
        public string ComplitionDate { get; set; }
        public List<GetQuestionForEvaluationModel> Questions { get; set; }
    }

    public class GetQuestionForEvaluationModel
    {
        public int QuestionId { get; set; }
        public string QuestionTitle { get; set; }
        public bool IsOpenQuestion { get; set; }
        public bool IsFileQuestion { get; set; }
        public GetUserAnswerForEvaluationModel UserAnswer { get; set; }
    }

    public class GetUserAnswerForEvaluationModel
    {
        public int? ResponseOptionId { get; set; }
        public string Value { get; set; }
        public int? SummaryGrade { get; set; }
        public int? AnswerMaxGrade { get; set; }
    }
}
