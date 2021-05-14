using System;
using System.Collections.Generic;

namespace DiplomaServices.Models
{
    public class CreateTestModel
    {
        public CreateTestModel()
        {
            Questions = new List<CreateQuestionModel>();
            Applicants = new List<CreateApplicantModel>();
        }
        public string Theme { get; set; }
        public int? CourseId { get; set; }
        public List<CreateQuestionModel> Questions { get; set; }
        public List<CreateApplicantModel> Applicants { get; set; }
        public DateTime DateTime { get; set; }
        public string ExpireTime { get; set; }
    }
}
