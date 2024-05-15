<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Homepage.aspx.cs" Inherits="Project_01.Homepage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link rel="stylesheet" href="Stylesheets/Homepage.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%--בלוק כחול מתחת לתפריט--%>
    <div id="nav-block_UnderneathNavigation">
        <%-- כותרות-לבן-בתוך הבלוק --%>
            <asp:Label ID="OrderTitle" CssClass="label-style-White-Title" runat="server" Text="רק תארזו, אנחנו נתכנן"></asp:Label>
            <asp:Label ID="OrderDescription" CssClass="label-style-White-Description" runat="server" Text="צרו חופשה בקליק"></asp:Label>

        <%-- בר הזמנה --%>
        <div class="Order">
            <%-- כפתורי בר הזמנה- בתוך דאטאליסט --%>
            <asp:DataList ID="OrderMenu" runat="server" OnItemCommand="OrderMenu_ItemCommand">
                <ItemTemplate>
                <div >
                <table >
                    <tr>
                        <td>
                            <asp:TextBox ID="FromDate" CssClass="Controls" TextMode="Date" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="ToDate" CssClass="Controls" TextMode="Date" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="Ages" CssClass="Controls" runat="server" Text="גילאים" CommandName="DoShow" />           
                        </td>
                        <td>
                            <asp:Button ID="Create" CssClass="Create" runat="server" Text="צור" CommandName="create" />
                        </td>
                    </tr>
                </table>
            </div>
        </ItemTemplate>
            </asp:DataList>
        </div>

        <%-- כיתוב-מ\ל תאריך-בתוך כפתורי בר הזמנה --%>
        <asp:Label ID="Label6" runat="server" Text="מתאריך" style=" position: absolute; top: 333px; left: 1442px; font-size: 12px;" ></asp:Label> <%--יותר קטן = שמאלה יותר קטן = מעלה--%>
        <asp:Label ID="Label7" runat="server" Text="לתאריך" style=" position: absolute; top: 333px; left: 1126.5px; font-size: 12px;" ></asp:Label>

        <%-- חלונית גילאים-נפתח בעת לחיצה על כפתור בר ההזמנה - גילאים --%>
        <div id="Age" class ="AgeDiv" style="display: none;" runat="server" >
            <%-- כפתורי חלון נפתח --%>
            <br/>
            <br/>
            <asp:Label ID="MinAgeLabel" cssclass="label-style-black" runat="server" Text="גיל נופש מינימלי" ></asp:Label>
            <asp:TextBox ID="MinAgeTextBox" cssclass="Textbox-style-black" runat="server" ></asp:TextBox>
            <br/>
            <br/>
            <asp:Label ID="MaxAgeLabel" cssclass="label-style-black" runat="server" Text="גיל נופש מקסימלי"></asp:Label>
            <asp:TextBox ID="MaxAgeTextBox" cssclass="Textbox-style-black" runat="server"></asp:TextBox>
            <br/>
            <br/>
            <br/>
            <br/>
            <asp:Button ID="Done" cssclass="button-style" runat="server" Text="אישור" OnClick="Button10_Click" />

        </div>


    <%-- בלוק רווח מתחת לבר הזמנה --%>
    </div> <div class ="Space" ></div>

    <%-- כותרת - כחולות --%>
    <asp:Label ID="Label3" CssClass=" label-style-navy " runat="server" Text="הועלו מאז הכניסה האחרונה"></asp:Label>

    <%-- דאטא ליסט אטרקציות - הועלו מאז הכניסה האחרונה --%>
    <div class="container">
    <asp:DataList ID="NewAttractionsSinceLastEntrance" runat="server" RepeatDirection="Horizontal" RepeatColumns="5" CellSpacing="3" OnItemDataBound="DataList1_ItemDataBound" OnItemCommand="DataList1_ItemCommand1">
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
                            <asp:Label ID="AttractionType_Type" runat="server" Text='<%# Bind("AttractionType_Type") %>'></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="PriceOrKilometers_Row">
                            <asp:Label ID="PriceOrKilometers" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </ItemTemplate>
    </asp:DataList>
    </div>

    <br/>
    <%-- כותרת - כחולות --%>
    <asp:Label ID="Label4" CssClass=" label-style-navy " runat="server" Text="החופשות שלי"></asp:Label>

    <%-- דאטא ליסט חופשות - החופשות שלי --%>
    <div class="container">
            <asp:DataList ID="DataList2" runat="server" RepeatDirection="Horizontal" RepeatColumns="5" CellSpacing="3" OnItemDataBound="DataList2_ItemDataBound" OnItemCommand="DataList2_ItemCommand">
                <ItemTemplate>
                     <div class="item_Order">
                        <table class="table_Order">
                            <tr>
                                <td class="OrderName_Row">
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Order_Name") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="DaysNum_Row">
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind ("Order_DaysNumber") %>'  />
                                </td>
                            </tr>
                            <tr>
                            <tr>
                                <td class="Image_Row">
                                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~\pictures\OrderBuild.png" Width="80px" Height="80px" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </ItemTemplate>
            </asp:DataList>
    </div>
            
    <br/>
    <br/>
    <%-- כותרת - כחולות --%>
    <asp:Label ID="Label5" CssClass=" label-style-navy " runat="server" Text="אטרקציות מומלצות לפי הזמנות קודמות"></asp:Label>

    <%-- דאטא ליסט אטרקציות - מומלצות לפי הזמנות קודמות --%>
    <div class="container">
            <asp:DataList ID="DataList3" runat="server" RepeatDirection="Horizontal" RepeatColumns="5" CellSpacing="3" OnItemDataBound="DataList1_ItemDataBound" OnItemCommand="DataList1_ItemCommand1">
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
                   </table>
                </div>
             </ItemTemplate>
         </asp:DataList>
   </div/>

    <%-- בלוק רווח- מרווח את סוף העמוד --%>
   <div class ="Space" ></div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
