using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BankOfBIT_KL.Data;
using BankOfBIT_KL.Models;
using Utility;

namespace BankService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TransactionManager" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TransactionManager.svc or TransactionManager.svc.cs at the Solution Explorer and start debugging.
    public class TransactionManager : ITransactionManager
    {
        protected static BankOfBIT_KLContext db = new BankOfBIT_KLContext();

        /// <summary>
        /// Implement the bill payment function.
        /// </summary>
        /// <param name="accountId">Account going to pay the bill.</param>
        /// <param name="amount">Bill payment amount</param>
        /// <param name="notes">notes</param>
        /// <returns>Returns the updated balance after bill paid.</returns>
        public double? BillPayment(int accountId, double amount, string notes)
        {
            double? balance;
            if (amount >= 0) amount *= -1;
            try
            {
                balance = UpdateBalance(accountId, amount);
                CreateTransaction(accountId, amount, (int)TransactionTypeValues.BILL_PAYMENT, notes);
                return balance;
            }
            catch (Exception)
            {
                return null;
            }

        }

        /// <summary>
        /// Implement the interest calculation method to calculate the interest of a bank account.
        /// </summary>
        /// <param name="accountId">Account for calculate the interest.</param>
        /// <param name="notes">Notes</param>
        /// <returns>Returns the updated balance after added the interest amount.</returns>
        public double? CalculateInterest(int accountId, string notes)
        {
            try
            {
                double? balance;
                BankAccount bankAcount = db.BankAccounts.Find(accountId);
                AccountState accountState = bankAcount.AccountState;
                double rate = accountState.RateAdjustment(bankAcount);
                double interest = (rate * bankAcount.Balance * 1) / 12;

                balance = UpdateBalance(accountId, interest);
                CreateTransaction(accountId, interest, (int)TransactionTypeValues.INTEREST, notes);
                return balance;
            }
            catch (Exception)
            {
                return null;
            }

        }

        /// <summary>
        /// Implement the deposit function.
        /// </summary>
        /// <param name="accountId">Account going to deposit</param>
        /// <param name="amount">Deposit amount</param>
        /// <param name="notes">notes</param>
        /// <returns>Returns the updated balance after deposit.</returns>
        public double? Deposit(int accountId, double amount, string notes)
        {
            double? balance;
            if (amount <= 0) amount = Math.Abs(amount);
            try
            {
                balance = UpdateBalance(accountId, amount);
                CreateTransaction(accountId, amount, (int)TransactionTypeValues.DEPOSIT, notes);
                return balance;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void DoWork()
        {
        }

        /// <summary>
        /// Implement the transfer function allow system transfer money from one account to another account. 
        /// </summary>
        /// <param name="fromAccountId">Transfer bank account</param>
        /// <param name="toAccountId">Recipient bank account</param>
        /// <param name="amount">Transfer amount</param>
        /// <param name="notes">notes</param>
        /// <returns>Returns the updated from account's balance after transfer.</returns>
        public double? Transfer(int fromAccountId, int toAccountId, double amount, string notes)
        {
            double? fromAccountBalance;
            double fromAmount = Math.Abs(amount) * -1;
            double toAmount = Math.Abs(amount);
            try
            {
                //Transfer from 
                fromAccountBalance = UpdateBalance(fromAccountId, fromAmount);
                CreateTransaction(fromAccountId, fromAmount, (int)TransactionTypeValues.TRANSFER, notes);

                //Transfer to
                UpdateBalance(toAccountId, toAmount);
                CreateTransaction(toAccountId, toAmount, (int)TransactionTypeValues.TRANSFER_RECIPIENT, notes);

                return fromAccountBalance;
            }
            catch (Exception)
            {
                return null;
            }

            
        }

        /// <summary>
        /// Implement the withdrawal function.
        /// </summary>
        /// <param name="accountId">Account going to withdrawal</param>
        /// <param name="amount">Deposit amount</param>
        /// <param name="notes">notes</param>
        /// <returns>Returns the updated balance after withdrawal.</returns>
        public double? Withdrawal(int accountId, double amount, string notes)
        {
            double? balance;
            if (amount >= 0) amount *= -1;
            try
            {
                balance = UpdateBalance(accountId, amount);
                CreateTransaction(accountId, amount, (int)TransactionTypeValues.WITHDRAWAL, notes);
                return balance;
            }
            catch (Exception)
            {
                return null;
            }

        }

        /// <summary>
        /// Update bank account's balance by adding the value of amount argument. 
        /// </summary>
        /// <param name="accountId">Bank account for update the balance</param>
        /// <param name="amount">New amount</param>
        /// <returns>Returns the new balance.</returns>
        private double? UpdateBalance(int accountId, double amount)
        {
            try
            {
                BankAccount bankAccount = db.BankAccounts.Find(accountId);
                bankAccount.Balance += amount;
                //db.Entry(bankAccount).State = EntityState.Modified;
                db.SaveChanges();
                bankAccount.ChangeState();
                return bankAccount.Balance;
            }catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Implement the function to handle new Deposit and Withdrawal transactions.
        /// </summary>
        /// <param name="accountId">Bank account's to be deposit/withdrawal.</param>
        /// <param name="amount">Amount of the transaction</param>
        /// <param name="transactionTypeId">Transaction type</param>
        /// <param name="notes">Notes of the transaction</param>
        private void CreateTransaction(int accountId, double amount, int transactionTypeId, string notes)
        {
            Transaction transaction = new Transaction();
            transaction.TransactionTypeId = transactionTypeId;
            transaction.BankAccount = db.BankAccounts.Find(accountId);
            transaction.Notes = notes;

            if (amount < 0)
            {
                transaction.Deposit = null;
                transaction.Withdrawal = Math.Abs(amount);
            }
            else
            {
                transaction.Deposit = amount;
                transaction.Withdrawal = null;
            }

            transaction.DateCreated = DateTime.Now;
            transaction.SetNextTransactionNumber();
            db.Transactions.Add(transaction);
            db.SaveChanges();
        }
    }
}
