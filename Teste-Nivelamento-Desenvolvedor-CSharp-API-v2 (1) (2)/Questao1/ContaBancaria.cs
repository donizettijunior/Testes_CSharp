namespace Questao1
{
    public class ContaBancaria {

        public int Numero { get; set; }
        public string Titular { get; set; }
        public double? Saldo { get; set; }

        //*** Taxa fixa de saque
        private readonly double Taxafixa = 3.5;

        public ContaBancaria(int numero, string titular, double? depositoInicial)
        {
            Numero = numero;
            Titular = titular;
            Saldo = depositoInicial;
        }

        public void Deposito(double quantia)
        {
            Saldo += quantia;
        }

        public void Saque(double quantia)
        {
            Saldo -= quantia;
            Saldo -= Taxafixa;
        }
    }
}
