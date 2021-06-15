using System;
using System.Collections.Generic;

namespace DiplomaServices.Models
{
    public class GetDetailedTestModel
    {
        public GetDetailedTestModel()
        {
            Questions = new List<GetDetailedQuestionModel>();
        }
        public string Theme { get; set; }
        public int TestId { get; set; }
        public List<GetDetailedQuestionModel> Questions { get; set; }
        public DateTime DateTime { get; set; }
        public string ExpireTime { get; set; }
    }

    public class GetDetailedQuestionModel
    {
        public GetDetailedQuestionModel()
        {
            ResponseOptions = new List<GetDetailedResponseOptionModel>();
        }
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public List<GetDetailedResponseOptionModel> ResponseOptions { get; set; }
        public bool IsOpenQuestion { get; set; }
        public bool IsFileQuestion { get; set; }
        public decimal MaxGrade { get; set; }
    }
    public class GetDetailedResponseOptionModel
    {
        public int ResponseOptionId { get; set; }
        public string Value { get; set; }
    }
}
