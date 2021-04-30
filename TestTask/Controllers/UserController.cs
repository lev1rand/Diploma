using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using TestTaskServices.Services.Interfaces;

namespace TestTaskAPI.Controllers
{
    [Route("api/users")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
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
    }
}
