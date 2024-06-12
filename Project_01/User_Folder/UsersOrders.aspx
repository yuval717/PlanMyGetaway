<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="UsersOrders.aspx.cs" Inherits="Project_01.UsersOrders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="UsersOrders.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div style="height:30px"></div>
    <asp:Label ID="AdminPage_Lable" runat="server" Text="החופשות שלי" CssClass="label-style-navy" Style="font-size:60px" ></asp:Label>
    <div style="height:60px"></div>

    <asp:Panel ID="Panel2" CssClass="container" runat="server" ScrollBars="Vertical" Height="576px" Style="overflow-y: auto;">
    <%-- דאטא ליסט חופשות - החופשות שלי --%>
    <div class="container">
            <asp:DataList ID="DataList2" runat="server" RepeatDirection="Horizontal" RepeatColumns="4" CellSpacing="3" OnItemDataBound="DataList2_ItemDataBound" OnItemCommand="DataList2_ItemCommand">
                <ItemTemplate>
                     <div class="item_Order">
                        <table class="table_Order">
                            <tr>
                                <td class="OrderName_Row">
                                    <asp:Label ID="Order_ID" runat="server" Text='<%# Bind("Order_ID") %>' Visible="false"></asp:Label>
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
                                <td class="DaysNum_Row">
                                    <asp:Label ID="OrderType" runat="server" Text='<%# Bind ("Order_Type") %>'  />
                            </td>
                            </tr>
                            <tr>
                                <td class="Image_Row">
                                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~\pictures\OrderBuild.png" Width="80px" Height="80px" Style="padding:0px;" CommandName="DoShow" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </ItemTemplate>
            </asp:DataList>
    </div>
        </asp:Panel>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
