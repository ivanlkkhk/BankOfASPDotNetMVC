namespace BankOfBIT_KL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Payee_Institution_and_Transaction : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        TransactionId = c.Int(nullable: false, identity: true),
                        BankAccountId = c.Int(nullable: false),
                        TransactionTypeId = c.Int(nullable: false),
                        TransactionNumber = c.Long(nullable: false),
                        Deposit = c.Double(),
                        Withdrawal = c.Double(),
                        DateCreated = c.DateTime(nullable: false),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.TransactionId)
                .ForeignKey("dbo.BankAccounts", t => t.BankAccountId, cascadeDelete: true)
                .ForeignKey("dbo.TransactionTypes", t => t.TransactionTypeId, cascadeDelete: true)
                .Index(t => t.BankAccountId)
                .Index(t => t.TransactionTypeId);
            
            CreateTable(
                "dbo.TransactionTypes",
                c => new
                    {
                        TransactionTypeId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.TransactionTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "TransactionTypeId", "dbo.TransactionTypes");
            DropForeignKey("dbo.Transactions", "BankAccountId", "dbo.BankAccounts");
            DropIndex("dbo.Transactions", new[] { "TransactionTypeId" });
            DropIndex("dbo.Transactions", new[] { "BankAccountId" });
            DropTable("dbo.TransactionTypes");
            DropTable("dbo.Transactions");
        }
    }
}
