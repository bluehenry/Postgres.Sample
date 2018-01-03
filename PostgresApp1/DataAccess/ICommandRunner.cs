using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace PostgresApp1.DataAccess
{
    public interface ICommandRunner
    {
        string ConnectionString { get; set; }
        NpgsqlCommand BuildCommand(string sql, params object[] args);
        IEnumerable<T> Execute<T>(string sql, params object[] args) where T: new();
        IEnumerable<dynamic> ExecuteDynamic(string sql, params object[] args);
        T ExecuteSingle<T>(string sql, params object[] args) where T : new();
        dynamic ExecuteSingleDynamic(string sql, params object[] args);
        NpgsqlDataReader OpenReader(string sql, params object[] args);
        List<int> Transcat(params NpgsqlCommand[] cmds);

    }
}
