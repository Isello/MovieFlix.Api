using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieFlix.Api.Data;
using MovieFlix.Api.Models;

namespace MovieFlix.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/users
        // Útil para verificar os dados que serão exportados para o users.csv
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users
                .Include(u => u.Ratings) // Inclui as avaliações que o utilizador já fez
                .ToListAsync();
        }

        // POST: api/users
        // Atende ao requisito de permitir o cadastro no ecossistema MovieFlix
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Retorna o utilizador criado com o ID gerado automaticamente pelo banco
            return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
        }
    }
}