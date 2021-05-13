using System.Collections.Generic;

namespace DiplomaServices.Models
{
    public class AddCourseApplicantsResponseModel
    {
        public AddCourseApplicantsResponseModel()
        {
            SuccessfullyAddedApplicants = new List<CreateApplicantModel>();
            FailedAddedApplicants = new List<FailedApplicantResponseModel>();
        }

        public List<CreateApplicantModel> SuccessfullyAddedApplicants { get; set; }
        public List<FailedApplicantResponseModel> FailedAddedApplicants { get; set; }
    }
}
