using Microsoft.AspNetCore.Mvc;
using Produtor.Model;
using Produtor.Services;

namespace Produtor.Controllers
{
    [ApiController]
    [Route("")]
    public class ProdutorController : Controller
    {
        private readonly KafkaService _kafkaService;
        public ProdutorController(KafkaService kafkaService) 
        {
            _kafkaService = kafkaService;
        }
        [HttpPost]
        public IActionResult Post([FromBody] Produto produto)
        {
            _kafkaService.SendMensage(produto);
            Console.WriteLine($"Mensagem enviada: {produto}");
            return Ok("Mensagem recebida com sucesso!");

        }
    }
}
