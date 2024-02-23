<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateTransaction.aspx.cs" Inherits="OnlineBanking.CreateTransaction" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <br />
        Account Number:&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblAccountNumber" runat="server"></asp:Label>
    </p>
    <p>
        Balance:&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblAccountBalance" runat="server" ></asp:Label>
    </p>
    <p>
        Transaction Type:&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="ddlTransactionType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTransactionType_SelectedIndexChanged">
        </asp:DropDownList>
    </p>
    <p>
        Amount:&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="txtAmount" runat="server" ></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ControlToValidate="txtAmount" Display="Dynamic" ErrorMessage="Amount is Required"></asp:RequiredFieldValidator>
        <asp:RangeValidator ID="rvAmount" runat="server" ControlToValidate="txtAmount" Display="Dynamic" ErrorMessage="Amount should between $0.01 and $10,000.00" MaximumValue="10000.00" MinimumValue="0.01" Type="Currency"></asp:RangeValidator>
    </p>
    <p>
        To:&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="ddlPayee" runat="server">
        </asp:DropDownList>
    </p>
    <p>
        <asp:LinkButton ID="lbCompleteTransaction" runat="server" OnClick="lbCompleteTransaction_Click">Complete Transaction</asp:LinkButton>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:LinkButton ID="lbReturnToAccountListing" runat="server" CausesValidation="False" OnClick="lbReturnToAccountListing_Click">Return to Account Listing</asp:LinkButton>
    </p>
    <p>
        <asp:Label ID="lblError" runat="server" Visible="False"></asp:Label>
    </p>
</asp:Content>
