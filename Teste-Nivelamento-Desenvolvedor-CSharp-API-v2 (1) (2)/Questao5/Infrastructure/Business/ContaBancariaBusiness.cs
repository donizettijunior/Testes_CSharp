using Newtonsoft.Json;
using Questao5.Domain.BO;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Interfaces;
using Questao5.Infrastructure.Services;
using System.Globalization;

namespace Questao5.Infrastructure.Business
{
    public class ContaBancariaBusiness : IContaBancariaBusiness
    {
        private CultureInfo culture;

        private readonly IContaBancariaService _contabancariaservice;

        public ContaBancariaBusiness(IContaBancariaService contabancariaservice)
        {
            _contabancariaservice = contabancariaservice;

            culture = CultureInfo.CreateSpecificCulture("en-US");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        public ContaBancariaBusiness()
        {
            if (_contabancariaservice == null)
                _contabancariaservice = new ContaBancariaService();

            culture = CultureInfo.CreateSpecificCulture("en-US");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
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
                        errors = new { error = "Conta Corrente INATIVA. Verifique!" },
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
                    errors = new { error = "Conta Corrente INVÁLIDA ou INEXISTENTE. Verifique!" },
                    Data = new RetornoSaldo
                    {
                        Saldo = 0,
                        DataHoraconsulta = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                    }
                };
        }

        public Retorno MovimentoContaCorrente(string idrequisicao, string nrocontacorrente, string datamovimento, TipoMovimento? tipomovimento, string valor)
        {
            if (ExisteIdContaCorrente(nrocontacorrente))
            {
                if (_contabancariaservice.GetStatusContaCorrente(nrocontacorrente))
                {
                    if (ValorValido(valor))
                    {
                        if (DataValida(datamovimento))
                        {
                            if (ValidaTipoMovimento(tipomovimento))
                            {
                                string formato = "dd/MM/yyyy HH:mm:ss";
                                DateTime dt;
                                dt = DateTime.ParseExact(datamovimento, formato, null);

                                valor = AjustaValor(valor);

                                if (ValidaIdempotencia(idrequisicao, nrocontacorrente, tipomovimento, valor))
                                {
                                    //*** Verifico se idrequisicao ja foi realizado
                                    string resultado = _contabancariaservice.GetResultadoIdempotencia(idrequisicao);
                                    if ((resultado != null) && (resultado.Length > 0))
                                    {
                                        try
                                        {
                                            Retorno ret = JsonConvert.DeserializeObject<Retorno>(resultado);
                                            ret.title = "Requisição ja Realizada ou Duplicada. Verifique!";
                                            return ret;
                                        }
                                        catch
                                        {
                                            return new()
                                            {
                                                type = "ERROR",
                                                title = "Requisição ja Realizada ou Duplicada. Verifique!",
                                                status = 400,
                                                traceId = "api/MovimentoContaCorrente",
                                                errors = new { error = "Já Existe uma requisição realizada com o idrequisicao: " + idrequisicao + ". Porém ocorreu um erro ao recuperar os dados. Tente Novamente!" },
                                                Data = new object()
                                            };
                                        }
                                    }

                                    return _contabancariaservice.MovimentoContaCorrente(idrequisicao, nrocontacorrente, dt, tipomovimento, float.Parse(valor, culture));
                                }
                                else
                                {
                                    return new()
                                    {
                                        type = "ERROR",
                                        title = "Erro",
                                        status = 400,
                                        traceId = "api/MovimentoContaCorrente",
                                        errors = new { error = "Erro ao gerar movimentação. Tente Novamente!" },
                                        Data = new object()
                                    };
                                }
                            }
                            else
                            {
                                return new()
                                {
                                    type = "INVALID_TYPE",
                                    title = "Erro",
                                    status = 400,
                                    traceId = "api/MovimentoContaCorrente",
                                    errors = new { error = "Tipo de Movimento Inválido. Tente Novamente!" },
                                    Data = new object()
                                };
                            }
                        }
                        else
                            return new()
                            {
                                type = "INVALID_DATE",
                                title = "Erro",
                                status = 400,
                                traceId = "api/MovimentoContaCorrente",
                                errors = new { error = "Data Inválida formato correto (dd/MM/yyyy HH:mm:ss). Verifique!" },
                                Data = new object()
                            };
                    }
                    else
                        return new()
                        {
                            type = "INVALID_VALUE",
                            title = "Erro",
                            status = 400,
                            traceId = "api/MovimentoContaCorrente",
                            errors = new { error = "Valor INVÁLIDO. Verifique!" },
                            Data = new object()
                        };
                }
                else
                    return new()
                    {
                        type = "INACTIVE_ACCOUNT",
                        title = "Erro",
                        status = 400,
                        traceId = "api/MovimentoContaCorrente",
                        errors = new { error = "Conta Corrente INATIVA. Verifique!" },
                        Data = new object()
                    };
            }
            else
                return new()
                {
                    type = "INVALID_ACCOUNT",
                    title = "Erro",
                    status = 400,
                    traceId = "api/MovimentoContaCorrente",
                    errors = new { error = "Conta Corrente INVÁLIDA ou INEXISTENTE. Verifique!" },
                    Data = new object()
                };
        }

        private bool ExisteIdContaCorrente(string nrocontacorrente)
        {
            string idcontacorrente = _contabancariaservice.GetIdContaCorrente(nrocontacorrente);

            return ((idcontacorrente != null) && (idcontacorrente.Length > 0));
        }

        private static bool ValorValido(string valor)
        {
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
                
                if (DateTime.TryParseExact(data, formato, null, DateTimeStyles.None, out dt))
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

        private bool ValidaIdempotencia(string idrequisicao, string nrocontacorrente, TipoMovimento? tipomovimento, string valor) {
            
            string requisicao = _contabancariaservice.GetRequisicaoIdempotencia(idrequisicao);
            if ((requisicao != null)  && (requisicao.Length > 0))
            {
                return true;
            }
            else
            {
                string datamovimento = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                object obj = new
                {
                    idrequisicao,
                    nrocontacorrente,
                    tipomovimento,
                    valor,
                    datamovimento
                };

                string json = JsonConvert.SerializeObject(obj);
                return _contabancariaservice.SetIdempotencia(json);
            }
        }

        private static bool ValidaTipoMovimento(TipoMovimento? tipomovimento)
        {
            try
            {
                string[] valoresPermitidos = { "C", "D" };
                return valoresPermitidos.Contains(tipomovimento.ToString());
            }
            catch { 
                return false;
            }
        }
    }
}
