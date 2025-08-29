using Microsoft.AspNetCore.Mvc;
using Produtor.Model;
using Produtor.Services;

namespace Produtor.Controllers
{
    [ApiController]
    [Route("")]
    public class ProdutorController : ControllerBase
    {
        private readonly KafkaService _kafkaService;

        public ProdutorController(KafkaService kafkaService)
        {
            _kafkaService = kafkaService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Produto produto)
        {
            string respose = await _kafkaService.SendMessage(produto);
            return Ok(new { message = respose, produto });
        }
    }
}
