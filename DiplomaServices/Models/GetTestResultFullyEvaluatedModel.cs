using System.Collections.Generic;

namespace DiplomaServices.Models
{
    public class GetTestResultFullyEvaluatedModel
    {
        public GetTestResultFullyEvaluatedModel()
        {
            Questions = new List<GetEvaluatedQuestionModel>();
        }
        public int TestId { get; set; }
        public string Theme { get; set; }
        public string CourseName { get; set; }
        public decimal SummaryGrade { get; set; }
        public List<GetEvaluatedQuestionModel> Questions { get; set; }
    }

    public class GetEvaluatedQuestionModel 
    {
        public GetEvaluatedQuestionModel()
        {
            ResponseOptions = new List<GetEvaluatedResponseOptions>();
        }
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public decimal MaxGrade { get; set; }
        public decimal SummaryGrade { get; set; }
        public bool IsOpenQuestion { get; set; }
        public bool IsFileQuestion { get; set; }
        public string ValueForFiledOrOpenQuestion { get; set; }
        public List<GetEvaluatedResponseOptions> ResponseOptions { get; set; }
    }

    public class GetEvaluatedResponseOptions
    {
        public int ResponseOptionId { get; set; }
        public string Value { get; set; }
        public bool IsChosen { get; set; }
        public bool IsValid { get; set; }
    }
}
