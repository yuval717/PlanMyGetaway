<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Automatic_Order.aspx.cs" Inherits="Project_01.WebForm2" Async="true"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="Automatic_Order.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%-- תצוגת שם משתמש --%>
    <asp:Label ID="MasterPage_UserName" runat="server" Text="" Style="position:absolute; top:30px; left:15px; margin: 0 20px; text-decoration: none; color: #FFFFFF; font-size: 20px; font-weight: bold; transition: color 0.3s;"></asp:Label>
    <%-- בחירת יצירת מסלול --%>
    <div id="OrderChoice_Div" runat="server">
    <%--בלוק כחול מתחת לתפריט--%>
    <div id="nav-block_UnderneathNavigation"></div>
    <%-- בלוק רווח --%>
    <%--<div style="height: 20px;"></div>--%>
    <asp:Label ID="OrderChoice_Lable" CssClass="label-style-White" runat="server" Text="יצירת חופשה" style="font-size: 60px; position:absolute; top: 14%; right: 41.8%;"></asp:Label>
    <br />
    <asp:Button ID="Automatic_Order_Button" CssClass="button-style" runat="server" Text="יצירת מסלול ידני" Style="margin-top: 107px; /* Adds space above the div */
    right: 28.9%; /* Moves container to 80% from the left of the parent */ position: relative; /* Adds relative positioning */" OnClick="manualOrder_Button_Click" />
    <asp:Button ID="manualOrder_Button" CssClass="button-style" runat="server" Text="יצירת מסלול אוטמטי" Style="margin-top: 107px; /* Adds space above the div */
    right: 41.1%; /* Moves container to 80% from the left of the parent */ position: relative; /* Adds relative positioning */" OnClick="Automatic_Order_Button_Click" />
    <br />
    </div>

    <%-- מסלול ידני --%>
    <div id="Manual_Div" style="display: none;" runat="server">
            <%-- בלוק רווח --%>
    <div style="height: 20px;"></div>

        <%-- כותרת יצירת מסלול ידני --%>
        <asp:Label ID="ManualOrder_Lable"  CssClass="label-style-navy" runat="server" Text="יצרת מסלול"></asp:Label>
        <%-- כותרת שם הזמנה - ידני --%>
        <asp:Label ID="ManualOrder_OrderName_Lable" CssClass="label-style-navy" runat="server" Text="שם הזמנה" style="font-size: 20px; position:absolute; top: 24%; right: 47.9%;"></asp:Label>
        <%-- טקסטבוקס שם הזמנה - ידני --%>
        <asp:TextBox ID="ManualOrder_OrderName" runat="server" placeholder="שם הזמנה" CssClass="Textbox-style-navy" style=" position:absolute; top: 34%; right: 29.5%;"></asp:TextBox>
        <%-- כפתור מעבר ליצרת מסלול ידני --%>
        <asp:Button ID="ManualOrder_Create" runat="server" Text="יצירה" CssClass="Create" style=" position:absolute; top: 54%; right: 29.3%;" OnClick="ManualOrder_Create_Click"/> 
        <br />
    <asp:Label ID="result_Manual" runat="server" Text="" CssClass="label-style-navy" Style="font-size:20px; color:red; position:absolute; top: 43%; right: 45.3%;"></asp:Label>
    <br />
    </div>


    <%-- מסלול אוטומטי --%>
    <div id="Automatic_Div" style="display: none;" runat="server">
    <%-- בלוק רווח --%>
    <div style="height: 20px;"></div>

    <%-- כותרת עמוד - יצירת הזמנה --%>
    <asp:Label ID="CreateVacation_Lable" runat="server" Text="יצירת חופשה" CssClass="label-style-navy"></asp:Label>

    <br />
    <%-- פרטי הזמנה - שם הזמנה --%>
    <asp:Label ID="OrderName_Lable" runat="server" Text="שם הזמנה" CssClass="label-style-navy" style="font-size: 20px"></asp:Label>
    <br />
    <asp:TextBox ID="OrderName" CssClass="Textbox-style-navy" runat="server" placeholder="שם הזמנה" ></asp:TextBox>
    <br />

    <%-- בלוק רווח --%>
    <div style="height: 20px;"></div>

    <%-- כותרת סוגי אטרקציות --%>
    <asp:Label ID="AttractionType_Lable" runat="server" Text="סוגי אטרקציות לבחירה" CssClass="label-style-navy" style="font-size: 20px"></asp:Label>


    <div class="center-container">
        <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Height="250px" Style="overflow-y: auto;" >
            <%-- צקבוקסליסט סוגי אטרקציות --%>
            <asp:CheckBoxList ID="AttractionTypePreference" runat="server" AutoPostBack="true" CssClass="checkbox-list" RepeatColumns="5" RepeatLayout="Flow" RepeatDirection="Horizontal"></asp:CheckBoxList>
   </asp:Panel>
            </div>

    <%-- בלוק רווח --%>
    <div style="height: 25px;"></div>
    <%-- כותרת פרטי ימי חופשה --%>
    <asp:Label ID="Label1" runat="server" Text="פרטי ימי החופשה" CssClass="label-style-navy" style="font-size: 20px"></asp:Label>

    <%-- כפתורי פרטי חופשה - בתוך דאטאליסט --%>
    <div class="container">
        <asp:Panel ID="Panel2" runat="server" ScrollBars="Vertical" Height="370px" Style="overflow-y: auto;" >
    <asp:DataList ID="DayPreferences" runat="server" RepeatDirection="Horizontal" RepeatColumns="3" CellSpacing="30" OnItemDataBound="DayPreferences_ItemDataBound">
        <ItemTemplate>
            <div class="item">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="DayDate_Lable" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="StartPlace_Lable" runat="server" Text="כתובת התחלת מסלול"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="StartPlace" runat="server" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="StartDayTime_Lable" runat="server" Text="שעת תחילת היום"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="StartDayTime" TextMode="Time" runat="server" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="EndDayTime_Lable" runat="server" Text="שעת סיום היום"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="EndDayTime" TextMode="Time" runat="server" ></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </ItemTemplate>
    </asp:DataList>
            </asp:Panel>
</div>

     <br />
    <asp:Label ID="Result_Day" runat="server" Text="" CssClass="label-style-navy" Style="font-size:20px; color:red;"></asp:Label>
    <br />

    <%-- בלוק רווח --%>
    <div style="height: 100px;"></div>

    <%-- כפתור יצירת חופשה אוטומטית --%>
    <asp:Button ID="Create" runat="server" Text="צור" CssClass="Create" OnClick="Create_Click" />
    

    <%-- בלוק רווח --%>
    <div style="height: 100px;"></div>
        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>

