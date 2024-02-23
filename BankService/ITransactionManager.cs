using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BankService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITransactionManager" in both code and config file together.
    [ServiceContract]
    public interface ITransactionManager
    {
        [OperationContract]
        void DoWork();

        /// <summary>
        /// Implement the deposit function.
        /// </summary>
        /// <param name="accountId">Account going to deposit</param>
        /// <param name="amount">Deposit amount</param>
        /// <param name="notes">notes</param>
        /// <returns>Returns the updated balance after deposit.</returns>
        [OperationContract]
        double? Deposit(int accountId, double amount, string notes);

        /// <summary>
        /// Implement the withdrawal function.
        /// </summary>
        /// <param name="accountId">Account going to withdrawal</param>
        /// <param name="amount">Deposit amount</param>
        /// <param name="notes">notes</param>
        /// <returns>Returns the updated balance after withdrawal.</returns>
        [OperationContract]
        double? Withdrawal(int accountId, double amount, string notes);

        /// <summary>
        /// Implement the bill payment function.
        /// </summary>
        /// <param name="accountId">Account going to pay the bill.</param>
        /// <param name="amount">Bill payment amount</param>
        /// <param name="notes">notes</param>
        /// <returns>Returns the updated balance after bill paid.</returns>
        [OperationContract]
        double? BillPayment(int accountId, double amount, string notes);

        /// <summary>
        /// Implement the transfer function allow system transfer money from one account to another account. 
        /// </summary>
        /// <param name="fromAccountId">Transfer bank account</param>
        /// <param name="toAccountId">Recipient bank account</param>
        /// <param name="amount">Transfer amount</param>
        /// <param name="notes">notes</param>
        /// <returns>Returns the updated from account's balance after transfer.</returns>
        [OperationContract]
        double? Transfer(int fromAccountId, int toAccountId, double amount, string notes);

        /// <summary>
        /// Implement the interest calculation method to calculate the interest of a bank account.
        /// </summary>
        /// <param name="accountId">Account for calculate the interest.</param>
        /// <param name="notes">Notes</param>
        /// <returns>Returns the updated balance after added the interest amount.</returns>
        [OperationContract]
        double? CalculateInterest(int accountId, string notes);
    }
}
