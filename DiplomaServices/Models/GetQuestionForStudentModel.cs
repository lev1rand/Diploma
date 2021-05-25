using System.Collections.Generic;

namespace DiplomaServices.Models
{
    public class GetQuestionForStudentModel
    {
        public GetQuestionForStudentModel()
        {
            ResponseOptions = new List<GetResponseOptionForStudentModel>();
        }
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public List<GetResponseOptionForStudentModel> ResponseOptions { get; set; }
    }
}
