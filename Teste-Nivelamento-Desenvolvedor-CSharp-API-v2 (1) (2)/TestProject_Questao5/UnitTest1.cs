using NUnit.Framework;
using Questao5.Domain.BO;
using Questao5.Infrastructure.Interfaces;

namespace TestProject_Questao5
{
    [TestFixture]
    public class UnitTest1
    {
        private readonly IContaBancariaBusiness _icontabancariabusiness;

        public UnitTest1(IContaBancariaBusiness icontabancariabusiness)
        {
            _icontabancariabusiness = icontabancariabusiness;
        }

        [Test]
        public void TesteGet_RetornaOk()
        {
            Retorno retorno = _icontabancariabusiness.ConsultaSaldo("123");
            NUnit.Framework.Assert.Equals(200, retorno.status);
        }

        [Test]
        public void TesteGet_RetornaErro()
        {
            Retorno retorno = _icontabancariabusiness.ConsultaSaldo("-1");
            NUnit.Framework.Assert.Equals(400, retorno.status);
        }
    }
}