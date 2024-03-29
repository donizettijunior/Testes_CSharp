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
        public Retorno Post([FromForm][Required(ErrorMessage = "O par�metro 'idrequisicao' � obrigat�rio.")] string idrequisicao,
                            [FromForm][Required(ErrorMessage = "O par�metro 'nrocontacorrente' � obrigat�rio.")] string nrocontacorrente,
                            [FromForm][Required(ErrorMessage = "O par�metro 'tipomovimento' � obrigat�rio.")] TipoMovimento tipomovimento,
                            [FromForm][Required(ErrorMessage = "O par�metro 'valor' � obrigat�rio.")] string valor)
        {
            return _icontabancariabusiness.MovimentoContaCorrente(idrequisicao, nrocontacorrente, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), tipomovimento, valor);
        }
    }
}