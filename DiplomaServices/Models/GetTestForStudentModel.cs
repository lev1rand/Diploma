using System;
using System.Collections.Generic;

namespace DiplomaServices.Models
{
    public class GetTestForStudentModel
    {
        public GetTestForStudentModel()
        {
            Questions = new List<GetQuestionForStudentModel>();
        }
        public int TestId { get; set; }
        public string Theme { get; set; }
        public List<GetQuestionForStudentModel> Questions { get; set; }
        public int TimeLimitInMinutes { get; set; }
        public DateTime ComplitionDate { get; set; }
        public string CourseName { get; set; }
    }

    public class GetQuestionForStudentModel
    {
        public GetQuestionForStudentModel()
        {
            ResponseOptions = new List<GetResponseOptionForStudentModel>();
        }
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public bool IsOpenQuestion { get; set; }
        public bool IsFileQuestion { get; set; }
        public List<GetResponseOptionForStudentModel> ResponseOptions { get; set; }
        public int? NumberOfRightAnswers { get; set; }
    }

    public class GetResponseOptionForStudentModel
    {
        public int ResponseOptionId { get; set; }
        public string Value { get; set; }
    }
}
