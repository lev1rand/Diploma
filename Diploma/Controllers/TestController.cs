using DiplomaAPI.Filters;
using DiplomaServices.Interfaces;
using DiplomaServices.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiplomaAPI.Controllers
{
    [Route("api/tests")]
    [TypeFilter(typeof(AuthFilter))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class TestController : ControllerBase
    {
        #region

        private readonly ITestService testService;

        #endregion

        public TestController(ITestService testService)
        {
            this.testService = testService;
        }

        [HttpPost]
        public IActionResult CreateTest(CreateTestModel model)
        {
            try
            {
                return Ok(testService.CreateTest(model));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public IActionResult GetTests()
        {
            try
            {
                return Ok(testService.GetAll());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("answers-processing")]
        public IActionResult SaveAnswers(List<SaveUserAnswersModel> answers)
        {
            try
            {
                var userId = Convert.ToInt32(new string(Encoding.ASCII.GetChars(HttpContext.Session.Get("userId"))));
                testService.ProcessAnswers(answers, userId);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("detailed-test-result")]
        public IActionResult GetDetailedTestResultByStudentId(int studentId)
        {
            return Ok();
        }

        //[HttpGet]
        //[Route("test-result")]
        //public IActionResult GetTestResultsByTestId(int testId)
        //{
        //    return Ok();
        //}

        [HttpGet]
        [Route("for-evaluation")]
        public IActionResult GetTestsPassedForEvaluation(int studentId)
        {
            return Ok();
        }
    }
}
