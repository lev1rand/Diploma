using DataAccess;
using DataAccess.Entities;
using DataAccess.Entities.ManyToManyEntities;
using DiplomaServices.Interfaces;
using DiplomaServices.Mapping;
using DiplomaServices.Models;
using System.Collections.Generic;

namespace DiplomaServices.Services
{
    public class CourseService : ICourseService
    {
        #region private members

        private readonly IUnitOfWork uow;

        private readonly IUserService userService;

        private readonly MapperService mapper;

        #endregion

        public CourseService(IUnitOfWork uow, IUserService userService)
        {
            this.uow = uow;
            this.userService = userService;

            mapper = new MapperService();
        }

        public bool CheckIfCourseExists(int id)
        {
            var course = uow.Courses.Get(c => c.Id == id);

            return course != null;
        }

        public AddCourseApplicantsResponseModel CreateCourse(CreateCourseModel model)
        {
            var course = mapper.Map<CreateCourseModel, Course>(model);
            uow.Courses.Create(course);
            uow.Save();

            var response = AddApplicants(model.Applicants, course.Id);

            return response;
        }

        private AddCourseApplicantsResponseModel AddApplicants(List<CreateApplicantModel> applicants, int courseId)
        {
            var applicantsToAdd = new List<CreateApplicantModel>();
            var failedApplicantsToAdd = new List<FailedApplicantResponseModel>();

            ValidateApplicants(applicantsToAdd, failedApplicantsToAdd, applicants);

            foreach (var applicant in applicantsToAdd)
            {
                var user = uow.Users.Get(u => u.Login == applicant.Login);
                var usersCourses = new UsersCourses
                {
                    UserId = user.Id,
                    CourseId = courseId
                };

                uow.UsersCourses.Create(usersCourses);
                uow.Save();
            }

            var response = new AddCourseApplicantsResponseModel();

            response.SuccessfullyAddedApplicants = applicantsToAdd;
            response.FailedAddedApplicants = failedApplicantsToAdd;

            return response;
        }

        private void ValidateApplicants(
            List<CreateApplicantModel> applicantsToAdd,
            List<FailedApplicantResponseModel> failedApplicantsToAdd,
            List<CreateApplicantModel> applicants)
        {

            foreach (var applicant in applicants)
            {
                var isExistResponse = userService.CheckIfUserExists(applicant.Login);

                if (isExistResponse.IsFound)
                {
                    applicantsToAdd.Add(applicant);
                }
                else
                {
                    failedApplicantsToAdd.Add(new FailedApplicantResponseModel
                    {
                        ErrorMessage = isExistResponse.ErrorMessage,
                        Login = isExistResponse.Login
                    });

                }
            }
        }
    }
}
