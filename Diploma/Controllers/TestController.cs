﻿using DiplomaAPI.Filters;
using DiplomaServices.Interfaces;
using DiplomaServices.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DiplomaAPI.Controllers
{
    [Route("api/tests")]
    // [ExceptionFilter]
    [TypeFilter(typeof(AuthFilter))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [EnableCors]
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
        public IActionResult GetTests([FromQuery] AuthTemplateModel model)
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
        [Route("test-results-savage")]
        public IActionResult SaveStudentTestResults(SavePassedTestResultsModel testResult)
        {
            try
            {
                testService.ProcessTestResultSaving(testResult);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("test-being-passed")]
        public IActionResult GetTestForPassing([FromQuery] AuthTemplateModel model, int testId)
        {
            try
            {
                var response = testService.GetTestForStudentPassing(model.SessionId, testId);

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("test-result-for-evaluation")]
        public IActionResult GetTestForEvaluation([FromQuery] AuthTemplateModel model, int testId, int studentId)
        {
            try
            {
                var response = testService.GetTestForEvaluation(model.SessionId, testId, studentId);

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("save-test-result-after-evaluation")]
        public IActionResult SaveTestAfterEvaluation(SaveTestAfterEvaluationModel model)
        {
            try
            {
                testService.SaveTestAfterEvaluation(model);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("test-result-fully-evaluated")]
        public IActionResult GetTestResultFullyEvaluated([FromQuery] AuthTemplateModel model, int testId, int studentId)
        {
            try
            {
                 var response = testService.GetTestResultFullyEvaluated(studentId, testId);

                 return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        [Route("detailed-for-student")]
        public IActionResult GetDetailedTestsListByStudentId([FromQuery] AuthTemplateModel model, int studentId)
        {
            try
            {
                var response = testService.GetDetailedTestsListByStudentId(studentId);

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //[HttpGet]
        //[Route("test-result")]
        //public IActionResult GetTestResultsByTestId(int testId)
        //{
        //    return Ok();
        //}

        /* [HttpGet]
         [Route("for-evaluation")]
         public IActionResult GetTestsPassedForEvaluation(int studentId)
         {
             return Ok();
         }*/
    }
}
