using Microsoft.Data.Sqlite;
using System.Data;

namespace Questao5.Infrastructure.Sqlite
{
    public interface IDatabaseBootstrap
    {
        void Setup();

        SqliteConnection GetConecction();

        DataTable ExecuteQuery(string query);

        bool SaveQueryNoParameters(string query);

        bool SaveQueryWithParameters(string query, List<SqliteParameter[]> parametros);
    }
}