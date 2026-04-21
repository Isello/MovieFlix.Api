namespace MovieFlix.Api.Models
{
    public class OmdbMovieDto
    {
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        
        // Adicione estas duas linhas:
        public string imdbID { get; set; } = string.Empty;
        public string Director { get; set; } = string.Empty;
    }
}