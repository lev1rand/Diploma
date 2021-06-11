using DiplomaAPI.Filters;
using DiplomaServices.Interfaces;
using DiplomaServices.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DiplomaAPI.Controllers
{
    [Route("api/courses")]
    [TypeFilter(typeof(AuthFilter))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [EnableCors]
    public class CourseController : ControllerBase
    {
        #region

        private readonly ICourseService courseService;

        #endregion

        public CourseController(ICourseService courseService)
        {
            this.courseService = courseService;
        }

        [HttpPost]
        public IActionResult CreateCourse([FromBody]CreateCourseModel model)
        {
            try
            {
                return Ok(courseService.CreateCourse(model));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public IActionResult GetCourses([FromQuery]AuthTemplateModel model)
        {
            try
            {
                return Ok(courseService.GetAll());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
