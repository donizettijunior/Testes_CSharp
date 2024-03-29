using Questao5.Domain.BO;
using Questao5.Domain.Enumerators;

namespace Questao5.Infrastructure.Interfaces
{
    public interface IContaBancariaService
    {
        string GetIdContaCorrente(string nrocontacorrente);

        bool GetStatusContaCorrente(string nrocontacorrente);

        string GetChaveIdempotencia(string idrequisicao);

        string GetResultadoIdempotencia(string idrequisicao);

        string GetRequisicaoIdempotencia(string idrequisicao);

        bool SetIdempotencia(string requisicao);

        Retorno MovimentoContaCorrente(string idrequisicao, string nrocontacorrente, DateTime? datamovimento, TipoMovimento? tipomovimento, float? valor);

        Retorno ConsultaSaldo(string nrocontacorrente);
    }
}
