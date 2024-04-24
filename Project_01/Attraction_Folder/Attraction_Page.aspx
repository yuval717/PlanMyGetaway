<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Attraction_Page.aspx.cs" Inherits="Project_01.Attraction_Page" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 50%;
        }
        .auto-style2 {
            height: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Attraction_Name" runat="server" Text=""></asp:Label>
            <br />
            <asp:Label ID="Attraction_Type" runat="server" Text=""></asp:Label>
            <br />
            <asp:Label ID="Attraction_Age" runat="server" Text=""></asp:Label>
            <br />
            <asp:Label ID="Attraction_Price" runat="server" Text=""></asp:Label>
            <br />
            <asp:Label ID="Attraction_Duration" runat="server" Text=""></asp:Label>
            <br />
            <asp:Label ID="Attraction_Address" runat="server" Text=""></asp:Label>
            <br />
            <asp:Label ID="Attraction_Gmail" runat="server" Text=""></asp:Label>
            <br />
            <asp:Label ID="Attraction_PhonNumber" runat="server" Text=""></asp:Label>
            <br />
            <asp:Label ID="Attraction_Text" runat="server" Text=""></asp:Label>
            <br />
            <asp:Image ID="Image1" runat="server" ImageUrl="" CssClass="auto-style1"/>
            <br />
            <br />
            <br />
            <asp:Button ID="Back" runat="server" Text="BACK" OnClick="Back_Click" />
        </div>
    </form>
</body>
</html>
