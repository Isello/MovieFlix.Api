using System.Text.Json.Serialization;

namespace MovieFlix.Api.Models
{
    public class User
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        // Propriedade para atender ao requisito de informações de usuários (ex: país, idade)
        public string Country { get; set; } = "Brasil";
        public int Age { get; set; }

        // Lista de avaliações que o usuário já fez
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}