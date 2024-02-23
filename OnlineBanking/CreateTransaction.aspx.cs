using BankOfBIT_KL.Data;
using BankOfBIT_KL.Models;
using Microsoft.Ajax.Utilities;
using OnlineBanking.TransactionManagerServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Util;
using Utility;

namespace OnlineBanking
{
    public partial class CreateTransaction : System.Web.UI.Page
    {
        BankOfBIT_KLContext db = new BankOfBIT_KLContext();
        const int BILL_PAYMENT = (int)TransactionTypeValues.BILL_PAYMENT;
        const int TRANSFER = (int)TransactionTypeValues.TRANSFER;

        /// <summary>
        /// Page initialization fore prepare initial data on screen. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblError.Visible = false;

            if (!IsPostBack)
            {
                if (this.Page.User.Identity.IsAuthenticated)
                {
                    try
                    {
                        int selectedAccountNumber = int.Parse((string)Session["SelectedAccountNumber"]);

                        // Retrieves the bankaccount object by using account number.
                        BankAccount bankAccount = db.BankAccounts
                                .Where(x => x.AccountNumber == selectedAccountNumber)
                                .SingleOrDefault();

                        lblAccountNumber.Text = selectedAccountNumber.ToString();
                        lblAccountBalance.Text = bankAccount.Balance.ToString("C2");

                        // Retrieves Transaction Type and bind data on dropdownlist.
                        IQueryable<TransactionType> transactionTypes = db.TransactionTypes.Where(x => x.TransactionTypeId >= BILL_PAYMENT && x.TransactionTypeId <= TRANSFER);
                       
                        ddlTransactionType.DataSource = transactionTypes.ToList();
                        ddlTransactionType.DataTextField = "Description";
                        ddlTransactionType.DataValueField = "TransactionTypeId";

                        this.PayeeDataBind();
                        this.DataBind();

                        // Format the amount on text box is align to right.
                        this.txtAmount.Style.Add("text-align", "right");
                    }
                    catch (ArgumentNullException)
                    {
                        lblError.Text = "Account number does not exist.";
                        lblError.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = ex.Message;
                        lblError.Visible = true;
                    }
                }
                else
                {
                    Response.Redirect("~/Account/Login.aspx");
                }
            }
        }

        /// <summary>
        /// Clear data binding from Payee's dropdroplist control.
        /// </summary>
        protected void ClearPayeeDataBind()
        {
            ddlPayee.DataSource = null;
            ddlPayee.DataTextField = null;
            ddlPayee.DataValueField = null;
        }

        /// <summary>
        /// Binding from Payee data on Payees's dropdroplist control.
        /// </summary>
        protected void PayeeDataBind()
        {
            // Retrieves Payee and bind data on dropdownlist.
            IQueryable<Payee> payees = db.Payees;
            ddlPayee.DataSource = payees.ToList();
            ddlPayee.DataTextField = "Description";
            ddlPayee.DataValueField = "PayeeId";
            ddlPayee.DataBind();
        }

        /// <summary>
        /// Binding from BankAccount data on Payees's dropdroplist control.
        /// </summary>
        protected void BankaccountDataBind()
        {
            int selectedAccountNumber = int.Parse((string)Session["SelectedAccountNumber"]);
            Client client = (Client)Session["Client"];
            long clientNumber = client.ClientNumber;

            // Retrieve bank accounts by using client number and bind data to girdview control.
            IQueryable<BankAccount> bankAccounts = db.BankAccounts
                .Where(ba => ba.Client.ClientNumber == clientNumber && ba.AccountNumber != selectedAccountNumber);
            
            ddlPayee.DataSource = bankAccounts.ToList();
            ddlPayee.DataTextField = "AccountNumber";
            ddlPayee.DataValueField = "BankAccountId";
            ddlPayee.DataBind();

        }

        /// <summary>
        /// Click's event on Return to account listing's Link Button control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbReturnToAccountListing_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AccountListing.aspx");
        }

        protected void ddlTransactionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // The data bind setting on Payee's dropdownlist control will be change according to the selected on TransactionType's dropdownlist control
            int currentAction = int.Parse(ddlTransactionType.SelectedItem.Value);
            ClearPayeeDataBind();

            switch (currentAction)
            {
                case BILL_PAYMENT:
                    PayeeDataBind();
                    break;
                case TRANSFER:
                    BankaccountDataBind();
                    break;
            }
        }

        /// <summary>
        /// Update balance after finished the transaction. 
        /// </summary>
        /// <param name="updatedBalance"></param>
        /// <param name="transactionType"></param>
        protected void UpdateBalanceText(double? updatedBalance, string transactionType)
        {
            if (updatedBalance == null)
            {
                lblError.Visible = true;
                lblError.Text = "System unable to process the " + transactionType + " transaction.";
            }
            else
            {
                lblAccountBalance.Text = ((double)updatedBalance).ToString("C2");
            }
        }

        /// <summary>
        /// Complete the transaction after Complete transaction link button was clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbCompleteTransaction_Click(object sender, EventArgs e)
        {
            // Initial WCF services.
            TransactionManagerServiceReference.TransactionManagerClient transactionManagerClient = new TransactionManagerServiceReference.TransactionManagerClient();

            // Enable the amount required field validator, and trigger the page validation. 
            rfvAmount.Enabled = true;
            Page.Validate();
            
            if (Page.IsValid)
            {
                lblError.Visible = false;
                string notes;
                int selectedAccountNumber = int.Parse((string)Session["SelectedAccountNumber"]);
                BankAccount bankAccount = db.BankAccounts
                    .Where(x => x.AccountNumber == selectedAccountNumber)
                    .SingleOrDefault();
                double balance = bankAccount.Balance;
                double amount = double.Parse(txtAmount.Text);
                int currentAction = int.Parse(ddlTransactionType.SelectedItem.Value);

                // Validate there are sufficient funds for complete the transaction. 
                if (balance < amount)
                {
                    lblError.Visible = true;
                    lblError.Text = "Insufficient funds to complete the transaction.";
                    return;
                }

                // Determine what transaction type is performing. 
                switch (currentAction)
                {
                    case BILL_PAYMENT:
                        notes = "Online Banking Payment to : " + ddlPayee.SelectedItem.Text;
                        try
                        {
                            double? updatedBalance = transactionManagerClient.BillPayment(bankAccount.BankAccountId, amount, notes);
                            this.UpdateBalanceText(updatedBalance, "Bill Payment");
                        }
                        catch (Exception ex)
                        {
                            lblError.Visible = true;
                            lblError.Text = ex.Message;
                        }
                        break;
                    case TRANSFER:
                        int toAccountId = int.Parse(ddlPayee.Text);
                        notes = "Online Banking Transfer From: " + bankAccount.AccountNumber + " to : " + ddlPayee.SelectedItem.Text;
                        try
                        {
                            double? updatedBalance = transactionManagerClient.Transfer(bankAccount.BankAccountId, toAccountId, amount, notes);
                            this.UpdateBalanceText(updatedBalance, "Transfer");
                        }
                        catch (Exception ex)
                        {
                            lblError.Visible = true;
                            lblError.Text = ex.Message;
                        }
                        break;
                }
            }
            
        }
    }
}