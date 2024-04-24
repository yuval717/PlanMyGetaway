<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllUserOrders.aspx.cs" Inherits="Project_01.AllUserOrders" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:DataList ID="DataList1" runat="server" RepeatDirection="Horizontal" RepeatColumns="5" CellSpacing="3" OnItemDataBound="DataList1_ItemDataBound" OnItemCommand="DataList1_ItemCommand">
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

            <asp:Button ID="Button2" runat="server" Text="חזור" OnClick="Button2_Click" />
        </div>
    </form>
</body>
</html>
