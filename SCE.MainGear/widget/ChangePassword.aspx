<%@ Page Title="" Language="C#" MasterPageFile="~/widget/Admin.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="SCE.MainGear.widget.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3></h3>
                </div>
            </div>
            <div class="clearfix">
            </div>
            <div class="row">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div class="x_panel">
                        <div class="x_title">
                            <div class="breadcrumb-plane">
                                <ol class="breadcrumb">
                                    <li><a href="index.aspx" class="text-primary"><i class="fa fa-home" aria-hidden="true"></i></a><i class="fa fa-chevron-right" aria-hidden="true"></i></li>

                                    <li class="active text-danger">Change Password<i class="fa fa-chevron-right" aria-hidden="true"></i></li>
                                </ol>
                            </div>
                            <ul class="nav navbar-right panel_toolbox">
                            </ul>
                            <h2>Change Password
                                
                                <small></small>
                            </h2>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <%--<form class="form-horizontal form-label-left col-xs-12" action="steps.html" novalidate>--%>
                            <div class="col-xs-8">
                                <div class="form-group">
                                    <label class="control-label col-xs-2" for="name">
                                        Old Password <span class="required"></span>
                                    </label>
                                    <div class="col-xs-10 item">
                                        <asp:TextBox ID="txtOldPassword" CssClass="form-control" TextMode="Password" runat="server" required=""
                                            placeholder="Old Password"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-xs-2">
                                        New Password <span class="required"></span>
                                    </label>
                                    <div class="col-xs-10">
                                        <asp:TextBox ID="txtNewPassword" TextMode="Password" CssClass="form-control" runat="server"
                                            required="" placeholder="New Password"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-xs-2">
                                        Confirm Password <span class="required"></span>
                                    </label>
                                    <div class="col-xs-10">
                                        <asp:TextBox ID="txtConfirmPassword" TextMode="Password" CssClass="form-control" runat="server"
                                            required="" placeholder="Confirm Password"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="clearfix">
                            </div>
                        </div>

                        <div class="ln_solid col-xs-12">
                        </div>
                        <div class="form-group col-xs-12">
                            <div class="col-xs-12 text-right">
                                <asp:Button runat="server" ID="btnChange" CssClass="btn btn-scheme-big btn-scheme"
                                    Text="Change" OnClick="btnChange_Click"></asp:Button>
                                <asp:Button runat="server" ID="btnCancel" UseSubmitBehavior="false" data-dismiss="modal" OnClick="btnCancel_Click" CssClass="btn btn-classic-big btn-classic" Text="Cancel"></asp:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
