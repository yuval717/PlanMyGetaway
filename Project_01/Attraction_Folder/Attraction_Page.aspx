<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Attraction_Page.aspx.cs" Inherits="Project_01.AttractionPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="Attraction_Page.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%-- כותרות --%>
            <asp:Label ID="Attraction_Page_Lable" runat="server" Text="דף אטרקציה" CssClass="label-style-navy" style="font-size:60px; position:relative;"></asp:Label>
            <asp:Label ID="Attraction_Name" runat="server" Text="" CssClass="label-style-navy" style="font-size:40px; position:relative;"></asp:Label>
            <br />

    <%-- תמונת אטרקציה גדולה --%>
    <div class="ContainerForDiv">
            <asp:Image ID="Attraction_BigPhoto" runat="server" ImageUrl="" style="width: 905px; height: 600px; padding: 0px; background-color: #ccc; border-radius: 13px; "/>
    </div>

    <%-- רווח --%>
    <div style="height:20px;" ></div>

    <%-- תמונות אטרקציה נוספות - בעת לחיצה על תמונה - הופכת להיות התמונה הגדולה --%>
    <div class="ContainerForDiv">
    <asp:DataList ID="MorePhotos" runat="server" RepeatDirection="Horizontal" RepeatColumns="6" CellSpacing="3" OnItemCommand="MorePhotos_ItemCommand" >
        <ItemTemplate>
            <div class="item">
                <table class="table">
                    <tr>
                        <td class="ImageButton_Row">
                            <%-- התמונה הנוספת --%>
                            <asp:ImageButton ID="Photo" runat="server" ImageUrl='<%# Bind("FileLocation") %>' Height="90px" Width="90px" CssClass="imageButtonStyle" Style="border-radius: 8px;" />
                        </td>
                    </tr>
                </table>
            </div>
        </ItemTemplate>
    </asp:DataList>
</div>

    <%-- רווח --%>
    <div style="height:70px;" ></div>

    <%-- דיב מידע על אטרקציה --%>
    <div class="ContainerForDiv">
    <asp:Image ID="Image8" ImageUrl="~/pictures/info.png" runat="server" Height="50px" Width="50px" />
    <br />
    <br />
    <asp:Panel ID="Panel1" runat="server" CssClass="scrollable-panel" ScrollBars="Vertical">
        <asp:Label ID="Attraction_Text" runat="server" Text="" CssClass="label-style-navy" style="font-size: 25px;"></asp:Label>
    </asp:Panel>
    </div>

    <%-- רווח --%>
    <div style="height:70px;" ></div>

    <%-- דיב פרטי אטרקציה --%>
    <div class="ContainerForDiv">
    <div class ="Order">   
            <asp:Label ID="Attraction_Type" runat="server" Text="" CssClass="label-style-navy" style="top: 11.5%; right: 20%;" ></asp:Label>
            <asp:Image ID="Image2" ImageUrl="~/pictures/Airplane_silhouette_navy.svg" runat="server" Height="50px" Width="50px" style="position:absolute; top: 13.2%; right: 13%;"/>
            <br />
             <asp:Label ID="Attraction_Price" runat="server" Text="" CssClass="label-style-navy" style="top: 12%; right: 69%;"></asp:Label>
            <asp:Image ID="Image4" ImageUrl="~/pictures/cash.png" runat="server" Height="50px" Width="50px" style="position:absolute; top: 13.2%; right: 55.5%;"/>
            <br />
            <asp:Label ID="Attraction_Age" runat="server" Text="" CssClass="label-style-navy" style="top: 30.5%; right: 20%;"></asp:Label>
            <asp:Image ID="Image3" ImageUrl="~/pictures/cake.png" runat="server" Height="50px" Width="50px" style="position:absolute; top: 30.2%; right: 13%;" />
            <br />
            <asp:Label ID="Attraction_Duration" runat="server" Text="" CssClass="label-style-navy" style="top: 30.5%; right: 67%;"></asp:Label>
            <asp:Image ID="Image9" ImageUrl="~/pictures/clock.png" runat="server" Height="50px" Width="50px" style="position:absolute; top: 32.2%; right: 55.5%;"/>
            <br />
            <asp:Label ID="Attraction_Gmail" runat="server" Text="" CssClass="label-style-navy" style="font-size:20px; top: 50.5%; right: 20%;"></asp:Label>
            <asp:Image ID="Image6" ImageUrl="~/pictures/gmail.png" runat="server" Height="50px" Width="50px" style="position:absolute; top: 50.2%; right: 13%;"/>
            <br />
            <asp:Label ID="Attraction_PhonNumber" runat="server" Text="" CssClass="label-style-navy" style="top: 48.5%; right: 61.5%;"></asp:Label>
            <asp:Image ID="Image7" ImageUrl="~/pictures/phone.png" runat="server" Height="50px" Width="50px" style="position:absolute; top: 50.2%; right: 55.5%;"/>
            <br />
            <asp:Label ID="Attraction_Address" runat="server" Text="" CssClass="label-style-navy" style="font-size:30px; top: 70.2%; right: 19.5%;"></asp:Label>
            <asp:Image ID="Image5" ImageUrl="~/pictures/location.png" runat="server" Height="50px" Width="50px" style="position:absolute; top: 70.2%; right: 13%;"/>
        </div>
        </div>

    <%-- רווח --%>
    <div style="height:80px;" ></div>

    <div class="centered-div-EditUser" >
    <asp:Button ID="BackTo" runat="server" Text="חזור" OnClick="BackTo_Click" CssClass="Create-EditUser" Style="background-color:#f9f9f9;"/>
    </div>

    <%-- רווח --%>
    <div style="height:100px;" ></div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
