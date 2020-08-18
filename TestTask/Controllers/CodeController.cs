using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
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
            catch (Exception e)
            {
                    return BadRequest(e.Message);
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
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            //PATCH: api/codes/5
            [HttpPatch("{id}")]
            public IActionResult UpdateCode(int id, [FromBody] JsonPatchDocument<UpdateCodeModel> item)
            {
                try
                {
                    codeService.UpdatePatch(id, item);

                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
    }
}
