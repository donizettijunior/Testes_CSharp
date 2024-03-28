using Questao5.Domain.BO;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Interfaces;
using System.Configuration;
using System.Globalization;

namespace Questao5.Infrastructure.Business
{
    public class ContaBancariaBusiness : IContaBancariaBusiness
    {
        private readonly IContaBancariaService _contabancariaservice;

        public ContaBancariaBusiness(IContaBancariaService contabancariaservice)
        {
            _contabancariaservice = contabancariaservice;
        }

        public Retorno MovimentoContaCorrente(string nrocontacorrente, string datamovimento, TipoMovimento? tipomovimento, string valor)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

            if (ExisteIdContaCorrente(nrocontacorrente))
            {
                if (_contabancariaservice.GetStatusContaCorrente(nrocontacorrente)) {
                    if (ValorValido(valor))
                    {
                        string formato = "dd/MM/yyyy HH:mm:ss";
                        if (DataValida(datamovimento))
                        {
                            DateTime dt;
                            dt = DateTime.ParseExact(datamovimento, formato, null);

                            valor = AjustaValor(valor);
                            return _contabancariaservice.MovimentoContaCorrente(nrocontacorrente, dt, tipomovimento, float.Parse(valor, culture));
                        }
                        else
                            return new()
                            {
                                type = "INVALID_DATE",
                                title = "Erro",
                                status = 400,
                                traceId = "api/MovimentoContaCorrente",
                                errors = new { error = "Data Inválida formato correto (dd/MM/yyyy HH:mm:ss). Verifique!" },
                                Data = new RetornoSaldo
                                {
                                    Saldo = 0,
                                    DataHoraconsulta = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                                }
                            };
                    }
                    else
                        return new()
                        {
                            type = "INVALID_VALUE",
                            title = "Erro",
                            status = 400,
                            traceId = "api/MovimentoContaCorrente",
                            errors = new { error = "Valor Inválido. Verifique!" },
                            Data = new RetornoSaldo
                            {
                                Saldo = 0,
                                DataHoraconsulta = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                            }
                        };
                }
                else
                    return new()
                    {
                        type = "INACTIVE_ACCOUNT",
                        title = "Erro",
                        status = 400,
                        traceId = "api/MovimentoContaCorrente",
                        errors = new { error = "Conta Corrente Não Cadastrada ou Inexistente. Verifique!" },
                        Data = new RetornoSaldo
                        {
                            Saldo = 0,
                            DataHoraconsulta = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                        }
                    };
            }
            else
                return new()
                {
                    type = "INVALID_ACCOUNT",
                    title = "Erro",
                    status = 400,
                    traceId = "api/MovimentoContaCorrente",
                    errors = new { error = "Conta Corrente Inexistente ou Inválida. Verifique!" },
                    Data = new RetornoSaldo
                    {
                        Saldo = 0,
                        DataHoraconsulta = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                    }
                };
        }

        public Retorno ConsultaSaldo(string nrocontacorrente)
        {
            if (ExisteIdContaCorrente(nrocontacorrente))
            {
                if (_contabancariaservice.GetStatusContaCorrente(nrocontacorrente))
                    return _contabancariaservice.ConsultaSaldo(nrocontacorrente);
                else
                    return new()
                    {
                        type = "INACTIVE_ACCOUNT",
                        title = "Erro",
                        status = 400,
                        traceId = "api/ConsultaSaldo",
                        errors = new { error = "Conta Corrente Não Cadastrada ou Inexistente. Verifique!" },
                        Data = new RetornoSaldo
                        {
                            Saldo = 0,
                            DataHoraconsulta = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                        }
                    };
            }
            else
                return new()
                {
                    type = "INVALID_ACCOUNT",
                    title = "Erro",
                    status = 400,
                    traceId = "api/ConsultaSaldo",
                    errors = new { error = "Conta Corrente Inexistente ou Inválida. Verifique!" },
                    Data = new RetornoSaldo
                    {
                        Saldo = 0,
                        DataHoraconsulta = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                    }
                };
        }

        private bool ExisteIdContaCorrente(string nrocontacorrente)
        {
            string idcontacorrente = _contabancariaservice.GetIdContaCorrente(nrocontacorrente);

            return ((idcontacorrente != null) && (idcontacorrente.Length > 0));
        }

        private static bool ValorValido(string valor)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            valor = AjustaValor(valor);

            if (float.TryParse(valor, out _))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string AjustaValor(string valor)
        {
            if ((valor.Contains(",")) && (valor.Contains(".")))
                valor = valor.Trim().Replace(".", "").Replace(",", ".");
            else if ((valor.Contains(",")) && (!valor.Contains(".")))
                valor = valor.Trim().Replace(",", ".");

                return valor;
        }

        private static bool DataValida(string data)
        {
            try
            {
                string formato = "dd/MM/yyyy HH:mm:ss";
                DateTime dt;
                dt = DateTime.ParseExact(data, formato, null);
                
                if (DateTime.TryParseExact(data, formato, null, System.Globalization.DateTimeStyles.None, out dt))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch { 
                return false;
            }
        }
    }
}
