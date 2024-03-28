using Microsoft.Data.Sqlite;
using Questao5.Domain.BO;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Interfaces;
using Questao5.Infrastructure.Sqlite;
using System.Data;

namespace Questao5.Infrastructure.Services
{
    public class ContaBancariaService : IContaBancariaService
    {
        private readonly IDatabaseBootstrap _idatabasebootstrap;

        public ContaBancariaService(IDatabaseBootstrap idatabasebootstrap)
        {
            _idatabasebootstrap = idatabasebootstrap;
        }

        public Retorno ConsultaSaldo(string nrocontacorrente)
        {
            RetornoSaldo retornoSaldo = new()
            {
                DataHoraconsulta = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                Nrocontacorrente = nrocontacorrente,
                Saldo = 0
            };

            try
            {
                string query = " select cc.nome, sum(COALESCE(cred.credito, 0) - COALESCE(deb.debito, 0)) saldo                               " +
                               "   from contacorrente cc                                                                                      " +
                               " left join (select sum(md.valor) as debito, md.idcontacorrente, md.tipomovimento                              " +
			                   "               from movimento md                                                                              " +
			                   "             where md.tipomovimento = 'D'                                                                     " +
			                   "             Group by md.idcontacorrente, md.tipomovimento) deb on deb.idcontacorrente = cc.idcontacorrente   " +
                               " left join (select sum(mc.valor) as credito, mc.idcontacorrente, mc.tipomovimento                             " +
                               "               from movimento mc                                                                              " +
                               "             where mc.tipomovimento = 'C'                                                                     " +
			                   "             Group by mc.idcontacorrente, mc.tipomovimento) cred on cred.idcontacorrente = cc.idcontacorrente " +
                               " where cc.idcontacorrente = '" + GetIdContaCorrente(nrocontacorrente)                                     + "'" +
                               " Group by cc.nome ";

                DataTable dt = _idatabasebootstrap.ExecuteQuery(query);

                if (dt.Rows.Count > 0) {

                    object titular = dt.Rows[0][0];
                    object saldo = dt.Rows[0][1];
                    
                    retornoSaldo.Titular = titular != DBNull.Value ? titular.ToString() : "";
                    retornoSaldo.Saldo = saldo != DBNull.Value ? (float)Math.Round(Convert.ToSingle(saldo), 2) : 0;

                    return new()
                    {
                        type = "SUCCESS",
                        title = "Sucesso",
                        status = 200,
                        traceId = "api/MovimentoContaCorrente",
                        errors = new object(),
                        Data = retornoSaldo
                    };
                }

                return new()
                {
                    type = "ERROR",
                    title = "Erro",
                    status = 400,
                    traceId = "api/MovimentoContaCorrente",
                    errors = new { error = "Conta Corrente Não Cadastrada ou Inexistente. Verifique!" },
                    Data = retornoSaldo
                };

            }
            catch (Exception ex)
            {
                return new()
                {
                    type = "ERROR",
                    title = "Erro",
                    status = 400,
                    traceId = "api/MovimentoContaCorrente",
                    errors = new { error = ex.Message },
                    Data = retornoSaldo
                };
            }
        }

        public Retorno MovimentoContaCorrente(string nrocontacorrente, DateTime? datamovimento, TipoMovimento? tipomovimento, float? valor)
        {
            string hashid = Guid.NewGuid().ToString("D").ToUpper();

            List<SqliteParameter[]> parametros = new()
            {
                new SqliteParameter[] {
                    new("@idmovimento", hashid),
                    new("@idcontacorrente", GetIdContaCorrente(nrocontacorrente)),
                    new("@datamovimento", datamovimento),
                    new("@tipomovimento", tipomovimento),
                    new("@valor", valor)
                }
            };

            string query = " insert into movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) " +
                           " values (@idmovimento, @idcontacorrente, @datamovimento, @tipomovimento, @valor)";

            if (_idatabasebootstrap.SaveQueryWithParameters(query, parametros))
                return new()
                {
                    type = "SUCCESS",
                    title = "Sucesso",
                    status = 200,
                    traceId = "api/MovimentoContaCorrente",
                    errors = new object(),
                    Data = new { idmovimentogerado = hashid },
                };
            else
                return new()
                {
                    type = "ERROR",
                    title = "Erro",
                    status = 400,
                    traceId = "api/MovimentoContaCorrente",
                    errors = new { error = "Problemas durante a gravação da movimentação. Contate Administrador do Sistema!" },
                    Data = new object()
                };

            
        }

        public bool GetStatusContaCorrente(string nrocontacorrente)
        {
            try
            {
                string query = " select count(1)                          " +
                               "   from contacorrente                     " +
                               " where numero = '" + nrocontacorrente + "'" +
                               "   and ativo  = 1";

                DataTable dt = _idatabasebootstrap.ExecuteQuery(query);

                object qtd = dt.Rows[0][0];
                return qtd != DBNull.Value ? (Convert.ToInt16(qtd) > 0) : false;
            }
            catch
            {
                return false;
            }
        }

        public string GetIdContaCorrente(string nrocontacorrente)
        {
            try
            {
                string query = " select idcontacorrente                   " +
                               "   from contacorrente                     " +
                               " where numero = '" + nrocontacorrente + "'" +
                               "   and ativo  = 1";

                DataTable dt = _idatabasebootstrap.ExecuteQuery(query);

                if (dt.Rows.Count > 0)
                {
                    object idcontacorrente = dt.Rows[0][0];
                    return idcontacorrente != DBNull.Value ? idcontacorrente.ToString() : "";
                }

                return "";
            }
            catch
            {
                return "";
            }
        }
    }
}
