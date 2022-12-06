using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace is5.cs.Models
{
    public class HistoryShares
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
        public int QuantityLot { get; set; }
        public string TypeVal { get; set; }
        public string BuySell { get; set; }
        public DateTime Date { get; set; }

        public HistoryShares()
        {
            //empty constructor for db template engine
        }
        public HistoryShares(int Id, string Name, int QuantityLot, double Cost, string TypeVal, string BuySell, DateTime Date)
        {
            this.Id = Id;
            this.Name = Name;
            this.Cost = Cost;
            this.QuantityLot = QuantityLot;
            this.TypeVal = TypeVal;
            this.BuySell = BuySell;
            this.Date = Date;
        }

    }
}