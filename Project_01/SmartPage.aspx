<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SmartPage.aspx.cs" Inherits="Project_01.SmartPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label3" runat="server" Text="האחרונות שהועלו מאז הכניסה האחרונה"></asp:Label>
            <asp:DataList ID="DataList1" runat="server" RepeatDirection="Horizontal" RepeatColumns="5" CellSpacing="3" OnItemDataBound="DataList1_ItemDataBound" OnItemCommand="DataList1_ItemCommand1">
                <ItemTemplate>
                    <div class="item">
                        <table class="style1" border="1px">
                            <tr>
                                <td class="auto-style4">
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Attraction_Name") %>'></asp:Label>
                                    <asp:Label ID="Attraction_ID" runat="server" Text='<%# Bind("Attraction_ID") %>' Visible ="false"></asp:Label>
                                    <asp:Label ID="Attraction_VacationType_ID" runat="server" Text='<%# Bind("VacationType_ID") %>' Visible="false" ></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style4">
                                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# Bind("Attraction_Photo") %>' Height="114px" Width="95px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style2">
                                    <asp:Label ID="PriceOrKilometers" runat="server" Text =""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style4">
                                    <asp:Button ID="Button1" runat="server" Text="ENTER" CommandName="DoShow" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </ItemTemplate>
            </asp:DataList>
            <br/>
            <asp:Button ID="Button2" runat="server" Text="ראה עוד" OnClick="Button2_Click" />
            <br/>
            <asp:Label ID="Label4" runat="server" Text="הזמנות אחרונות"></asp:Label>
            <asp:DataList ID="DataList2" runat="server" RepeatDirection="Horizontal" RepeatColumns="5" CellSpacing="3" OnItemDataBound="DataList2_ItemDataBound" OnItemCommand="DataList2_ItemCommand">
                <ItemTemplate>
                    <div>
                        <table class="style1" border="1px">
                            <tr>
                                <td class="auto-style4">
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Order_Name") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style4">
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind ("Order_DaysNumber") %>'  />
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style4">
                                    <asp:Button ID="Button1" runat="server" Text="ENTER" CommandName="DoShow" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </ItemTemplate>
            </asp:DataList>
            <asp:Button ID="Button3" runat="server" Text="ראה עוד" OnClick="Button3_Click" />
            <br/>
            <br/>
            <asp:Label ID="Label5" runat="server" Text="אטרקציות מומלצות לפי הזמנות אחרונות"></asp:Label>
            <asp:DataList ID="DataList3" runat="server" RepeatDirection="Horizontal" RepeatColumns="5" CellSpacing="3" OnItemDataBound="DataList3_ItemDataBound" OnItemCommand="DataList3_ItemCommand">
                <ItemTemplate>
                    <div class="item">
                        <table class="style1" border="1px">
                            <tr>
                                <td class="auto-style4">
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Attraction_Name") %>'></asp:Label>
                                    <asp:Label ID="Attraction_ID" runat="server" Text='<%# Bind("Attraction_ID") %>' Visible ="false"></asp:Label>
                                    <asp:Label ID="Attraction_VacationType_ID" runat="server" Text='<%# Bind("VacationType_ID") %>' Visible="false" ></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style4">
                                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# Bind("Attraction_Photo") %>' Height="114px" Width="95px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style2">
                                    <asp:Label ID="PriceOrKilometers" runat="server" Text =""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style4">
                                    <asp:Button ID="Button1" runat="server" Text="ENTER" CommandName="DoShow" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </ItemTemplate>
            </asp:DataList>
            <asp:Button ID="Button4" runat="server" Text="ראה עוד" OnClick="Button4_Click" />
            <br/>

        </div>
    </form>
</body>
</html>
