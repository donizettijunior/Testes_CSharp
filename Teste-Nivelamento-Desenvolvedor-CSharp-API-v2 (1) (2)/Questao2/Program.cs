using Newtonsoft.Json;
using Questao2;
using System.Net;

public class Program
{
    private static int cursortoptelarequisicao = 0;
    private static string ano = "";
    private static string time1 = "";
    private static string time2 = "";
    private static int page = 1;
    private static List<BODataFootGames> listaallMatches = new();

    public static void Main()
    {
        string opcaoselecionada = MontarMenu();
        while (true)
        {
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
                    case 1: //*** Nova Requisição
                        {
                            while (true)
                            {
                                MontarTelaRequisicao();

                                int cursorTop = Console.CursorTop;
                                Console.WriteLine("");
                                Console.SetCursorPosition(0, cursorTop);
                                Console.Write("Confirma Parametros Informados (S/N)? ");
                                int cursorLeft = Console.CursorLeft;
                                Console.SetCursorPosition(cursorLeft, cursorTop);
                                string opc = Console.ReadLine();

                                Console.SetCursorPosition(0, cursorTop);
                                Console.Write(new string(' ', 50));

                                if ((opc != null) && (opc.ToUpper() == "S"))
                                {
                                    get();
                                    break;
                                }

                            }
                            break;
                        }
                    case 2: //*** Pesquisar Gols por Time
                        {
                            if (listaallMatches.Count > 0)
                            {
                                string teamName;
                                int yearteam;
                                try
                                {
                                    Console.Write("Informe o Time.: ");
                                    teamName = Console.ReadLine();

                                    Console.Write("Informe o Ano..: ");
                                    yearteam = int.Parse(Console.ReadLine());

                                    if (((teamName != null) && (teamName.Length > 0)) && (yearteam > 0))
                                    {
                                        int totalGoals = getTotalScoredGoals(teamName, yearteam);

                                        Console.WriteLine("");
                                        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + yearteam);

                                        Console.WriteLine("");
                                        Console.WriteLine("Pressione ENTER para retornar...");
                                        Console.ReadLine();
                                    }
                                    else
                                        MensagemAlerta("É obrigatório informar os campos Time e Ano. Verifique!");
                                }
                                catch
                                {
                                    MensagemAlerta("Dados não Encontrados. Verifique se parâmetros estão corretos!");
                                }
                            }
                            else
                                MensagemAlerta("Não existem dados de requisição. Verifique!");
                            break;
                        }
                    default:
                        break;
                }
            }
            opcaoselecionada = null;
        }
    }

    public static int getTotalScoredGoals(string team, int year)
    {
        if (listaallMatches.Count > 0)
        {
            try
            {
                int golstime1 = listaallMatches.Where(item => item.Team1.ToUpper() == team.ToUpper() && item.Year == year).Sum(item => item.Team1Goals);
                int golstime2 = listaallMatches.Where(item => item.Team2.ToUpper() == team.ToUpper() && item.Year == year).Sum(item => item.Team2Goals);
                return golstime1 + golstime2;
            } catch { }
        }
        return 0;
    }

    private static void get()
    {
        Console.WriteLine("");
        Console.Write("AGUARDE ENQUANTO A REQUISIÇÃO É EXECUTADA...");
        int posfirstcursor = Console.CursorTop;
        
        string url = "https://jsonmock.hackerrank.com/api/football_matches";

        if (page > 0)
        {
            try
            {
                HttpClient client = new();
                HttpResponseMessage response = client.GetAsync(url + GetParams()).Result;
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;

                BODataFoot games = JsonConvert.DeserializeObject<BODataFoot>(responseBody);

                if (games != null)
                    listaallMatches.AddRange(games.data);

                MensagemAlerta("Requisição realizada com Sucesso!");
            }
            catch (WebException ex)
            {
                MensagemAlerta("Erro na requisição: " + ex.Message);
            }
            catch (JsonException ex)
            {
                MensagemAlerta("Erro ao desserializar JSON: " + ex.Message);
            }
        }
        else
        {
            page = 1;

            Console.WriteLine("");
            Console.Write("*** Progresso da requisição: CALCULANDO...");
            int poscursor = Console.CursorTop;

            while (true)
            {
                BODataFoot games = new();
                try
                {
                    HttpClient client = new();
                    HttpResponseMessage response = client.GetAsync(url + GetParams()).Result;
                    response.EnsureSuccessStatusCode();
                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    games = JsonConvert.DeserializeObject<BODataFoot>(responseBody);

                    if (games != null)
                        listaallMatches.AddRange(games.data);
                }
                catch { }

                Console.SetCursorPosition(0, poscursor);
                Console.Write(new string(' ', 100));
                Console.SetCursorPosition(0, poscursor);
                Console.Write("*** Progresso da requisição: " + page + " de " + games.total_pages);

                if (page >= games?.total_pages)
                {
                    break;
                }

                page++;
            }

            Console.SetCursorPosition(0, poscursor);
            Console.Write(new string(' ', 100));

            MensagemAlerta("Requisição realizada com Sucesso!");
        }

        Console.SetCursorPosition(0, posfirstcursor);
        Console.Write(new string(' ', 100));
    }

    private static string GetParams()
    {
        bool existe = false;
        string param = "";

        if (ano?.Length > 0)
        {
            existe = true;
            param = param + "?year=" + ano;
        }

        if (existe)
        {
            if (time1?.Length > 0)
                param = param + "&team1=" + time1;

            if (time2?.Length > 0)
                param = param + "&team2=" + time2;

            if (page > 0)
                param = param + "&page=" + page;
        }
        else
        {
            if (time1?.Length > 0)
            {
                existe = true;
                param  = param + "?team1=" + time1;
            }

            if (existe)
            {
                if (time2?.Length > 0)
                    param = param + "&team2=" + time2;

                if (page > 0)
                    param = param + "&page=" + page;
            }
            else
            {
                if (time2?.Length > 0)
                {
                    existe = true;
                    param = param + "?team2=" + time2;
                }

                if (existe)
                {
                    if (page > 0)
                    {
                        param = param + "&page=" + page;
                    }
                }
                else
                {
                    if (page > 0)
                    {
                        param = param + "?page=" + page;
                    }
                }
            }
        }

        return param;
    }

    private static string MontarMenu()
    {
        Console.Clear();
        Console.WriteLine("########## OPÇÕES DO MENU ########## ");
        Console.WriteLine("#   1. Nova Requisição             # ");
        Console.WriteLine("#   2. Consultar Gols              # ");
        Console.WriteLine("#                                  # ");
        Console.WriteLine("#   0. Sair                        # ");
        Console.WriteLine("#################################### ");

        Console.WriteLine("");
        Console.Write("Por favor digite a opção desejada: ");
        return Console.ReadLine();
    }

    private static void MontarTelaRequisicao()
    {
        if (cursortoptelarequisicao == 0)
            cursortoptelarequisicao = Console.CursorTop;
        else
            Console.SetCursorPosition(0, cursortoptelarequisicao);

        Console.WriteLine("");
        Console.WriteLine("PARÂMETROS OPCIONAIS. Informe se julgar necessário");
        Console.WriteLine("");

        int cursorTop = Console.CursorTop;
        
        Console.SetCursorPosition(0, cursorTop);
        Console.Write("Ano.....: ");

        Console.SetCursorPosition(0, cursorTop + 1);
        Console.Write("Time 1..: ");

        Console.SetCursorPosition(0, cursorTop + 2);
        Console.Write("Time 2..: ");

        Console.SetCursorPosition(0, cursorTop + 3);
        Console.Write("Pagina..: ");

        Console.SetCursorPosition(10, cursorTop);
        Console.Write(new string(' ', 50));
        Console.SetCursorPosition(10, cursorTop);
        ano = Console.ReadLine();

        Console.SetCursorPosition(10, cursorTop + 1);
        Console.Write(new string(' ', 50));
        Console.SetCursorPosition(10, cursorTop + 1);
        time1 = Console.ReadLine();

        Console.SetCursorPosition(10, cursorTop + 2);
        Console.Write(new string(' ', 50));
        Console.SetCursorPosition(10, cursorTop + 2);
        time2 = Console.ReadLine();

        Console.SetCursorPosition(10, cursorTop + 3);
        try
        {
            Console.Write(new string(' ', 50));
            Console.SetCursorPosition(10, cursorTop + 3);
            page = int.Parse(Console.ReadLine());
        }
        catch {
            page = 0;
        }
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

    private static bool ValidaOpcao(string opc)
    {
        if ((opc == null) || (opc.Length == 0) || (opc.Length > 1))
            return false;
        try
        {
            int opcao = int.Parse(opc);
            if (opcao <= 2)
                return true;
            else return false;
        }
        catch (Exception)
        {
            return false;
        }
    }

}