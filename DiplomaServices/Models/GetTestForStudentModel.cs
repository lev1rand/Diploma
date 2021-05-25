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

    }
}
