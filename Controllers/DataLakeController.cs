using Microsoft.AspNetCore.Mvc;
using MovieFlix.Api.Services;

namespace MovieFlix.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataLakeController : ControllerBase
    {
        private readonly DataLakeService _service;

        public DataLakeController(DataLakeService service)
        {
            _service = service;
        }

        [HttpPost("export")]
        public async Task<IActionResult> Export()
        {
            await _service.ExportToCsv();
            return Ok("Arquivos CSV gerados com sucesso no diretório /app/DataLake");
        }
    }
}