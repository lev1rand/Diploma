using System.Collections.Generic;

namespace DiplomaServices.Models
{
    public class CreateCourseModel: AuthTemplateModel
    {
        public CreateCourseModel()
        {
            Applicants = new List<CreateApplicantModel>();
        }
        public string Description { get; set; }
        public string Name { get; set; }
        public List<CreateApplicantModel> Applicants { get; set; }
    }
}
