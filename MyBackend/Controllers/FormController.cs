using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBackend.Data;
using MyBackend.Models;
using System.Linq;

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

        // CREATE (POST)
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

        // READ ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var submissions = await _context.FormSubmissions
                    .OrderByDescending(s => s.Id)
                    .ToListAsync();

                return Ok(submissions);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return StatusCode(500, "Error retrieving data");
            }
        }
        // READ ALL
[HttpGet("all")]
public async Task<IActionResult> GetWithPath()
{
    try
    {
        var submissions = await _context.FormSubmissions
            .OrderByDescending(s => s.Id)
            .ToListAsync();

        return Ok(submissions);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine(ex);
        return StatusCode(500, "Error retrieving data");
    }
}


        // READ BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var submission = await _context.FormSubmissions.FindAsync(id);
            if (submission == null)
                return NotFound();

            return Ok(submission);
        }

        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FormModel updatedUser)
        {
            if (updatedUser == null || id != updatedUser.Id)
                return BadRequest("Invalid data");

            var existingUser = await _context.FormSubmissions.FindAsync(id);
            if (existingUser == null)
                return NotFound();

            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.Email = updatedUser.Email;
            existingUser.Phone = updatedUser.Phone;
            existingUser.City = updatedUser.City;
            existingUser.State = updatedUser.State;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(existingUser);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return StatusCode(500, "Error updating user");
            }
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.FormSubmissions.FindAsync(id);
            if (user == null)
                return NotFound();

            try
            {
                _context.FormSubmissions.Remove(user);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Deleted successfully" });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return StatusCode(500, "Error deleting user");
            }
        }
    }
}
