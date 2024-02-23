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
    public partial class AccountListing : System.Web.UI.Page
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
                        // Obtains the client number from user id.
                        String clientNo = Page.User.Identity.Name.Substring(0, Page.User.Identity.Name.IndexOf("@"));
                        int clientNumber = int.Parse(clientNo);

                        // Retrieves the client object by using client number.
                        Client client = db.Clients
                                .Where(x => x.ClientNumber == clientNumber)
                                .SingleOrDefault();

                        //Assign the client name on screen and save client information to Session variable.
                        
                        lblClientName.Text = client.FullName;

                        // Retrieve bank accounts by using client number and bind data to girdview control.
                        IQueryable<BankAccount> bankAccounts = db.BankAccounts
                            .Where(ba => ba.Client.ClientNumber == clientNumber);
                        gvAccountList.DataSource = bankAccounts.ToList();
                        this.DataBind();

                        //Save Client and BankAccounts records to session.
                        Session["Client"] = client;
                        Session["BankAccounts"] = bankAccounts;
                    }
                    catch (ArgumentNullException)
                    {
                        lblError.Text = "Client number not exist or No accounts for this client." ;
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
        /// Selected Index Changed event of GridView Account List. It will take the account number and redirect to TransactionList page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAccountList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Set selected Account Number to session variable. 
            Session["SelectedAccountNumber"] = gvAccountList.Rows[gvAccountList.SelectedIndex].Cells[1].Text;
            Response.Redirect("~/TransactionListing.aspx");
        }
    }
}