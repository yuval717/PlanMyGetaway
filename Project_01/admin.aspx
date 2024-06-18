<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="Project_01.admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="Admin.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%-- תצוגת שם משתמש --%>
    <asp:Label ID="MasterPage_UserName" runat="server" Text="" Style="position:absolute; top:30px; left:15px; margin: 0 20px; text-decoration: none; color: #FFFFFF; font-size: 20px; font-weight: bold; transition: color 0.3s;"></asp:Label>
<asp:Label ID="AdminPage_Lable" runat="server" Text="דף מנהל" CssClass="label-style-navy" Style="font-size:60px" ></asp:Label>
    <br/>
<asp:Label ID="User_Search" runat="server" Text="חפש משתמש" CssClass="label-style-navy" ></asp:Label>
<br/>
<asp:TextBox ID="SearchBar" runat="server" AutoPostBack ="true" OnTextChanged="SearchBar_TextChanged" CssClass="Textbox-style-navy" ></asp:TextBox>
<br/>
<div class="datalist-container">
    <asp:DataList ID="DataList1" runat="server" CellSpacing="3" OnItemCommand="DataList1_ItemCommand" OnItemDataBound="DataList1_ItemDataBound" CssClass="custom-datalist">
        <ItemTemplate>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="User_Name" runat="server" Text='<%# Bind("User_Name") %>' CssClass="name-label"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="User_FirstName" runat="server" Text='<%# Bind("User_FirstName") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="User_LastName" runat="server" Text='<%# Bind("User_LastName") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="User_Gmail" runat="server" Text='<%# Bind("User_Gmail") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="User_PhoneNumber" runat="server" Text='<%# Bind("User_PhoneNumber") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="User_Gender" runat="server" Text='<%# Bind("User_Gender") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="User_LastEntrance" runat="server" Text='<%# Bind("User_LastEntrance") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Button ID="User_VacationsOrAttractions" runat="server" Text="חופשות המשתמש" CssClass="block-button" Style="background-color:#b3e0ff;" CommandName="Move"/>
                    </td>
                    <td>
                        <asp:Label ID="User_IsBlocked" runat="server" Text='<%# Bind("User_IsBlocked") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Button ID="Is_Blocked" runat="server" Text="Block / Unblock" CommandName="DoShow" CssClass="block-button" />
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:DataList>
</div>

<br/>
<br/>
    <asp:Label ID="BlockedUsers_Lalbe" runat="server" Text="משתמשים חסומים" CssClass="label-style-navy" ></asp:Label>
