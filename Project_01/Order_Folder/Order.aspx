<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Order.aspx.cs" Inherits="Project_01.Order" Async="true" %>
<link rel="stylesheet" href="Stylesheets/Order.css" />

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

            <%-- כותרת עמוד - יצירת הזמנה --%>
            <asp:Label ID="CreatVacation_Lable" runat="server" Text="יצירת חופשה"></asp:Label>

            <%-- פרטי הזמנה - שם הזמנה --%>
            <asp:Label ID="OrderName_Lable" runat="server" Text="שם הזמנה"></asp:Label>
            <asp:TextBox ID="OrderName" runat="server"></asp:TextBox>
            <br />

            <%-- כפתורי פרטי חופשה - בתוך דאטאליסט --%>
            <asp:DataList ID="DayPreferences" runat="server" RepeatDirection="Horizontal" RepeatColumns="5" CellSpacing="30" OnItemDataBound="DayPreferences_ItemDataBound">
                <ItemTemplate>
                    <table>
                        <tr>
                            <td >
                                <asp:Label ID="DayDate_Lable" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <asp:Label ID="StartPlace_Lable" runat="server" Text="כתובת התחלת מסלול"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <asp:TextBox ID="StartPlace" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                            <tr>
                            <td >
                                <asp:Label ID="StartDayTime_Lable" runat="server" Text="שעת תחילת היום"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <asp:TextBox ID="StartDayTime" TextMode="Time" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <asp:Label ID="EndDayTime_Lable" runat="server" Text="שעת סיום היום"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <asp:TextBox ID="EndDayTime" TextMode="Time" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>

            <br/>
            <br/>
            <%-- צקבוקסליסט סוגי אטרקציות --%>
            <asp:CheckBoxList ID="AttractionTypePreference" runat="server" AutoPostBack ="true"></asp:CheckBoxList>
            <br/>
            <%-- כפתור יצירת חופשה אוטומטית --%>
            <asp:Button ID="Create" runat="server" Text="Create" OnClick="Create_Click" />
            <br/>
            
        </div>
    </form>
</body>
</html>
