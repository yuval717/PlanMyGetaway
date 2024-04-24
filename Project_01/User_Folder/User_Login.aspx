<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_Login.aspx.cs" Inherits="Project_01.User_Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="User Name"></asp:Label>
            <asp:TextBox ID="User_Name" runat="server" ></asp:TextBox>
            <br />
            <asp:Label ID="Label2" runat="server" Text="User Password"></asp:Label>
            <asp:TextBox ID="User_Password" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
            <br />
            <asp:Label ID="Result" runat="server" Text=""></asp:Label>
        </div>
    </form>
</body>
</html>
