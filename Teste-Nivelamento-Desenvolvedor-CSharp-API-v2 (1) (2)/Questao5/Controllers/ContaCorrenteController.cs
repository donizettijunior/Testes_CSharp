using Microsoft.AspNetCore.Mvc;
using Questao5.Domain.BO;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Questao5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IContaBancariaBusiness _icontabancariabusiness;

        private readonly ILogger<ContaCorrenteController> _logger;

        public ContaCorrenteController(ILogger<ContaCorrenteController> logger,
            IContaBancariaBusiness icontabancariabusiness)
        {
            _logger = logger;
            _icontabancariabusiness = icontabancariabusiness;
        }

        [HttpGet("{nrocontacorrente}", Name = "GetSaldoContaCorrente")]
        public Retorno Get(string nrocontacorrente)
        {
            return _icontabancariabusiness.ConsultaSaldo(nrocontacorrente);
        }

        [HttpPost(Name = "MovimentoContaCorrente")]
        public Retorno Post([FromForm][Required(ErrorMessage = "O parâmetro 'nrocontacorrente' é obrigatório.")] string nrocontacorrente,
                            [FromForm][Required(ErrorMessage = "O parâmetro 'datamovimento' é obrigatório.")] string datamovimento,
                            [FromForm][Required(ErrorMessage = "O parâmetro 'tipomovimento' é obrigatório.")] TipoMovimento tipomovimento,
                            [FromForm][Required(ErrorMessage = "O parâmetro 'valor' é obrigatório.")] string valor)
        {
            return _icontabancariabusiness.MovimentoContaCorrente(nrocontacorrente, datamovimento, tipomovimento, valor);
        }
    }
}