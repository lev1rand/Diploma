using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using DiplomaServices.Models;
using DiplomaServices.Interfaces;

namespace DiplomaAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
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

                HttpContext.Session.Set("refreshToken", Encoding.ASCII.GetBytes(response.RefreshToken));
                HttpContext.Session.Set("accessToken", Encoding.ASCII.GetBytes(response.AccessToken));
                HttpContext.Session.Set("userLogin", Encoding.ASCII.GetBytes(response.UserLogin));
                HttpContext.Session.Set("userName", Encoding.ASCII.GetBytes(response.UserName));
                HttpContext.Session.Set("userId", Encoding.ASCII.GetBytes(response.UserId));

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
                var userId = Convert.ToInt32(new string(Encoding.ASCII.GetChars(HttpContext.Session.Get("userId"))));
                signOut.UserId = userId;
                authService.SignOut(signOut);

                HttpContext.Session.Clear();

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
                if (HttpContext.Session.Get("refreshToken") != null)
                {
                    var incomingRefresh = model.RefreshToken;
                    var refreshFromSession = new string(Encoding.ASCII.GetChars(HttpContext.Session.Get("refreshToken")));

                    if (refreshFromSession == incomingRefresh)
                    {
                        var accessToken = authService.RefreshAccessToken();
                        HttpContext.Session.Set("accessToken", Encoding.ASCII.GetBytes(accessToken));

                        return Ok(new { AccessToken = accessToken });
                    }
                    else
                    {
                        //We can't give new access if refresh token wasn't found

                        return BadRequest("Your refresh token isn't right, access denied");
                    }
                }

                return BadRequest("Your session has expired. Please, re-login to the system.");

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
