<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Attraction_Edit_Register.aspx.cs" Inherits="Project_01.Attraction_Edit_Register"  Async="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label2" runat="server" Text="Attraction Name"></asp:Label>
            <asp:TextBox ID="Attraction_Name" runat="server" ></asp:TextBox>
            <br />
            <asp:Label ID="Label3" runat="server" Text="Attraction Type"></asp:Label>
            <asp:DropDownList ID="Attraction_Type" runat="server" AutoPostBack=true ></asp:DropDownList>
            <br />
            <asp:Label ID="Label4" runat="server" Text="Attraction MinAge"></asp:Label>
            <asp:TextBox ID="Attraction_MinAge" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label5" runat="server" Text="Attraction MaxAge"></asp:Label>
            <asp:TextBox ID="Attraction_MaxAge" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label6" runat="server" Text="Attraction Price"></asp:Label>
            <asp:TextBox ID="Attraction_Price" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label7" runat="server" Text="Attraction Duration"></asp:Label>
            <asp:TextBox ID="Attraction_Duration" runat="server" Height="25px" Width="166px"></asp:TextBox>
            <br />
            <asp:Label ID="Label9" runat="server" Text="Attraction Address"></asp:Label>
            <asp:TextBox ID="Attraction_Address" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label10" runat="server" Text="Attraction Gmail"></asp:Label>
            <asp:TextBox ID="Attraction_Gmail" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label11" runat="server" Text="Attraction PhonNumber"></asp:Label>
            <asp:TextBox ID="Attraction_PhonNumber" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label12" runat="server" Text="Attraction recommendedMonth"></asp:Label>
            <asp:DropDownList ID="Attraction_RecommendedMonth" runat="server" AutoPostBack=true >
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
                <asp:ListItem>12</asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:Label ID="Label13" runat="server" Text="Attraction FreeEntry"></asp:Label>
            <asp:DropDownList ID="Attraction_FreeEntry" runat="server" AutoPostBack=true CssClass="auto-style1">
                <asp:ListItem Value="true">כן</asp:ListItem>
                <asp:ListItem Value="false">לא</asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:Label ID="Label14" runat="server" Text="Attraction Text"></asp:Label>
            <asp:TextBox ID="Attraction_Text" runat="server" ></asp:TextBox>
            <br />
            <asp:Label ID="Label15" runat="server" Text="Attraction Photo"></asp:Label>
            <asp:FileUpload ID="FileUpload1" runat="server" />
            <br />
            <asp:Label ID="Label16" runat="server" Text=""></asp:Label>
            <br />
            <br />
            <asp:Button ID="Button1" runat="server" Text="Register" OnClick="Button1_Click"/>
            <br />
            <asp:Label ID="Result" runat="server" Text=""></asp:Label>
        </div>
    </form>
</body>
</html>
