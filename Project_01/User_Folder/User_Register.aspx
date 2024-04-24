<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_Register.aspx.cs" Inherits="Project_01.User_Register" %>

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
            <asp:Label ID="Label3" runat="server" Text="User FirstName"></asp:Label>
            <asp:TextBox ID="User_FirstName" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label4" runat="server" Text="User LastName"></asp:Label>
            <asp:TextBox ID="User_LastName" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label5" runat="server" Text="User Gmail"></asp:Label>
            <asp:TextBox ID="User_Gmail" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label6" runat="server" Text="User PhoneNumber"></asp:Label>
            <asp:TextBox ID="User_PhoneNumber" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label7" runat="server" Text="User Gender"></asp:Label>
            <asp:DropDownList ID="User_Gender" runat="server">
                <asp:ListItem>זכר </asp:ListItem>
                <asp:ListItem>נקבה</asp:ListItem>
                <asp:ListItem>אחר</asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:Label ID="Label8" runat="server" Text="User Country"></asp:Label>
            <asp:DropDownList ID="User_Country" runat="server"></asp:DropDownList>
            <br />
            <asp:Label ID="Label15" runat="server" Text="User Photo"></asp:Label>
            <asp:FileUpload ID="FileUpload1" runat="server" />
            <br />
            <asp:Label ID="Label16" runat="server" Text=""></asp:Label>
            <br />
            <asp:Button ID="Button1" runat="server" Text="enter" OnClick="Button1_Click" />
            <br />
            <asp:Label ID="Result" runat="server" Text=""></asp:Label>
        </div>
    </form>
</body>
</html>
