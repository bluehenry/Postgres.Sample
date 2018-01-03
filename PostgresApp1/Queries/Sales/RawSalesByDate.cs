using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostgresApp1.DataAccess;

namespace PostgresApp1
{
    public class RawSale
    {
        public string Title { get; set; }
        public decimal Total { get; set; }
        public int Quarter { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string QuarterYear { get; set; }
    }
    public class RawSalesByDate
    {
        public string Title { get; set; }
        public decimal? Total { get; set; }
        public int? Quarter { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public string QuarterYear { get; set; }

        private ICommandRunner Runner;

        public RawSalesByDate(ICommandRunner runner)
        {
            this.Runner = runner;
        }

        //public IEnumerable<RawSale> Execute()
        //{
        //    var args = new List<object>();

        //    var sqlFormat = @"select distinct title, quarter, month, year, qyear as quarter year 
        //                             sum(amount) over (partition by title{0} as Total from raw_sales";
        //    var partitionValue = "";

        //    if (this.Year.HasValue)
        //    {
        //        args.Add(this.Year.Value);
        //        partitionValue = ",year";
        //    }

        //    var result = new RawSale();
        //    using (var rdr = Runner.OpenReader(sqlFormat))
        //    {
        //        result = rdr.ToSingle<RawSale>();
        //    }

        //    return result;
        //}
    }
}
