using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace PostgresApp1.DataAccess
{
    public class CommandRunner : ICommandRunner
    {
        public string ConnectionString { get; set; }

        /// <summary>
        /// Constructor - takes a connection string name
        /// </summary>
        /// <param name="connectionStringName"></param>
        public CommandRunner(string connectionStringName)
        {
            this.ConnectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }
        public NpgsqlCommand BuildCommand(string sql, params object[] args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return a single record, typed as you need
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public T ExecuteSingle<T>(string sql, params object[] args) where T : new()
        {
            return this.Execute<T>(sql, args).First();
        }

        /// <summary>
        /// Executes a typed query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public IEnumerable<T> Execute<T>(string sql, params object[] args) where T: new()
        {
            using (var conn = new NpgsqlConnection(this.ConnectionString))
            {
                var cmd = BuildCommand(sql, args);
                cmd.Connection = conn;
                //defer opeing to the last minute
                conn.Open();
                //use a rdr here and yield back the projection
                //connection will close where rdr is finished
                var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (rdr.Read())
                {
                    yield return rdr.ToSingle<T>();
                }
                //housekeeping
                cmd.Dispose();
            }
        }

        /// <summary>
        /// Executes a query returning items in a dynamic list
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> ExecuteDynamic(string sql, params object[] args)
        {
            using (var conn = new NpgsqlConnection(this.ConnectionString))
            {
                var cmd = BuildCommand(sql, args);
                cmd.Connection = conn;
                //defer opening to the last minute
                conn.Open();
                var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (rdr.Read())
                {
                    yield return rdr.RecordToExpando();
                }
            }
        }


        /// <summary>
        /// Executes a query returning items in a single dynamic
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public dynamic ExecuteSingleDynamic(string sql, params object[] args)
        {
            throw new NotImplementedException();
        }

        public NpgsqlDataReader OpenReader(string sql, params object[] args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// A Transacation helper that executes a series of commands in a sinlge trascation
        /// </summary>
        /// <param name="cmds">Commands built with BuildCommand</param>
        /// <returns></returns>
        public List<int> Transcat(params NpgsqlCommand[] cmds)
        {
           var results = new List<int>();
            using (var conn = new NpgsqlConnection(this.ConnectionString))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var cmd in cmds)
                        {
                            cmd.Transaction = tx;
                            cmd.Connection = conn;
                            results.Add(cmd.ExecuteNonQuery());
                        }
                    }
                    catch (NpgsqlException x)
                    {
                        tx.Rollback();
                        throw(x);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return results;
        }
    }
}
