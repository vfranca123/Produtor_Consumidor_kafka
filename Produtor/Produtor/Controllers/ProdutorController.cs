using Microsoft.AspNetCore.Mvc;
using Produtor.Model;
using Produtor.Services;

namespace Produtor.Controllers
{
    [ApiController]
    [Route("")]
    public class ProdutorController : ControllerBase
    {
        private readonly KsqlService _KsqlService;

        public ProdutorController(KsqlService KsqlService)
        {
            _KsqlService = KsqlService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Produto produto)
        {
            string respose = await _KsqlService.SendMessage(produto);
            return Ok(new { message = respose, produto });
        }
    }
}
