using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace is5.cs.Models
{
    public class TransferAndReplenishment
    {
        public int Id { get; set; }
        public double RUB { get; set; }
        public double USD { get; set; }
        public double EUR { get; set; }
        public int NumberPhoneCard { get; set; }
        public string FlagTR { get; set; }
        public DateTime Date { get; set; }

        public TransferAndReplenishment()
        {
            //empty constructor for db template engine
        }
        public TransferAndReplenishment(int Id, double RUB, double USD, double EUR, int NumberPhoneCard, string FlagTR, DateTime Date)
        {
            this.Id = Id;
            this.RUB = RUB;
            this.USD = USD;
            this.EUR = EUR;
            this.NumberPhoneCard = NumberPhoneCard;
            this.FlagTR = FlagTR;
            this.Date = Date;
        }

    }
}