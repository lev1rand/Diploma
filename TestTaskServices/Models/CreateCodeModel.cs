using System.Text.Json.Serialization;

namespace TestTaskServices.Models
{
    public class CreateCodeModel: CodeModel
    {
        [JsonIgnore]
        public override int Id { get; set; }

    }
}
