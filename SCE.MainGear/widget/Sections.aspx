<%@ Page Title="" Language="C#" MasterPageFile="~/widget/Admin.Master" AutoEventWireup="true"
    CodeBehind="Sections.aspx.cs" Inherits="SCE.MainGear.admin.Sections" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- jquery sortable -->
    <%--<script type="text/javascript" src="../vendors/sortable/jquery-sortable.js"></script>--%>
    <!-- bootstrap-wysiwyg -->
    <script type="text/javascript" src="../vendors/bootstrap-wysiwyg/js/bootstrap-wysiwyg.min.js"></script>
    <script type="text/javascript" src="../vendors/jquery.hotkeys/jquery.hotkeys.js"></script>
    <script type="text/javascript" src="../vendors/google-code-prettify/src/prettify.js"></script>
    <!-- Custom Theme Scripts -->
    <script type="text/javascript" src="../build/js/custom.min.js"></script>
    <script type="text/javascript" src='//cdn.tinymce.com/4/tinymce.min.js'></script>
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
                                    <li><a href="index.aspx" class="text-primary">Product List</a><i class="fa fa-chevron-right"
                                        aria-hidden="true"></i></li>
                                    <li class="active">Sections</li>
                                </ol>
                            </div>
                            <h2>
                                <asp:Literal runat="server" ID="ltrlProductTitle"></asp:Literal>
                                - Sections</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li>
                                    <asp:LinkButton ID="lbtnAddSection" CssClass="btn btn-classic" runat="server" OnClick="lbtnAddSection_Click"><i
                                    class="fa fa-plus-circle" aria-hidden="true"></i>Add Section </asp:LinkButton></li>
                                <li runat="server" id="liAddFromMaster">
                                    <asp:LinkButton ID="lbtnAddSectionFromMaster" CssClass="btn btn-classic" runat="server" OnClick="lbtnAddSectionFromMaster_Click"><i
                                    class="fa fa-plus-circle" aria-hidden="true"></i>Add Section From Master</asp:LinkButton></li>
                            </ul>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="x_content">
                            <div class="table-responsive">
                                <table class="table table-striped jambo_table bulk_action ">
                                    <thead>
                                        <tr class="headings">
                                            <th class="column-title">Section Title
                                            </th>
                                            <th class="column-title no-link last">
                                                <span class="nobr">Action</span>
                                            </th>
                                            <th class="bulk-actions">
                                                <a class="antoo" style="color: #fff; font-weight: 500;">Bulk Actions ( <span class="action-cnt"></span>) <i class="fa fa-chevron-down"></i></a>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody class="sortable">
                                        <asp:Repeater runat="server" ID="rptrProducts" OnItemCommand="rptrSections_ItemCommand">
                                            <ItemTemplate>
                                                <tr class="even pointer" id='<%# Eval("SectionID") %>-<%# Eval("DisplayOrder") %>'>
                                                    <td class=" ">
                                                        <%# Eval("Title") %>
                                                    </td>
                                                    <td class=" ">
                                                        <asp:LinkButton runat="server" ID="lbtnInstructions" CommandName="instruction" CommandArgument='<%# Eval("SectionID") %>'
                                                            class="btn btn-sm btn-classic">Instructions</asp:LinkButton>
                                                        <asp:LinkButton runat="server" ID="lbtnEditSection" CommandName="edit" CommandArgument='<%# Eval("SectionID") %>'
                                                            class="btn btn-sm btn-classic">
                                                    Edit</asp:LinkButton>
                                                        <asp:LinkButton runat="server" ID="lbtnDelSection" CommandName="del" CommandArgument='<%# Eval("SectionID") %>'
                                                            CssClass="btn btn-sm btn-classic" OnClientClick="return confirm('Are you certain you want to delete this section?');">Delete</asp:LinkButton>
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
    <div class="modal fade" id="instModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog modal-for-tinyeditor" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel1">Add Instruction</h4>
                </div>
                <div class="modal-body">
                    <div class="item form-group">
                        <div class="clearfix"></div>
                        <label class="control-label col-xs-2" for="name">
                            Title <span class="required"></span>
                        </label>
                        <div class="col-xs-10">
                            <asp:TextBox runat="server" ID="txtInstructionTitle" CssClass="form-control col-md-7 col-xs-12"
                                data-validate-length-range="10" name="txtInstructionTitle" placeholder="Instruction Title"
                                required="required" />
                            <asp:HiddenField runat="server" ID="hdnSectionIDForInst" Value="" />
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
    <div class="modal fade" id="masterModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel2">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel2">Add Section from Master</h4>
                </div>
                <div class="modal-body">
                    <div class="item form-group">
                        <label class="control-label col-xs-2" for="name">
                            Master Sections 
                        </label>
                        <div class="col-xs-8">
                            <asp:DropDownList runat="server" ID="drpMasterSections" Width="100%"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="clearfix">
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnAddFromMaster" runat="server" CssClass="btn btn-primary btn-scheme" UseSubmitBehavior="false" data-dismiss="modal" OnClick="btnAddFromMaster_Click" Text="Save"></asp:Button>
                    <button class="btn btn-classic" data-dismiss="modal">
                        Cancel</button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            function initToolbarBootstrapBindings() {
                var fonts = ['Serif', 'Sans', 'Arial', 'Arial Black', 'Courier',
                    'Courier New', 'Comic Sans MS', 'Helvetica', 'Impact', 'Lucida Grande', 'Lucida Sans', 'Tahoma', 'Times',
                    'Times New Roman', 'Verdana'
                ],
                  fontTarget = $('[title=Font]').siblings('.dropdown-menu');
                $.each(fonts, function (idx, fontName) {
                    fontTarget.append($('<li><a data-edit="fontName ' + fontName + '" style="font-family:\'' + fontName + '\'">' + fontName + '</a></li>'));
                });
                $('a[title]').tooltip({
                    container: 'body'
                });
                $('.dropdown-menu input').click(function () {
                    return false;
                })
                  .change(function () {
                      $(this).parent('.dropdown-menu').siblings('.dropdown-toggle').dropdown('toggle');
                  })
                  .keydown('esc', function () {
                      this.value = '';
                      $(this).change();
                  });

                $('[data-role=magic-overlay]').each(function () {
                    var overlay = $(this),
                      target = $(overlay.data('target'));
                    overlay.css('opacity', 0).css('position', 'absolute').offset(target.offset()).width(target.outerWidth()).height(target.outerHeight());
                });

                if ("onwebkitspeechchange" in document.createElement("input")) {
                    var editorOffset = $('#editor').offset();

                    $('.voiceBtn').css('position', 'absolute').offset({
                        top: editorOffset.top,
                        left: editorOffset.left + $('#editor').innerWidth() - 35
                    });
                } else {
                    $('.voiceBtn').hide();
                }
            }

            function showErrorAlert(reason, detail) {
                var msg = '';
                if (reason === 'unsupported-file-type') {
                    msg = "Unsupported format " + detail;
                } else {
                    console.log("error uploading file", reason, detail);
                }
                $('<div class="alert"> <button type="button" class="close" data-dismiss="alert">&times;</button>' +
                  '<strong>File upload error</strong> ' + msg + ' </div>').prependTo('#alerts');
            }

            initToolbarBootstrapBindings();

            $('#editor').wysiwyg({
                fileUploadError: showErrorAlert
            });

            prettyPrint();

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
                        url: "./Sections.aspx/ChangeDisplayOrder",
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
        });
    </script>
    <!-- /bootstrap-wysiwyg -->
    <!-- compose -->
    <script type="text/javascript">
        $('#compose, .compose-close').click(function () {
            $('.compose').slideToggle();
        });
    </script>
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
