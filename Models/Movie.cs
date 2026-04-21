namespace MovieFlix.Api.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        
        // Adicione estas duas linhas abaixo:
        public string? ImdbId { get; set; }
        public string? Director { get; set; }

        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}