using System;
namespace DiplomaServices.Models
{
    public class GetTestSimpleModel
    {
        public int Id { get; set; }
        public string Theme { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateTime { get; set; }
        public GetCourseSimpleModel Course { get; set; }
    }
}
