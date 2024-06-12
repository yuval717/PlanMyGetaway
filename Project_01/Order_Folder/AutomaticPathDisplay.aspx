<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="AutomaticPathDisplay.aspx.cs" Inherits="Project_01.AutomaticPathDisplay" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="AutomaticPathDisplay.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <%-- כותרת תצוגה --%>
    <asp:Label ID="AutomaticPathDisplay_Lable" runat="server" Text="תצוגת חופשה אוטמטית" CssClass="label-style-navy" style="font-size: 40px; position:absolute; top: 22%; right: 7.5%;"></asp:Label>
    <%-- תאריך יום המסלול --%>
    <asp:Label ID="DayDate_Lable" runat="server" Text="" CssClass="label-style-navy" style="font-size: 30px; position:absolute; top: 30%; right: 13%;"></asp:Label>
    <%-- שם ההזמנה --%>
    <asp:Label ID="OrderName_Lable" runat="server" Text="" CssClass="label-style-navy" style="font-size: 25px; position:absolute; top: 35%; right: 11%;"></asp:Label>
    <%-- כפתור ניווט בימי המסלול --%>
    <asp:Button ID="NextDayInPath" runat="server" Text="יום הבא" CssClass="Next_Prev" style="position:absolute; top: 44%; right: 17%;" OnClick="NextDayInPath_Click"/>
    <asp:Button ID="PrevDayInPath" runat="server" Text="יום קודם" CssClass="Next_Prev" style="position:absolute; top: 44%; right: 10.2%;" OnClick="PrevDayInPath_Click"/>

    <%-- בלוק רווח --%>
    <div style="height: 20px;"></div>

    <%-- דאטאליסט - מיקום ראשוני --%>
    <div class="container">
    <asp:DataList ID="StartPlace" runat="server" OnItemDataBound="StartPlace_ItemDataBound">
        <ItemTemplate>
            <div class="item_StartPlace">
                <table>
                    <tr>
                        <td class="center-content">
                            <asp:Label ID="StartPlaceStartHour" runat="server" Text='<%# Bind("Day_StartHour") %>'></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="center-content">
                            <asp:Label ID="StartLocationAddress_Lable" runat="server" Text='נקודת התחלה'></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="center-content">
                            <asp:Label ID="StartLocationAddress" runat="server" Text='<%# Bind("Day_StartLocationAddress") %>'></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </ItemTemplate>
    </asp:DataList>
</div>


    <%-- דאטאליסט - תחבורה --%>
    <div class="container" style="position: absolute; top: -6.2%; right: 26.8%">
    <asp:DataList ID="Transportation" runat="server"  CellSpacing="287" OnItemDataBound="Transportation_ItemDataBound">
        <ItemTemplate>
            <div class="item_Transportation">
                <table class="table">
                    <tr>
                        <td class="center-content">
                            <asp:Label ID="TransportationStartHour" runat="server" Text='<%# Bind("StartHour") %>'></asp:Label>
                            <asp:Label ID="TransportationDuration" runat="server" Text=" - "></asp:Label>
                            <asp:Label ID="TransportationEndHour" runat="server" Text='<%# Bind("EndHour") %>'></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="center-content" >
                            <asp:Label ID="FromAttraction" runat="server" Text='<%# Bind("FromAttraction") %>' Visible="false"></asp:Label>
                            <asp:Label ID="FromAttraction_Lable" runat="server" Text='' Style="font-size: 0.8em;"></asp:Label>
                            <asp:Label ID="AttractionsDistance" runat="server" Text=" - " Style="font-size: 0.8em;"></asp:Label>
                            <asp:Label ID="ToAttraction" runat="server" Text='<%# Bind("ToAttraction") %>' Visible="false"></asp:Label>
                            <asp:Label ID="ToAttraction_Lable" runat="server" Text='' Style="font-size: 0.8em;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="center-content">
                            <asp:Label ID="TravelType" runat="server" Text='<%# Bind("TravelType") %>' Visible="false"></asp:Label>
                            <asp:ImageButton ID="TravelType_Photo" runat="server" ImageUrl='' Height="60px" Width="50px" CommandName="DoShow" />
                        </td>
                    </tr>
                </table>
            </div>
        </ItemTemplate>
    </asp:DataList>
</div>


    <%-- דאטאליסט - אטרקציות --%>
    <div class="container" style="position: absolute; top: 24.3%; right: 29.3%"">
    <asp:DataList ID="Attraction" runat="server" CellSpacing="137" OnItemDataBound="Attraction_ItemDataBound">
        <ItemTemplate>
            <div class="item">
                <table class="table">
                    <tr>
                        <td class="ImageButton_Row">
                            <asp:Image ID="Attraction_Photo" runat="server" ImageUrl='' Height="207px" Width="457px" CommandName="DoShow" CssClass="imageButtonStyle" Style="border-radius: 8px; cursor:auto;" />
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
