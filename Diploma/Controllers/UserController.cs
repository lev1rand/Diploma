using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using DiplomaAPI.Filters;
using DiplomaServices.Interfaces;
using DiplomaServices.Pagination;
using Microsoft.AspNetCore.Cors;

namespace DiplomaAPI.Controllers
{
    [Route("api/users")]
    [TypeFilter(typeof(AuthFilter))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [EnableCors]
    public class UserController : ControllerBase
    {
        #region

        private readonly IUserService userService;

        #endregion

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        // GET: api/users
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                return Ok(userService.GetAll());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("paginated")]
        public IActionResult GetUsersPaginated([FromQuery] PaginationFilter filter)
        {
            try
            {
                var response = userService.GetUsersPaginated(filter);

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
