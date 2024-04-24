<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Attraction_Display.aspx.cs" Inherits="Project_01.Attraction_Display" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Attraction Display</title>
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtFilter").on("input", function () {
                var filterText = $(this).val().trim().toLowerCase();
                $("#<%=DataList1.ClientID%> div.item").hide();
                $("#<%=DataList1.ClientID%> div.item:contains('" + filterText + "')").show();
            });
        });
    </script>
    <style type="text/css">
        .auto-style1 {
            text-align: left;
        }
        .auto-style2 {
            width: 95px;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>תצוגת אטרקציות</h2>
            <asp:TextBox ID="txtFilter" runat="server" placeholder="הכנס שם אטרקציה" />
            <br />
            <div class="auto-style1">
            <asp:DataList ID="DataList1" runat="server"  OnItemCommand="DataList1_ItemCommand" OnItemDataBound="DataList1_ItemDataBound"  RepeatDirection="Horizontal" RepeatColumns="5" CellSpacing="3">
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

            <br />
            <asp:Label ID="Label8" runat="server" Text="סינונים"></asp:Label>
            <br />
            <br />
            <asp:Label ID="Label3" runat="server" Text="סוגי אטרקציה "></asp:Label>
            <br />
            <asp:CheckBoxList ID="CheckBoxList1" runat="server" ></asp:CheckBoxList>
            <br />
            <asp:Label ID="Label4" runat="server" Text="טווח גילאים"></asp:Label>
            <br />
            <asp:TextBox ID="Min_Age" runat="server" placeholder="גיל מינימלי"  ></asp:TextBox>
            <br />
            <asp:TextBox ID="Max_Age" runat="server" placeholder="גיל מקסימלי"  ></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="Label5" runat="server" Text="מחיר"></asp:Label>
            <br />
            <asp:TextBox ID="Price" runat="server" placeholder="מחיר" ></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="Label6" runat="server" Text="משך אטרקציה"></asp:Label>
            <br />
            <asp:TextBox ID="Duration" runat="server" placeholder="משך אטרקציה" ></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="Label7" runat="server" Text="כניסה חופשית"></asp:Label>
            <br />
            <asp:DropDownList ID="Attraction_FreeEntry" runat="server" AutoPostBack="true" CssClass="auto-style1">
            <asp:ListItem Value="האם כניסה חופשית">האם כניסה חופשית</asp:ListItem>
            <asp:ListItem Value="true">כן</asp:ListItem>
            <asp:ListItem Value="false">לא</asp:ListItem>
            </asp:DropDownList>
            <br />
            <%--<asp:Label ID="Label2" runat="server" Text="סידור לפי האחרונות שהועלו"></asp:Label>
            <br />
                <asp:CheckBoxList ID="Attraction_OrderByDate" runat="server" AutoPostBack="true" OnSelectedIndexChanged="CheckBoxList2_SelectedIndexChanged" >
                    <asp:ListItem>כן</asp:ListItem>
                    <asp:ListItem>לא</asp:ListItem>
                </asp:CheckBoxList>--%>
            <br />
            <br />
            <asp:Button ID="filter" runat="server" Text="חפש" OnClick="filter_Click1"  />
            <br />
            <asp:Button ID="Button_All" runat="server" Text="הכל" OnClick="Button_All_Click" />

                <asp:Button ID="Button2" runat="server" Text="חזור" OnClick="Button2_Click" />
        </div>
    </form>
</body>
</html>

