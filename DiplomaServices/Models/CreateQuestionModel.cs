using System.Collections.Generic;
using System.Text.Json.Serialization;

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
        public decimal Grade { get; set; }
        [JsonIgnore]
        public int TestId { get; set; }
    }
}
