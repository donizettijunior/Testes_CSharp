using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Questao1
{
    class Program {

        static void Main(string[] args) {

            List<ContaBancaria> _listaContas = new();
            int numero;
            string titular;
            char resp;
            double depositoInicial;
            ContaBancaria conta;

            string opcaoselecionada = MontarMenu();

            while (true) {

                //*** Mantém o menu até o usuário selecionar uma opção correta
                while (!ValidaOpcao(opcaoselecionada))
                {
                    opcaoselecionada = MontarMenu();
                }

                //*** Saída para opção de leitura 0
                if (int.Parse(opcaoselecionada) == 0)
                    return;

                Console.WriteLine("");
                if (int.TryParse(opcaoselecionada, out int vlropcao))
                {
                    switch (vlropcao)
                    {
                        case 1: //*** Incluir Nova Conta
                            {

                                numero = GetNumeroConta();

                                conta = _listaContas.FirstOrDefault(x => x.Numero == numero);

                                if (conta == null)
                                {
                                    Console.Write("Entre com o titular da conta: ");
                                    titular = Console.ReadLine();

                                    try
                                    {
                                        Console.Write("Haverá depósito inicial (s/n)? ");
                                        resp = char.Parse(Console.ReadLine());

                                        depositoInicial = 0;
                                        if (resp == 's' || resp == 'S')
                                        {
                                            Console.Write("Entre com o valor de depósito inicial: ");
                                            depositoInicial = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                                        }

                                        int cursorTop = Console.CursorTop;
                                        Console.WriteLine("");
                                        Console.SetCursorPosition(0, cursorTop);
                                        Console.Write("Confirma os Dados do Cadastro (S/N)? ");
                                        int cursorLeft = Console.CursorLeft;
                                        Console.SetCursorPosition(cursorLeft, cursorTop);
                                        string opc = Console.ReadLine();

                                        Console.SetCursorPosition(0, cursorTop);
                                        Console.Write(new string(' ', 50));

                                        if ((opc != null) && (opc.ToUpper() == "S"))
                                        {
                                            _listaContas.Add(new ContaBancaria(numero, titular, depositoInicial));
                                            MensagemAlerta("Conta Cadastrada com Sucesso! ");
                                        }
                                        else
                                            MensagemAlerta("Conta NÃO Cadastrada! ");

                                    }
                                    catch {
                                        MensagemAlerta("Erro ao validar primeiro depósito. Conta Não Cadastrada!");
                                    }
                                }
                                else
                                    MensagemAlerta("Conta Já cadastrada. Verifique! ");

                                break;
                            }
                        case 2: //*** Alterar Titularidade
                            {
                                if (_listaContas.Count > 0)
                                {
                                    conta = LocalizaContaBancaria(_listaContas);

                                    DadosConta(conta);
                                    Console.Write("Informe o novo Titular: ");
                                    conta.Titular = Console.ReadLine();

                                    MensagemAlerta("Titularidade Alterada com Sucesso! ");
                                } else
                                    MensagemAlerta("Não existem contas cadastradas. Verifique! ");

                                break;
                            }
                        case 3: //*** Depósitos
                            {
                                if (_listaContas.Count > 0)
                                {
                                    conta = LocalizaContaBancaria(_listaContas);

                                    DadosConta(conta);
                                    Console.Write("Informe o valor para depósito: ");
                                    conta.Deposito(double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture));

                                    MensagemAlerta("Depósito Realizado com Sucesso! ");
                                }
                                else
                                    MensagemAlerta("Não existem contas cadastradas. Verifique! ");
                                break;
                            }
                        case 4: //*** Saques
                            {
                                if (_listaContas.Count > 0)
                                {
                                    conta = LocalizaContaBancaria(_listaContas);

                                    DadosConta(conta);
                                    Console.Write("Informe o valor para Saque: ");
                                    conta.Saque(double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture));

                                    MensagemAlerta("Saque Realizado com Sucesso! ");
                                }
                                else
                                    MensagemAlerta("Não existem contas cadastradas. Verifique! ");
                                break;
                            }
                        case 5: //*** Listar Contas
                            {
                                foreach (ContaBancaria item in _listaContas)
                                {
                                    Console.WriteLine("");
                                    Console.WriteLine("Número da conta: " + item.Numero);
                                    Console.WriteLine("Titular........: " + item.Titular);
                                    Console.WriteLine("Saldo..........: " + item.Saldo);
                                }

                                Console.WriteLine("");
                                Console.Write("Pressione ENTER para voltar... ");
                                Console.ReadLine();

                                break;
                            }
                        default:
                            break;
                    }
                }

                opcaoselecionada = null;
            }

        }

        private static string MontarMenu()
        {
            Console.Clear();
            Console.WriteLine("########## OPÇÕES DO MENU ########## ");
            Console.WriteLine("#   1. Incluir Nova Conta          # ");
            Console.WriteLine("#   2. Alterar Nome Titular        # ");
            Console.WriteLine("#   3. Depósitos                   # ");
            Console.WriteLine("#   4. Saques                      # ");
            Console.WriteLine("#   5. Listar Contas               # ");
            Console.WriteLine("#                                  # ");
            Console.WriteLine("#   0. Sair                        # ");
            Console.WriteLine("#################################### ");

            Console.WriteLine("");
            Console.Write("Por favor digite a opção desejada: ");
            return Console.ReadLine();
        }

        private static bool ValidaOpcao(string opc)
        {
            if ((opc == null) || (opc.Length == 0) || (opc.Length > 1))
                return false;
            try
            {
                int opcao = int.Parse(opc);
                if (opcao <= 6)
                    return true;
                else return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static int GetNumeroConta()
        {
            int result;
            int originalTop = 0;
            while (true)
            {
                try
                {
                    Console.Write("Entre com o número da conta: ");
                    originalTop = Console.CursorTop;
                    result = int.Parse(Console.ReadLine());
                    break;
                }
                catch (Exception)
                {
                    MensagemAlerta("Número/Formato de Conta Inválido. Verifique!");

                    Console.SetCursorPosition(0, originalTop);
                    Console.Write(new string(' ', 100));

                    Console.SetCursorPosition(0, originalTop);
                }
            }

            return result;
        }

        private static ContaBancaria LocalizaContaBancaria(List<ContaBancaria> listaContas)
        {
            int numeroconta;
            int originalTop = 0;
            ContaBancaria conta;

            while (true)
            {
                numeroconta = GetNumeroConta();
                originalTop = Console.CursorTop -1;

                conta = listaContas.FirstOrDefault(x => x.Numero == numeroconta);

                if (conta != null)
                    break;

                MensagemAlerta("Conta Não Encontrada ou Inexistente. Verifique!");
                Console.SetCursorPosition(0, originalTop);
            }


            return conta;
        }

        private static void DadosConta(ContaBancaria conta)
        {
            Console.WriteLine("");
            Console.WriteLine("*** DADOS DA CONTA *** ");
            Console.WriteLine("Número da conta: " + conta.Numero);
            Console.WriteLine("Titular........: " + conta.Titular);
            Console.WriteLine("Saldo..........: " + conta.Saldo);
            Console.WriteLine("");
        }

        private static void MensagemAlerta(string mensagem)
        {
            int intervalo = 300;
            DateTime fim = DateTime.Now + TimeSpan.FromMilliseconds(2500);

            while (DateTime.Now < fim)
            {
                Console.SetCursorPosition(0, Console.WindowHeight - 1);
                Console.Write(mensagem);
                Thread.Sleep(intervalo);

                Console.SetCursorPosition(0, Console.WindowHeight - 1);
                Console.Write(new string(' ', mensagem.Length));
                Thread.Sleep(intervalo);
            }
        }
    }
}
