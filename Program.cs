using Microsoft.EntityFrameworkCore;
using MovieFlix.Api.Data;
using MovieFlix.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURAÇÃO DE SERVIÇOS (Dependency Injection) ---

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Banco de Dados Operacional (SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Serviços de Engenharia de Dados (Devem estar antes do builder.Build())
builder.Services.AddScoped<DataLakeService>();
builder.Services.AddScoped<DataWarehouseService>();

// -----------------------------------------------------------

var app = builder.Build();

// --- 2. CONFIGURAÇÃO DO PIPELINE (Middleware) ---

// Ativa o Swagger em qualquer ambiente para facilitar os teus testes no Docker
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

// --- 3. AUTOMAÇÃO DE STARTUP (Seed -> Data Lake -> Data Warehouse) ---

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var lakeService = services.GetRequiredService<DataLakeService>();
        var dwService = services.GetRequiredService<DataWarehouseService>();

        logger.LogInformation("Iniciando rotina de automação de dados...");

        // Passo A: Popula o SQLite (Base Operacional)
        AppDbContext.SeedData(context);
        logger.LogInformation("[OK] Seed do SQLite concluído.");

        // Passo B: Extract & Transform (Gera CSVs no Data Lake)
        await lakeService.ExportToCsv();
        logger.LogInformation("[OK] Data Lake (CSVs) atualizado.");

        // Passo C: Load (Carrega no PostgreSQL e cria as Views do Data Mart)
        await dwService.LoadAllToPostgres();
        logger.LogInformation("[OK] Data Warehouse e Views analíticas prontos!");

    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Ocorreu um erro na automação de startup. Verifique se o Postgres está pronto.");
    }
}

app.Run();