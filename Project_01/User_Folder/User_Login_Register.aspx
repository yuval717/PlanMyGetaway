<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_Login_Register.aspx.cs" Inherits="Project_01.User_Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="Login_Register.css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%-- בלוק רווח --%>
            <div style="height: 70px;" > </div>

            <%-- דיב כניסה --%>
            <div id="Login_div" class="container" runat="server" style="display: block;">
            <br />
            <%-- כותרת כניסה --%>
            <asp:Label ID="Login_Lable" CssClass="label-style-navy" runat="server" Text="כניסה"></asp:Label>
            <br />
            <%-- טקסטבוקס שם משתמש --%>
            <asp:TextBox ID="User_Name" runat="server" placeholder="שם משתמש" ></asp:TextBox>
            <br />
            <br />
            <br />
            <%-- טקסטבוקס סיסמה --%>
            <asp:TextBox ID="User_Password"  runat="server" placeholder="סיסמה" TextMode="Password"></asp:TextBox>
            <br />
            <br />
            <%-- כפתור כניסה --%>
            <asp:Button ID="Login" runat="server" Text="היכנס" OnClick="Button1_Click" />
            <br />
            <br />
            <%-- כותרת תשובה לכניסה - נכנס/לא קיים --%>
            <asp:Label ID="Result_LogIn" runat="server" Text=""></asp:Label>
            <br />
            <br />
            <%-- כפתור הרשמה --%>
            <asp:Button ID="DoRegister" runat="server" Text=" הירשם" OnClick="DoRegister_Click" />
            <%-- כותרת הרשמה --%>
            <asp:Label ID="DoRegister_Lable" runat="server" Text="? אין לך משתמש "></asp:Label>
            <br />
            <%-- כפתור שכחתי סיסמא --%>
            <asp:Button ID="ForgotPassword" runat="server" Text="שכחתי סיסמה" OnClick="ForgotPassword_Click" style="color:#003366" />
            </div>


            <%-- דיב שכחתי סיסמא --%>
            <div id="ForgotPassword_Div" class="container" runat="server" style="display: none;">
            <br />
            <%-- כותרת שכחתי סיסמא --%>
            <asp:Label ID="ForgotPassword_Lable" CssClass="label-style-navy" runat="server" Text="שכחתי סיסמה"></asp:Label>
            <br />
            <%-- טקסטבוקס שם משתמש --%>
            <asp:TextBox ID="Forgot_UserName" runat="server" placeholder="שם משתמש" ></asp:TextBox>
            <br />
            <br />
            <br />
            <%-- טקסטבוקס אימייל --%>
            <asp:TextBox ID="Forgot_Gmail"  runat="server" placeholder="אימייל" TextMode="Email"></asp:TextBox>
            <br />
            <br />
            <%-- כפתור שכחתי סיסמה --%>
            <asp:Button ID="ForgotPassWord_Button" runat="server" Text="שחזור סיסמה" OnClick="ForgotPassWord_Button_Click" />
            <br />
            <br />
            <%-- כותרת תשובה לשכחתי סיסמה - נכנס/לא קיים --%>
            <asp:Label ID="Forgot_Result" runat="server" Text=""></asp:Label>
            <br />
            <br />
            <%-- כפתור הרשמה --%>
            <asp:Button ID="Forgot_Register" runat="server" Text=" הירשם" OnClick="DoRegister_Click" />
            <%-- כותרת הרשמה --%>
            <asp:Label ID="Label3" runat="server" Text="? אין לך משתמש "></asp:Label>
            <br />
            </div>

            <%--  דיב הרשמה - פרטים אישיים --%>
            <div id="Register_div" class="container" runat="server" style="height: 716px; display: none;">

            <%-- כותרת - הרשמה --%>
            <asp:Label ID="Register_Lable" CssClass="label-style-navy" runat="server" Text="הרשמה"></asp:Label>
            <br />
            <%-- טקסטבוקס שם משתמש --%>
            <asp:TextBox ID="UserName" runat="server" placeholder="שם משתמש" ></asp:TextBox>
            <br />
            <%-- טקסטבוקס סיסמה --%>
            <asp:TextBox ID="Password" runat="server" placeholder="סיסמה"></asp:TextBox>
            <br />
            <%-- טקסטבוקס שם פרטי --%>
            <asp:TextBox ID="User_FirstName" runat="server" placeholder="שם פרטי"></asp:TextBox>
            <br />
            <%-- טקסטבוקס שם משפחה --%>
            <asp:TextBox ID="User_LastName" runat="server" placeholder="שם משפחה"></asp:TextBox>
            <div class="form-group">
                <asp:DropDownList ID="User_Gender" runat="server" CssClass="dropdown-class">
                <asp:ListItem>זכר</asp:ListItem>
                <asp:ListItem>נקבה</asp:ListItem>
                <asp:ListItem>אחר</asp:ListItem>
                </asp:DropDownList>
            </div>
            <%-- טקסטבוקס אימייל --%>
            <asp:TextBox ID="User_Gmail" runat="server" placeholder="אימייל" TextMode="Email"></asp:TextBox>
            <br />
            <%-- טקסטבוקס מספר טלפון --%>
            <asp:TextBox ID="User_PhoneNumber" runat="server" placeholder="מספר טלפון"></asp:TextBox>
            <div class="form-group">
                <asp:DropDownList ID="UserType" runat="server" CssClass="dropdown-class">
                <asp:ListItem>משתמש</asp:ListItem>
                <asp:ListItem>בעל עסק</asp:ListItem>
                </asp:DropDownList>
            </div>
            <%-- תשובות הרשמה --%>
            <asp:Label ID="Result_Register" runat="server" Text=""></asp:Label>
            <br />
            <br />
            <br />
            <%-- כפתור הרשמה --%>
            <asp:Button ID="Register" runat="server" Text="המשך" OnClick="Button2_Click" />

            <%-- בלוק רווח- סוף דף --%>
            <div style="height: 210px;" > </div>
            </div>



            <%--  דיב הרשמה - תשלום --%>
            <div id="Payment_div" class="container" runat="server" style="display: none; height: 580px;">

            <%-- כותרת - הרשמה --%>
            <asp:Label ID="Register_Lable2" CssClass="label-style-navy" runat="server" Text="הרשמה"></asp:Label>
            <%-- כותרת - תשלום --%>
            <asp:Label ID="Payment_Lable" CssClass="label-style-navy" runat="server" Text="תשלום" Style="font-size: 28px; padding: 4px 0; "></asp:Label>
            <br />
            <asp:TextBox ID="Number" runat="server" placeholder="מספר כרטיס אשראי"></asp:TextBox>
            <br />
            <asp:TextBox ID="DateOfExpiration" runat="server" TextMode ="month" placeholder="תוקף" ></asp:TextBox>
            <br />
            <asp:TextBox ID="CVV" runat="server" placeholder="CVV"></asp:TextBox>
            <br />
            <asp:DropDownList ID="Provider" runat="server">
                <asp:ListItem>ויזה</asp:ListItem>
                <asp:ListItem>אמריקן אקספרס</asp:ListItem>
                <asp:ListItem>מאסטר קארד</asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:TextBox ID="Owner_ID" runat="server" placeholder="תעודת זהות"></asp:TextBox>
            <br />
            <%-- כותרת תשובה להרשמה - נרשם/בעיה בתשלום/בפרטים --%>
            <asp:Label ID="Result_Payment" runat="server" Text=""></asp:Label>
            <br />
            <asp:Label ID="PayNumber" runat="server" Text="שח 100" Style="cursor:auto;"></asp:Label>
             <br />
             <br />
            <asp:Button ID="enter" runat="server" Text="תשלום והרשמה" Style="padding: 14px 0;" OnClick="Button3_Click" />

            <%-- בלוק רווח- סוף דף --%>
            <div style="height: 210px;" > </div>
            </div>


        </div>
    </form>
</body>
</html>
