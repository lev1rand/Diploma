using DataAccess.Entities;
using DiplomaServices.Models;
using System.Collections.Generic;

namespace DiplomaServices.Interfaces
{
    public interface ICourseService
    {
        public AddCourseApplicantsResponseModel CreateCourse(CreateCourseModel model);
        public IEnumerable<GetCourseSimpleModel> GetAll();
        public bool CheckIfCourseExists(int id);
    }
}
