<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Attraction_Register.aspx.cs" Inherits="Project_01.Attraction_Register" Async="true"%>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            margin-bottom: 0px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label2" runat="server" Text="Attraction Name"></asp:Label>
            <asp:TextBox ID="Attraction_Name" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label3" runat="server" Text="Attraction Type"></asp:Label>
            <asp:DropDownList ID="Attraction_Type" runat="server" AutoPostBack=true OnSelectedIndexChanged="Attraction_Type_SelectedIndexChanged" ></asp:DropDownList>
            <br />
            <asp:Label ID="Label4" runat="server" Text="Attraction MinAge"></asp:Label>
            <asp:TextBox ID="Attraction_MinAge" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label5" runat="server" Text="Attraction MaxAge"></asp:Label>
            <asp:TextBox ID="Attraction_MaxAge" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label9" runat="server" Text="Attraction Address"></asp:Label>
            <asp:TextBox ID="Attraction_Address" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label14" runat="server" Text="Attraction Text"></asp:Label>
            <asp:TextBox ID="Attraction_Text" runat="server" ></asp:TextBox>
            <br />
            <asp:Label ID="Label15" runat="server" Text="Attraction Photo"></asp:Label>
            <asp:FileUpload ID="FileUpload1" runat="server" />
            <br />
            <asp:Label ID="Label16" runat="server" Text=""></asp:Label>
            <br />

            <asp:Label ID="LabelOPH" runat="server" Text="Attraction Opening hour" Visible ="false"></asp:Label>
            <asp:TextBox ID="IndoorAttraction_OpeningHour" runat="server" Visible ="false"></asp:TextBox>
            <br />
            <asp:Label ID="LabelCLH" runat="server" Text="Attraction Closing hour" Visible ="false" ></asp:Label>
            <asp:TextBox ID="IndoorAttraction_ClosingHour" runat="server" Visible ="false"></asp:TextBox>
            <br />
            <asp:Label ID="LabePR" runat="server" Text="Attraction Price" Visible ="false"></asp:Label>
            <asp:TextBox ID="Attraction_Price" runat="server" Visible ="false"></asp:TextBox>
            <br />
            <asp:Label ID="LabeDU" runat="server" Text="Attraction Duration" Visible ="false"></asp:Label>
            <asp:TextBox ID="Attraction_Duration" runat="server" Visible ="false"></asp:TextBox>
            <br />
            <asp:Label ID="LabelFR" runat="server" Text="Attraction FreeEntry" Visible ="false"></asp:Label>
            <asp:DropDownList ID="Attraction_FreeEntry" runat="server" AutoPostBack=true CssClass="auto-style1" Visible ="false">
                <asp:ListItem Value="true">כן</asp:ListItem>
                <asp:ListItem Value="false">לא</asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:Label ID="LabelPH" runat="server" Text="Attraction PhonNumber" Visible ="false"></asp:Label>
            <asp:TextBox ID="Attraction_PhonNumber" runat="server" Visible ="false"></asp:TextBox>
            <br />
            <asp:Label ID="LabelGM" runat="server" Text="Attraction Gmail" Visible ="false"></asp:Label>
            <asp:TextBox ID="Attraction_Gmail" runat="server" Visible ="false"></asp:TextBox>
            <br />

            <asp:Label ID="LabelKIN" runat="server" Text="Attraction Kilometers Number" Visible ="false"></asp:Label>
            <asp:TextBox ID="NatureAttraction_KilometersNumber" runat="server" Visible ="false"></asp:TextBox>
            <br />
            <asp:Label ID="LabelDI" runat="server" Text="Attraction Difficulty" Visible ="false"></asp:Label>
            <asp:TextBox ID="NatureAttraction_Difficulty" runat="server" Visible ="false"></asp:TextBox>
            <br />


            <asp:Button ID="Button1" runat="server" Text="Register" OnClick="Button1_Click"/>
            <br />
            <asp:Label ID="Result" runat="server" Text=""></asp:Label>
            <br />
        </div>
    </form>
</body>
</html>
