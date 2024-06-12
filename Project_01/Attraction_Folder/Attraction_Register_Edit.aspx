<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Attraction_Register_Edit.aspx.cs" Inherits="Project_01.Attraction_Register_Edit"  Async="true"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="Attraction_Register_Edit.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%-- כותרות --%>
    <asp:Label ID="AttractionRegister_Lable" CssClass="label-style-navy" runat="server" Text="הוספת אטרקציה" style="font-size: 60px; position:absolute; top: 12%; right: 39.8%;"></asp:Label>
    <asp:Label ID="AttractionRegisterInfo_Lable" CssClass="label-style-navy" runat="server" Text="פרטי אטרקציה" style=" font-size: 40px; position:absolute; top: 24%; right: 44.4%;"></asp:Label>

    <%-- רווח --%>
    <div style="height:300px" ></div>

    <%-- שם אטרקציה --%>
            <asp:TextBox ID="Attraction_Name"  CssClass="Textbox-style-navy" placeholder="שם האטרקציה" runat="server" ></asp:TextBox>
            <br />
    <%-- סוג אטרציה --%>
            <asp:Label ID="Attraction_Type_Lable" runat="server" CssClass="label-style-navy" style=" font-size: 25px;" Text="סוג האטרקציה"></asp:Label>
            <asp:DropDownList ID="Attraction_Type" runat="server" CssClass="Textbox-style-navy" AutoPostBack=true OnSelectedIndexChanged="Attraction_Type_SelectedIndexChanged" style=" height: 50px; width: 36.35%;" ></asp:DropDownList>
            <br />
    <%-- גיל מינימלי --%>
            <asp:TextBox ID="Attraction_MinAge" CssClass="Textbox-style-navy" placeholder="גיל מינימלי של משתתף" runat="server" ></asp:TextBox>
            <br />
    <%-- גיל מקסימלי --%>
            <asp:TextBox ID="Attraction_MaxAge" CssClass="Textbox-style-navy" placeholder="גיל מקסימלי של משתתף" runat="server" ></asp:TextBox>
            <br />
    <%-- כתובת --%>
            <asp:TextBox ID="Attraction_Address" CssClass="Textbox-style-navy" placeholder="כתובת" runat="server" ></asp:TextBox>
            <br />
    <%-- מידע על האטרקציה --%>
            <asp:TextBox ID="Attraction_Text" CssClass="Textbox-style-navy" placeholder="מידע על האטרקציה" runat="server" ></asp:TextBox>
            <br />
    <%-- שעת פתיחה --%>
            <asp:Label ID="Attraction_OpeningHour_Lable" CssClass="label-style-navy" runat="server" Text="שעת פתיחה"  style=" font-size: 24px;" ></asp:Label>
            <asp:TextBox ID="Attraction_OpeningHour" CssClass="Textbox-style-navy" TextMode="Time" runat="server" ></asp:TextBox>
            <br />
    <%-- שעת סגירה --%>
            <asp:Label ID="Attraction_ClosingHour_Lable" CssClass="label-style-navy" runat="server" Text="שעת סגירה" style=" font-size: 24px;" ></asp:Label>
            <asp:TextBox ID="Attraction_ClosingHour" CssClass="Textbox-style-navy" TextMode="Time" runat="server" ></asp:TextBox>
            <br />
    <%-- מחיר --%>
            <asp:TextBox ID="Attraction_Price" CssClass="Textbox-style-navy" placeholder="מחיר" runat="server" ></asp:TextBox>
            <br />
    <%-- משך --%>
            <asp:TextBox ID="Attraction_Duration" CssClass="Textbox-style-navy" placeholder="משך זמן מומלץ לבילוי באטרקציה - בדקות" runat="server" ></asp:TextBox>
            <br />
    <%-- טלפון --%>
            <asp:TextBox ID="Attraction_PhonNumber" CssClass="Textbox-style-navy" TextMode="Phone" placeholder="מספר טלפון" runat="server" ></asp:TextBox>
            <br />
    <%--  אימייל--%>
            <asp:TextBox ID="Attraction_Gmail" CssClass="Textbox-style-navy" TextMode="Email" placeholder="אימייל" runat="server" ></asp:TextBox>
            <br />
    <%-- במידה ומסוג חופשה טיול בטבע --%>
    <%-- מס קילומטרים --%>
            <asp:TextBox ID="Attraction_KilometersNumber" CssClass="Textbox-style-navy" placeholder="מספר קילומטרים" runat="server" visible="false" ></asp:TextBox>
            <br />
    <%-- קושי אטרקציה --%>
            <asp:Label ID="Attraction_Difficulty_Lable" CssClass="label-style-navy" runat="server" Text="קושי אטרקציה" visible="false" style=" font-size: 25px;" ></asp:Label>
             <asp:DropDownList ID="Attraction_Difficulty" CssClass="Textbox-style-navy" runat="server" AutoPostBack=true visible="false" style=" height: 50px; width: 36.35%;">
                <asp:ListItem Value="קל מאוד">קל מאוד</asp:ListItem>
                <asp:ListItem Value="קל">קל</asp:ListItem>
                <asp:ListItem Value="בינוני">בינוני</asp:ListItem>
                <asp:ListItem Value="קשה">קשה</asp:ListItem>
                 <asp:ListItem Value="קשה מאוד">קשה מאוד</asp:ListItem>
            </asp:DropDownList>
    <%-- תמונה ראשית --%>
            <asp:Label ID="Attraction_MainPhoto_Lable" CssClass="label-style-navy" runat="server" Text="תמונה" style=" font-size: 25px;" ></asp:Label>
            <asp:FileUpload ID="Attraction_MainPhoto" CssClass="Textbox-style-navy" runat="server" />
            <br />
            <div style="text-align: center;"><%-- מרכוז כפתור--%>
            <asp:Button ID="Addphoto_Button" runat="server" Text="העלאה" CssClass="button-style" Style="font-size:20px;" OnClick="Addphoto_Button_Click" />
            </div>
            <br />
    <%-- התמונה הראשית שהועלתה --%>
    <div class="container">
    <asp:DataList ID="MainPhoto" runat="server" RepeatDirection="Horizontal" RepeatColumns="6" CellSpacing="3" OnItemCommand="MorePhotos_ItemCommand1">
        <ItemTemplate>
            <div class="item">
                <table class="table">
                    <tr>
                        <td class="ImageButton_Row">
                            <%-- התמונה הנוספת --%>
                            <asp:Image ID="Photo" runat="server" ImageUrl='<%# Bind("FileLocation") %>' Height="207px" Width="257px" CssClass="imageButtonStyle" Style="border-radius: 8px;" />
                        </td>
                    </tr>
                </table>
            </div>
        </ItemTemplate>
    </asp:DataList>
