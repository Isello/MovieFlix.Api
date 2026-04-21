using System.Text;
using MovieFlix.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace MovieFlix.Api.Services
{
    public class DataLakeService
    {
        private readonly AppDbContext _context;
        private readonly string _path = "/app/DataLake";

        public DataLakeService(AppDbContext context)
        {
            _context = context;
            if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);
        }

        public async Task ExportToCsv()
        {
            // 1. Exportar Movies
            var movies = await _context.Movies.ToListAsync();
            var movieCsv = new StringBuilder();
            movieCsv.AppendLine("Id;Title;Genre;ReleaseYear;Director;ImdbId");
            foreach (var m in movies) 
                movieCsv.AppendLine($"{m.Id};{m.Title};{m.Genre};{m.ReleaseYear};{m.Director};{m.ImdbId}");
            await File.WriteAllTextAsync(Path.Combine(_path, "movies.csv"), movieCsv.ToString(), Encoding.UTF8);

            // 2. Exportar Users
            var users = await _context.Users.ToListAsync();
            var userCsv = new StringBuilder();
            userCsv.AppendLine("Id;Name;Age;Country");
            foreach (var u in users) 
                userCsv.AppendLine($"{u.Id};{u.Name};{u.Age};{u.Country}");
            await File.WriteAllTextAsync(Path.Combine(_path, "users.csv"), userCsv.ToString(), Encoding.UTF8);

            // 3. Exportar Ratings
            var ratings = await _context.Ratings.ToListAsync();
            var ratingCsv = new StringBuilder();
            ratingCsv.AppendLine("Id;MovieId;UserId;Score");
            foreach (var r in ratings) 
                ratingCsv.AppendLine($"{r.Id};{r.MovieId};{r.UserId};{r.Score}");
            await File.WriteAllTextAsync(Path.Combine(_path, "ratings.csv"), ratingCsv.ToString(), Encoding.UTF8);
        }
    }
}