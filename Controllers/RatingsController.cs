using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieFlix.Api.Data;
using MovieFlix.Api.Models;

namespace MovieFlix.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RatingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ratings
        // Esse método permite visualizar todas as avaliações para validar o fluxo de dados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rating>>> GetRatings()
        {
            return await _context.Ratings.ToListAsync();
        }

        // POST: api/ratings
        [HttpPost]
        public async Task<ActionResult<Rating>> PostRating(Rating rating)
        {
            // Atende ao requisito de permitir avaliar filmes com notas [cite: 14]
            var movieExists = await _context.Movies.AnyAsync(m => m.Id == rating.MovieId);
            
            if (!movieExists)
            {
                return NotFound("Filme não encontrado.");
            }

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            return Ok(rating);
        }
    }
}