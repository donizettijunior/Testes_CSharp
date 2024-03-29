using Questao5.Domain.BO;
using Questao5.Infrastructure.Business;

namespace Questao5_Test
{
    public class UnitTest_ContaCorrente
    {
        private readonly ContaBancariaBusiness _contabancariabusiness = new();

        [Fact]
        public void TesteGet_RetornaOk()
        {
            Retorno retorno = _contabancariabusiness.ConsultaSaldo("123");
            Assert.Equal(200, retorno.status);
        }

        [Fact]
        public void TesteGet_RetornaErro()
        {
            Retorno retorno = _contabancariabusiness.ConsultaSaldo("-1");
            Assert.Equal(400, retorno.status);
        }
    }
}