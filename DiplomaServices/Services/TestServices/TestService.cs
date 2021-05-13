using DataAccess;
using DataAccess.Entities.Test;
using DiplomaServices.Interfaces;
using DiplomaServices.Mapping;
using DiplomaServices.Models;
using System;

namespace DiplomaServices.Services.TestServices
{
    public class TestService : ITestService
    {
        #region private members

        private readonly IUnitOfWork uow;

        private readonly ICourseService courseService;

        //private readonly IQuestionsService questionsService;

        private readonly IEmailService emailService;

        private readonly IUserService userService;

        private readonly MapperService mapper;

        #endregion

        public TestService(IUnitOfWork uow, 
            ICourseService courseService,
            //IQuestionsService questionsService,
            IEmailService emailService,
            IUserService userService)
        {
            this.uow = uow;
            this.courseService = courseService;
           // this.questionsService = questionsService;
            this.emailService = emailService;
            this.userService = userService;

            mapper = new MapperService();
        }
        public CreateTestModel CreateTest(CreateTestModel model)
        {
            var test = new Test();

            //Bind Course to Test
            if (courseService.CheckIfCourseExists(model.CourseId.Value))
            {
                test.CourseId = model.CourseId.Value;
            }
            else
            {
                throw new Exception(string.Format("Course with id {0} doesn't exist!", model.CourseId));
            }

            //Create Questions
            if (model.Questions.Count > 0)
            {
                foreach (var question in model.Questions)
                {
                   // questionsService.CreateQuestion(question);
                }
            }
           // else
           // {
           //     throw new Exception("Test doesn't contain questions!");
           // }

            //Add Applicants
            //if (model.Applicants.Count > 0)
            //{
                //AddApplicants(model.Applicants);
           // }

            //Notify Applicants about Test
            if (model.Applicants.Count > 0)
            {
                foreach (var applicant in model.Applicants)
                {
                    var isExistResponse = userService.CheckIfUserExists(applicant.Login);

                    if (isExistResponse.IsFound)
                    {
                        string messageText = string.Format("You have been subscribed to the test in the TestOn system, which will be hold on {0}. " +
                            "Test theme: {1}.", model.DateTime, model.Theme);

                        emailService.SendMessage(applicant.Login, messageText, null);
                    }
                   
                }
                
            }

            return null;
        }

        private void AddApplicants()
        {

        }

        /*public IEnumerable<UserModel> GetWithPagination()
        {
            return mapper.Map<IQueryable<User>, IEnumerable<UserModel>>(uow.Users.GetAll()).ToList();
        }*/
    }
}
