<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="User_Edit.aspx.cs" Inherits="Project_01.User_Edit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="User_Edit.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%-- תצוגת שם משתמש --%>
    <asp:Label ID="MasterPage_UserName" runat="server" Text="" Style="position:absolute; top:30px; left:15px; margin: 0 20px; text-decoration: none; color: #FFFFFF; font-size: 20px; font-weight: bold; transition: color 0.3s;"></asp:Label>
    <div style="height:20px"></div>
    <%-- כותרת עדכון פרטי משתמש --%>
    <asp:Label ID="UserEdit_Lable" runat="server" Text="עדכון פרטי משתמש" CssClass="label-style-navy"></asp:Label>
    <br />

    <%-- טקסטבוקס סיסמה --%>
    <asp:Label ID="Password_Lable" runat="server" Text="סיסמה" CssClass="label-style-navy" Style="font-size:20px; padding:0px;"></asp:Label>
    <br />
    <asp:TextBox ID="Password" runat="server" placeholder="סיסמה" CssClass="Textbox-style-navy" ></asp:TextBox>
    <br />
    <%-- טקסטבוקס שם פרטי --%>
    <asp:Label ID="User_FirstName_Lable" runat="server" Text="שם פרטי" CssClass="label-style-navy" Style="font-size:20px; padding:0px;"></asp:Label>
    <br />
    <asp:TextBox ID="User_FirstName" runat="server" placeholder="שם פרטי" CssClass="Textbox-style-navy" ></asp:TextBox>
    <br />
    <%-- טקסטבוקס שם משפחה --%>
    <asp:Label ID="User_LastName_Lable" runat="server" Text="שם משפחה" CssClass="label-style-navy" Style="font-size:20px; padding:0px;"></asp:Label>
    <br />
    <asp:TextBox ID="User_LastName" runat="server" placeholder="שם משפחה" CssClass="Textbox-style-navy" ></asp:TextBox>
    <%-- מגדר --%>
    <asp:Label ID="User_Gender_Lable" runat="server" Text="מגדר" CssClass="label-style-navy" Style="font-size:20px; padding:0px;"></asp:Label>
    <br />
    <div class="form-group">
           <asp:DropDownList ID="User_Gender" runat="server" CssClass="Textbox-style-navy" style=" height: 50px; width: 36.35%; text-align:center;" >
           <asp:ListItem>זכר </asp:ListItem>
           <asp:ListItem>נקבה</asp:ListItem>
           <asp:ListItem>אחר</asp:ListItem>
           </asp:DropDownList>
        </div>
    <%-- טקסטבוקס אימייל --%>
    <asp:Label ID="User_Gmail_Lable" runat="server" Text="אימייל" CssClass="label-style-navy" Style="font-size:20px; padding:0px;"></asp:Label>
    <br />
    <asp:TextBox ID="User_Gmail" runat="server" placeholder="אימייל" TextMode="Email" CssClass="Textbox-style-navy" ></asp:TextBox>
    <br />
    <%-- טקסטבוקס מספר טלפון --%>
    <asp:Label ID="User_PhoneNumber_Label" runat="server" Text="מספר טלפון" CssClass="label-style-navy" Style="font-size:20px; padding:0px;"></asp:Label>
    <br />
    <asp:TextBox ID="User_PhoneNumber" runat="server" placeholder="מספר טלפון" CssClass="Textbox-style-navy" ></asp:TextBox>
    <br />
    <asp:Label ID="Update_Result" runat="server" Text="" CssClass="label-style-navy" Style="font-size:20px; color:red;"></asp:Label>
    <br />
    <div Class="centered-div">
    <asp:Button ID="Edit" runat="server" Text="עדכון" CssClass="Create" OnClick="Edit_Click"/>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
