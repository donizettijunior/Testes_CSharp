using Questao5.Domain.BO;
using Questao5.Infrastructure.Interfaces;

namespace TestProject_Questao5
{
    [TestClass]
    public class UnitTest1
    {
        private readonly IContaBancariaBusiness _icontabancariabusiness;

        public UnitTest1(IContaBancariaBusiness icontabancariabusiness)
        {
            _icontabancariabusiness = icontabancariabusiness;
        }

        [TestMethod]
        public void TesteGet_RetornaOk()
        {
            Retorno retorno = _icontabancariabusiness.ConsultaSaldo("123");
            Assert.Equals(200, retorno.status);
        }

        [TestMethod]
        public void TesteGet_RetornaErro()
        {
            Retorno retorno = _icontabancariabusiness.ConsultaSaldo("-1");
            Assert.Equals(400, retorno.status);
        }
    }
}