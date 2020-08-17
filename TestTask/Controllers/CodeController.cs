using Microsoft.AspNetCore.Mvc;
using TestTaskServices.Interfaces;
using TestTaskServices.Models;

namespace TestTaskAPI.Controllers
{
    [Route("api/codes")]
    [ApiController]
    public class CodeController : ControllerBase
    {
        #region

        private readonly ICodeService codeService;

        #endregion

        public CodeController(ICodeService codeService)
        {
            this.codeService = codeService;
        }
        // POST: api/codes
        [HttpPost]
        public IActionResult AddCode(CreateCodeModel added)
        {
            try
            {
                codeService.Create(added);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        // GET: api/codes
        [HttpGet]
        public IActionResult GetAllCodes()
        {
            try
            {
                return Ok(codeService.GetAll());
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
