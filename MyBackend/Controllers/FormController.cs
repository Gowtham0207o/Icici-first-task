using Microsoft.AspNetCore.Mvc;
using MyBackend.Data;
using MyBackend.Models;

namespace MyBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FormController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] FormModel model)
        {
            if (model == null)
                return BadRequest("Invalid data");

            try
            {
                _context.FormSubmissions.Add(model);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Form submitted and saved to database" });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var submissions = _context.FormSubmissions.ToList();
            return Ok(submissions);
        }
    }
}
