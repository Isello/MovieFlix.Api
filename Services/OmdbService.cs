using MovieFlix.Api.Data;
using MovieFlix.Api.Models;
using System.Text.Json;

namespace MovieFlix.Api.Services
{
    public class OmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _context;
        private readonly string _apiKey = "220bfd95";

        public OmdbService(HttpClient httpClient, AppDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public async Task<Movie?> FetchAndSaveMovieAsync(string imdbId)
        {
            var response = await _httpClient.GetAsync($"http://www.omdbapi.com/?i={imdbId}&apikey={_apiKey}");
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var omdbMovie = JsonSerializer.Deserialize<OmdbMovieDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (omdbMovie == null || string.IsNullOrEmpty(omdbMovie.Title)) return null;

            // Converter o ano (remove caracteres não numéricos caso venham da API)
            int releaseYear = 0;
            if (!string.IsNullOrEmpty(omdbMovie.Year) && omdbMovie.Year.Length >= 4)
            {
                int.TryParse(omdbMovie.Year.Substring(0, 4), out releaseYear);
            }

            var movie = new Movie
            {
                Title = omdbMovie.Title,
                Genre = omdbMovie.Genre,
                ReleaseYear = releaseYear,
                ImdbId = omdbMovie.imdbID,    // Adicione esta linha
                Director = omdbMovie.Director // Adicione esta linha
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return movie;
        }
    }
}