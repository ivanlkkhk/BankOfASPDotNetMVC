using BankOfBIT_KL.Data;
using BankOfBIT_KL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineBanking
{
    public partial class TransactionListing : System.Web.UI.Page
    {
        BankOfBIT_KLContext db = new BankOfBIT_KLContext();

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
                        //Assign the client name on screen.
                        Client client = (Client)Session["Client"];
                        lblClientName.Text = client.FullName;

                        int selectedAccountNumber = int.Parse((string)Session["SelectedAccountNumber"]);

                        // Retrieves the bankAccount object by using account number.
                        BankAccount bankAccount = db.BankAccounts
                                .Where(x => x.AccountNumber == selectedAccountNumber)
                                .SingleOrDefault();

                        lblAccountNumber.Text = selectedAccountNumber.ToString();
                        lblAccountBalance.Text = bankAccount.Balance.ToString("C2");
                        Session["SelectedBankAccounts"] = bankAccount;

                        // Retrieves the transactions object by using account number.
                        IQueryable<Transaction> transactions = db.Transactions
                                .Where(x => x.BankAccount.AccountNumber == selectedAccountNumber);
                        gvTransactions.DataSource = transactions.ToList();
                        this.DataBind();

                    }
                    catch (ArgumentNullException)
                    {
                        lblError.Text = "Exception: Client number not exist or No accounts for this client.";
                        lblError.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "Exception: " + ex.Message;
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
        /// Click's event of Return to Bank Account List button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbReturnToBankAccountList_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AccountListing.aspx");
        }

        /// <summary>
        /// Click's event of Pay bills and Transfer funds button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbPayBillsAndTransferFunds_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/CreateTransaction.aspx");
        }
    }
}