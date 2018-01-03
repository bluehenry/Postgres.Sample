using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using PostgresApp1.DataAccess;
using PostgresApp1.Models;

namespace PostgresApp1
{
    [Table("film", Schema="public")]
    public class Film
    {
        [Column("film_id")]
        [Key]
        public int ID { get; set; }
        [Column("title")]
        public string Title { get; set; }
    }

    public class DB : DbContext
    {
        public DB() : base("dvds")
        {
        }

        public DbSet<Film> Films { get; set; }
    }
    class Program
    {
        public string ConnectionString { get; private set; }

        static void Main(string[] args)
        {
            //CallOne();
            //CallTwo();

            var db = new CommandRunner("dvds");
            var cmd1 = db.BuildCommand("insert into actor(first_name, last_name, last_update");
            var result = db.Transcat(cmd1);
            Console.WriteLine(result.Sum());
            Console.Read();
        }

        static void CallOne()
        {
            var db = new DB();
            foreach (var film in db.Films)
            {
                Console.WriteLine(film.Title);
            }
            Console.Read();
        }

        static void CallTwo()
        {
            using (var conn =
                new NpgsqlConnection("server=localhost;user id=postgres;password=admin;database=dvdrental"))
            {
                conn.Open();
                var cmd = new NpgsqlCommand("select * from film", conn);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine(rdr["title"]);
                }
                cmd.Dispose();
                conn.Dispose();

            }
            Console.Read();
        }

        static void AddBatchOfActors()
        {
            var db = new CommandRunner("dvds");
            var q = new Commands.Actors.AddBatchOfActors(db);
            var actor1 = new Actor { First = "Joe", Last = "Tonks" };
            var actor2 = new Actor { First = "Joe", Last = "Biff" };
            var actor3 = new Actor { First = "Jolene", Last = "Silidkdk" };

            var result = q.Execute(actor1, actor2, actor3);
            Console.WriteLine("There were {0} actors added", result);
            Console.Read();
        }
    }
}
