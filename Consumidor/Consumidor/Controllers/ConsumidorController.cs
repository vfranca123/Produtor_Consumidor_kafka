using Consumidor.Model;
using Consumidor.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;


namespace Consumidor.Controllers
{
    [Route("")]
    [ApiController]
    public class ConsumidorController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly KsqlConsumerService _ksqlConsumerService;
        public ConsumidorController(HttpClient httpClient , KsqlConsumerService consumerService)
        {
            _ksqlConsumerService = consumerService;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:8088");
        }

        [HttpPost]
        public async Task<IActionResult> PostStream([FromBody] StreamRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.NomeStream) || string.IsNullOrWhiteSpace(request.Filtros))
            {
                return BadRequest(new { status = 400, message = "Nome da stream e filtro são obrigatórios." });
            }

            var ksqlCommand = new
            {
                ksql = $"CREATE STREAM {request.NomeStream} " +
                       $"WITH (KAFKA_TOPIC='{request.KafkaTopic}', VALUE_FORMAT='JSON') AS " +
                       $"SELECT * FROM produtos WHERE {request.Filtros};",
                streamsProperties = new { }
            };

            var content = new StringContent(JsonSerializer.Serialize(ksqlCommand), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/ksql", content, cancellationToken);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                // Inicia a push query para consumir os eventos filtrados
                //await _ksqlConsumerService.ExecutePushQueryAsync(request.NomeStream, cancellationToken);
                return Ok(new { status = 200, message = "Stream derivada criada com sucesso!", ksqlResult = result });
            }
            else
            {
                return StatusCode((int)response.StatusCode, new { status = response.StatusCode, message = "Erro ao criar stream", ksqlResult = result });
            }
        }
    }
}
