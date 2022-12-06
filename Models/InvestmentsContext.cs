using is5.cs.Models;
using System.Data.Entity;


namespace is5.cs.Models
{
    class InvestmentsContext : DbContext
    {
        public InvestmentsContext() : base("Investments4DB") { }
        public DbSet<Score> Score { get; set; }
        public DbSet<TransferAndReplenishment> TransferAndReplenishment { get; set; }
        public DbSet<Shares> Shares { get; set; }        
        public DbSet<RegisterShares> RegisterShares { get; set; }        
        public DbSet<HistoryShares> HistoryShares { get; set; }        

    }
}
