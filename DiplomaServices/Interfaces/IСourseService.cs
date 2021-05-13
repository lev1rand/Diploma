using DiplomaServices.Models;

namespace DiplomaServices.Interfaces
{
    public interface ICourseService
    {
        public AddCourseApplicantsResponseModel CreateCourse(CreateCourseModel model);
        public bool CheckIfCourseExists(int id);
    }
}
