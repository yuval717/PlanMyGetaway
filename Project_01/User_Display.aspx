<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_Display.aspx.cs" Inherits="Project_01.User_Display" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            height: 24px;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="User_Search" runat="server" Text="חפש משתמש"></asp:Label>
            <br/>
            <asp:TextBox ID="SearchBar" runat="server" AutoPostBack ="true" OnTextChanged="SearchBar_TextChanged"></asp:TextBox>
            <br/>
            <asp:DataList ID="DataList1" runat="server" CellSpacing="3" OnItemCommand="DataList1_ItemCommand" OnItemDataBound="DataList1_ItemDataBound" >
                <ItemTemplate>
                    <table class="auto-style1">
                        <tr>
                            <td class="auto-style2" style="border: 1px solid black;">
                                <asp:Label ID="User_Name" runat="server" Text='<%# Bind("User_Name") %>'></asp:Label>
                            </td>
                            <td class="auto-style2" style="border: 1px solid black;">
                                <asp:Label ID="User_FirstName" runat="server" Text='<%# Bind("User_FirstName") %>'></asp:Label>
                            </td>
                            <td class="auto-style2" style="border: 1px solid black;">
                                <asp:Label ID="User_LastName" runat="server" Text='<%# Bind("User_LastName") %>'></asp:Label>
                            </td>
                            <td class="auto-style2" style="border: 1px solid black;">
                                <asp:Label ID="User_Gmail" runat="server" Text='<%# Bind("User_Gmail") %>'></asp:Label>
                            </td>
                            <td class="auto-style2" style="border: 1px solid black;">
                                <asp:Label ID="User_PhoneNumber" runat="server" Text='<%# Bind("User_PhoneNumber") %>'></asp:Label>
                            </td>
                            <td class="auto-style2" style="border: 1px solid black;">
                                <asp:Label ID="User_Gender" runat="server" Text='<%# Bind("User_Gender") %>'></asp:Label>
                            </td>
                            <td class="auto-style2" style="border: 1px solid black;">
                                <asp:Label ID="User_Country" runat="server" Text='<%# Bind("User_Country") %>'></asp:Label>
                            </td>
                            <td class="auto-style2" style="border: 1px solid black;">
                                <asp:Label ID="User_LastEntrance" runat="server" Text='<%# Bind("User_LastEntrance") %>'></asp:Label>
                            </td>
                            <td class="auto-style2" style="border: 1px solid black;">
                                <asp:Image ID="User_Photo" runat="server" ImageUrl='<%# Bind("User_Photo") %>' Width ="20px" Height="20px" />
                            </td>
                            <td class="auto-style2" style="border: 1px solid black;">
                                <asp:Label ID="User_IsBlocked" runat="server" Text='<%# Bind("User_IsBlocked") %>'></asp:Label>
                            </td>
                            <td class="auto-style2" style="border: 1px solid black;">
                                <asp:Button ID="Is_Blocked" runat="server" Text="Block / Unblock" CommandName="DoShow" />
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
            <br/>
            <br/>
            <asp:DataList ID="DataList2" runat="server" CellSpacing="3" OnItemCommand= "DataList2_ItemCommand" OnItemDataBound="DataList2_ItemDataBound" >
                <ItemTemplate>
                    <table class="auto-style1">
                        <tr>
                            <td class="auto-style2" style="border: 1px solid black;">
                                <asp:Label ID="User_Name" runat="server" Text='<%# Bind("User_Name") %>'></asp:Label>
                            </td>
                            <td class="auto-style2" style="border: 1px solid black;">
                                <asp:Label ID="User_FirstName" runat="server" Text='<%# Bind("User_FirstName") %>'></asp:Label>
                            </td>
                            <td class="auto-style2" style="border: 1px solid black;">
                                <asp:Label ID="User_LastName" runat="server" Text='<%# Bind("User_LastName") %>'></asp:Label>
                            </td>
                            <td class="auto-style2" style="border: 1px solid black;">
                                <asp:Label ID="User_Gmail" runat="server" Text='<%# Bind("User_Gmail") %>'></asp:Label>
                            </td>
                            <td class="auto-style2" style="border: 1px solid black;">
                                <asp:Label ID="User_PhoneNumber" runat="server" Text='<%# Bind("User_PhoneNumber") %>'></asp:Label>
                            </td>
                            <td class="auto-style2" style="border: 1px solid black;">
                                <asp:Label ID="User_Gender" runat="server" Text='<%# Bind("User_Gender") %>'></asp:Label>
                            </td>
                            <td class="auto-style2" style="border: 1px solid black;">
                                <asp:Label ID="User_Country" runat="server" Text='<%# Bind("User_Country") %>'></asp:Label>
                            </td>
                            <td class="auto-style2" style="border: 1px solid black;">
                                <asp:Label ID="User_LastEntrance" runat="server" Text='<%# Bind("User_LastEntrance") %>'></asp:Label>
                            </td>
                            <td class="auto-style2" style="border: 1px solid black;">
                                <asp:Image ID="User_Photo" runat="server" ImageUrl='<%# Bind("User_Photo") %>' Width ="20px" Height="20px" />
                            </td>
                            <td class="auto-style2" style="border: 1px solid black;">
                                <asp:Label ID="User_IsBlocked" runat="server" Text='<%# Bind("User_IsBlocked") %>'></asp:Label>
                            </td>
                            <td class="auto-style2" style="border: 1px solid black;">
                                <asp:Button ID="Is_Blocked" runat="server" Text="Block / Unblock" CommandName="DoShow" />
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
            <br/>
            <br/>
        </div>
    </form>
</body>
</html>
