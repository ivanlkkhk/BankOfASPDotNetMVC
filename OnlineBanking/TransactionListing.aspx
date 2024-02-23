<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TransactionListing.aspx.cs" Inherits="OnlineBanking.TransactionListing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        &nbsp;</p>
    <p>
        <asp:Label ID="lblClientName" runat="server" Font-Bold="True"></asp:Label>
    </p>
    <p>
        Account Number:
        <asp:Label ID="lblAccountNumber" runat="server"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp; Balance:
        <asp:Label ID="lblAccountBalance" runat="server"></asp:Label>
    </p>
    <p>
        <asp:GridView ID="gvTransactions" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2" ForeColor="Black">
            <Columns>
                <asp:BoundField DataField="DateCreated" DataFormatString="{0:d}" HeaderText="Date">
                <ItemStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="TransactionType.Description" HeaderText="Transaction Type" />
                <asp:BoundField DataField="Deposit" DataFormatString="{0:c}" HeaderText="Amount In" NullDisplayText ="$0.00">
                <ItemStyle HorizontalAlign="Right" Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="Withdrawal" DataFormatString="{0:c}" HeaderText="Account Out" NullDisplayText ="$0.00">
                <ItemStyle HorizontalAlign="Right" Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="Notes" HeaderText="Details">
                <ItemStyle Width="400px" />
                </asp:BoundField>
            </Columns>
            <FooterStyle BackColor="#CCCCCC" />
            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
            <RowStyle BackColor="White" />
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="Gray" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#383838" />
        </asp:GridView>
    </p>
    <p>
        <asp:LinkButton ID="lbPayBillsAndTransferFunds" runat="server" OnClick="lbPayBillsAndTransferFunds_Click">Pay Bills and Transfer Funds</asp:LinkButton>
&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:LinkButton ID="lbReturnToBankAccountList" runat="server" OnClick="lbReturnToBankAccountList_Click">Return to Account Listing</asp:LinkButton>
    </p>
    <p>
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </p>
</asp:Content>
