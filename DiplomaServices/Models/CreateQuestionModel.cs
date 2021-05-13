using System.Collections.Generic;

namespace DiplomaServices.Models
{
    public class CreateQuestionModel
    {
        public CreateQuestionModel()
        {
            ResponseOptions = new List<CreateResponseOptionModel>();
        }
        public string Title { get; set; }
        public List<CreateResponseOptionModel> ResponseOptions { get; set; }
        public bool IsOpenQuestion { get; set; }
        public bool IsFileQuestion { get; set; }

    }
}
