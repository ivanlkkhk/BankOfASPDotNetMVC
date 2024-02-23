using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utility;
using System.Data;
using BankOfBIT_KL.Data;
using System.EnterpriseServices;
using System.Data.Entity;
using System.Data.SqlClient;

namespace BankOfBIT_KL.Models
{
    /// <summary>
    /// CLient Model - to represent the Client table in the database
    /// </summary>
    public class Client
    {
        /// <summary>
        /// The client ID.
        /// </summary>
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ClientId { get; set; }

        /// <summary>
        /// The client number.
        /// </summary>
        [Display(Name = "Client\nNumber")]
        public long ClientNumber { get; set; }

        /// <summary>
        /// The first name of client.
        /// </summary>
        [Required]
        [StringLength(35, MinimumLength = 1)]
        [Display(Name = "First\nName")]
        public String FirstName { get; set; }

        /// <summary>
        /// The last name of client.
        /// </summary>
        [Required]
        [StringLength(35, MinimumLength = 1)]
        [Display(Name = "Last\nName")]
        public String LastName { get; set; }

        /// <summary>
        /// The address of client.
        /// </summary>
        [Required]
        [StringLength(35, MinimumLength = 1)]
        public String Address { get; set; }

        /// <summary>
        /// The city of client.
        /// </summary>
        [Required]
        [StringLength(35, MinimumLength = 1)]
        public String City { get; set; }

        /// <summary>
        /// Province of client.
        /// </summary>
        [Required]
        [RegularExpression("^(N[BLSTU]|[AMN]B|[BQ]C|ON|PE|SK|YT)", ErrorMessage = "Invalid Canadian province code is not allowed.")]
        public String Province { get; set; }

        /// <summary>
        /// Record create date.
        /// </summary>
        [Required]
        [Display(Name = "Date\nCreated")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime DateCreated  { get; set; }

        /// <summary>
        /// Note about this client.
        /// </summary>
        public String Notes { get; set; }

        /// <summary>
        /// Full name of client. ie. FirstName LastName
        /// </summary>
        [Display(Name = "Name")]
        public String FullName 
        {
            get
            { 
                return String.Format("{0} {1}", FirstName, LastName);
            } 
        }

        /// <summary>
        /// Full address of client. ie. Address City Province
        /// </summary>
        [Display(Name = "Address")]
        public String FullAddress 
        { 
            get
            {
                return String.Format("{0} {1} {2}", Address, City, Province);
            }
        }

        /// <summary>
        /// The bank accounts that the client holding.
        /// </summary>
        // Navigation properties.
        public virtual ICollection<BankAccount> BankAccount { get; set; }
        
        /// <summary>
        /// Set next account number.
        /// </summary>
        public void SetNextClientNumber()
        {
            this.ClientNumber = (long)StoredProcedure.NextNumber("Next" + GetType().Name);
        }
    }

    /// <summary>
    /// AccountState Model - the generic account state model, to represent the Account State
    /// </summary>
    public abstract class AccountState
    {
        protected static BankOfBIT_KLContext db = new BankOfBIT_KLContext();

        /// <summary>
        /// Account state ID.
        /// </summary>
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AccountStateId { get; set; }

        /// <summary>
        /// Lower balance limit of this account state.
        /// </summary>
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c2}")]
        [Display(Name = "Lower\nLimit")]
        public double LowerLimit { get; set; }

        /// <summary>
        /// Upper balance limit of this account state.
        /// </summary>
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c2}")]
        [Display(Name = "Upper\nLimit")]
        public double UpperLimit { get; set; }

