<%@ Page Title="" Language="C#" MasterPageFile="~/widget/Admin.Master" AutoEventWireup="true"
    CodeBehind="Index.aspx.cs" Inherits="SCE.MainGear.admin.Index" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<script type="text/javascript" src="../tinymce/jscripts/tiny_mce/tiny_mce.js"></script>--%>
    <script type="text/javascript" src='//cdn.tinymce.com/4/tinymce.min.js'></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right_col" role="main">
        <div class="">
            <div class="page-title">
                <div class="title_left">
                    <h3>
                        <asp:HiddenField runat="server" ID="hdnMode" Value="add" />
                    </h3>
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
                                    <li class="active">Product List<i class="fa fa-chevron-right" aria-hidden="true"></i></li>
                                </ol>
                            </div>
                            <h2>Products <small>(manage all products)</small></h2>
                            <ul class="nav navbar-right panel_toolbox" runat="server" id="ulButtons">
                                <li>
                                    <asp:LinkButton runat="server" ID="lbtnRefresh" CssClass="btn btn-classic" OnClick="lbtnRefresh_Click"><i class="fa fa-refresh" aria-hidden="true">
                                </i>Refresh Items </asp:LinkButton></li>
                                <li>
                                    <asp:LinkButton ID="lbtnAddProduct" CssClass="btn btn-classic" runat="server" OnClick="lbtnAddProduct_Click"><i
                                    class="fa fa-plus-circle" aria-hidden="true"></i>Add Product </asp:LinkButton></li>
                            </ul>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <div class="table-responsive">
                                <table class="table table-striped jambo_table bulk_action">
                                    <thead>
                                        <tr class="headings">
                                            <th class="column-title">Product Title
                                            </th>
                                            <th class="column-title no-link last">
                                                <span class="nobr">Action</span>
                                            </th>
                                            <th class="bulk-actions">
                                                <a class="antoo" style="color: #fff; font-weight: 500;">Bulk Actions ( <span class="action-cnt"></span>) <i class="fa fa-chevron-down"></i></a>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater runat="server" ID="rptrProducts" OnItemCommand="rptrProducts_ItemCommand" OnItemDataBound="rptrProducts_ItemDataBound">
                                            <ItemTemplate>
                                                <tr class="even pointer">
                                                    <td class=" ">
                                                        <%# Eval("Title") %>
                                                    </td>
                                                    <td class=" ">
                                                        <asp:LinkButton runat="server" ID="lbtnAddProduct" CommandName="edit" CommandArgument='<%# Eval("ProductID") %>'
                                                            class="btn btn-sm btn-classic">
                                                    Edit</asp:LinkButton>
                                                        <asp:LinkButton runat="server" ID="lbtnAddSection" CommandName="section" CommandArgument='<%# Eval("ProductID") %>'
                                                            class="btn btn-sm btn-classic">Sections</asp:LinkButton>
                                                        <asp:LinkButton runat="server" ID="lbtnInstructions" CommandName="instruction" CommandArgument='<%# Eval("ProductID") %>'
                                                            class="btn btn-sm btn-classic">Instructions</asp:LinkButton>
                                                        <asp:LinkButton runat="server" ID="lbtnDelProduct" CommandName="del" CommandArgument='<%# Eval("ProductID") %>'
                                                            CssClass="btn btn-sm btn-classic" OnClientClick="return confirm('Are you certain you want to delete this product?');">Delete</asp:LinkButton>
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
    <!-- /page content -->
    <!-- Modal for add product -->
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document">
            <%--<form class="form-horizontal form-label-left" action="steps.html" novalidate>--%>
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel">Add Product</h4>
                </div>
                <div class="modal-body">
                    <div class="item form-group">
                        <label class="control-label col-xs-2" for="name">
                            Title <span class="required"></span>
                        </label>
                        <div class="col-xs-8">
                            <asp:TextBox runat="server" ID="txtTitle" CssClass="form-control col-md-7 col-xs-12"
                                data-validate-length-range="10" name="txtTitle" placeholder="Product Title"
                                required="required" />
                            <asp:HiddenField runat="server" ID="hdnProductID" Value="" />
                        </div>
                    </div>
                    <asp:Panel runat="server" ID="pnlSteps" CssClass="stepspopup">
                        <%--<asp:HiddenField runat="server" ID="hdnTotalSteps" Value="4" />--%><div class="clearfix"></div>
                        <asp:Repeater runat="server" ID="rptrSteps" OnItemCommand="rptrSteps_ItemCommand">
                            <HeaderTemplate>
                                <%--<div class="item form-group">
                                    <label class="control-label col-xs-2" for="name">
                                        Step 1 <span class="required"></span>
                                    </label>
                                    <div class="col-xs-8">
                                        <asp:TextBox ID="txtStepHeader" runat="server" CssClass="form-control col-md-7 col-xs-12"
                                            data-validate-length-range="10" name="txtStepHeader" placeholder="Step 1"
                                            required="required"></asp:TextBox>
                                        <asp:HiddenField runat="server" ID="hdnStepHeader" Value='<%#Eval("ProductStepID") %>' />
                                    </div>
                                    <div class="col-xs-2">
                                        <a href="#" onclick='$(this).parent().parent().hide();'>
                                            <img alt="edit" src="../images/minus.png" width="18px"></a>
                                    </div>
                                </div>
                                <div class="clearfix"></div>--%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="item form-group" style='display: <%# DataBinder.Eval(Container.DataItem, "Display") %>;'>
                                    <label class="control-label col-xs-2" for="name">
                                        Step <%#Eval("StepNumber") %> <asp:Label runat="server" Visible='<%#Eval("IsRemovable") %>'><span class="required"></span></asp:Label>
                                    </label>
                                    <div class="col-xs-8">
                                        <asp:TextBox ID="txtStep" runat="server" CssClass="form-control col-md-7 col-xs-12"
                                            data-validate-length-range="10" name="txtStep1" placeholder='<%#Eval("StepNumberDisplay") %>'
                                            Text='<%#Eval("Title") %>'></asp:TextBox>
                                        <asp:HiddenField runat="server" ID="hdnStep1" Value='<%#Eval("ProductStepID") %>' />
                                    </div>
                                    <div class="col-xs-2">
                                        <asp:LinkButton runat="server" ID="lbtnRemoveStep" CommandArgument='<%#Eval("StepNumber") %>' Visible='<%#Eval("IsRemovable") %>' CommandName="remove">
                                            <img alt="edit" src="../images/minus.png" width="18px"></asp:LinkButton>
                                    </div>
                                </div>
                                <div class="clearfix"></div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <%--<div class="form-group" runat="server" id="divStep2">
                            <label for="middle-name" class="control-label col-xs-2">
                                Step 2</label>
                            <div class="col-xs-8">
                                <asp:TextBox ID="txtStep2" runat="server" CssClass="form-control col-md-7 col-xs-12"
                                    data-validate-length-range="10" name="txtStep2" placeholder="Step 2"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdnStep2" Value="" />
                                
                            </div>
                            <div class="col-xs-2">
                                <a href="#" onclick='$(this).parent().parent().hide();'>
                                    <img alt="edit" src="../images/minus.png" width="18px"></a>
                            </div>
                        </div>
                        <div class="clearfix"></div>
                        <div class="form-group" runat="server" id="divStep3">
                            <label for="middle-name" class="control-label col-xs-2">
                                Step 3</label>
                            <div class="col-xs-8">
                                <asp:TextBox ID="txtStep3" runat="server" CssClass="form-control col-md-7 col-xs-12"
                                    data-validate-length-range="10" name="txtStep3" placeholder="Step 3"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdnStep3" Value="" />
                               
                            </div>
                        </div>
                        <div class="clearfix"></div>
                        <div class="form-group" runat="server" id="divStep4">
                            <label for="middle-name" class="control-label col-xs-2">
                                Step 4</label>
                            <div class="col-xs-8">
                                <asp:TextBox ID="txtStep4" runat="server" CssClass="form-control col-md-7 col-xs-12"
                                    data-validate-length-range="10" name="txtStep4" placeholder="Step 4"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdnStep4" Value="" />
                               
                            </div>
                        </div>
                        <div id="addMoreSteps">
                        </div>--%>
                    </asp:Panel>
                    <div class="frow">
                        <div class="col-xs-2">
                        </div>
                        <div class="col-xs-8">
                            <asp:LinkButton runat="server" OnClick="lbtnAddSteps_Click" ID="lbtnAddSteps" data-value='4' class="btn btn-classic"><i
                                class="fa fa-plus" aria-hidden="true"></i>Add Steps </asp:LinkButton>
                        </div>
                    </div>
                    <div class="clearfix">
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btnFakeSave" type="button" class="btn btn-primary btn-scheme" value="Save" onclick="if ($('#ContentPlaceHolder1_txtTitle').val() != '') { $('#ContentPlaceHolder1_btnSaveProduct').click(); } else { $('#ContentPlaceHolder1_txtTitle').css('border', '1px solid red'); return false; }">Save</button>
                    <asp:Button style="display: none;" ID="btnSaveProduct" runat="server" CssClass="btn btn-primary btn-scheme" UseSubmitBehavior="false" data-dismiss="modal" OnClick="btnSaveProduct_Click" Text="Save"></asp:Button>
                    <button id="btnFakeSaveNext" type="button" class="btn btn-primary btn-scheme" onclick="if ($('#ContentPlaceHolder1_txtTitle').val() != '') { $('#ContentPlaceHolder1_btnSaveAndNextProduct').click(); } else { $('#ContentPlaceHolder1_txtTitle').css('border', '1px solid red'); return false; }" >Save & Next</button>
                    <asp:Button style="display: none;" ID="btnSaveAndNextProduct" runat="server" CssClass="btn btn-primary btn-scheme" UseSubmitBehavior="false" data-dismiss="modal" OnClick="btnSaveAndNextProduct_Click" Text="Save & Next"></asp:Button>
                    <button class="btn btn-classic" data-dismiss="modal">
                        Cancel</button>
                </div>
            </div>
            <%--</form>--%>
        </div>
    </div>
    <div class="modal fade" id="instModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog modal-for-tinyeditor" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel1">Add Instruction</h4>
                </div>
                <div class="modal-body">
                    <div class="clearfix"></div>
                    <div class="item form-group">
                        <label class="control-label col-xs-2" for="name">
                            Title <span class="required"></span>
                        </label>
                        <div class="col-xs-10">
                            <asp:TextBox runat="server" ID="txtInstructionTitle" CssClass="form-control col-md-7 col-xs-12"
                                data-validate-length-range="10" name="txtInstructionTitle" placeholder="Instruction Title"
                                required="required" />
                            <asp:HiddenField runat="server" ID="hdnProductIDForInst" Value="" />
                        </div>
                    </div>
                    <div class="clearfix"></div>
                    <div class="item form-group">
                        <label class="control-label col-xs-2" for="name">
                            Short Description
                        </label>
                        <div class="col-xs-10">
                            <asp:TextBox ID="txtShortDesc" runat="server" CssClass="form-control col-md-7 col-xs-12"
                                data-validate-length-range="10" name="txtShortDesc" placeholder="Short Description" TextMode="MultiLine" Rows="5"></asp:TextBox>
                        </div>
                    </div>
                    <div class="clearfix"></div>
                    <div class="form-group">
                        <label for="middle-name" class="control-label col-xs-2">
                            Long Description</label>
                        <div class="col-xs-10 tiny-editor">
                            <textarea id="txtLongDesc" name="txtLongDesc" runat="server" rows="5" class="form-control col-md-7 col-xs-12 tinymce"
                                cols="40" style="width: 80%"></textarea>
                        </div>
                    </div>
                    <div class="clearfix">
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnSaveInstructions" runat="server" CssClass="btn btn-primary btn-scheme" UseSubmitBehavior="false" data-dismiss="modal" OnClick="btnSaveInstructions_Click" Text="Save"></asp:Button>
                    <button class="btn btn-classic" data-dismiss="modal">
                        Cancel</button>
                </div>
            </div>
        </div>
    </div>
    <!-- validator -->
    <script type="text/javascript">
        // initialize the validator function
        //validator.message.date = 'not a real date';

        // validate a field on "blur" event, a 'select' on 'change' event & a '.reuired' classed multifield on 'keyup':
    //    $('form')
    //.on('blur', 'input[required], input.optional, select.required', validator.checkField)
    //.on('change', 'select.required', validator.checkField)
    //.on('keypress', 'input[required][pattern]', validator.keypress);

    //    $('.multi.required').on('keyup blur', 'input', function () {
    //        validator.checkField.apply($(this).siblings().last()[0]);
    //    });

    //    $('form').submit(function (e) {
    //        e.preventDefault();
    //        var submit = true;

    //        // evaluate the form using generic validaing
    //        if (!validator.checkAll($(this))) {
    //            submit = false;
    //        }

    //        if (submit)
    //            this.submit();

    //        return false;
    //    });
    </script>
    <!-- /validator -->
    <!-- TinyMCE -->
    <script type="text/javascript">
        tinymce.init({
            selector: '#ContentPlaceHolder1_txtLongDesc',
            theme: 'modern',
            width: 500,
            height: 100,
            encoding: "xml",
            plugins: [
              //'advlist autolink link image lists charmap print preview hr anchor pagebreak spellchecker',
              //'searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking',
              //'save table contextmenu directionality emoticons template paste textcolor'
                'hr anchor pagebreak',
              'wordcount insertdatetime',
              'save table emoticons paste textcolor'
            ],
            setup: function (editor) {
                editor.on("SaveContent", function (i) {
                    i.content = i.content.replace(/&#39/g, "&apos");
                });
            },
            content_css: 'css/content.css',
            toolbar: 'insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | print preview media fullpage | forecolor backcolor emoticons'
        });
    </script>
    <!-- /TinyMCE -->
</asp:Content>