</div>

    <%-- רווח --%>
    <div style="height:30px" ></div>

    <%-- הוספת תמונות נוספות לאטרקציה --%>
    <asp:Label ID="MorePhotos_Lable" CssClass="label-style-navy" runat="server" Text="הוספת תמונות נוספות" style=" font-size: 40px; position:center; "></asp:Label>
    <%-- כפתור בחירת תמונה --%>
    <asp:FileUpload ID="AddPhoto" CssClass="Textbox-style-navy" runat="server" />
    <br />
    <%-- כפתור אישור העלאה --%>
    <div Class="center-button">
    <asp:Button ID="AddPhoto_Confirm" CssClass="button-style" runat="server" Text="העלאת תמונה" OnClick="AddPhoto_Confirm_Click1"/> 
    </div>

    <%-- רווח --%>
    <div style="height:20px" ></div>

    <%-- דאטאליסט תצוגת תמונות נוספות לאטרקציה - מכיל את התמונה וכפתור מחיקה --%>
            <div class="container">
    <asp:DataList ID="MorePhotos" runat="server" RepeatDirection="Horizontal" RepeatColumns="6" CellSpacing="3" OnItemCommand="MorePhotos_ItemCommand1">
        <ItemTemplate>
            <div class="item">
                <table class="table">
                    <tr>
                        <td class="ImageButton_Row">
                            <%-- התמונה הנוספת --%>
                            <asp:Image ID="Photo" runat="server" ImageUrl='<%# Bind("FileLocation") %>' Height="207px" Width="257px" CssClass="imageButtonStyle" Style="border-radius: 8px;" />
                        </td>
                    </tr>
                    <tr>
                        <%-- כפתור מחיקה --%>
                        <td class="Button_Row">
                            <asp:ImageButton ID="Remove" ImageUrl="~/pictures/trash-can-icon.jpg" Height="30px" Width="30px" runat="server" CommandName="remove" CssClass="removeButtonStyle" />
                        </td>
                    </tr>
                </table>
            </div>
        </ItemTemplate>
    </asp:DataList>
</div>

    <br />
    <asp:Label ID="Result_Att" runat="server" Text="" CssClass="label-style-navy" Style="font-size:20px; color:red;"></asp:Label>
    <br />
     <%-- רווח --%>
    <div style="height:30px" ></div>

    <%-- כפתור הרשמה --%>
    <asp:Button ID="Register" runat="server" Text="הרשמה" CssClass="Create" OnClick="Register_Click"/>
    <%-- כפתור הרשמה --%>
    <asp:Button ID="Edit" runat="server" Text="עדכון" CssClass="Create" OnClick="Edit_Click" Style="display:none;"/>

    <%-- רווח --%>
    <div style="height:100px" ></div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