<div class="datalist-container">
    <asp:DataList ID="DataList2" runat="server" CellSpacing="3" OnItemCommand="DataList2_ItemCommand" OnItemDataBound="DataList2_ItemDataBound" CssClass="custom-datalist">
        <ItemTemplate>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="User_Name" runat="server" Text='<%# Bind("User_Name") %>' CssClass="name-label"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="User_FirstName" runat="server" Text='<%# Bind("User_FirstName") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="User_LastName" runat="server" Text='<%# Bind("User_LastName") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="User_Gmail" runat="server" Text='<%# Bind("User_Gmail") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="User_PhoneNumber" runat="server" Text='<%# Bind("User_PhoneNumber") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="User_Gender" runat="server" Text='<%# Bind("User_Gender") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="User_LastEntrance" runat="server" Text='<%# Bind("User_LastEntrance") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="User_Type" runat="server" Text='<%# Bind("User_IsAttractionOwner") %>' Visible="false"></asp:Label>
                        <asp:Button ID="User_VacationsOrAttractions" runat="server" Text="חופשות המשתמש" CssClass="block-button" Style="background-color:#b3e0ff;" CommandName="Move" />
                    </td>
                    <td>
                        <asp:Label ID="User_IsBlocked" runat="server" Text='<%# Bind("User_IsBlocked") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Button ID="Is_Blocked" runat="server" Text="Block / Unblock" CommandName="DoShow" CssClass="block-button" />
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:DataList>
</div>

    <%-- רווח --%>
    <div style="height:80px;"></div>

    <asp:Label ID="AttractionType_Label" runat="server" Text="סוגי אטרקציה" CssClass="label-style-navy" ></asp:Label>
    <br />
    <div class="datalist-container">
    <asp:Button ID="AttractionType_Add" runat="server" Text="הוספת  סוג אטרקציה" CssClass="block-button" Style="width:400px; height:50px; border-radius: 10px; font-size:25px;" OnClick="AttractionType_Add_Click" />
    <asp:Button ID="AttractionType_Edit" runat="server" Text="עריכת סוג אטרקציה" CssClass="block-button" Style="width:400px; height:50px; border-radius: 10px; font-size:25px;" OnClick="AttractionType_Edit_Click" />
    <asp:Button ID="AttractionType_Remove" runat="server" Text="מחיקת סוג אטרקציה" CssClass="block-button" Style="width:400px; height:50px; border-radius: 10px; font-size:25px;" OnClick="AttractionType_Remove_Click" />
    </div>
    <br />
    <br />

    <div id="AddDivAttraction" class="datalist-container" style="display: none; text-align: center;" runat="server">
    <asp:DropDownList id="VactionType_Add_DropDownList" runat="server" AutoPostBack=true CssClass="Textbox-Typs" style="width:300px;"></asp:DropDownList>
    <asp:TextBox ID="AttractionType_Add_TextBox" runat="server" placeholder="שם אטרקציה להוספה" CssClass="Textbox-Typs"></asp:TextBox>
    <asp:Button ID="AttractionType_Add_Button" runat="server" Text="הוספה" CssClass="Button-Typs" OnClick="AttractionType_Add_Button_Click"/>
    </div>

    <div id="RemoveDivAttraction" class="datalist-container" style="display: none; text-align: center;" runat="server">
    <asp:DropDownList id="AttractionType_Remove_DropDownList" runat="server" AutoPostBack=true CssClass="Textbox-Typs" style="width:300px;"></asp:DropDownList>
    <asp:Button ID="AttractionType_Remove_Button" runat="server" Text="מחיקה" CssClass="Button-Typs" OnClick="AttractionTypeRemove_Button_Click"/>
    </div>

    <div id="EditDivAttraction" class="datalist-container" style="display: none; text-align: center;" runat="server">
    <asp:DropDownList id="AttractionType_Edit_DropDownList" runat="server" AutoPostBack=true CssClass="Textbox-Typs" style="width:300px;"></asp:DropDownList>    
        <asp:TextBox ID="AttractionType_Edit_TextBox" runat="server" placeholder="שם  סוג אטרקציה חדש" CssClass="Textbox-Typs"></asp:TextBox>
        <asp:Button ID="AttractionType_Edit_Button" runat="server" Text="עריכה" CssClass="Button-Typs" OnClick="AttractionType_Edit_Button_Click"/>
    </div>

     <br />
    <%-- רווח --%>
    <div style="height:50px;"></div>
    <asp:Label ID="VacationType_Label" runat="server" Text="סוגי חופשה" CssClass="label-style-navy" ></asp:Label>
    <br />
    <div class="datalist-container">
    <asp:Button ID="VacationType_Add" runat="server" Text="הוספת סוג חופשה" CssClass="block-button" Style="width:400px; height:50px; border-radius: 10px; font-size:25px;"  OnClick="VacationType_Add_Click"/>
    <asp:Button ID="VacationType_Edit" runat="server" Text="עריכת סוג חופשה" CssClass="block-button" Style="width:400px; height:50px; border-radius: 10px; font-size:25px;" OnClick="VacationType_Edit_Click"/>
    <asp:Button ID="VacationType_Remove" runat="server" Text="מחיקת סוג חופשה" CssClass="block-button" Style="width:400px; height:50px; border-radius: 10px; font-size:25px;" OnClick="VacationType_Remove_Click"/>
    </div>

    <br />
    <br />

    <div id="AddDivVacation" class="datalist-container" style="display: none; text-align: center;" runat="server">
    <asp:TextBox ID="VacationType_ToAdd_Drop" runat="server" placeholder="שם אטרקציה להוספה" CssClass="Textbox-Typs"></asp:TextBox>
    <asp:Button ID="VacationTypeAdd_Button" runat="server" Text="הוספה" CssClass="Button-Typs" OnClick="VacationTypeAdd_Button_Click"/>
    </div>

    <div id="RemoveDivVacation" class="datalist-container" style="display: none; text-align: center;" runat="server">
    <asp:DropDownList id="VacationType_ToRemove_DropDownList" runat="server" AutoPostBack=true CssClass="Textbox-Typs" style="width:300px;"></asp:DropDownList>
    <asp:Button ID="VacationTypeRemove_Button" runat="server" Text="מחיקה" CssClass="Button-Typs" OnClick="VacationTypeRemove_Button_Click"/>
    </div>

    <div id="EditDivVacation" class="datalist-container" style="display: none; text-align: center;" runat="server">
    <asp:DropDownList id="VacationType_ToEdit_DropDownList" runat="server" AutoPostBack=true CssClass="Textbox-Typs" style="width:300px;"></asp:DropDownList>    
        <asp:TextBox ID="VacationType_ToEdit" runat="server" placeholder="שם  סוג אטרקציה חדש" CssClass="Textbox-Typs"></asp:TextBox>
        <asp:Button ID="VacationEditAdd_Button" runat="server" Text="עריכה" CssClass="Button-Typs" OnClick="VacationEditAdd_Button_Click"/>
    </div>

    <br />
    <asp:Label ID="Result_Att" runat="server" Text="" CssClass="label-style-navy" Style="font-size:20px; color:red;"></asp:Label>
    <br />

    <%-- רווח --%>
    <div style="height:100px;"></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
