using System.Text.Json.Serialization;

namespace MovieFlix.Api.Models
{
    public class Rating
    {
        [JsonIgnore] // Esconde do Swagger e do JSON de entrada/saída
        public int Id { get; set; } // O banco gera automático
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public int Score { get; set; }

        [JsonIgnore] // Esconde do Swagger e do JSON de entrada/saída
        public Movie? Movie { get; set; }
        [JsonIgnore]
        public User? User { get; set; } // Propriedade de navegação para o Usuário
    }
}