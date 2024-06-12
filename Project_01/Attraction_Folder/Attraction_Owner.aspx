<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Attraction_Owner.aspx.cs" Inherits="Project_01.Attraction_Owner" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="Attraction_Owner.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%-- כותרת בעל עסק --%>
    <asp:Label ID="Attraction_Owner_Lable" runat="server" Text="בעל עסק" CssClass="label-style-navy" style="font-size: 60px; position:absolute; top: 11%; right: 44.8%;"></asp:Label>
    <br />
    <%-- כותרת האטרקציות שלי --%>
    <asp:Label ID="MyAttractions_Lable" runat="server" Text="האטרקציות שלי" CssClass="label-style-navy" style="font-size: 40px; position:absolute; top: 21%; right: 43.8%;"></asp:Label>
    
    <%-- רווח --%>
    <div style="height:180px"></div>

    <%-- דאטאליסט אטרקציות משתמש+ נתונים ואפשריות --%>
<asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Height="1030px" Style="overflow-y: auto;">
<asp:DataList ID="Attractions" runat="server" RepeatDirection="Vertical" CellSpacing="3" OnItemCommand="Attractions_ItemCommand" OnItemDataBound="Attractions_ItemDataBound" CssClass="attractions">
    <ItemTemplate>
        <div class="attraction-item">
            <table>
            <%-- מרכוז השם והתמונה --%>
            <div class="centered-div">
            <div class="content">
            <!-- Attraction Name -->
            <asp:Label ID="Attraction_Name" runat="server" Text='<%# Bind("Attraction_Name") %>' CssClass="label-style-navy"></asp:Label>
            <asp:Label ID="Attraction_ID" runat="server" Text='<%# Bind("Attraction_ID") %>' Visible="false"></asp:Label>
            </div>
            <div class="content">
            <!-- Attraction Photo -->
            <asp:Image ID="Photo" runat="server" ImageUrl='<%# Bind("Attraction_Photo") %>' style="width: 600px; height: 400px; padding: 0px; background-color: #ccc; border-radius: 13px;"/>
            </div>
            </div>

                </tr>
                <div class="centered-div">
                <div class="content">
                    <br />  
                    <br /> 
                        <!-- Statistics Title -->
                        <asp:Label ID="Statistics_Label" runat="server" Text="סטטיסטיקות" CssClass="label-style-navy" style="font-size: 30px;"></asp:Label>
                </div>
                </div>
                </tr>
                <tr class="statistics">
                    <td>
                        <!-- Add Number -->
                        <asp:Label ID="AddNumber_Label" runat="server" Text="מספר שיבוצים במסלולים" CssClass="label-style-navy" style="font-size: 20px;"></asp:Label>
                        <br />
                        <asp:Label ID="AddNumber" runat="server" Text="" CssClass="label-style-navy" style="font-size: 20px; font-weight:bold; color:#b3e0ff;"></asp:Label>
                    </td>
                    <td>
                        <br />  
                        <!-- Main Gender -->
                        <asp:Label ID="MainGender_Label" runat="server" Text="מגדר רוב המשתמשים " CssClass="label-style-navy" style="font-size: 20px; padding:0px; margin:0px;"></asp:Label>
                        <asp:Label ID="Label1" runat="server" Text="ששיבצו את האטרקציה" CssClass="label-style-navy" style="font-size: 20px; padding:0px; margin:0px;"></asp:Label>
                        <br />
                        <asp:Label ID="MainGender" runat="server" Text="" CssClass="label-style-navy" style="font-size: 20px; font-weight:bold; color:#b3e0ff;" ></asp:Label>
                     <br />  
                    <br /> 
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                        <br />
                        <td>
                <tr>
                <tr>
                    <td>
                        <!-- Remove Attraction -->
                        <asp:Button ID="remove" runat="server" Text="מחיקת אטרקציה"  CssClass="Create" CommandName="remove"/>
                    </td>
                    <td>
                        <!-- Edit Attraction -->
                        <asp:Button ID="edit" runat="server" Text="עריכת אטרקציה" CssClass="Create" CommandName="edit" />
                    </td>
                </tr>

            </table>
        </div>
    </ItemTemplate>
</asp:DataList>
</asp:Panel>

    <%-- רווח --%>
    <div style="height:100px;"></div>

    <%-- הוספת אטרקציה --%>
    <div class="centered-div" >   
    <asp:Button ID="AddAttraction" runat="server" Text="הוספת אטרקציה" CssClass="Create" OnClick="AddAttraction_Click" />
    </div>

    <%-- עריכת פרטי משתמש --%>
    <div class="centered-div" >   
    <asp:Button ID="User_Edit" runat="server" Text="עריכת פרטי משתמש" CssClass="Create" OnClick="User_Edit_Click" Style="background-color:#f9f9f9;" />
    </div>

    <%-- רווח --%>
    <div style="height:100px;"></div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