        /// <summary>
        /// Rate of this account state.
        /// </summary>
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c2}")]
        public double Rate { get; set; }

        /// <summary>
        /// Account state description.
        /// </summary>
        [Display(Name = "Account\nState")]
        public String Description
        {
            get
            {
                return BusinessRules.GetTypeNameDescription(GetType().Name, "State");
            }
        }

        /// <summary>
        /// Bank account that associated with this account state.
        /// </summary>
        // Navigation properties.
        public virtual ICollection<BankAccount> BankAccount { get; set; }

        /// <summary>
        /// The interest rate that a BankAccount can earn is based in part on the State of the account. 
        /// Each account state subtypes will have different adjustment on the rate value. 
        /// </summary>
        /// <param name="bankAccount">Current bank account.</param>
        /// <returns>Returns the rate that suitable for current account.</returns>
        public abstract double RateAdjustment(BankAccount bankAccount);

        /// <summary>
        /// This method will change the account state from one state to another according to the account balance value and the upper and lower limit of each of the account state subtypes.
        /// </summary>
        /// <param name="bankAccount">Current bank account</param>
        public abstract void StateChangeCheck(BankAccount bankAccount);


    }


    /// <summary>
    /// BronzeState Model - the inherit class of AccountState, to represent the Account State
    /// </summary>
    public class BronzeState : AccountState
    {
        private static BronzeState bronzeState;
        // Default values.
        private const double LOWER_LIMIT = 0.0;
        private const double UPPER_LIMIT = 5000.0;
        private const double INITIAL_RATE = 0.0100;
        private const double BRONZE_RATE = 0.055;

        /// <summary>
        /// The constructor will be use by GetInstance only.
        /// </summary>
        private BronzeState() 
        { 
            this.LowerLimit = LOWER_LIMIT;
            this.UpperLimit = UPPER_LIMIT;
            this.Rate = INITIAL_RATE;
        }

        /// <summary>
        /// The GetInstance methods will return the instance of BronzeState.
        /// </summary>
        /// <returns>Return the instance of BrozeState</returns>
        public static BronzeState GetInstance()
        {
            // Check if bronzeState is null.
            if (bronzeState == null)
            {
                // get data object from database.
                bronzeState = db.BronzeStates.SingleOrDefault();
            }

            // Check if bronzeState is still null, that means no record in database, then use constructor to populate the static variable to object.
            if (bronzeState == null)
            {
                bronzeState = new BronzeState();
                db.AccountStates.Add(bronzeState);
                db.SaveChanges();
            }
            
            return bronzeState;
        }

        /// <summary>
        /// This method will change the account state from one state to another according to the account balance value and the upper and lower limit of each of the account state subtypes.
        /// </summary>
        /// <param name="bankAccount">Current bank account</param>
        public override void StateChangeCheck(BankAccount bankAccount)
        {
            BronzeState bronzeState = BronzeState.GetInstance();
            if (bankAccount.Balance > bronzeState.UpperLimit)
            {
                bankAccount.AccountStateId = SilverState.GetInstance().AccountStateId;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// The interest rate that a BankAccount can earn is based in part on the State of the account. 
        /// Each account state subtypes will have different adjustment on the rate value. 
        /// </summary>
        /// <param name="bankAccount">Current bank account.</param>
        /// <returns>Returns the rate that suitable for current account.</returns>
        public override double RateAdjustment(BankAccount bankAccount)
        {
            return (bankAccount.Balance > 0) ? this.Rate : BRONZE_RATE;
        }

    }

    /// <summary>
    /// SilverState Model - the inherit class of AccountState, to represent the Account State
    /// </summary>
    public class SilverState : AccountState
    {
        private static SilverState silverState;
        // Default values.
        private const double LOWER_LIMIT = 5000.0;
        private const double UPPER_LIMIT = 10000.0;
        private const double INITIAL_RATE = 0.0125;

        /// <summary>
        /// The constructor will be use by GetInstance only.
        /// </summary>
        private SilverState()
        {
            this.LowerLimit = LOWER_LIMIT;
            this.UpperLimit = UPPER_LIMIT;
            this.Rate = INITIAL_RATE;
        }

        /// <summary>
        /// The GetInstance methods will return the instance of SilverState.
        /// </summary>
        /// <returns>Return the instance of SilverState</returns>
        public static SilverState GetInstance()
        {
            // Check if bronzeState is null.
            if (silverState == null)
            {
                // get data object from database.
                silverState = db.SilverStates.SingleOrDefault();

                // Check if silverState is still null, that means no record in database, then use constructor to populate the static variable to object.
                if (silverState == null)
                {
                    silverState = new SilverState();
                    db.AccountStates.Add(silverState);
                    db.SaveChanges();
                }
            }

            return silverState;
        }

        /// <summary>
        /// This method will change the account state from one state to another according to the account balance value and the upper and lower limit of each of the account state subtypes.
        /// </summary>
        /// <param name="bankAccount">Current bank account</param>
        public override void StateChangeCheck(BankAccount bankAccount)
        {
            SilverState silverState = SilverState.GetInstance();
            // Check the balance more than the upper limit.
            if (bankAccount.Balance > silverState.UpperLimit)
            {
                bankAccount.AccountStateId = GoldState.GetInstance().AccountStateId;
                db.SaveChanges();
            }
            // Check the balance less than the upper limit.
            if (bankAccount.Balance < silverState.LowerLimit)
            {
                bankAccount.AccountStateId = BronzeState.GetInstance().AccountStateId;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// The interest rate that a BankAccount can earn is based in part on the State of the account. 
        /// Each account state subtypes will have different adjustment on the rate value. 
        /// </summary>
        /// <param name="bankAccount">Current bank account.</param>
        /// <returns>Returns the rate that suitable for current account.</returns>
        public override double RateAdjustment(BankAccount bankAccount)
        {
            return this.Rate;
        }
    }

    /// <summary>
    /// GoldState Model - the inherit class of AccountState, to represent the Account State
    /// </summary>
    public class GoldState : AccountState
    {
        private static GoldState goldState;
        // Default values.
        private const double LOWER_LIMIT = 10000.0;
        private const double UPPER_LIMIT = 20000.0;
        private const double INITIAL_RATE = 0.0200;
        private const double YEAR_BONUS_RATE = 0.01;

        /// <summary>
        /// The constructor will be use by GetInstance only.
        /// </summary>
        private GoldState()
        {
            this.LowerLimit = LOWER_LIMIT;
            this.UpperLimit = UPPER_LIMIT;
            this.Rate = INITIAL_RATE;
        }

        /// <summary>
        /// The GetInstance methods will return the instance of GoldState.
        /// </summary>
        /// <returns>Return the instance of GoldState</returns>
        public static GoldState GetInstance()
        {
            // Check if goldState is null.
            if (goldState == null)
            {
                // Get data object from database.
                goldState = db.GoldStates.SingleOrDefault();

                // Check if goldState is still null, that means no record in database, then use constructor to populate the static variable to object.
                if (goldState == null)
                {
                    goldState = new GoldState();
                    db.AccountStates.Add(goldState);
                    db.SaveChanges();
                }
            }
            return goldState;
        }

        /// <summary>
        /// This method will change the account state from one state to another according to the account balance value and the upper and lower limit of each of the account state subtypes.
        /// </summary>
        /// <param name="bankAccount">Current bank account</param>
        public override void StateChangeCheck(BankAccount bankAccount)
        {
            GoldState goldState = GoldState.GetInstance();
            if (bankAccount.Balance > goldState.UpperLimit)
            {
                bankAccount.AccountStateId = PlatinumState.GetInstance().AccountStateId;
                db.SaveChanges();
            }
            if (bankAccount.Balance < goldState.LowerLimit)
            {
                bankAccount.AccountStateId = SilverState.GetInstance().AccountStateId;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// The interest rate that a BankAccount can earn is based in part on the State of the account. 
        /// Each account state subtypes will have different adjustment on the rate value. 
        /// </summary>
        /// <param name="bankAccount">Current bank account.</param>
        /// <returns>Returns the rate that suitable for current account.</returns>
        public override double RateAdjustment(BankAccount bankAccount)
        {
            DateTime startDate;
            DateTime endDate;
            DateTime tenYears;

            // Get the start date without time.
            startDate = new DateTime(bankAccount.DateCreated.Year, bankAccount.DateCreated.Month, bankAccount.DateCreated.Day);
            endDate = DateTime.Today;
            tenYears = startDate.AddYears(10);

            // If account created more than 10 years, additional rate will be added.
            return (endDate >= tenYears) ? this.Rate + YEAR_BONUS_RATE : this.Rate;
        }
    }

    /// <summary>
    /// PlatinumState Model - the inherit class of AccountState, to represent the Account State
    /// </summary>
    public class PlatinumState : AccountState
    {
        private static PlatinumState platinumState;
        // Default values.
        private const double LOWER_LIMIT = 20000.0;
        private const double UPPER_LIMIT = 0.0;
        private const double INITIAL_RATE = 0.0250;
        private const double YEAR_BONUS_RATE = 0.01;
        private const double DOUBLE_LOWER_LIMIT_BONUS_RATE = 0.005;

        /// <summary>
        /// The constructor will be use by GetInstance only.
        /// </summary>
        private PlatinumState()
        {
            this.LowerLimit = LOWER_LIMIT;
            this.UpperLimit = UPPER_LIMIT;
            this.Rate = INITIAL_RATE;
        }

        /// <summary>
        /// The GetInstance methods will return the instance of GoldState.
        /// </summary>
        /// <returns>Return the instance of GoldState</returns>
        public static PlatinumState GetInstance()
        {
            // Check if platinumState is null.
            if (platinumState == null)
            {
                // get data object from database.
                platinumState = db.PlatinumStates.SingleOrDefault();

                // Check if platinumState is still null, that means no record in database, then use constructor to populate the static variable to object.
                if (platinumState == null)
                {
                    platinumState = new PlatinumState();
                    db.AccountStates.Add(platinumState);
                    db.SaveChanges();
                }
            }

            return platinumState;
        }

        /// <summary>
        /// This method will change the account state from one state to another according to the account balance value and the upper and lower limit of each of the account state subtypes.
        /// </summary>
        /// <param name="bankAccount">Current bank account</param>
        public override void StateChangeCheck(BankAccount bankAccount)
        {
            PlatinumState platinumState = PlatinumState.GetInstance();
            if (bankAccount.Balance < platinumState.LowerLimit)
            {
                bankAccount.AccountStateId = GoldState.GetInstance().AccountStateId;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// The interest rate that a BankAccount can earn is based in part on the State of the account. 
        /// Each account state subtypes will have different adjustment on the rate value. 
        /// </summary>
        /// <param name="bankAccount">Current bank account.</param>
        /// <returns>Returns the rate that suitable for current account.</returns>
        public override double RateAdjustment(BankAccount bankAccount)
        {
            DateTime startDate;
            DateTime endDate;
            DateTime tenYears;

            // Get the start date without time.
            startDate = new DateTime(bankAccount.DateCreated.Year, bankAccount.DateCreated.Month, bankAccount.DateCreated.Day);
            endDate = DateTime.Today;
            tenYears = startDate.AddYears(10);
            double platinumRate = 0.0;

            // If account created more than 10 years, additional rate will be added.
            platinumRate = (endDate >= tenYears) ? this.Rate + YEAR_BONUS_RATE : this.Rate;

            // If the Balance of the account is greater than 2x the LowerLimit, an additional 0.5% will be applied to the interest rate.
            return (bankAccount.Balance > (this.LowerLimit * 2)) ? platinumRate + DOUBLE_LOWER_LIMIT_BONUS_RATE : platinumRate;
        }
    }

    /// <summary>
    /// BankAccount Model - to represent the Bank account information.
    /// </summary>
    public abstract class BankAccount
    {
        private BankOfBIT_KLContext db = new BankOfBIT_KLContext();

        /// <summary>
        /// Bank account ID of the bank account.
        /// </summary>
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int BankAccountId { get; set; }

        /// <summary>
        /// Client ID of the bank account.
        /// </summary>
        [ForeignKey("Client")]
        public int ClientId { get; set; }

        /// <summary>
        /// Account state ID of the bank account.
        /// </summary>
        [ForeignKey("AccountState")]
        public int AccountStateId { get; set; }

        /// <summary>
        /// Account number of the bank account.
        /// </summary>
        [Display(Name = "Account\nNumber")]
        public long AccountNumber { get; set; }

        /// <summary>
        /// Balance of the bank account.
        /// </summary>
        [Required]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public double Balance { get; set; }

        /// <summary>
        /// Account create date of the bank account.
        /// </summary>
        [Required]
        [Display(Name = "Date\nCreated")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Notes of the bank account.
        /// </summary>
        public String Notes { get; set; }

        /// <summary>
        /// Description of the bank account.
        /// </summary>
        public String Description 
        { 
            get
            {
                return BusinessRules.GetTypeNameDescription(GetType().Name, "Account");
            }
        }

        /// <summary>
        /// Account state of the bank account.
        /// </summary>
        // Navigation properties.
        public virtual AccountState AccountState { get; set; }

        /// <summary>
        /// Client of the bank account.
        /// </summary>
        // Navigation properties.
        public virtual Client Client { get; set; }

        /// <summary>
        /// Transactions that belong to this bank account.
        /// </summary>
        // Navigation properties.
        public virtual ICollection<Transaction> Transaction { get; set; }

        /// <summary>
        /// The ChangeState method will initiate the process of ensuring that the BankAccount is always associated with the correct state.
        /// </summary>
        public void ChangeState()
        {
            AccountState accountState;
            do
            {
                accountState = db.AccountStates.Find(this.AccountStateId);
                accountState.StateChangeCheck(this);

                // If the account state changed, then check again to ensure get the correct state.
            } while (accountState.AccountStateId != this.AccountStateId);
            
        }

        /// <summary>
        /// Set next account number.
        /// </summary>
        public abstract void SetNextAccountNumber();
    }

    /// <summary>
    /// SavingsAccount Model - inherit from BankAccount, to represent the Saving Account type
    /// </summary>
    public class SavingsAccount : BankAccount
    {
        /// <summary>
        /// Saving service charges of this account.
        /// </summary>
        [Required]
        [Display(Name = "Savings\nServices\nCharges")]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public double SavingsServiceCharges { get; set; }

        /// <summary>
        /// Set next account number.
        /// </summary>
        public override void SetNextAccountNumber()
        {
            this.AccountNumber = (long)StoredProcedure.NextNumber("Next" + GetType().Name);
        }
    }

    /// <summary>
    /// MortgageAccount Model - inherit from BankAccount, to represent the Mortgage Account type
    /// </summary>
    public class MortgageAccount : BankAccount
    {
        /// <summary>
        /// Mortgage rate of this account.
        /// </summary>
        [Required]
        [Display(Name = "Mortgage\nRate")]
        [DisplayFormat(DataFormatString = "{0:p2}")]
        public double MortgageRate { get; set; }

        /// <summary>
        /// Amortization of this account.
        /// </summary>
        [Required]
        public int Amortization { get; set; }

        /// <summary>
        /// Set next account number.
        /// </summary>
        public override void SetNextAccountNumber()
        {
            this.AccountNumber = (long)StoredProcedure.NextNumber("Next" + GetType().Name);
        }
    }

    /// <summary>
    /// InvestmentAccount Model - inherit from BankAccount, to represent the Investment Account type
    /// </summary>
    public class InvestmentAccount : BankAccount
    {
        /// <summary>
        /// Interest rate of this account.
        /// </summary>
        [Required]
        [Display(Name = "Interest\nRate")]
        [DisplayFormat(DataFormatString = "{0:p2}")]
        public double InterestRate { get; set; }

        /// <summary>
        /// Set next account number.
        /// </summary>
        public override void SetNextAccountNumber()
        {
            this.AccountNumber = (long)StoredProcedure.NextNumber("Next" + GetType().Name);
        }
    }

    /// <summary>
    /// ChequingAccount Model - inherit from BankAccount, to represent the Chequing Account type
    /// </summary>
    public class ChequingAccount : BankAccount
    {
        /// <summary>
        /// Chequing service charges of this account.
        /// </summary>
        [Required]
        [Display(Name = "Chequing\nService\nCharges")]
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public double ChequingServiceCharges { get; set; }

        /// <summary>
        /// Set next account number.
        /// </summary>
        public override void SetNextAccountNumber()
        {
            this.AccountNumber = (long)StoredProcedure.NextNumber("Next" + GetType().Name);
        }
    }

    /// <summary>
    /// Payee.
    /// </summary>
    public class Payee
    {
        /// <summary>
        /// Payee ID.
        /// </summary>
        /// 
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int PayeeId { get; set; }

        /// <summary>
        /// Payee.
        /// </summary>
        [Display(Name = "Payee")]
        [Required]
        public String Description { get; set; }
    }

    /// <summary>
    /// Institution.
    /// </summary>
    public class Institution
    {
        /// <summary>
        /// Institution ID.
        /// </summary>
        /// 
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int InstitutionId { get; set; }

        /// <summary>
        /// Institution number
        /// </summary>
        [Display(Name = "Number")]
        [Required]
        public int InstitutionNumber { get; set; }

        /// <summary>
        /// Institution description
        /// </summary>
        [Display(Name = "Institution")]
        [Required]
        public String Description { get; set; }
    }

    public class TransactionType
    {
        /// <summary>
        /// Transaction Type ID.
        /// </summary>
        /// 
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int TransactionTypeId { get; set; }

        /// <summary>
        /// Transaction Type
        /// </summary>
        [Display(Name = "Type")]
        [Required]
        public String Description { get; set; }

        /// <summary>
        /// The transactions that belong to this transaction type.
        /// </summary>
        // Navigation properties.
        public virtual ICollection<Transaction> Transaction { get; set; }
    }


    /// <summary>
    /// Transaction
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Transaction ID.
        /// </summary>
        /// 
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int TransactionId { get; set; }

        /// <summary>
        /// Bank Account ID.
        /// </summary>
        [ForeignKey("BankAccount")]
        public int BankAccountId { get; set; }

        /// <summary>
        /// Transaction Type ID.
        /// </summary>
        [ForeignKey("TransactionType")]
        public int TransactionTypeId { get; set; }

        /// <summary>
        /// Transaction Number.
        /// </summary>
        [Display(Name = "Number")]
        [Required]
        public long TransactionNumber { get; set; }

        /// <summary>
        /// Deposit.
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public double? Deposit { get; set; }

        /// <summary>
        /// Withdrawal.
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:c2}")]
        public double? Withdrawal { get; set; }

        /// <summary>
        /// Transaction creation date.
        /// </summary>
        [Required]
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Note.
        /// </summary>
        public String Notes { get; set; }

        /// <summary>
        /// Bank account.
        /// </summary>
        // Navigation properties.
        public virtual BankAccount BankAccount { get; set; }

        /// <summary>
        /// Transaction type.
        /// </summary>
        // Navigation properties.
        public virtual TransactionType TransactionType { get; set; }

        public void SetNextTransactionNumber()
        {
            this.TransactionNumber = (long)StoredProcedure.NextNumber("Next" + GetType().Name);
        }

    }

    /// <summary>
    /// Next Unique Number model for generate next available number for specific table.
    /// </summary>
    public abstract class NextUniqueNumber
    {
        protected static BankOfBIT_KLContext db = new BankOfBIT_KLContext();

        /// <summary>
        /// Next Unique Number ID.
        /// </summary>
        /// 
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int NextUniqueNumberId { get; set; }

        /// <summary>
        /// Next Available Number.
        /// </summary>
          [Required]
        public long NextAvailableNumber { get; set; }
    }

    /// <summary>
    /// NextSavingsAccount Model.
    /// </summary>
    public class NextSavingsAccount : NextUniqueNumber
    {
        private static NextSavingsAccount nextSavingsAccount;
        private const int DEFAULT_NEXT_AVAILABLE_NUMBER = 20000;

        /// <summary>
        /// The constructor will be use by GetInstance only.
        /// </summary>
        private NextSavingsAccount()
        {
            this.NextAvailableNumber = DEFAULT_NEXT_AVAILABLE_NUMBER;
        }

        /// <summary>
        /// The GetInstance methods will return the instance of NextSavingsAccount.
        /// </summary>
        /// <returns>Return the instance of NextSavingsAccount</returns>
        public static NextSavingsAccount GetInstance()
        {
            // Check if nextSavingsAccount is null.
            if (nextSavingsAccount == null)
            {
                // get data object from database.
                nextSavingsAccount = db.NextSavingsAccounts.SingleOrDefault();
            }

            // Check if nextSavingsAccount is still null, that means no record in database, then use constructor to populate the static variable to object.
            if (nextSavingsAccount == null)
            {
                nextSavingsAccount = new NextSavingsAccount();
                db.NextUniqueNumbers.Add(nextSavingsAccount);
                db.SaveChanges();
            }

            return nextSavingsAccount;
        }

    }

    /// <summary>
    /// NextInvestmentAccount Model.
    /// </summary>
    public class NextInvestmentAccount : NextUniqueNumber
    {
        private static NextInvestmentAccount nextInvestmentAccount;
        private const int DEFAULT_NEXT_AVAILABLE_NUMBER = 2000000;

        /// <summary>
        /// The constructor will be use by GetInstance only.
        /// </summary>
        private NextInvestmentAccount()
        {
            this.NextAvailableNumber = DEFAULT_NEXT_AVAILABLE_NUMBER;
        }

        /// <summary>
        /// The GetInstance methods will return the instance of NextInvestmentAccount.
        /// </summary>
        /// <returns>Return the instance of NextInvestmentAccount</returns>
        public static NextInvestmentAccount GetInstance()
        {
            // Check if nextInvestmentAccount is null.
            if (nextInvestmentAccount == null)
            {
                // get data object from database.
                nextInvestmentAccount = db.NextInvestmentAccounts.SingleOrDefault();
            }

            // Check if nextInvestmentAccount is still null, that means no record in database, then use constructor to populate the static variable to object.
            if (nextInvestmentAccount == null)
            {
                nextInvestmentAccount = new NextInvestmentAccount();
                db.NextUniqueNumbers.Add(nextInvestmentAccount);
                db.SaveChanges();
            }

            return nextInvestmentAccount;
        }

    }

    /// <summary>
    /// NextMortgageAccount Model.
    /// </summary>
    public class NextMortgageAccount : NextUniqueNumber
    {
        private static NextMortgageAccount nextMortgageAccount;
        private const int DEFAULT_NEXT_AVAILABLE_NUMBER = 200000;

        /// <summary>
        /// The constructor will be use by GetInstance only.
        /// </summary>
        private NextMortgageAccount()
        {
            this.NextAvailableNumber = DEFAULT_NEXT_AVAILABLE_NUMBER;
        }

        /// <summary>
        /// The GetInstance methods will return the instance of NextMortgageAccount.
        /// </summary>
        /// <returns>Return the instance of NextMortgageAccount</returns>
        public static NextMortgageAccount GetInstance()
        {
            // Check if nextMortgageAccount is null.
            if (nextMortgageAccount == null)
            {
                // Get data object from database.
                nextMortgageAccount = db.NextMortgageAccounts.SingleOrDefault();
            }

            // Check if nextMortgageAccount is still null, that means no record in database, then use constructor to populate the static variable to object.
            if (nextMortgageAccount == null)
            {
                nextMortgageAccount = new NextMortgageAccount();
                db.NextUniqueNumbers.Add(nextMortgageAccount);
                db.SaveChanges();
            }

            return nextMortgageAccount;
        }

    }

    /// <summary>
    /// NextChequingAccount Model.
    /// </summary>
    public class NextChequingAccount : NextUniqueNumber
    {
        private static NextChequingAccount nextChequingAccount;
        private const int DEFAULT_NEXT_AVAILABLE_NUMBER = 20000000;

        /// <summary>
        /// The constructor will be use by GetInstance only.
        /// </summary>
        private NextChequingAccount()
        {
            this.NextAvailableNumber = DEFAULT_NEXT_AVAILABLE_NUMBER;
        }

        /// <summary>
        /// The GetInstance methods will return the instance of NextChequingAccount.
        /// </summary>
        /// <returns>Return the instance of NextChequingAccount</returns>
        public static NextChequingAccount GetInstance()
        {
            // Check if nextChequingAccount is null.
            if (nextChequingAccount == null)
            {
                // Get data object from database.
                nextChequingAccount = db.NextChequingAccounts.SingleOrDefault();
            }

            // Check if nextChequingAccount is still null, that means no record in database, then use constructor to populate the static variable to object.
            if (nextChequingAccount == null)
            {
                nextChequingAccount = new NextChequingAccount();
                db.NextUniqueNumbers.Add(nextChequingAccount);
                db.SaveChanges();
            }

            return nextChequingAccount;
        }

    }

    /// <summary>
    /// NextClient Model.
    /// </summary>
    public class NextClient : NextUniqueNumber
    {
        private static NextClient nextClient;
        private const int DEFAULT_NEXT_AVAILABLE_NUMBER = 20000000;

        /// <summary>
        /// The constructor will be use by GetInstance only.
        /// </summary>
        private NextClient()
        {
            this.NextAvailableNumber = DEFAULT_NEXT_AVAILABLE_NUMBER;
        }

        /// <summary>
        /// The GetInstance methods will return the instance of NextClient.
        /// </summary>
        /// <returns>Return the instance of NextClient</returns>
        public static NextClient GetInstance()
        {
            // Check if nextClient is null.
            if (nextClient == null)
            {
                // Get data object from database.
                nextClient = db.NextClients.SingleOrDefault();
            }

            // Check if nextClient is still null, that means no record in database, then use constructor to populate the static variable to object.
            if (nextClient == null)
            {
                nextClient = new NextClient();
                db.NextUniqueNumbers.Add(nextClient);
                db.SaveChanges();
            }

            return nextClient;
        }

    }

    /// <summary>
    /// NextTransaction Model.
    /// </summary>
    public class NextTransaction : NextUniqueNumber
    {
        private static NextTransaction nextTransaction;
        private const int DEFAULT_NEXT_AVAILABLE_NUMBER = 700;

        /// <summary>
        /// The constructor will be use by GetInstance only.
        /// </summary>
        private NextTransaction()
        {
            this.NextAvailableNumber = DEFAULT_NEXT_AVAILABLE_NUMBER;
        }

        /// <summary>
        /// The GetInstance methods will return the instance of NextTransaction.
        /// </summary>
        /// <returns>Return the instance of NextTransaction</returns>
        public static NextTransaction GetInstance()
        {
            // Check if nextTransaction is null.
            if (nextTransaction == null)
            {
                // Get data object from database.
                nextTransaction = db.NextTransactions.SingleOrDefault();
            }

            // Check if nextTransaction is still null, that means no record in database, then use constructor to populate the static variable to object.
            if (nextTransaction == null)
            {
                nextTransaction = new NextTransaction();
                db.NextUniqueNumbers.Add(nextTransaction);
                db.SaveChanges();
            }

            return nextTransaction;
        }

    }

    /// <summary>
    /// Model for execute stored procedures.
    /// </summary>
    public static class StoredProcedure
    {
        /// <summary>
        /// Generate next number.
        /// </summary>
        /// <param name="discriminator"></param>
        /// <returns>Returns the next number.</returns>
        public static long? NextNumber(string discriminator)
        {
            try
            {
                // Create connection object to establish the connection to the database.
                using (SqlConnection connection = new SqlConnection("Data Source=localhost; Initial Catalog=BankOfBIT_KLContext;Integrated Security=True"))
                { 
                    // Initialize the return Value.
                    long? returnValue = 0;
                    // Create the SQLCommand object to call the stored procedure (next_number) from the connected database.
                    SqlCommand storedProcedure = new SqlCommand("next_number", connection);
                    // Specify the command type is Stored Procedure.
                    storedProcedure.CommandType = CommandType.StoredProcedure;
                    // Specify the method argument as the stored procedure Input parameter and add it into the SQLCommand object.
                    storedProcedure.Parameters.AddWithValue("@Discriminator", discriminator);
                    // Create and specify the Output parameter and it's type, so after finished the execution of stored procedure, we could capture the output value from this parameter.
                    SqlParameter outputParameter = new SqlParameter("@NewVal", SqlDbType.BigInt)
                    {
                        // Specify this is a output parameter.
                        Direction = ParameterDirection.Output
                    };
                    // Add the output parameter into the SQLcommand object.
                    storedProcedure.Parameters.Add(outputParameter);
                    // Connecting to database.
                    connection.Open();
                    // Execute the stored procedure
                    storedProcedure.ExecuteNonQuery();
                    // Close the database connection.
                    connection.Close();
                    // Get the return value.
                    returnValue = (long?)outputParameter.Value;
                    return returnValue;
                }
            }
            catch (Exception){
                // Returns null, if exception occurs.
                return null;
            }
        }
    }
}