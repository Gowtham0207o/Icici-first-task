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
        private readonly ILogger<FormController> _logger;
        private readonly AppDbContext _context;

        public FormController(AppDbContext context , ILogger<FormController> logger)
        {
            _logger = logger;
            _context = context;
        }

        // POST: api/form
        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] FormModel model)
        {
            if (model == null)
            {
                _logger.LogWarning("Form submission failed: Received null model");
                return BadRequest("Invalid data");
            }

            try
            {
                _context.FormSubmissions.Add(model);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Form submitted successfully with ID {FormId} by {Email}", model.Id, model.Email);
                return Ok(new { message = "Form submitted and saved to database" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while saving form for {Email}", model.Email);
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/form
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all form submissions...");
                var submissions = await _context.FormSubmissions
                    .OrderByDescending(s => s.Id)
                    .ToListAsync();

                _logger.LogInformation("Retrieved {Count} submissions", submissions.Count);
                return Ok(submissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving all submissions");
                return StatusCode(500, "Error retrieving data");
            }
        }

        // GET: api/form/all
        [HttpGet("all")]
        public async Task<IActionResult> GetWithPath()
        {
            try
            {
                _logger.LogInformation("Fetching all form submissions via /all endpoint...");
                var submissions = await _context.FormSubmissions
                    .OrderByDescending(s => s.Id)
                    .ToListAsync();

                _logger.LogInformation("Retrieved {Count} submissions", submissions.Count);
                return Ok(submissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving submissions from /all endpoint");
                return StatusCode(500, "Error retrieving data");
            }
        }

        // GET: api/form/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching submission with ID {Id}", id);

            var submission = await _context.FormSubmissions.FindAsync(id);
            if (submission == null)
            {
                _logger.LogWarning("Submission with ID {Id} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully retrieved submission with ID {Id}", id);
            return Ok(submission);
        }

        // PUT: api/form/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FormModel updatedUser)
        {
            if (updatedUser == null || id != updatedUser.Id)
            {
                _logger.LogWarning("Update failed: Invalid data for ID {Id}", id);
                return BadRequest("Invalid data");
            }

            var existingUser = await _context.FormSubmissions.FindAsync(id);
            if (existingUser == null)
            {
                _logger.LogWarning("Update failed: No submission found with ID {Id}", id);
                return NotFound();
            }

            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.Email = updatedUser.Email;
            existingUser.Phone = updatedUser.Phone;
            existingUser.City = updatedUser.City;
            existingUser.State = updatedUser.State;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Updated submission with ID {Id}", id);
                return Ok(existingUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating submission with ID {Id}", id);
                return StatusCode(500, "Error updating user");
            }
        }

        // DELETE: api/form/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Attempting to delete submission with ID {Id}", id);

            var user = await _context.FormSubmissions.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning("Delete failed: No submission found with ID {Id}", id);
                return NotFound();
            }

            try
            {
                _context.FormSubmissions.Remove(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Deleted submission with ID {Id}", id);
                return Ok(new { message = "Deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting submission with ID {Id}", id);
                return StatusCode(500, "Error deleting user");
            }
        }
    }
}
