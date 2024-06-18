<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="AttractionSearch_ManualOrder.aspx.cs" Inherits="Project_01.AttractionSearch_ManualOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="AttractionSearch_ManualOrder.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <%-- תצוגת שם משתמש --%>
    <asp:Label ID="MasterPage_UserName" runat="server" Text="" Style="position:absolute; top:30px; left:15px; margin: 0 20px; text-decoration: none; color: #FFFFFF; font-size: 20px; font-weight: bold; transition: color 0.3s;"></asp:Label>
    <%--בלוק כחול מתחת לתפריט--%>
    <div id="nav-block_UnderneathNavigation"> </div>

    <%-- כותרת שם אטרקציה --%>
    <asp:Label ID="Attraction_Lable" CssClass="label-style-White-Title" runat="server" Text="אטרקציות" Style="position: absolute; top: 10.2%; right: 45.5%;/* ככל שהמספר גדול יותר זז שמאלה*/" ></asp:Label>
    <%-- טסקטבוקז שם אטרקציה --%>
    <asp:TextBox ID="txtFilter" runat="server" placeholder="הכנס שם אטרקציה" CssClass="Textbox-style-navy" Style="position: absolute; top: 24.2%; right: 30%;"  AutoPostBack ="true" OnTextChanged="SearchBar_TextChanged"/>
    <%-- הודעת לא נמצאו תוצאות סינון --%>
    <asp:Label ID="NoResult_Lable" runat="server" CssClass="label-style-White-Title" Text="לא נמצאו תוצאות" Visible="false" Style=" color:red; font-size:25px; position: absolute; top: 50.2%; right: 45.8%;/* ככל שהמספר גדול יותר זז שמאלה*/"></asp:Label>

    <%-- בר סינון --%>
    <div class="Order">
        <asp:DataList ID="FilterMenu" runat="server" OnItemCommand="FilterMenu_ItemCommand">
            <ItemTemplate>
                <div>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="AttractionType_Button" runat="server" Text="סוגי אטרקציה" CommandName="Type" CssClass="Controls"/>
                            </td>
                            <td>
                                <asp:Button ID="Age_button" runat="server" Text="טווח גילאים" CommandName="Age" CssClass="Controls"/>
                            </td>
                            <td>
                                <asp:TextBox ID="Price" runat="server" placeholder="מחיר" CssClass="Controls" style="height:30px; text-align: center;"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="Duration" runat="server" placeholder="משך אטרקציה" CssClass="Controls" style="height:30px; text-align: center;"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="filter" runat="server" Text="סנן" CommandName="Filter" CssClass="Create" Style="width: 90px"/>
                            </td>
                            <td>
                                <asp:Button ID="Button_All" runat="server" Text="הכל" CommandName="All" CssClass="Controls" Style="width: 90px"/>
                            </td>
                        </tr>
                    </table>
                </div>
            </ItemTemplate>
        </asp:DataList>
    </div>

    <%-- דיב נפתח - גילאים --%>
    <div id="Age" class="GapDiv" style="display: none; right:27.756%; top:42.8%" runat="server">
        <br />
        <br />
        <asp:Label ID="Min_AgeLabel" CssClass="label-style-black" runat="server" Text="גיל נופש מינימלי"></asp:Label>
        <asp:TextBox ID="Min_Age" CssClass="Textbox-style-black" runat="server"></asp:TextBox>
        <br />
        <br />
        <asp:Label ID="Max_AgeLabel" CssClass="label-style-black" runat="server" Text="גיל נופש מקסימלי"></asp:Label>
        <asp:TextBox ID="Max_Age" CssClass="Textbox-style-black" runat="server"></asp:TextBox>
        <br />
        <br />
        <br />
        <br />
        <asp:Button ID="AgeDone" CssClass="button-style" runat="server" Text="אישור" OnClick="AgeDone_Click" />
    </div>

    <%-- דיב נפתח - סוגי אטרקציות --%>
    <div id="AttractionFilter" class="GapDiv" style="display: none; right:16.465%; top:42.8%" runat="server">
        <br />
        <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Height="126.5"  CssClass="center-div">
            <asp:CheckBoxList ID="AttractionType" runat="server" cssClass="checkbox-list">
            </asp:CheckBoxList>
        </asp:Panel>
        <br />
        <asp:Button ID="AttractionFilterDone" CssClass="button-style" runat="server" Text="אישור" OnClick="AttractionFilterDone_Click" Style="top:10px;" />
    </div>

    <br />
    <asp:Label ID="result_Order" runat="server" Text="" CssClass="label-style-navy" Style="font-size:20px; color:red; position:absolute; top: 45%; right: 45.3%;"></asp:Label>
    <br />

    <%-- דאטאליסט אטרקציות --%>
    <div style="height:100px;"></div>
    <asp:Panel ID="Panel2" CssClass="container" runat="server" ScrollBars="Vertical" Height="556px" Style="overflow-y: auto;">
    <asp:DataList ID="AttractaionDisplay" runat="server" RepeatDirection="Horizontal" RepeatColumns="4" CellSpacing="3" OnItemDataBound="AttractaionDisplay_ItemDataBound" OnItemCommand="AttractaionDisplay_OnItemCommand">
        <ItemTemplate>
            <div class="item">
                <table class="table">
                    <tr>
                        <td class="ImageButton_Row">
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl='<%# Bind("Attraction_Photo") %>' Height="207px" Width="257px" CommandName="DoShow" CssClass="imageButtonStyle" Style="border-radius: 8px;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="AttractionName_Row">
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Attraction_Name") %>'></asp:Label>
                            <asp:Label ID="Attraction_ID" runat="server" Text='<%# Bind("Attraction_ID") %>' Visible="false"></asp:Label>
                            <asp:Label ID="Attraction_VacationType_ID" runat="server" Text='<%# Bind("VacationType_ID") %>' Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="PriceOrKilometers_Row">
                            <asp:Label ID="PriceOrKilometers" runat="server"></asp:Label>
                        </td>
                    </tr>
                     <tr>
                        <td style="text-align:center;">
                            <asp:Button ID="AddToPath" runat="server" Text="הוסף" CssClass="Create" style="width:60px; height:20px;" CommandName="AddToPath" />
                        </td>
                    </tr>
                </table>
            </div>
        </ItemTemplate>
    </asp:DataList>
</asp:Panel>




   <%-- בלוק רווח- מרווח את סוף העמוד --%>
   <div class ="Space" ></div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
