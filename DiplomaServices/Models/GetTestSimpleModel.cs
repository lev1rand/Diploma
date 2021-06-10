using System;
namespace DiplomaServices.Models
{
    public class GetTestSimpleModel: AuthTemplateModel
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public GetCourseSimpleModel Course { get; set; }
    }
}
