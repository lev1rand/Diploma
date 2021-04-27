using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTaskAPI.Models;
using TestTaskServices.Models;
using TestTaskServices.Services;
using TestTaskServices.Services.Interfaces;

namespace TestTaskAPI.Controllers
{
    public class AuthController : ControllerBase
    {
        #region

        private readonly IAuthService authService;
        private readonly MapperService mapper;

        #endregion

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }


        [HttpPost]
        public IActionResult SignIn(Models.SignInModel signIn)
        {
            try
            {
                return Ok(authService.SignIn(mapper.Map<Models.SignInModel, TestTaskServices.Models.SignInModel>(signIn)));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IActionResult SignOut(Models.SignOutModel signOut)
        {
            try
            {
                authService.SignOut(mapper.Map<Models.SignOutModel, TestTaskServices.Models.SignOutModel>(signOut));

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IActionResult Refresh(RefreshModel refreshToken)
        {
            try
            {
               // authService.(mapper.Map<Models.SignOutModel, TestTaskServices.Models.SignOutModel>(signOut));

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
