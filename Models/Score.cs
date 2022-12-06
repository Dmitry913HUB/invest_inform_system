using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace is5.cs.Models
{
    public class Score
    {
        public int Id { get; set; }
        public double RUB { get; set; }
        public double USD { get; set; }
        public double EUR { get; set; }
        public double OllRUB { get; set; }
        public int Number { get; set; }

        public Score()
        {
            //empty constructor for db template engine
        }
        public Score(int Id, double RUB, double USD, double EUR, int Number, double OllRUB)
        {
            this.Id = Id;
            this.RUB = RUB;
            this.USD = USD;
            this.EUR = EUR;
            this.Number = Number;
            this.OllRUB = OllRUB;
        }

    }
}