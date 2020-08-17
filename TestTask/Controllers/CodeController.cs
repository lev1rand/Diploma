using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace TestTaskAPI.Controllers
{
    [Route("api/codes")]
    [ApiController]
    public class CodeController : ControllerBase
    {
        IUnitOfWork uow;
        public CodeController(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        // POST: api/codes
        [HttpPost]
        public IActionResult AddCode(Code added)
        {
            try
            {
                uow.Codes.Create(added);
                uow.Save();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public IActionResult GetAllCodes()
        {
            try
            {
                return Ok(uow.Codes.GetAll());
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
