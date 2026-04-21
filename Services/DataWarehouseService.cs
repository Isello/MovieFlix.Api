using Npgsql;
using System.Text;

namespace MovieFlix.Api.Services
{
    public class DataWarehouseService
    {
        private readonly IConfiguration _config;
        private readonly string _lakePath = "/app/DataLake";

        public DataWarehouseService(IConfiguration config)
        {
            _config = config;
        }

        public async Task LoadAllToPostgres()
        {
            var connectionString = _config.GetConnectionString("DWConnection");
            using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();

            // 1. Prepara o terreno (Cria tabelas e limpa dados)
            await PrepareDatabaseSchema(conn);

            // 2. Carrega os dados brutos do Data Lake
            await ProcessLoad(conn, "movies.csv", "dw_movies");
            await ProcessLoad(conn, "users.csv", "dw_users");
            await ProcessLoad(conn, "ratings.csv", "dw_ratings");

            // 3. CRIA AS VISÕES (Isso resolve o seu erro no terminal!)
            await CreateDataMartViews(conn);
            
            Console.WriteLine("[ETL] Processo concluído com sucesso.");
        }

        private async Task PrepareDatabaseSchema(NpgsqlConnection conn)
        {
            var sql = @"
                CREATE TABLE IF NOT EXISTS dw_movies (id INT PRIMARY KEY, title TEXT, genre TEXT, release_year INT, director TEXT, imdb_id TEXT);
                CREATE TABLE IF NOT EXISTS dw_users (id INT PRIMARY KEY, name TEXT, age INT, country TEXT);
                CREATE TABLE IF NOT EXISTS dw_ratings (id SERIAL PRIMARY KEY, movie_id INT, user_id INT, score INT);
                TRUNCATE dw_movies, dw_users, dw_ratings CASCADE;";
            
            using var cmd = new NpgsqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync();
        }

        private async Task ProcessLoad(NpgsqlConnection conn, string csvName, string tableName)
        {
            string filePath = Path.Combine(_lakePath, csvName);
            if (!File.Exists(filePath)) return;

            using (var writer = await conn.BeginTextImportAsync($"COPY {tableName} FROM STDIN (FORMAT CSV, DELIMITER ';', HEADER)"))
            {
                using (var reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                        if (!string.IsNullOrWhiteSpace(line)) await writer.WriteLineAsync(line);
                    }
                }
            }
        }

        private async Task CreateDataMartViews(NpgsqlConnection conn)
        {
            // Aqui definimos as regras de negócio solicitadas
            var sql = @"
                -- View 1: Top 10 filmes
                CREATE OR REPLACE VIEW view_top_10_filmes AS
                SELECT m.title, m.genre, ROUND(AVG(r.score), 2) as media
                FROM dw_movies m JOIN dw_ratings r ON m.id = r.movie_id
                GROUP BY m.title, m.genre ORDER BY media DESC LIMIT 10;

                -- View 2: Média por idade
                CREATE OR REPLACE VIEW view_media_etaria AS
                SELECT 
                    CASE WHEN age <= 25 THEN 'Jovem' WHEN age <= 50 THEN 'Adulto' ELSE 'Sênior' END as faixa,
                    ROUND(AVG(score), 2) as media_nota
                FROM dw_users u JOIN dw_ratings r ON u.id = r.user_id
                GROUP BY faixa;

                -- View 3: Votos por País
                CREATE OR REPLACE VIEW view_votos_pais AS
                SELECT country, COUNT(*) as total
                FROM dw_users u JOIN dw_ratings r ON u.id = r.user_id
                GROUP BY country;";

            using var cmd = new NpgsqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}