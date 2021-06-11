using Microsoft.AspNetCore.Mvc;
using System;
using DiplomaServices.Models;
using DiplomaServices.Interfaces;
using Microsoft.AspNetCore.Cors;

namespace DiplomaAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [EnableCors]
    public class AuthController : ControllerBase
    {
        #region

        private readonly IAuthService authService;

        #endregion

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }


        [HttpPost]
        [Route("signin")]
        public IActionResult SignIn(SignInModel signIn)
        {
            try
            {
                var response = authService.SignIn(signIn);

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("signout")]
        public IActionResult SignOut(SignOutModel signOut)
        {
            try
            {
                authService.SignOut(signOut);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh(RefreshModel model)
        {
            try
            {
                var accessToken = authService.RefreshAccessToken(model);

                return Ok(new { AccessToken = accessToken });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
