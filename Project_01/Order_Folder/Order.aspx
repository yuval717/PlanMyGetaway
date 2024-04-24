﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Order.aspx.cs" Inherits="Project_01.Order" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style2 {
            width: 100%;
        }
        .auto-style3 {
            height: 34px;
            text-align: center;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
            <br/>
            <asp:Label ID="Label1" runat="server" Text="כתובת התחלת מסלול"></asp:Label>
            <asp:TextBox ID="StartPlace" runat="server"></asp:TextBox> 
            <br/>
            <asp:Label ID="Label2" runat="server" Text="שעת תחילת היום"></asp:Label>
            <asp:TextBox ID="StartDayTime" runat="server"></asp:TextBox> 
            <br/>
            <asp:Label ID="Label3" runat="server" Text="שעת סיום היום"></asp:Label>
            <asp:TextBox ID="EndDayTime" runat="server"></asp:TextBox> 
            <br/>
            <asp:Button ID="Button2" runat="server" Text="Button" OnClick="Button2_Click" />
            <br/>
            <asp:DataList ID="DataList1" runat="server" OnItemDataBound="DataList1_ItemDataBound" Visible ="false">
                <ItemTemplate>
                    <table class="auto-style2" align="center">
                        <tr>
                            <td class="auto-style3">
                                <asp:Button ID="Button1" runat="server" Text='<%# Bind("VacationType_Type") %>' CommandName="VacationType_Button" />
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style4">
                                <asp:CheckBoxList ID="CheckBoxList1" runat="server">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
            <asp:Button ID="Create" runat="server" Text="Create" OnClick="Create_Click" Visible ="false"/>
            <br/>
            <asp:Button ID="Button3" runat="server" Text="Button" OnClick="Button3_Click" Visible ="false"/>
            <%--<asp:TextBox ID="TextBox1" runat="server" TextMode="Date" ></asp:TextBox>--%>
        </div>
    </form>
</body>
</html>