<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pay.aspx.cs" Inherits="Project_01.Pay" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label4" runat="server" Text="סוג אשראי"></asp:Label>
            <asp:DropDownList ID="Provider" runat="server">
                <asp:ListItem>ויזה</asp:ListItem>
                <asp:ListItem>אמריקן אקספרס</asp:ListItem>
                <asp:ListItem>מאסטר קארד</asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:Label ID="Label2" runat="server" Text="מספר כרטיס"></asp:Label>
            <asp:TextBox ID="Number" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label1" runat="server" Text="תאריך תפוגה"></asp:Label>
            <asp:TextBox ID="DateOfExpiration" runat="server" TextMode ="month"></asp:TextBox>
            <br />
            <asp:Label ID="Label5" runat="server" Text="CVV"></asp:Label>
            <asp:TextBox ID="CVV" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label3" runat="server" Text="תעודת זהות"></asp:Label>
            <asp:TextBox ID="Owner_ID" runat="server"></asp:TextBox>
            <br />
            <asp:Button ID="enter" runat="server" Text="Button" OnClick="enter_Click" />
        </div>
    </form>
</body>
</html>
