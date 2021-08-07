<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SCE.MainGear.admin.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html lang="en">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <!-- Meta, title, CSS, favicons, etc. -->
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Admin Login | MainGear </title>
    <!-- Bootstrap -->
    <link href="../vendors/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet">
    <!-- Font Awesome -->
    <link href="../vendors/font-awesome/css/font-awesome.min.css" rel="stylesheet">
    <!-- NProgress -->
    <link href="../vendors/nprogress/nprogress.css" rel="stylesheet">
    <!-- Animate.css -->
    <link href="../vendors/animate.css/animate.min.css" rel="stylesheet">
    <!-- Custom Theme Style -->
    <link href="../build/css/custom.min.css" rel="stylesheet">
    <!-- customized -->
    <link rel="stylesheet" type="text/css" href="../css/customized.css">
    <link href="../css/toastr.css" rel="stylesheet" />
    <script src="../Scripts/jquery-1.4.1.min.js"></script>
    <script src="../js/jquery.toaster.js"></script>
    <style>
        html 
        {
            background: -webkit-linear-gradient(top, #131111 30%, #0e0d0d 80%);
            background: -moz-linear-gradient(top, #131111 30%, #0e0d0d 80%);
            background: -o-linear-gradient(top, #131111 30%, #0e0d0d 80%);
            background: -ms-linear-gradient(top, #131111 30%, #0e0d0d 80%);
            height: 100%;
        }
    </style>
</head>
<body class="login">
    <div class="container-area">
        <!-- top navigation -->
        <div class="header-wrapper">
            <div class="text-center">
                <img src="../images/logo.png">
            </div>
        </div>
        <!-- /top navigation -->
        <div class="login_wrapper">
            <div class="animate form login_form">
                <section class="login_content">
                    <form id="Form1" runat="server">
                    <h1>
                        Admin Login</h1>
                    <div>
                        <%--<input type="text" class="form-control" placeholder="Username" required="" />--%>
                        <asp:TextBox ID="txtUsername" CssClass="form-control" runat="server" required=""
                            placeholder="Username"></asp:TextBox>
                    </div>
                    <div>
                        <%--<input type="password" class="form-control" placeholder="Password" required="" />--%>
                        <asp:TextBox ID="txtPassword" TextMode="Password" CssClass="form-control" runat="server"
                            required="" placeholder="Password"></asp:TextBox>
                    </div>
                    <div>
                        <%--<a class="btn btn-scheme-big btn-scheme submit" href="index.html">Log in</a>--%>
                        <asp:Button ID="btnLogin" CssClass="btn btn-scheme-big btn-scheme submit" 
                            runat="server" Text="Log in" onclick="btnLogin_Click"></asp:Button>
                        <%--<asp:LinkButton runat="server" ID="lbtnChangePass" CssClass="reset_pass" OnClick="lbtnChangePass_Click">Change Password</asp:LinkButton>--%>
                    </div>
                    <div class="clearfix">
                    </div>
                    </form>
                </section>
            </div>
        </div>
    </div>
    <!-- <div class="login-footer text-center">
      
    </div> -->
</body>
</html>
