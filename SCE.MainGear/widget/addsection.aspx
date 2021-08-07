<%@ Page Title="" Language="C#" MasterPageFile="~/widget/Admin.Master" AutoEventWireup="true"
    CodeBehind="addsection.aspx.cs" Inherits="SCE.MainGear.admin.addsection" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- select 2 -->
    <link href="../vendors/select2/dist/css/select2.min.css" rel="stylesheet" />
    <!-- Switchery -->
    <link href="../vendors/switchery/dist/switchery.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../vendors/bootstrap-wysiwyg/js/bootstrap-wysiwyg.min.js"></script>
    <script type="text/javascript" src="../vendors/jquery.hotkeys/jquery.hotkeys.js"></script>
    <script type="text/javascript" src="../vendors/google-code-prettify/src/prettify.js"></script>
    <script type="text/javascript" src="../js/tagging.min.js"></script>
    <link href="../css/tag-basic-style.css" rel="stylesheet" />
    <style type="text/css">
        .BTOItemOptionsSelectedtxt, .hdntagsInput, .hdnRowFilterNo, .BTOItemSelectedWholesaleID{
            display:none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- page content -->
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
                                    <li><a href="index.aspx" class="text-primary">Product List</a><i class="fa fa-chevron-right"
                                        aria-hidden="true"></i></li>
                                    <li>
                                        <asp:LinkButton runat="server" ID="lbtnSections" CssClass="text-primary" OnClick="lbtnSections_Click">Sections</asp:LinkButton><i class="fa fa-chevron-right"
                                            aria-hidden="true"></i></li>
                                    <li class="active text-danger">
                                        <asp:Literal runat="server" ID="ltrlAddSectionBreadcrumb">Add Section</asp:Literal><i class="fa fa-chevron-right" aria-hidden="true"></i></li>
                                </ol>
                            </div>
                            <ul class="nav navbar-right panel_toolbox">
                            </ul>
                            <h2>Add Section
                                <asp:HiddenField ID="hdnSectionID" Value="" runat="server" />
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
                                        Title <span class="required"></span>
                                    </label>
                                    <div class="col-xs-10 item">
                                        <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control col-md-7 col-xs-12"
                                            data-validate-length-range="10" data-validate-words="1" name="txtTitle" placeholder="Section Title"
                                            required="required"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-xs-2">
                                        Description <span class="required"></span>
                                    </label>
                                    <div class="col-xs-10">
                                        <asp:TextBox TextMode="MultiLine" runat="server" ID="txtaDescription" Rows="5" CssClass="form-control"
                                            placeholder="Add Some Description" Style="margin: 0px; height: 104px;"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-xs-2">
                                        Image <span class="required"></span>
                                    </label>
                                    <div class="col-xs-10">
                                        <label class="img-upload">
                                            <p class="img-upload-text">
                                                Click to select an image
                                            </p>
                                            <input type="file" name="fuImage" runat="server" id="fuImage" />
                                        </label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-xs-2">
                                        Pricing <span class="required"></span>
                                    </label>
                                    <div class="col-xs-10">
                                        <asp:DropDownList runat="server" ID="drpPricings" Width="100%" CssClass="tempselect">
                                        </asp:DropDownList>
                                        <%--<select style="width: 100%;">
                                            <option>Relative</option>
                                            <option>Absolute</option>
                                        </select>--%>
                                    </div>
                                </div>
                                <div id="divSteps" class="form-group" runat="server">
                                    <label class="control-label col-xs-2">
                                        Step <span class="required"></span>
                                    </label>
                                    <div class="col-xs-10">
                                        <asp:DropDownList runat="server" ID="drpProductSteps" Width="100%" CssClass="tempselect">
                                        </asp:DropDownList>
                                        <%--<select style="width: 100%;">
                                            <option>Step 1 Design</option>
                                            <option>Step 2 Chipset</option>
                                            <option selected>Step 3 Performance</option>
                                            <option>Step 4 Peripherals, Software and Gear</option>
                                        </select>--%>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-xs-2">
                                        Render Mode <span class="required"></span>
                                    </label>
                                    <div class="col-xs-10">
                                        <asp:DropDownList runat="server" ID="drpRenderModes" Width="100%" CssClass="tempselect">
                                        </asp:DropDownList>
                                        <%--<select style="width: 100%;">
                                            <option>Product View</option>
                                            <option>Comparison View</option>
                                            <option selected>List View</option>
                                        </select>--%>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-xs-2">
                                        Required <span class="required"></span>
                                    </label>
                                    <div class="col-xs-10">
                                        <div class="checkbox">
                                            <label class="nopadding">
                                                <asp:CheckBox CssClass="flat  quantity-check" runat="server" ID="chkIsRequired" Checked="false" /><span
                                                    class="radio-styled"></span>
                                            </label>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-xs-2">
                                    </label>
                                    <div class="col-xs-10">
                                        <div class="col-sm-6" style="padding: 0px 10px 0px 0px;">
                                            <%--<button type="button" class="btn btn-classic btn-block" data-toggle="modal" data-target="#subHeadingModal"
                                                type="button">
                                                Add Sub Heading
                                            </button>--%>
                                            <asp:Button ID="btnAddSubHeading" runat="server" Text="Add Sub Heading" UseSubmitBehavior="false"
                                                data-dismiss="modal" CssClass="btn btn-classic btn-block" OnClick="btnAddSubHeading_Click" />
                                        </div>
                                        <div class="col-sm-6" style="padding: 0px 0px 0px 10px;">
                                            <%--<button type="button" class="btn btn-classic btn-block" onclick="$('#<%=hdnSubSectionID.ClientID %>').val('0'); modalCustom('BTOModal');">
                                                Add Item</button>--%>
                                            <asp:Button ID="btnAddItemFromMain" runat="server" Text="Add Item" UseSubmitBehavior="false"
                                                data-dismiss="modal" CssClass="btn btn-classic btn-block" OnClick="btnAddItemFromMain_Click" />
                                        </div>
                                    </div>
                                </div>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="col-xs-4 text-center">
                                <div id="image-holder" class="col-xs-12">
                                    <%--<i class="fa fa-file-image-o" aria-hidden="true"></i>
                                    <br>
                                    <br>
                                    <span>No Image Selected</span>--%>
                                    <asp:Image ID="imgSection" runat="server" />
                                </div>
                            </div>
                            <asp:Repeater runat="server" ID="rptrSubSections" OnItemCommand="rptrSubSections_ItemCommand"
                                OnItemDataBound="rptrSubSections_ItemDataBound" OnItemCreated="rptrSubSections_ItemCreated">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="data col-xs-12 nopadding">
                                        <div class="table-heading">
                                            <table width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <h4>
                                                            <%# Eval("Title") %></h4>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button runat="server" ID="btnAddItemFromRepeater" CommandArgument='<%# Eval("SubSectionID") %>'
                                                            CommandName="additem" CssClass="btn btn-classic " Text="Add Item"
                                                            UseSubmitBehavior="false" data-dismiss="modal"></asp:Button>
                                                        <asp:Button runat="server" ID="btnDeleteSubSection" CommandArgument='<%# Eval("SubSectionID") %>'
                                                            CommandName="delsub" CssClass="btn btn-classic " Text="Delete"
                                                            UseSubmitBehavior="false" data-dismiss="modal" ></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <asp:HiddenField ID="hdnSubSectionID" Value='<%# Eval("SubSectionID") %>' runat="server" />
                                        <asp:Repeater runat="server" ID="rptrSectionItems">
                                            <HeaderTemplate>
                                                <table class="table">
                                                    <thead>
                                                        <tr>
                                                            <th>Title
                                                            </th>
                                                            <th>Price Level
                                                            </th>
                                                            <th>Action
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody class="sortable">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr id='<%# Eval("SubSectionID") %>-<%# Eval("SectionItemID") %>-<%# Eval("DisplayOrder") %>'>
                                                    <td>
                                                        <%# Eval("ProductTitle") %>
                                                    </td>
                                                    <td>
                                                        <%# Eval("WholesaleTitle") %>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton runat="server" ID="lbtnEditBTOItem" CommandArgument='<%# Eval("SectionItemID") %>'
                                                            CommandName="edit">
                                                            <img alt="edit" src="../images/edit.png" width="18px"></asp:LinkButton>
                                                        &nbsp;&nbsp;
                                                        <asp:LinkButton runat="server" ID="lbtnDeleteBTOItem" CommandArgument='<%# Eval("SectionItemID") %>'
                                                            CommandName="del" OnClientClick="return confirm('Are you certain you want to delete this item?');">
                                                            <img alt="delete" src="../images/recycle-bin.png" width="18px">
                                                        </asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody> </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:Repeater>
                            <div class="ln_solid col-xs-12">
                            </div>
                            <div class="form-group col-xs-12">
                                <div class="col-xs-12 text-right">
                                    <asp:Button runat="server" ID="btnSave" CssClass="btn btn-scheme-big btn-scheme"
                                        Text="Save" OnClick="btnSave_Click" UseSubmitBehavior="false" data-dismiss="modal"></asp:Button>
                                    <asp:Button runat="server" ID="btnSaveAndNext" CssClass="btn btn-scheme-big btn-scheme"
                                        Text="Save & Next" OnClick="btnSaveAndNext_Click" UseSubmitBehavior="false" data-dismiss="modal"></asp:Button>
                                    <asp:Button runat="server" ID="btnCancelSection" OnClick="btnCancelSection_Click" CssClass="btn btn-classic-big btn-classic" UseSubmitBehavior="false" data-dismiss="modal" Text="Cancel"></asp:Button><asp:HiddenField ID="hdnSubSectionID" Value="0" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- /page content -->
    <!-- Modal for add sub heading -->
    <div class="modal fade" id="subHeadingModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel">Add Sub Heading</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label class="control-label col-md-3 col-sm-3 col-xs-12 text-right" for="name">
                            Title
                        </label>
                        <div class="col-md-6 col-sm-6 col-xs-12">
                            <asp:TextBox ID="txtSubHeading" CssClass="form-control col-md-7 col-xs-12" autocomplete="off"
                                placeholder="Sub Heading" runat="server" />
                        </div>
                    </div>
                    <div class="clearfix">
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Cancel</button>
                    <asp:Button ID="btnSaveSubSection" runat="server" CssClass="btn btn-scheme " Text="Save"
                        UseSubmitBehavior="false" data-dismiss="modal" OnClick="btnSaveSubSection_Click1" />
                    <%--<asp:Button runat="server" ID="btnSaveSubSection" 
                        CssClass="btn btn-scheme " Text="Save" 
                        OnClick="btnSaveSubSection_Click"></asp:Button>--%>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal for BTO item -->
    <div class="modal-custom" id="BTOModal">
        <div class="modalc-inner">
            <div class="modal-header">
                <button type="button" class="close" onclick="modalCustomClose('BTOModal')">
                    <span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="myModalLabel1">Add Items</h4>
            </div>
            <div class="modalc-body">
                <asp:Panel runat="server" ID="pnlBTOItems" CssClass="row bto-items">
                    <asp:Repeater ID="rptrBTOItems" runat="server" OnItemDataBound="rptrBTOItems_ItemDataBound">
                        <ItemTemplate>
                            <div class="col-sm-4">
                                <div class="bto-item">
                                    <div class="col-xs-12">
                                        <div class="radio">
                                            <label>
                                                <asp:RadioButton ID="rbtnIsDefault" GroupName="isdefault" runat="server" class=""
                                                    Checked="false" name="defaultItem" /><span class="radio-styled"></span> Default<asp:HiddenField
                                                        ID="hdnItemID" Value="0" runat="server" />
                                            </label>
                                        </div>
                                    </div>
                                    <div class="col-xs-12">
                                        <table style="width: 100%; table-layout: fixed;">
                                            <tr>
                                                <td style="width: 30%">Item
                                                </td>
                                                <td style="width: 70%">
                                                    
                                                    <asp:DropDownList ID="ddlBTOItem" Style="width: 100%;" runat="server" CssClass="tempselect" onchange="BTOItemDDLOnChange(this);">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="col-xs-12 cntnrWholesale">
                                        <table style="width: 100%; table-layout: fixed;">
                                            <tr>
                                                <td style="width: 30%">Wholesale
                                                </td>
                                                <td style="width: 70%">
                                                    <asp:TextBox ID="BTOItemSelectedWholesaleID" runat="server" Text="" CssClass="BTOItemSelectedWholesaleID"></asp:TextBox>
                                                    <asp:DropDownList ID="drpWholesale" Style="width: 100%;" runat="server" CssClass="tempselect" onchange="ApplyWholesale(this);">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="col-xs-12 cntnrBTOItemPrice">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 30%">Price
                                                </td>
                                                <td style="width: 70%" class="inline-checkboxes">
                                                    <div>
                                                        <asp:Label CssClass="form-control" runat="server" ID="txtPrice" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="col-xs-12 cntnrBTOItemOptions">
                                        <table style="width: 100%; table-layout: fixed;">
                                            <tr>
                                                <td style="width: 30%">
                                                    <asp:Label ID="ltrlBTOItemOptionTitle" class="ltrlBTOItemOptionTitle" runat="server" Text="Option Title"></asp:Label>
                                                </td>
                                                <td style="width: 70%">
                                                    <asp:TextBox ID="txtBTOItemOptionsSelectedOptions" runat="server" class="BTOItemOptionsSelectedtxt" Text=""></asp:TextBox>
                                                    <asp:ListBox ID="lstbxBTOItemOptions" Style="width: 100%;" CssClass="OptionsSelect" SelectionMode="Multiple" runat="server" onchange="SaveSelectedValuesAsString(this);">

                                                    </asp:ListBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="col-xs-12">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 30%">Display Mode
                                                </td>
                                                <td style="width: 70%" class="inline-checkboxes">
                                                    <div class="checkbox">
                                                        <label>
                                                            <asp:RadioButton ID="rbtnDisplayModeRadio" GroupName="DisplayMode1" Checked="true"
                                                                runat="server" /><span class="radio-styled"></span> Radio
                                                        </label>
                                                    </div>
                                                    <div class="checkbox">
                                                        <label>
                                                            <asp:RadioButton ID="rbtnDisplayModeCheck" GroupName="DisplayMode1" runat="server" /><span
                                                                class="radio-styled"></span> Checkbox
                                                        </label>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="col-xs-12">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 30%">Processing Time
                                                </td>
                                                <td style="width: 70%" class="inline-checkboxes">
                                                    <div>
                                                        <asp:TextBox type="number" placeholder="1" CssClass="form-control" min="0" runat="server"
                                                            ID="txtProcessingTime" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="col-xs-12">
                                        <table style="width: 100%; table-layout: fixed;">
                                            <tr>
                                                <td style="width: 30%">Recommendation
                                                </td>
                                                <td style="width: 70%" class="inline-checkboxes">
                                                    <asp:DropDownList ID="drpRecommendations" Style="width: 100%;" runat="server" CssClass="tempselect">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="col-xs-12 quantity-check-outer">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 30%">Quantity
                                                </td>
                                                <td style="width: 70%" class="inline-checkboxes">
                                                    <div class="checkbox inline-block">
                                                        <label>
                                                            <asp:CheckBox runat="server" ID="chkIsQuantity" CssClass=" quantity-check" name="checkbox" /><span
                                                                class="radio-styled"></span>
                                                        </label>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 30%">Quantity Required
                                                </td>
                                                <td style="width: 70%" class="inline-checkboxes">
                                                    <div class="checkbox inline-block">
                                                        <label>
                                                            <asp:CheckBox runat="server" ID="chkIsQuantityRequired" name="required-check" /><span
                                                                class="radio-styled"></span>
                                                        </label>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 30%">Quantity Increment
                                                </td>
                                                <td style="width: 70%" class="inline-checkboxes">
                                                    <div class="checkbox inline-block">
                                                        <label>
                                                            <asp:TextBox runat="server" ID="txtQuantityIncrement" type="number" CssClass="quantity-check-input"
                                                                placeholder="1" min="1" />
                                                        </label>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="col-xs-12">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 30%">Tags
                                                    <asp:TextBox ID="txtHidden" CssClass='hdnTags<%# Container.ItemIndex %> hdntagsInput' runat="server"></asp:TextBox>
                                                </td>
                                                <td style="width: 70%">
                                                    <div data-tags-input-name="tag" id='tagBox<%# Container.ItemIndex %>' class="tagBox"></div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="col-xs-12">
                                        <table style="width: 100%" id="tblFilters" class="add-items-filters" runat="server">
                                            <tr>
                                                <td style="width: 100%" colspan="4">Filters
                                                </td>
                                            </tr>
                                            <tr id="trFilter1" runat="server" style="display: block;">
                                                <td style="width: 30%">
                                                    <input runat="server" id="txtSectionName1" type="text" class="form-control col-md-7 col-xs-12" autocomplete="off"
                                                        placeholder="Section Name" />
                                                </td>
                                                <td style="width: 30%">
                                                    <select id="drpTags1" class="form-control" style="width: 100%;" runat="server" onmousedown='<%# "LoadTagDropdown(this, " + Container.ItemIndex + ")" %>' onchange='$(this).next().val(this.value);'>
                                                    </select><asp:HiddenField ID="hdnSelectedTag1" Value="" runat="server" />
                                                </td>
                                                <td style="width: 30%">
                                                    <select id="drpFilterTypes1" class="form-control" style="width: 100%;" runat="server">
                                                        <option value="Contains Disable" selected="selected">Contains Disable</option>
                                                        <option value="Contains Hidden">Contains Hidden</option>
                                                        <option value="Contains Visible">Contains Visible</option>
                                                    </select>
                                                </td>
                                                <td style="width: 10%">
                                                     <a href="#" onclick='minusFilterRow(this);'>
                                                        <img alt="edit" src="../images/minus.png" width="18px"></a>
                                                    <asp:TextBox ID="txtFilterRowNo1" CssClass="hdnRowFilterNo" runat="server" Text="visible"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trFilter2" runat="server" style="display: block;">
                                                <td style="width: 30%">
                                                    <input runat="server" id="txtSectionName2" type="text" class="form-control col-md-7 col-xs-12" autocomplete="off"
                                                        placeholder="Section Name" />
                                                </td>
                                                <td style="width: 30%">
                                                    <select id="drpTags2" style="width: 100%;" class="form-control" runat="server" onmousedown='<%# "LoadTagDropdown(this, " + Container.ItemIndex + ")" %>' onchange='$(this).next().val(this.value);'>
                                                    </select><asp:HiddenField ID="hdnSelectedTag2" Value="" runat="server" />
                                                </td>
                                                <td style="width: 30%">
                                                    <select id="drpFilterTypes2" class="form-control" style="width: 100%;" runat="server">
                                                        <option value="Contains Disable" selected="selected">Contains Disable</option>
                                                        <option value="Contains Hidden">Contains Hidden</option>
                                                        <option value="Contains Visible">Contains Visible</option>
                                                    </select>
                                                </td>
                                                <td style="width: 10%">
                                                    <a href="#" onclick='minusFilterRow(this);'>
                                                        <img alt="edit" src="../images/minus.png" width="18px"></a>
                                                    <asp:TextBox ID="txtFilterRowNo2" CssClass="hdnRowFilterNo" runat="server" Text="visible"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trFilter3" runat="server" style="display: block;">
                                                
                                                <td style="width: 30%">
                                                    <input runat="server" id="txtSectionName3" type="text" class="form-control col-md-7 col-xs-12" autocomplete="off"
                                                        placeholder="Section Name" />
                                                </td>
                                                <td style="width: 30%">
                                                    <select id="drpTags3" style="width: 100%;" class="form-control" runat="server" onmousedown='<%# "LoadTagDropdown(this, " + Container.ItemIndex + ")" %>' onchange='$(this).next().val(this.value);'>
                                                    </select><asp:HiddenField ID="hdnSelectedTag3" Value="" runat="server" />
                                                </td>
                                                <td style="width: 30%">
                                                    <select id="drpFilterTypes3" class="form-control" style="width: 100%;" runat="server">
                                                        <option value="Contains Disable" selected="selected">Contains Disable</option>
                                                        <option value="Contains Hidden">Contains Hidden</option>
                                                        <option value="Contains Visible">Contains Visible</option>
                                                    </select>
                                                </td>
                                                <td style="width: 10%">
                                                    <a href="#" onclick='minusFilterRow(this);'>
                                                        <img alt="edit" src="../images/minus.png" width="18px"></a>
                                                    <asp:TextBox ID="txtFilterRowNo3" CssClass="hdnRowFilterNo" runat="server" Text="visible"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trFilter4" runat="server" style="display: none;">
                                                <td style="width: 30%">
                                                    <input runat="server" id="txtSectionName4" type="text" class="form-control col-md-7 col-xs-12" autocomplete="off"
                                                        placeholder="Section Name" />
                                                </td>
                                                <td style="width: 30%">
                                                    <select id="drpTags4" style="width: 100%;" class="form-control" runat="server" onmousedown='<%# "LoadTagDropdown(this, " + Container.ItemIndex + ")" %>' onchange='$(this).next().val(this.value);'>
                                                    </select><asp:HiddenField ID="hdnSelectedTag4" Value="" runat="server" />
                                                </td>
                                                <td style="width: 30%">
                                                    <select id="drpFilterTypes4" class="form-control" style="width: 100%;" runat="server">
                                                        <option value="Contains Disable" selected="selected">Contains Disable</option>
                                                        <option value="Contains Hidden">Contains Hidden</option>
                                                        <option value="Contains Visible">Contains Visible</option>
                                                    </select>
                                                </td>
                                                <td style="width: 10%">
                                                    <a href="#" onclick='minusFilterRow(this);'>
                                                        <img alt="edit" src="../images/minus.png" width="18px"></a>
                                                    <asp:TextBox ID="txtFilterRowNo4" CssClass="hdnRowFilterNo" runat="server" Text="hidden"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trFilter5" runat="server" style="display: none;">
                                                <td style="width: 30%">
                                                    <input runat="server" id="txtSectionName5" type="text" class="form-control col-md-7 col-xs-12" autocomplete="off"
                                                        placeholder="Section Name" />
                                                </td>
                                                <td style="width: 30%">
                                                    <select id="drpTags5" class="form-control" style="width: 100%;" runat="server" onmousedown='<%# "LoadTagDropdown(this, " + Container.ItemIndex + ")" %>' onchange='$(this).next().val(this.value);'>
                                                    </select><asp:HiddenField ID="hdnSelectedTag5" Value="" runat="server" />
                                                </td>
                                                <td style="width: 30%">
                                                    <select id="drpFilterTypes5" class="form-control" style="width: 100%;" runat="server">
                                                        <option value="Contains Disable" selected="selected">Contains Disable</option>
                                                        <option value="Contains Hidden">Contains Hidden</option>
                                                        <option value="Contains Visible">Contains Visible</option>
                                                    </select>
                                                </td>
                                                <td style="width: 10%">
                                                    <a href="#" onclick='minusFilterRow(this);'>
                                                        <img alt="edit" src="../images/minus.png" width="18px"></a>
                                                    <asp:TextBox ID="txtFilterRowNo5" CssClass="hdnRowFilterNo" runat="server" Text="hidden"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trFilter6" runat="server" style="display: none;">
                                                <td style="width: 30%">
                                                    <input runat="server" id="txtSectionName6" type="text" class="form-control col-md-7 col-xs-12" autocomplete="off"
                                                        placeholder="Section Name" />
                                                </td>
                                                <td style="width: 30%">
                                                    <select id="drpTags6" class="form-control" style="width: 100%;" runat="server" onmousedown='<%# "LoadTagDropdown(this, " + Container.ItemIndex + ")" %>' onchange='$(this).next().val(this.value);'>
                                                    </select><asp:HiddenField ID="hdnSelectedTag6" Value="" runat="server" />
                                                </td>
                                                <td style="width: 30%">
                                                    <select id="drpFilterTypes6" class="form-control" style="width: 100%;" runat="server">
                                                        <option value="Contains Disable" selected="selected">Contains Disable</option>
                                                        <option value="Contains Hidden">Contains Hidden</option>
                                                        <option value="Contains Visible">Contains Visible</option>
                                                    </select>
                                                </td>
                                                <td style="width: 10%">
                                                    <a href="#" onclick='minusFilterRow(this);'>
                                                        <img alt="edit" src="../images/minus.png" width="18px"></a>
                                                    <asp:TextBox ID="txtFilterRowNo6" CssClass="hdnRowFilterNo" runat="server" Text="hidden"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trFilter7" runat="server" style="display: none;">
                                                <td style="width: 30%">
                                                    <input runat="server" id="txtSectionName7" type="text" class="form-control col-md-7 col-xs-12" autocomplete="off"
                                                        placeholder="Section Name" />
                                                </td>
                                                <td style="width: 30%">
                                                    <select id="drpTags7" class="form-control" style="width: 100%;" runat="server" onmousedown='<%# "LoadTagDropdown(this, " + Container.ItemIndex + ")" %>' onchange='$(this).next().val(this.value);'>
                                                    </select><asp:HiddenField ID="hdnSelectedTag7" Value="" runat="server" />
                                                </td>
                                                <td style="width: 30%">
                                                    <select id="drpFilterTypes7" class="form-control" style="width: 100%;" runat="server">
                                                        <option value="Contains Disable" selected="selected">Contains Disable</option>
                                                        <option value="Contains Hidden">Contains Hidden</option>
                                                        <option value="Contains Visible">Contains Visible</option>
                                                    </select>
                                                </td>
                                                <td style="width: 10%">
                                                    <a href="#" onclick='minusFilterRow(this);'>
                                                        <img alt="edit" src="../images/minus.png" width="18px"></a>
                                                    <asp:TextBox ID="txtFilterRowNo7" CssClass="hdnRowFilterNo" runat="server" Text="hidden"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trFilter8" runat="server" style="display: none;">
                                                <td style="width: 30%">
                                                    <input runat="server" id="txtSectionName8" type="text" class="form-control col-md-7 col-xs-12" autocomplete="off"
                                                        placeholder="Section Name" />
                                                </td>
                                                <td style="width: 30%">
                                                    <select id="drpTags8" class="form-control" style="width: 100%;" runat="server" onmousedown='<%# "LoadTagDropdown(this, " + Container.ItemIndex + ")" %>' onchange='$(this).next().val(this.value);'>
                                                    </select><asp:HiddenField ID="hdnSelectedTag8" Value="" runat="server" />
                                                </td>
                                                <td style="width: 30%">
                                                    <select id="drpFilterTypes8" class="form-control" style="width: 100%;" runat="server">
                                                        <option value="Contains Disable" selected="selected">Contains Disable</option>
                                                        <option value="Contains Hidden">Contains Hidden</option>
                                                        <option value="Contains Visible">Contains Visible</option>
                                                    </select>
                                                </td>
                                                <td style="width: 10%">
                                                    <a href="#" onclick='minusFilterRow(this);'>
                                                        <img alt="edit" src="../images/minus.png" width="18px"></a>
                                                    <asp:TextBox ID="txtFilterRowNo8" CssClass="hdnRowFilterNo" runat="server" Text="hidden"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trFilter9" runat="server" style="display: none;">
                                                <td style="width: 30%">
                                                    <input runat="server" id="txtSectionName9" type="text" class="form-control col-md-7 col-xs-12" autocomplete="off"
                                                        placeholder="Section Name" />
                                                </td>
                                                <td style="width: 30%">
                                                    <select id="drpTags9" class="form-control" style="width: 100%;" runat="server" onmousedown='<%# "LoadTagDropdown(this, " + Container.ItemIndex + ")" %>' onchange='$(this).next().val(this.value);'>
                                                    </select><asp:HiddenField ID="hdnSelectedTag9" Value="" runat="server" />
                                                </td>
                                                <td style="width: 30%">
                                                    <select id="drpFilterTypes9" class="form-control" style="width: 100%;" runat="server">
                                                        <option value="Contains Disable" selected="selected">Contains Disable</option>
                                                        <option value="Contains Hidden">Contains Hidden</option>
                                                        <option value="Contains Visible">Contains Visible</option>
                                                    </select>
                                                </td>
                                                <td style="width: 10%">
                                                    <a href="#" onclick='minusFilterRow(this);'>
                                                        <img alt="edit" src="../images/minus.png" width="18px"></a>
                                                    <asp:TextBox ID="txtFilterRowNo9" CssClass="hdnRowFilterNo" runat="server" Text="hidden"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trFilter10" runat="server" style="display: none;">
                                                <td style="width: 30%">
                                                    <input runat="server" id="txtSectionName10" type="text" class="form-control col-md-7 col-xs-12" autocomplete="off"
                                                        placeholder="Section Name" />
                                                </td>
                                                <td style="width: 30%">
                                                    <select id="drpTags10" class="form-control" style="width: 100%;" runat="server" onmousedown='<%# "LoadTagDropdown(this, " + Container.ItemIndex + ")" %>' onchange='$(this).next().val(this.value);'>
                                                    </select><asp:HiddenField ID="hdnSelectedTag10" Value="" runat="server" />
                                                </td>
                                                <td style="width: 30%">
                                                    <select id="drpFilterTypes10" class="form-control" style="width: 100%;" runat="server">
                                                        <option value="Contains Disable" selected="selected">Contains Disable</option>
                                                        <option value="Contains Hidden">Contains Hidden</option>
                                                        <option value="Contains Visible">Contains Visible</option>
                                                    </select>
                                                </td>
                                                <td style="width: 10%">
                                                    <a href="#" onclick='minusFilterRow(this);'>
                                                        <img alt="edit" src="../images/minus.png" width="18px"></a>
                                                    <asp:TextBox ID="txtFilterRowNo10" CssClass="hdnRowFilterNo" runat="server" Text="hidden"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100%" align="right" colspan="4">
                                                    <button class="btn btn-classic" type="button" onclick='addFilterRow(this);'>Add Filter</button>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </asp:Panel>
                <div class="row" id="resultCustomCloned">
                </div>
                <div class="clearfix">
                </div>
                <div class="row add-new-item-btn text-center">
                    <asp:Button runat="server" ID="btnAddNewItem" CssClass="btn btn-scheme" UseSubmitBehavior="false"
                        data-dismiss="modal" Text="Add Item" OnClick="btnAddNewItem_Click"></asp:Button>
                    <%--<i class="fa fa-plus-circle" aria-hidden="true"></i>Add More--%>
                    <%--<button id="addNewCustomItem" class="btn btn-classic">
                        Add Custom Field</button>--%>
                </div>
            </div>
            <div class="modalc-close">
                <button class="btn btn-classic" onclick="modalCustomClose('BTOModal')">
                    Close</button>
                <asp:Button ID="btnSaveItem" runat="server" Text="Save" OnClick="btnSaveItem_Click"
                    UseSubmitBehavior="false" data-dismiss="modal" CssClass="btn btn-scheme" OnClientClick='SaveTags();' />
                <%--<button class="btn btn-scheme">
                    Save
                </button>--%>
            </div>
        </div>
    </div>
    <div class="modal-custom" id="BTOModalEdit">
        <div class="modalc-inner single-item-container-popup">
            <div class="modal-header">
                <button type="button" class="close" onclick="modalCustomClose('BTOModalEdit')">
                    <span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="H1">Add Item</h4>
            </div>
            <div class="modalc-body">
                <asp:Panel runat="server" ID="Panel1" CssClass="row bto-items">
                    <div class="col-sm-12">
                        <div class="bto-item">
                            <div class="col-xs-12">
                                <div class="checkbox">
                                    <label>
                                        <asp:CheckBox ID="chkEditIsDefault" runat="server" class="" Checked="true" name="defaultItem" /><span
                                            class="radio-styled"></span> Default<asp:HiddenField ID="hdnEditSectionItemID" Value="0"
                                                runat="server" />
                                    </label>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                        <table style="width: 100%; table-layout: fixed;">
                                            <tr>
                                                <td style="width: 30%">Item
                                                </td>
                                                <td style="width: 70%">
                                                    <asp:DropDownList ID="drpEditBTOItems" Style="width: 100%;" runat="server" CssClass="tempselect" onchange="BTOItemDDLOnChange(this);">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="col-xs-12 cntnrWholesale">
                                        <table style="width: 100%; table-layout: fixed;">
                                            <tr>
                                                <td style="width: 30%">Wholesale
                                                </td>
                                                <td style="width: 70%">
                                                    <asp:TextBox ID="BTOItemSelectedWholesaleID" runat="server" Text="" CssClass="BTOItemSelectedWholesaleID"></asp:TextBox>
                                                    <asp:DropDownList ID="drpEditWholesales" Style="width: 100%;" runat="server" CssClass="tempselect" onchange="ApplyWholesale(this);">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="col-xs-12 cntnrBTOItemPrice">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 30%">Price
                                                </td>
                                                <td style="width: 70%" class="inline-checkboxes">
                                                    <div>
                                                        <asp:Label CssClass="form-control" runat="server" ID="txtEditPrice" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="col-xs-12 cntnrBTOItemOptions">
                                        <table style="width: 100%; table-layout: fixed;">
                                            <tr>
                                                <td style="width: 30%">
                                                    <asp:Label ID="ltrlBTOItemOptionTitleEdit" class="ltrlBTOItemOptionTitle" runat="server" Text="Option Title"></asp:Label>
                                                </td>
                                                <td style="width: 70%">
                                                    <asp:TextBox ID="txtBTOItemOptionsSelectedOptionsEdit" runat="server" class="BTOItemOptionsSelectedtxt" Text=""></asp:TextBox>
                                                    <asp:ListBox ID="lstbxBTOItemOptionsEdit" Style="width: 100%;" CssClass="OptionsSelect" SelectionMode="Multiple" runat="server" onchange="SaveSelectedValuesAsString(this);">
                                                    </asp:ListBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                            <div class="col-xs-12">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 30%">Display Mode
                                        </td>
                                        <td style="width: 70%" class="inline-checkboxes">
                                            <div class="checkbox">
                                                <label>
                                                    <asp:RadioButton ID="rbtnEditDisplayModeRadio" GroupName="DisplayMode1" Checked="true"
                                                        runat="server" /><span class="radio-styled"></span> Radio
                                                </label>
                                            </div>
                                            <div class="checkbox">
                                                <label>
                                                    <asp:RadioButton ID="rbtnEditDisplayModeCheckbox" GroupName="DisplayMode1" runat="server" /><span
                                                        class="radio-styled"></span> Checkbox
                                                </label>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="col-xs-12">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 30%">Processing Time
                                        </td>
                                        <td style="width: 70%" class="inline-checkboxes">
                                            <div>
                                                <asp:TextBox type="number" placeholder="1" CssClass="form-control" min="0" runat="server"
                                                    ID="txtEditProcessingTime" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="col-xs-12">
                                <table style="width: 100%; table-layout: fixed;">
                                    <tr>
                                        <td style="width: 30%">Recommendation
                                        </td>
                                        <td style="width: 70%" class="inline-checkboxes">
                                            <asp:DropDownList ID="drpEditRecommendations" Style="width: 100%;" runat="server" CssClass="tempselect">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="col-xs-12 quantity-check-outer">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 30%">Quantity
                                        </td>
                                        <td style="width: 70%" class="inline-checkboxes">
                                            <div class="checkbox inline-block">
                                                <label>
                                                    <asp:CheckBox runat="server" ID="chkEditIsQuantity" CssClass=" quantity-check" name="checkbox" /><span
                                                        class="radio-styled"></span>
                                                </label>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%">Quantity Required
                                        </td>
                                        <td style="width: 70%" class="inline-checkboxes">
                                            <div class="checkbox inline-block">
                                                <label>
                                                    <asp:CheckBox runat="server" ID="chkEditIsQuantityRequired" name="required-check" /><span
                                                        class="radio-styled"></span>
                                                </label>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 30%">Quantity Increment
                                        </td>
                                        <td style="width: 70%" class="inline-checkboxes">
                                            <div class="checkbox inline-block">
                                                <label>
                                                    <asp:TextBox runat="server" ID="txtEditQuantityIncrement" type="number" CssClass="quantity-check-input"
                                                        placeholder="1" min="1" />
                                                </label>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="col-xs-12">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 30%">Tags
                                        </td>
                                        <td style="width: 70%">
                                            <div data-tags-input-name="tag" class="tagBox" id="tagBoxBTOEdit"></div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="col-xs-12">
                                <table style="width: 100%" id="tblEditFilters" class="add-items-filters" runat="server">
                                    <tr>
                                        <td style="width: 100%" colspan="4">Filters
                                        </td>
                                    </tr>
                                    <tr id="trFilter1" runat="server" style="display:block;">
                                        <td style="width: 30%">
                                             <asp:HiddenField ID="hdnItemFilterID1" Value="" runat="server" />
                                            <input runat="server" id="txtSectionName1" type="text" class="form-control col-md-7 col-xs-12" autocomplete="off"
                                                placeholder="Section Name" />
                                        </td>
                                        <td style="width: 30%">
                                            <select id="drpTags1" style="width: 100%;" class="form-control" runat="server" onmousedown='LoadTagDropdownEdit(this)' onchange='$(this).next().val(this.value);'>
                                            </select><asp:HiddenField ID="hdnSelectedTag1" Value="" runat="server" />
                                        </td>
                                        <td style="width: 30%">
                                            <select id="drpFilterTypes1" style="width: 100%;" runat="server" class="form-control">
                                                <option value="Contains Disable" selected="selected">Contains Disable</option>
                                                <option value="Contains Hidden">Contains Hidden</option>
                                                <option value="Contains Visible">Contains Visible</option>
                                            </select>
                                        </td>
                                        <td style="width: 10%">
                                             <a href="#" onclick='minusFilterRow(this)'>
                                                <img alt="edit" src="../images/minus.png" width="18px"></a>
                                             <asp:TextBox ID="txtFilterRowNo1" CssClass="hdnRowFilterNo" runat="server" Text="visible"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trFilter2" runat="server" style="display:block;">
                                        <td style="width: 30%">
                                            <asp:HiddenField ID="hdnItemFilterID2" Value="" runat="server" />
                                            <input runat="server" id="txtSectionName2" type="text" class="form-control col-md-7 col-xs-12" autocomplete="off"
                                                placeholder="Section Name" />
                                        </td>
                                        <td style="width: 30%">
                                            <select id="drpTags2" style="width: 100%;" class="form-control" runat="server" onmousedown='LoadTagDropdownEdit(this)' onchange='$(this).next().val(this.value);'>
                                            </select><asp:HiddenField ID="hdnSelectedTag2" Value="" runat="server" />
                                        </td>
                                        <td style="width: 30%">
                                            <select id="drpFilterTypes2" style="width: 100%;" class="form-control" runat="server">
                                                <option value="Contains Disable" selected="selected">Contains Disable</option>
                                                <option value="Contains Hidden">Contains Hidden</option>
                                                <option value="Contains Visible">Contains Visible</option>
                                            </select>
                                        </td>
                                        <td style="width: 10%">
                                            <a href="#" onclick='minusFilterRow(this)'>
                                                <img alt="edit" src="../images/minus.png" width="18px"></a>
                                             <asp:TextBox ID="txtFilterRowNo2" CssClass="hdnRowFilterNo" runat="server" Text="visible"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trFilter3" runat="server" style="display:block;">
                                        <td style="width: 30%">
                                             <asp:HiddenField ID="hdnItemFilterID3" Value="" runat="server" />
                                            <input runat="server" id="txtSectionName3" type="text" class="form-control col-md-7 col-xs-12" autocomplete="off"
                                                placeholder="Section Name" />
                                        </td>
                                        <td style="width: 30%">
                                            <select id="drpTags3" style="width: 100%;" class="form-control" runat="server" onmousedown='LoadTagDropdownEdit(this)' onchange='$(this).next().val(this.value);'>
                                            </select><asp:HiddenField ID="hdnSelectedTag3" Value="" runat="server" />
                                        </td>
                                        <td style="width: 30%">
                                            <select id="drpFilterTypes3" class="form-control" style="width: 100%;" runat="server">
                                                <option value="Contains Disable" selected="selected">Contains Disable</option>
                                                <option value="Contains Hidden">Contains Hidden</option>
                                                <option value="Contains Visible">Contains Visible</option>
                                            </select>
                                        </td>
                                        <td style="width: 10%">
                                            <a href="#" onclick='minusFilterRow(this);'>
                                                <img alt="edit" src="../images/minus.png" width="18px"></a>
                                            <asp:TextBox ID="txtFilterRowNo3" CssClass="hdnRowFilterNo" runat="server" Text="visible"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trFilter4" runat="server" style="display:none;">
                                        <td style="width: 30%">
                                            <asp:HiddenField ID="hdnItemFilterID4" Value="" runat="server" />
                                            <input runat="server" id="txtSectionName4" type="text" class="form-control col-md-7 col-xs-12" autocomplete="off"
                                                placeholder="Section Name" />
                                        </td>
                                        <td style="width: 30%">
                                            <select id="drpTags4" style="width: 100%;" class="form-control" runat="server" onmousedown='LoadTagDropdownEdit(this)' onchange='$(this).next().val(this.value);'>
                                            </select><asp:HiddenField ID="hdnSelectedTag4" Value="" runat="server" />
                                        </td>
                                        <td style="width: 30%">
                                            <select id="drpFilterTypes4" class="form-control" style="width: 100%;" runat="server">
                                                <option value="Contains Disable" selected="selected">Contains Disable</option>
                                                <option value="Contains Hidden">Contains Hidden</option>
                                                <option value="Contains Visible">Contains Visible</option>
                                            </select>
                                        </td>
                                        <td style="width: 10%">
                                            <a href="#" onclick='minusFilterRow(this);'>
                                                <img alt="edit" src="../images/minus.png" width="18px"></a>
                                            <asp:TextBox ID="txtFilterRowNo4" CssClass="hdnRowFilterNo" runat="server" Text="hidden"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trFilter5" runat="server" style="display:none;">
                                        <td style="width: 30%">
                                            <asp:HiddenField ID="hdnItemFilterID5" Value="" runat="server" />
                                            <input runat="server" id="txtSectionName5" type="text" class="form-control col-md-7 col-xs-12" autocomplete="off"
                                                placeholder="Section Name" />
                                        </td>
                                        <td style="width: 30%">
                                            <select id="drpTags5" style="width: 100%;" class="form-control" runat="server" onmousedown='LoadTagDropdownEdit(this)' onchange='$(this).next().val(this.value);'>
                                            </select><asp:HiddenField ID="hdnSelectedTag5" Value="" runat="server" />
                                        </td>
                                        <td style="width: 30%">
                                            <select id="drpFilterTypes5" style="width: 100%;" class="form-control" runat="server">
                                                <option value="Contains Disable" selected="selected">Contains Disable</option>
                                                <option value="Contains Hidden">Contains Hidden</option>
                                                <option value="Contains Visible">Contains Visible</option>
                                            </select>
                                        </td>
                                        <td style="width: 10%">
                                            <a href="#" onclick='minusFilterRow(this);'>
                                                <img alt="edit" src="../images/minus.png" width="18px"></a>
                                             <asp:TextBox ID="txtFilterRowNo5" CssClass="hdnRowFilterNo" runat="server" Text="hidden"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trFilter6" runat="server" style="display:none;">
                                        <td style="width: 30%">
                                            <asp:HiddenField ID="hdnItemFilterID6" Value="" runat="server" />
                                            <input runat="server" id="txtSectionName6" type="text" class="form-control col-md-7 col-xs-12" autocomplete="off"
                                                placeholder="Section Name" />
                                        </td>
                                        <td style="width: 30%">
                                            <select id="drpTags6" style="width: 100%;" class="form-control" runat="server" onmousedown='LoadTagDropdownEdit(this)' onchange='$(this).next().val(this.value);'>
                                            </select><asp:HiddenField ID="hdnSelectedTag6" Value="" runat="server" />
                                        </td>
                                        <td style="width: 30%">
                                            <select id="drpFilterTypes6" style="width: 100%;" class="form-control" runat="server">
                                                <option value="Contains Disable" selected="selected">Contains Disable</option>
                                                <option value="Contains Hidden">Contains Hidden</option>
                                                <option value="Contains Visible">Contains Visible</option>
                                            </select>
                                        </td>
                                        <td style="width: 10%">
                                            <a href="#" onclick='minusFilterRow(this);'>
                                                <img alt="edit" src="../images/minus.png" width="18px"></a>
                                             <asp:TextBox ID="txtFilterRowNo6" CssClass="hdnRowFilterNo" runat="server" Text="hidden"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trFilter7" runat="server" style="display:none;">
                                        <td style="width: 30%">
                                             <asp:HiddenField ID="hdnItemFilterID7" Value="" runat="server" />
                                            <input runat="server" id="txtSectionName7" type="text" class="form-control col-md-7 col-xs-12" autocomplete="off"
                                                placeholder="Section Name" />
                                        </td>
                                        <td style="width: 30%">
                                            <select id="drpTags7" style="width: 100%;" class="form-control" runat="server" onmousedown='LoadTagDropdownEdit(this)' onchange='$(this).next().val(this.value);'>
                                            </select><asp:HiddenField ID="hdnSelectedTag7" Value="" runat="server" />
                                        </td>
                                        <td style="width: 30%">
                                            <select id="drpFilterTypes7" style="width: 100%;" class="form-control" runat="server">
                                                <option value="Contains Disable" selected="selected">Contains Disable</option>
                                                <option value="Contains Hidden">Contains Hidden</option>
                                                <option value="Contains Visible">Contains Visible</option>
                                            </select>
                                        </td>
                                        <td style="width: 10%">
                                            <a href="#" onclick='minusFilterRow(this);'>
                                                <img alt="edit" src="../images/minus.png" width="18px"></a>
                                             <asp:TextBox ID="txtFilterRowNo7" CssClass="hdnRowFilterNo" runat="server" Text="hidden"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trFilter8" runat="server" style="display:none;">
                                        <td style="width: 30%">
                                            <asp:HiddenField ID="hdnItemFilterID8" Value="" runat="server" />
                                            <input runat="server" id="txtSectionName8" type="text" class="form-control col-md-7 col-xs-12" autocomplete="off"
                                                placeholder="Section Name" />
                                        </td>
                                        <td style="width: 30%">
                                            <select id="drpTags8" style="width: 100%;" class="form-control" runat="server" onmousedown='LoadTagDropdownEdit(this)' onchange='$(this).next().val(this.value);'>
                                            </select><asp:HiddenField ID="hdnSelectedTag8" Value="" runat="server" />
                                        </td>
                                        <td style="width: 30%">
                                            <select id="drpFilterTypes8" style="width: 100%;" class="form-control" runat="server">
                                                <option value="Contains Disable" selected="selected">Contains Disable</option>
                                                <option value="Contains Hidden">Contains Hidden</option>
                                                <option value="Contains Visible">Contains Visible</option>
                                            </select>
                                        </td>
                                        <td style="width: 10%">
                                            <a href="#" onclick='minusFilterRow(this);'>
                                                <img alt="edit" src="../images/minus.png" width="18px"></a>
                                            <asp:TextBox ID="txtFilterRowNo8" CssClass="hdnRowFilterNo" runat="server" Text="hidden"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trFilter9" runat="server" style="display:none;">
                                        <td style="width: 30%">
                                             <asp:HiddenField ID="hdnItemFilterID9" Value="" runat="server" />
                                            <input runat="server" id="txtSectionName9" type="text" class="form-control col-md-7 col-xs-12" autocomplete="off"
                                                placeholder="Section Name" />
                                        </td>
                                        <td style="width: 30%">
                                            <select id="drpTags9" style="width: 100%;" class="form-control" runat="server" onmousedown='LoadTagDropdownEdit(this)' onchange='$(this).next().val(this.value);'>
                                            </select><asp:HiddenField ID="hdnSelectedTag9" Value="" runat="server" />
                                        </td>
                                        <td style="width: 30%">
                                            <select id="drpFilterTypes9" class="form-control" style="width: 100%;" runat="server">
                                                <option value="Contains Disable" selected="selected">Contains Disable</option>
                                                <option value="Contains Hidden">Contains Hidden</option>
                                                <option value="Contains Visible">Contains Visible</option>
                                            </select>
                                        </td>
                                        <td style="width: 10%">
                                            <a href="#" onclick='minusFilterRow(this);'>
                                                <img alt="edit" src="../images/minus.png" width="18px"></a>
                                            <asp:TextBox ID="txtFilterRowNo9" CssClass="hdnRowFilterNo" runat="server" Text="hidden"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trFilter10" runat="server" style="display:none;">
                                        <td style="width: 30%">
                                            <asp:HiddenField ID="hdnItemFilterID10" Value="" runat="server" />
                                            <input runat="server" id="txtSectionName10" type="text" class="form-control col-md-7 col-xs-12" autocomplete="off"
                                                placeholder="Section Name" />
                                        </td>
                                        <td style="width: 30%">
                                            <select id="drpTags10" style="width: 100%;" class="form-control" runat="server" onmousedown='LoadTagDropdownEdit(this)' onchange='$(this).next().val(this.value);'>
                                            </select><asp:HiddenField ID="hdnSelectedTag10" Value="" runat="server" />
                                        </td>
                                        <td style="width: 30%">
                                            <select id="drpFilterTypes10" style="width: 100%;" class="form-control" runat="server">
                                                <option value="Contains Disable" selected="selected">Contains Disable</option>
                                                <option value="Contains Hidden">Contains Hidden</option>
                                                <option value="Contains Visible">Contains Visible</option>
                                            </select>
                                        </td>
                                        <td style="width: 10%">
                                            <a href="#" onclick='minusFilterRow(this);'>
                                                <img alt="edit" src="../images/minus.png" width="18px"></a>
                                              <asp:TextBox ID="txtFilterRowNo10" CssClass="hdnRowFilterNo" runat="server" Text="hidden"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%" align="right" colspan="4">
                                            <button class="btn btn-classic" type="button" onclick='addFilterRow(this);'>Add Filter</button>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <div class="row" id="Div2">
                </div>
                <div class="clearfix">
                </div>
            </div>
            <div class="modalc-close">
                <button class="btn btn-classic" onclick="modalCustomClose('BTOModalEdit')">
                    Close</button>
                <asp:Button ID="btnBTOItemEdit" runat="server" Text="Save" OnClick="btnBTOItemEdit_Click" OnClientClick='GetTags("tagBoxBTOEdit");'
                    UseSubmitBehavior="false" data-dismiss="modal" CssClass="btn btn-scheme" />
                <asp:HiddenField ID="hdnTags" Value="" runat="server" />
            </div>
        </div>
    </div>
    <!-- validator -->
    <script type="text/javascript">
        // initialize the validator function
        validator.message.date = 'not a real date';

        // validate a field on "blur" event, a 'select' on 'change' event & a '.reuired' classed multifield on 'keyup':
        $('form')
        .on('blur', 'input[required], input.optional, select.required', validator.checkField)
        .on('change', 'select.required', validator.checkField)
        .on('keypress', 'input[required][pattern]', validator.keypress);

        $('.multi.required').on('keyup blur', 'input', function () {
            validator.checkField.apply($(this).siblings().last()[0]);
        });

        $('form').submit(function (e) {
            e.preventDefault();
            var submit = true;

            // evaluate the form using generic validaing
            if (!validator.checkAll($(this))) {
                submit = false;
            }

            if (submit)
                this.submit();

            return false;
        });
    </script>
    <!-- /validator -->
    <!-- select 2 defin -->
    <script type="text/javascript">
        // select to call
        $('.tempselect').select2();
        $('.OptionsSelect').select2({
            placeholder: "Select an Option",
            allowClear: false
        });

        // file upload function
        $("#file").on('change', function () {

            if (typeof (FileReader) != "undefined") {

                var image_holder = $("#image-holder");
                image_holder.empty();

                var reader = new FileReader();
                reader.onload = function (e) {
                    $("<img />", {
                        "src": e.target.result,
                        "class": "thumb-image"
                    }).appendTo(image_holder);

                }
                image_holder.show();
                reader.readAsDataURL($(this)[0].files[0]);
            } else {
                alert("This browser does not support FileReader.");
            }
        });

        // onclick save button close modal sub heading
        $(".closemodal").click(function () {
            if ($("#subHeadingText").val() != "") {
                $('#subHeadingModal').modal('hide');
                addText('subHeadingText', 'newHeading');
                $("#subHeadingText").val('');
            }
        });

        //jquery ui spinner
        $(".spinner").spinner({ step: 2, min: 0 });


        $('span[name="defaultItem"] input').filter(function () {
            var defaultBTO = "default-bto-item"; // class will be add
            if ($(this).is(":checked")) {
                $(this).closest('.bto-item').addClass(defaultBTO);
            }

            $(document.body).on('change', 'span[name="defaultItem"] input', function () {
                var defaultBTO = "default-bto-item";
                $('span[name="defaultItem"] input').closest('.bto-item').removeClass(defaultBTO)
                if ($(this).is(':checked')) {
                    $(this).closest('.bto-item').addClass(defaultBTO);
                }
            });
        });

        // on quantity check enable inputs in add section lightbox add bto items
        $(document.body).on('change', ".quantity-check input", function () {
            if (this.checked) {
                $(this).closest("table").addClass("quantity-checked")
            } else {
                $(this).closest("table").removeClass("quantity-checked")
            }
        });

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".tagBox").tagging();

            // Sortable rows
            $('.sortable').sortable({
                containerSelector: 'table',
                itemPath: '> tbody',
                itemSelector: 'tr',
                placeholder: '<tr class="placeholder"/></tr>',
                onDragStart: function () {

                    $(".placeholder").remove();
                },
                stop: function (e, ui) {
                    //alert("New position: " + ui.item.index());
                    //$('td.drag', ui.item).click();

                    var ID_To_Submit = ui.item.attr("id");
                    var New_Position = ui.item.index();
                    var options = {
                        type: "POST",
                        url: "./addsection.aspx/ChangeDisplayOrder",
                        data: JSON.stringify({
                            lIdToReposition: ID_To_Submit,
                            lNewPosition: New_Position
                        }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {

                        }
                    };

                    $.ajax(options);
                }
            });

            $(".checkbox input").change(function () {
                if ($(this).is(":checked")) {
                    $(this).parent().addClass('checkbox-checked');
                } else {
                    $(this).parent().removeClass('checkbox-checked');
                }
            });

            $(window).load(function () {
                $(".checkbox input").filter(function () {
                    if ($(this).is(":checked")) {
                        $(this).parent().addClass('checkbox-checked');
                    } else {
                        $(this).parent().removeClass('checkbox-checked');
                    }
                });
            });
        });

        function SaveTags() {
            var totalItems = $(".col-sm-4").length - 1;
            var alltext = "";

            for (i = 0; i < totalItems; i++) {
                var text = $("#tagBox" + i.toString()).tagging("getTags");
                $("#ContentPlaceHolder1_rptrBTOItems_txtHidden_" + i.toString()).val(text);
                alltext += " -- " + text;
                //alert($(".hdnTags" + i.toString()).val());
            }

            //alert(alltext);
        }

        function GetTags(tagBox) {
            var tags = $("#" + tagBox).tagging("getTags");
            $('#<%= hdnTags.ClientID %>').val(tags);
        }

        function SetTags() {

        }

        //var i = 4;
        //function AddNewRow(btn) {
        //    var tbl = $(btn).closest("table");
        //    $(tbl).append($('<input>').attr('type', 'text').attr('id', 'param' + i).val(i));
        //    var row = $(".tbltoclone tbody").html();
        //    row = row.replace('<input', '<input id="txt' + i + '"');
        //    alert(row);
        //    $(row).insertBefore($(btn).closest("tr"));
        //    i += 1;
        //    return false;
        //}

        function LoadTagDropdown(drp, itemNumber) {
            var text = $("#tagBox" + itemNumber.toString()).tagging("getTags");
            console.log(text);
            console.log($(drp).attr('id'));

            for (i = 0; i < text.length; i++) {
                if ($("#" + $(drp).attr('id') + " option[value='" + text[i] + "']").length <= 0) {
                    $(drp).append($('<option>', {
                        value: text[i],
                        text: text[i]
                    }));
                }
            }
            setValueOfSelctedTagForFilter(drp);
        }

        function LoadTagDropdownEdit(drp) {
            var text = $("#tagBoxBTOEdit").tagging("getTags");
            console.log(text);
            console.log($(drp).attr('id'));

            for (i = 0; i < text.length; i++) {
                if ($("#" + $(drp).attr('id') + " option[value='" + text[i] + "']").length <= 0) {
                    $(drp).append($('<option>', {
                        value: text[i],
                        text: text[i]
                    }));
                }
            }
            setValueOfSelctedTagForFilter(drp);
        }

    </script>
     <script type="text/javascript">
        
         function ApplyWholesale(ddl) {
             try {
                 var btoitmid = $(ddl).closest('div').prev().find('select').val();
                 var wholeText = $(ddl).find('option:selected').text();
                 var wholeID = $(ddl).find('option:selected').val();
                 var discount = wholeText.split(" ")[0].replace("%", "");
                 var txtPr = $(ddl).closest('div').next().find('span');
                 $(ddl).closest('td').find(".BTOItemSelectedWholesaleID").val(wholeID);

                 $.ajax({
                     type: "POST",
                     url: "./addsection.aspx/GetBTOItemPrice",
                     data: JSON.stringify({
                         pBTOItemID: btoitmid
                     }),
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     success: function (r) {
                         //alert(r.d);
                         txtPr.text(r.d);
                         var num = $(txtPr).text() - ((discount / 100) * $(txtPr).text());
                         var price = num.toFixed(2);

                         if (isNaN(price)) {
                             price = "";
                         }
                         else {
                             price = "$" + price;

                             if (price.indexOf('.') < 0) {
                                 price = price + ".00";
                             }
                         }

                         $(txtPr).text(price);
                     }
                 });

             }
             catch (err) {
                 
             }
         }
         
         function SaveSelectedValuesAsString(lstbx) {
             var selectedTexts = $(lstbx).find("option:selected").text();
             //alert(selectedTexts);
             //if (selectedTexts.indexOf("All") >= 0) {
             //    $(lstbx).find('option').prop('selected', true);
             //    var allOption = $(lstbx).find('option').filter(function () { return $(this).html() == "All"; });
             //    //alert(allOption.val());
             //    allOption.prop('selected', false);
             //}
             var hdnSelectedValuesField = $(lstbx).closest(".cntnrBTOItemOptions").find('.BTOItemOptionsSelectedtxt');
             //alert(hdnSelectedValuesField.val());
             var selectedValues = $(lstbx).val();
             hdnSelectedValuesField.val(selectedValues.toString());
             //alert(hdnSelectedValuesField.val());
         }
         function BTOItemDDLOnChange(ddl) {
             var btoitemid = $(ddl).val();

             //var ddlwhole = $(ddl).closest('div').next().find('select');
             var ddlwhole = $(ddl).closest('.bto-item').find('.cntnrWholesale').find('select');

             //var ddloptions = $(ddl).closest('div').next().next().next().find('select');
             var ddloptions = $(ddl).closest('.bto-item').find('.cntnrBTOItemOptions').find('select');
             var hdnSelectedValuesField = $(ddl).closest(".bto-item").find('.BTOItemOptionsSelectedtxt');
             hdnSelectedValuesField.val();

             
             //var ddloptions = $(ddl).closest('div').next().next().next().find('select');
             var txtBTOItemOptionTitle = $(ddl).closest('.bto-item').find('.cntnrBTOItemOptions').find('.ltrlBTOItemOptionTitle');

             var txtPr = $(ddl).closest('div').next().next().find('span');
             //alert($(ddlwhole).html());

             $.ajax({
                 type: "POST",
                 url: "./addsection.aspx/GetWholesales",
                 data: JSON.stringify({
                     pBTOItemID: btoitemid
                 }),
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (r) {
                     //alert("success");
                     //var ddlCustomers = $("[id*=ddlCustomers]");
                     ddlwhole.empty();
                     var flag = true;

                     $.each(r.d, function () {
                         if (flag) {
                             flag = false;
                             ddlwhole.append('<option selected="selected" value="0">Please select wholesale</option>');
                         }

                         ddlwhole.append($("<option></option>").val(this['Value']).html(this['Text']));
                     });

                     if (flag) {
                         ddlwhole.append('<option selected="selected" value="0">No wholesale found</option>');
                     }
                 }
             });

             $.ajax({
                 type: "POST",
                 url: "./addsection.aspx/GetBTOItemPrice",
                 data: JSON.stringify({
                     pBTOItemID: btoitemid
                 }),
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (r) {
                     //alert(r.d);
                     txtPr.text("$" + r.d);
                 }
             });
             $.ajax({
                 type: "POST",
                 url: "./addsection.aspx/GetBTOItemOptions",
                 data: JSON.stringify({
                     pBTOItemID: btoitemid
                 }),
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (r) {
                     //alert("success");
                     //var ddlCustomers = $("[id*=ddlCustomers]");
                     ddloptions.empty();
                     var flag = true;

                     if (!jQuery.isEmptyObject(r.d.OptionsTitle)) {
                         txtBTOItemOptionTitle.html(r.d.OptionsTitle);
                     }

                     $.each(r.d.Options, function () {
                         if (flag) {
                             flag = false;
                             ddloptions.append($("<option selected='selected'></option>").val(this['Value']).html(this['Text']));
                             hdnSelectedValuesField.val(this['Value']);
                         } else {
                             ddloptions.append($("<option></option>").val(this['Value']).html(this['Text']));
                         }
                     });

                     if (flag) {
                         ddloptions.append('<option value="" disabled>No options found</option>');
                     }
                 }
             });
         }

         function setValueOfSelctedTagForFilter(ddlforFilter) {
             var hdnInput = $(ddlforFilter).next();
             hdnInput.val($(ddlforFilter).val());
             //alert(hdnInput.val());
         }

         function minusFilterRow(btn) {
             $(btn).closest("tr").hide();
             var hdnRowFilterNo = $(btn).closest("tr").find(".hdnRowFilterNo");
             $(btn).closest("tr").find(".hdnRowFilterNo").val("hidden");
         }

         function addFilterRow(btn) {
             var tRow = $(btn).closest(".add-items-filters").find("tbody").find("tr:hidden:first");
             tRow.css("display", "block");
             var hdnRowFilterNo = tRow.find(".hdnRowFilterNo");
             hdnRowFilterNo.val("visible");
         }

</script>
</asp:Content>
