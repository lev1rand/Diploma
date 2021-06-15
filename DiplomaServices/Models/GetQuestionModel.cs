using System.Collections.Generic;

namespace DiplomaServices.Models
{
    public class GetQuestionModel
    {
        public GetQuestionModel()
        {
            ResponseOptions = new List<GetResponseOptionModel>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public List<GetResponseOptionModel> ResponseOptions { get; set; }
        public bool IsOpenQuestion { get; set; }
        public bool IsFileQuestion { get; set; }
        public decimal MaxGrade { get; set; }
    }
}
