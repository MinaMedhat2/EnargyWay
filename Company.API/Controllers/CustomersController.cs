using Company.API.Data;
using Company.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetCustomers()
        {
            // We only want to get users with the 'User' role, as they are our customers.
            return await _context.Users
                                 .Where(u => u.Role == Role.User)
                                 .OrderBy(u => u.Username)
                                 .ToListAsync();
        }

        // DELETE: api/customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            // Find the user by their ID
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Ensure we are not deleting an admin or employee by mistake
            if (user.Role != Role.User)
            {
                return BadRequest("Cannot delete non-customer accounts from this endpoint.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
