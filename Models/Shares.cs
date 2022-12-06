using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace is5.cs.Models
{
    public class Shares
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public int Lot { get; set; }
        public string TypeVal { get; set; }
        public Shares()
        {
            //empty constructor for db template engine
        }
        public Shares(int Id, string Name, string Description, double Cost, int Lot, string TypeVal)
        {
            this.Id = Id;
            this.Name = Name;
            this.Description = Description;
            this.Cost = Cost;
            this.Lot = Lot;
            this.TypeVal = TypeVal;
        }

        public int? Flag { get; set; }

    }
}