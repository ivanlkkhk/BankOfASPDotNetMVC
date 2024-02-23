namespace BankOfBIT_KL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSavingsAccountColumnName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BankAccounts", "SavingsServiceCharges", c => c.Double());
            DropColumn("dbo.BankAccounts", "SavingServiceCharges");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BankAccounts", "SavingServiceCharges", c => c.Double());
            DropColumn("dbo.BankAccounts", "SavingsServiceCharges");
        }
    }
}
