using Questao5.Domain.BO;
using Questao5.Infrastructure.Business;

namespace Questao5_Test
{
    public class UnitTest_ContaCorrente
    {
        private readonly ContaBancariaBusiness _contabancariabusiness;

        public UnitTest_ContaCorrente()
        {
            _contabancariabusiness = new();
        }

        [Fact]
        public void TesteGet_RetornaSaldoOk()
        {
            Retorno retorno = _contabancariabusiness.ConsultaSaldo("123");
            Assert.Equal(200, retorno.status);
        }

        [Fact]
        public void TesteGet_RetornaSaldoErro()
        {
            Retorno retorno = _contabancariabusiness.ConsultaSaldo("963");
            Assert.Equal(400, retorno.status);
        }

        [Fact]
        public void TesteMovimentoContaCorrenteOK()
        {
            string hashid = Guid.NewGuid().ToString("D").ToUpper();
            Retorno retorno = _contabancariabusiness.MovimentoContaCorrente(hashid, "123", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), Questao5.Domain.Enumerators.TipoMovimento.C, "0");
            Assert.Equal(200, retorno.status);
        }

        [Fact]
        public void TesteMovimentoContaCorrenteErro()
        {
            string hashid = Guid.NewGuid().ToString("D").ToUpper();
            Retorno retorno = _contabancariabusiness.MovimentoContaCorrente(hashid, "963", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), Questao5.Domain.Enumerators.TipoMovimento.C, "A");
            Assert.Equal(400, retorno.status);
        }

    }
}