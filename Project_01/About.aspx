<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="Project_01.About" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="About.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%-- רווח --%>
    <div Style="height:20px;" ></div>

    <%-- כותרת תצוגה --%>
    <asp:Label ID="About_Lable" runat="server" Text="אודות Plan My Getaway" CssClass="label-style-navy" style="font-size: 50px;"></asp:Label>

    <%-- מידע --%>
    <asp:Label ID="Label1" runat="server" Text="Plan My Getaway הינו אתר תכנון חופשות חדשני המיועד לתכנון חופשות בניו יורק." CssClass="label-style-navy" style="font-size: 20px;"></asp:Label>
    <asp:Label ID="Label2" runat="server" Text="מטרתו העיקרית של האתר היא לחשוף את המשתמשים למגוון אטרקציות שונות, המסודרות לפי קטגוריות, ולהקל עליהם ביצירת מסלולי " CssClass="label-style-navy" style="font-size: 20px;"></asp:Label>
    <asp:Label ID="Label3" runat="server" Text="חופשה מותאמים אישית." CssClass="label-style-navy" style="font-size: 20px;"></asp:Label>

        <%-- רווח --%>
    <div Style="height:20px;" ></div>
    <asp:Label ID="Label11" runat="server" Text="דרכים ליצירת מסלולי חופשה" CssClass="label-style-navy" style="font-size: 30px;"></asp:Label>

    <asp:Label ID="Label4" runat="server" Text="האתר מציע שתי דרכים שונות ליצירת מסלולי חופשה:" CssClass="label-style-navy" style="font-size: 20px;"></asp:Label>
    <asp:Label ID="Label5" runat="server" Text=" אוטומטית: המשתמש מפרט את נתוניו והעדפותיו האישיות, והאתר יוצר עבורו מסלול חופשה מותאם אישית באופן אוטומטי." CssClass="label-style-navy" style="font-size: 20px;"></asp:Label>
    <asp:Label ID="Label6" runat="server" Text="ידנית: המשתמש בוחר ומשבץ את האטרקציות הרצויות מתוך הרשימה המקוטלגת של האתר, וכך בונה את מסלול החופשה בעצמו." CssClass="label-style-navy" style="font-size: 20px;"></asp:Label>

        <%-- רווח --%>
    <div Style="height:20px;" ></div>
    <asp:Label ID="Label12" runat="server" Text="פיצ'רים מרכזיים" CssClass="label-style-navy" style="font-size: 30px;"></asp:Label>
    <asp:Label ID="Label7" runat="server" Text="המלצות מותאמות: האתר ממליץ על אטרקציות חדשות בהתאם למסלולים קודמים שנוצרו על ידי המשתמש." CssClass="label-style-navy" style="font-size: 20px;"></asp:Label>
    <asp:Label ID="Label8" runat="server" Text="מידע מפורט: לכל אטרקציה באתר מצורף הסבר מפורט, כולל תמונות, שעות פעילות, מיקום, טיפים והמלצות." CssClass="label-style-navy" style="font-size: 20px;"></asp:Label>
    
        <%-- רווח --%>
    <div Style="height:20px;" ></div>
    <asp:Label ID="Label13" runat="server" Text="ייעוד האתר" CssClass="label-style-navy" style="font-size: 30px;"></asp:Label>
    <asp:Label ID="Label9" runat="server" Text="Plan My Getaway  נועד לקצר, להקל ולסדר את התהליך המורכב של תכנון חופשה. האתר מיועד לכלל האוכלוסייה ומאפשר לכל משתמש, " CssClass="label-style-navy" style="font-size: 20px;"></asp:Label>
    <asp:Label ID="Label10" runat="server" Text="בין אם הוא תייר חדש או מבקר חוזר, ליצור חופשה מושלמת לפי טעמו האישי." CssClass="label-style-navy" style="font-size: 20px;"></asp:Label>

    <%-- רווח --%>
    <div Style="height:20px;" ></div>
    <asp:Label ID="Label14" runat="server" Text="צור קשר" CssClass="label-style-navy" style="font-size: 30px;"></asp:Label>
    <asp:Label ID="Label15" runat="server" Text="לשאלות נוספות או תמיכה, ניתן להשאיר הודעה." CssClass="label-style-navy" style="font-size: 20px;"></asp:Label>
            <%-- רווח --%>
    <div Style="height:20px;" ></div>
    <asp:TextBox ID="Contact" runat="server" Placeholder="צור קשר" CssClass="Textbox-style-navy" Autopostback="true" OnTextChanged="Contact_TextChanged"></asp:TextBox>

        <%-- רווח --%>
    <div Style="height:40px;" ></div>
    <asp:Label ID="Label16" runat="server" Text="Plan My Getaway - רק תארזו, אנחנו נתכנן!" CssClass="label-style-navy" style="font-size: 35px;"></asp:Label>
            <%-- רווח --%>
    <div Style="height:60px;" ></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
