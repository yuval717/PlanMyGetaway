<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Manual_Order.aspx.cs" Inherits="Project_01.Manual_Order" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="Manual_Order.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%-- כותרת תצוגה --%>
    <asp:Label ID="AutomaticPathDisplay_Lable" runat="server" Text="תצוגת חופשה ידנית" CssClass="label-style-navy" style="font-size: 40px; position:absolute; top: 22%; right: 9%;"></asp:Label>
    <%-- תאריך יום המסלול --%>
    <asp:Label ID="DayDate_Lable" runat="server" Text="" CssClass="label-style-navy" style="font-size: 30px; position:absolute; top: 30%; right: 13%;"></asp:Label>
    <%-- שם ההזמנה --%>
    <asp:Label ID="OrderName_Lable" runat="server" Text="" CssClass="label-style-navy" style="font-size: 25px; position:absolute; top: 35%; right: 11%;"></asp:Label>
    <%-- כפתור ניווט בימי המסלול --%>
    <asp:Button ID="NextDayInPath" runat="server" Text="יום הבא" CssClass="Next_Prev" style="position:absolute; top: 44%; right: 17%;" OnClick="NextDayInPath_Click"/>
    <asp:Button ID="PrevDayInPath" runat="server" Text="יום קודם" CssClass="Next_Prev" style="position:absolute; top: 44%; right: 10.2%;" OnClick="PrevDayInPath_Click"/>
    
    
    <%-- הוספת אטרקציה --%>
    <div id="AttractionAdd" runat="server">
    <%-- שיבוץ אטרקציה - מהשעה --%>
    <asp:TextBox ID="FromHour" runat="server" TextMode="Time" CssClass="Textbox-style-black" style="position:absolute; top: 54%; right: 7%;" ></asp:TextBox>
    <%-- שיבוץ אטרקציה - עד השעה --%>
    <asp:TextBox ID="ToHour" runat="server" TextMode="Time" CssClass="Textbox-style-black" style="position:absolute; top: 54%; right: 11.6%;" ></asp:TextBox>
    <%-- כפתור חיפוש אטרקציות לשיבוץ לפי השעות --%>
    <asp:Button ID="SearchForAttraction" runat="server" Text="חיפוש אטרקציה להוספה"  CssClass="Next_Prev" style="width:200px; position:absolute; top: 54%; right: 16.3%;" OnClick="SearchForAttraction_Click"/>
    <%-- כיתוב מ/ל שעה מעל טקסטבוקס שיבוץ שעות --%>
    <asp:Label ID="Label6" runat="server" Text="מהשעה" style=" position: absolute; top: 54%; right: 7.3%; font-size: 10px;" ></asp:Label> <%--יותר קטן = שמאלה יותר קטן = מעלה--%>
    <asp:Label ID="Label7" runat="server" Text="לשעה" style=" position: absolute; top: 54%; right: 11.9%; font-size: 10px;" ></asp:Label>
    <%-- הודעת שגיאה --%>
    <asp:Label ID="NoResult_Lable" runat="server" CssClass="label-style-White-Title" Text="בשעות אלו משובצת אטרקציה" Visible="false" Style=" color:red; font-size:18px; position: absolute; top: 59.2%; right: 9%;/* ככל שהמספר גדול יותר זז שמאלה*/"></asp:Label>
    </div>


    <%-- מחיקת אטרקציה --%>
    <div id="AttractionRemove" runat="server">
    <%-- מחיקת אטרקציה - מהשעה --%>
    <asp:TextBox ID="FromHour_Remove" runat="server" TextMode="Time" CssClass="Textbox-style-black" style="position:absolute; top: 64%; right: 7%;" ></asp:TextBox>
    <%-- מחיקת אטרקציה - עד השעה --%>
    <asp:TextBox ID="ToHour_Remove" runat="server" TextMode="Time" CssClass="Textbox-style-black" style="position:absolute; top: 64%; right: 11.6%;" ></asp:TextBox>
    <%-- כפתור מחיקת אטרקציות לפי השעות --%>
    <asp:Button ID="RemoveAttraction" runat="server" Text="מחיקת אטרקציה"  CssClass="Next_Prev" style="width:200px; position:absolute; top: 64%; right: 16.3%;" OnClick="RemoveAttraction_Click" />
    <%-- כיתוב מ/ל שעה מעל טקסטבוקס מחיקת אטרקציות --%>
    <asp:Label ID="Label1" runat="server" Text="מהשעה" style=" position: absolute; top: 64%; right: 7.3%; font-size: 10px;" ></asp:Label> <%--יותר קטן = שמאלה יותר קטן = מעלה--%>
    <asp:Label ID="Label2" runat="server" Text="לשעה" style=" position: absolute; top: 64%; right: 11.9%; font-size: 10px;" ></asp:Label>
    <%-- הודעת שגיאה --%>
    <asp:Label ID="NoAttraction_Lable" runat="server" CssClass="label-style-White-Title" Text="בשעות אלו בדיוק לא משובצת אטרקציה" Visible="false" Style=" color:red; font-size:18px; position: absolute; top: 69.2%; right: 11%;/* ככל שהמספר גדול יותר זז שמאלה*/"></asp:Label>
        </div>

    <%-- בלוק רווח --%>
    <div style="height: 20px;"></div>

    <%-- דאטאליסט - אטרקציות --%>
    <div class="container" style="position: absolute; top: 18.3%; right: 34.3%">
    <asp:DataList ID="Attraction" runat="server" CellSpacing="30" OnItemDataBound="Attraction_ItemDataBound" >
        <ItemTemplate>
            <div class="item">
                <table class="table">
                    <tr>
                        <td class="ImageButton_Row">
                            <asp:Image ID="Attraction_Photo" runat="server" Height="207px" Width="457px" CommandName="DoShow" CssClass="imageButtonStyle" Style="border-radius: 8px;"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="AttractionName_Row">
                            <asp:Label ID="Attraction_Name" runat="server" Text=''></asp:Label>
                            <asp:Label ID="Attraction_ID" runat="server" Text='<%# Bind("Attraction_ID") %>' Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr >
                           
                        <td class="AttractionHours">
                            <asp:Label ID="AttractionStartHour" runat="server" Text='<%# Bind("StartHour") %>' ></asp:Label>
                            <asp:Label ID="AttractionDuration" runat="server" Text=" - "></asp:Label>
                            <asp:Label ID="AttractionEndHour" runat="server" Text='<%# Bind("EndHour") %>'></asp:Label>
                        </td>
                        
                    </tr>
                </table>
            </div>
        </ItemTemplate>
    </asp:DataList>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
