using Questao5.Domain.BO;
using Questao5.Domain.Enumerators;

namespace Questao5.Infrastructure.Interfaces
{
    public interface IContaBancariaBusiness
    {
        Retorno MovimentoContaCorrente(string nrocontacorrente, string datamovimento, TipoMovimento? tipomovimento, string valor);

        Retorno ConsultaSaldo(string nrocontacorrente);
    }
}
