using Microsoft.AspNetCore.Mvc;
using System;
using TestTaskServices.Models;
using TestTaskServices.Services;
using TestTaskServices.Services.Interfaces;

namespace TestTaskAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region

        private readonly IAccountService accountService;

        private readonly IEmailVerificator emailService;

        private readonly MapperService mapper;

        #endregion

        public AccountController(IAccountService accountService, IEmailVerificator emailService)
        {
            this.accountService = accountService;
            this.emailService = emailService;

            mapper = new MapperService();
        }
        // POST: api/account
        [HttpPost]
        public IActionResult CreateAccount(CreateAccountModel added)
        {
            try
            {
                var response = accountService.CreateAccount(added);

                //verifying email
                var confirmationLink = Url.Action("ConfirmEmail", "Email",
                    new
                    {
                        userId = response,
                        token = emailService.GenerateConfirmationToken()
                    },
                    Request.Scheme);

                emailService.SetConfirmationLink(confirmationLink);
                emailService.SendVerificationMessage(added.Login);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/account
       /* [HttpGet]
        public IActionResult GetAllAccounts()
        {
            try
            {
                return Ok(accountService.GetAll());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }*/
    }
}
