using Microsoft.AspNetCore.Mvc;
using System;
using DiplomaServices.Services.Interfaces;

namespace DiplomaAPI.Controllers
{
    [Route("api/email")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        #region

        private readonly IEmailVerificator emailService;

        #endregion

        public EmailController(IEmailVerificator emailService)
        {
            this.emailService = emailService;
        }


        [HttpGet]
        [Route("confirm-email")]
        public IActionResult ConfirmEmail(string userId, string token)
        {
            try
            {
                emailService.SetEmailAsVerified(Convert.ToInt32(userId));

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
