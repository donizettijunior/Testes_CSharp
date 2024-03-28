using Questao5.Domain.BO;
using Questao5.Domain.Enumerators;

namespace Questao5.Infrastructure.Interfaces
{
    public interface IContaBancariaService
    {
        string GetIdContaCorrente(string nrocontacorrente);

        bool GetStatusContaCorrente(string nrocontacorrente);

        Retorno MovimentoContaCorrente(string nrocontacorrente, DateTime? datamovimento, TipoMovimento? tipomovimento, float? valor);

        Retorno ConsultaSaldo(string nrocontacorrente);
    }
}
