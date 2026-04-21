using Microsoft.EntityFrameworkCore;
using MovieFlix.Api.Models;

namespace MovieFlix.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Alimenta o banco de dados com dados iniciais para simular o ecossistema de dados.
        /// </summary>
        public static void SeedData(AppDbContext context)
        {
            context.Database.EnsureCreated();

            // 1. Filmes Diversificados (Para o Top 10 por Gênero)
            if (!context.Movies.Any())
            {
                context.Movies.AddRange(
                    new Movie { Title = "Inception", Genre = "Sci-Fi", ReleaseYear = 2010, Director = "Christopher Nolan", ImdbId = "tt1375666" },
                    new Movie { Title = "Interstellar", Genre = "Sci-Fi", ReleaseYear = 2014, Director = "Christopher Nolan", ImdbId = "tt0816692" },
                    new Movie { Title = "The Godfather", Genre = "Crime", ReleaseYear = 1972, Director = "Francis Ford Coppola", ImdbId = "tt0068646" },
                    new Movie { Title = "Pulp Fiction", Genre = "Crime", ReleaseYear = 1994, Director = "Quentin Tarantino", ImdbId = "tt0110912" },
                    new Movie { Title = "The Matrix", Genre = "Action", ReleaseYear = 1999, Director = "Lana Wachowski", ImdbId = "tt0133093" },
                    new Movie { Title = "Mad Max: Fury Road", Genre = "Action", ReleaseYear = 2015, Director = "George Miller", ImdbId = "tt1392190" },
                    new Movie { Title = "Parasite", Genre = "Thriller", ReleaseYear = 2019, Director = "Bong Joon-ho", ImdbId = "tt6751668" },
                    new Movie { Title = "The Dark Knight", Genre = "Action", ReleaseYear = 2008, Director = "Christopher Nolan", ImdbId = "tt0468569" },
                    new Movie { Title = "Spirited Away", Genre = "Animation", ReleaseYear = 2001, Director = "Hayao Miyazaki", ImdbId = "tt0245429" },
                    new Movie { Title = "Toy Story", Genre = "Animation", ReleaseYear = 1995, Director = "John Lasseter", ImdbId = "tt0114709" },
                    new Movie { Title = "Forrest Gump", Genre = "Drama", ReleaseYear = 1994, Director = "Robert Zemeckis", ImdbId = "tt0109830" },
                    new Movie { Title = "The Shawshank Redemption", Genre = "Drama", ReleaseYear = 1994, Director = "Frank Darabont", ImdbId = "tt0111161" }
                );
                context.SaveChanges();
            }

            // 2. Usuários de Diferentes Países e Idades (Para Faixa Etária e País)
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { Name = "Ana (BR)", Age = 22, Country = "Brasil" },
                    new User { Name = "Bruno (BR)", Age = 35, Country = "Brasil" },
                    new User { Name = "Carlos (BR)", Age = 50, Country = "Brasil" },
                    new User { Name = "John (US)", Age = 25, Country = "USA" },
                    new User { Name = "Mary (US)", Age = 48, Country = "USA" },
                    new User { Name = "Sato (JP)", Age = 31, Country = "Japan" },
                    new User { Name = "Elena (IT)", Age = 19, Country = "Italy" },
                    new User { Name = "Lucas (PT)", Age = 27, Country = "Portugal" }
                );
                context.SaveChanges();
            }

            // 3. Avaliações Cruzadas (Para o Data Mart)
            if (!context.Ratings.Any())
            {
                var movies = context.Movies.ToList();
                var users = context.Users.ToList();

                context.Ratings.AddRange(
                    // Inception - Boas notas de jovens e adultos
                    new Rating { MovieId = movies[0].Id, UserId = users[0].Id, Score = 5 },
                    new Rating { MovieId = movies[0].Id, UserId = users[3].Id, Score = 4 },
                    new Rating { MovieId = movies[0].Id, UserId = users[5].Id, Score = 5 },

                    // Godfather - Notas altas de usuários mais velhos
                    new Rating { MovieId = movies[2].Id, UserId = users[2].Id, Score = 5 },
                    new Rating { MovieId = movies[2].Id, UserId = users[4].Id, Score = 5 },

                    // Toy Story - Notas variadas por país
                    new Rating { MovieId = movies[9].Id, UserId = users[0].Id, Score = 4 },
                    new Rating { MovieId = movies[9].Id, UserId = users[3].Id, Score = 5 },
                    new Rating { MovieId = movies[9].Id, UserId = users[7].Id, Score = 3 },

                    // Parasite - Sucesso internacional
                    new Rating { MovieId = movies[6].Id, UserId = users[0].Id, Score = 5 },
                    new Rating { MovieId = movies[6].Id, UserId = users[5].Id, Score = 5 },
                    new Rating { MovieId = movies[6].Id, UserId = users[6].Id, Score = 4 }
                );
                context.SaveChanges();
            }
        }
    }
}