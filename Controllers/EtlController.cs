using Microsoft.AspNetCore.Mvc;
using MovieFlix.Api.Services;

namespace MovieFlix.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EtlController : ControllerBase
    {
        private readonly DataLakeService _lakeService;
        private readonly DataWarehouseService _dwService;

        public EtlController(DataLakeService lakeService, DataWarehouseService dwService)
        {
            _lakeService = lakeService;
            _dwService = dwService;
        }

        // Endpoint principal que executa o pipeline completo
        [HttpPost("sync-all")]
        public async Task<IActionResult> SyncAll()
        {
            try
            {
                // 1. EXTRAÇÃO: Gera os CSVs brutos no Data Lake a partir do SQLite
                await _lakeService.ExportToCsv();

                // 2. CARGA: Lê os CSVs e carrega no PostgreSQL (DW)
                await _dwService.LoadAllToPostgres();

                return Ok("Sincronização completa: Data Lake atualizado e dados carregados no DW!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro no processo de ETL: {ex.Message}");
            }
        }
    }
}