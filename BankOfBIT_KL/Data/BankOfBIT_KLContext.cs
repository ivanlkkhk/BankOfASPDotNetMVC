using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BankOfBIT_KL.Data
{
    public class BankOfBIT_KLContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public BankOfBIT_KLContext() : base("name=BankOfBIT_KLContext")
        {
        }

        public System.Data.Entity.DbSet<BankOfBIT_KL.Models.Client> Clients { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_KL.Models.AccountState> AccountStates { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_KL.Models.BankAccount> BankAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_KL.Models.BronzeState> BronzeStates { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_KL.Models.GoldState> GoldStates { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_KL.Models.PlatinumState> PlatinumStates { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_KL.Models.SilverState> SilverStates { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_KL.Models.ChequingAccount> ChequingAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_KL.Models.InvestmentAccount> InvestmentAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_KL.Models.MortgageAccount> MortgageAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_KL.Models.SavingsAccount> SavingsAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_KL.Models.NextUniqueNumber> NextUniqueNumbers { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_KL.Models.NextSavingsAccount> NextSavingsAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_KL.Models.NextMortgageAccount> NextMortgageAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_KL.Models.NextInvestmentAccount> NextInvestmentAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_KL.Models.NextChequingAccount> NextChequingAccounts { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_KL.Models.NextTransaction> NextTransactions { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_KL.Models.NextClient> NextClients { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_KL.Models.Payee> Payees { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_KL.Models.Institution> Institutions { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_KL.Models.TransactionType> TransactionTypes { get; set; }

        public System.Data.Entity.DbSet<BankOfBIT_KL.Models.Transaction> Transactions { get; set; }
    }
}
