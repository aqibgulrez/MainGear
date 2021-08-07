<%@ Page Title="" Language="C#" MasterPageFile="~/widget/Admin.Master" AutoEventWireup="true" CodeBehind="BTOItems.aspx.cs" Inherits="SCE.MainGear.admin.BTOItems" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../vendors/select2/dist/css/select2.min.css" rel="stylesheet" />
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
                                    <li><a href="BTOItems.aspx" class="text-primary">BTO Items</a><i class="fa fa-chevron-right"
                                        aria-hidden="true"></i></li>
                                </ol>
                            </div>
                            <h2>BTO Items</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li>
                                    <asp:TextBox ID="txtSearchText" runat="server" CssClass="form-control col-md-7 col-xs-12"
                                        data-validate-length-range="10" data-validate-words="1" name="txtSearchText" placeholder="Product Title"></asp:TextBox></li>
                                <li class="btoitem-header-select">
                                    <asp:DropDownList ID="drpActive" runat="server">
                                        <asp:ListItem Text="All" Selected="True" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Inactive" Value="2"></asp:ListItem>
                                    </asp:DropDownList></li>
                                <li>
                                    <asp:LinkButton ID="lbtnSearch" CssClass="btn btn-classic" runat="server" OnClick="lbtnSearch_Click"><i
                                    class="fa fa-plus-circle" aria-hidden="true"></i>Search</asp:LinkButton></li>
                                <li><asp:LinkButton ID="lbtnExport" CssClass="btn btn-classic" runat="server" OnClick="lbtnExport_Click"><i
                                    class="fa fa-plus-circle" aria-hidden="true"></i>Export</asp:LinkButton></li>
                            </ul>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <div class="table-responsive">
                                <table class="table table-striped jambo_table bulk_action ">
                                    <thead>
                                        <tr class="headings">
                                            <th class="column-title">Product Title
                                            </th>
                                            <th class="column-title no-link last">
                                                <span class="nobr">Active?</span>
                                            </th>
                                            <th class="bulk-actions">
                                                <a class="antoo" style="color: #fff; font-weight: 500;">Bulk Actions ( <span class="action-cnt"></span>) <i class="fa fa-chevron-down"></i></a>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody class="sortable">
                                        <asp:Repeater runat="server" ID="rptrBTOItems" OnItemCreated="rptrBTOItems_ItemCreated">
                                            <ItemTemplate>
                                                <tr class="even pointer">
                                                    <td class=" ">
                                                        <%# Eval("ProductTitle") %><asp:HiddenField ID="hdnBTOItemID" Value='<%# Eval("BTOItemID") %>' runat="server" />
                                                    </td>
                                                    <td class=" ">
                                                        <asp:CheckBox runat="server" ID="chkIsActive" AutoPostBack="true" Checked='<%# Eval("IsActive") %>' />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        // select to call
        $('select').select2();
    </script>
</asp:Content>
