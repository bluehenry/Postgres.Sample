using PostgresApp1.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using PostgresApp1.Models;

namespace PostgresApp1.Commands.Actors
{
   public class AddBatchOfActors
    {
        ICommandRunner Runner;

        public AddBatchOfActors(ICommandRunner runner)
        {
            Runner = runner;
        }

        public int Execute(params Actor[] actors)
        {
            var sql = "insert into actor(first_name, last_name, last_update) value (@0, @1, @2);";
            var commands = new List<NpgsqlCommand>();
            foreach (var actor in actors)
            {
                commands.Add(Runner.BuildCommand(sql, actor.First, actor.Last, DateTime.Today));
            }

            var results = Runner.Transcat(commands.ToArray());
            return results.Sum();
        }

    }
}
