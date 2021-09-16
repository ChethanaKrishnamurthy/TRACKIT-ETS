<%@ Page Title="EditCategory"  Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="EditProjectTypes.aspx.cs" Inherits="TrackItWeb.EditCategory" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" />
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
   
    <style type="text/css">
        .ErrorMsg{
            color:red;
        }

        .temp{
            display:none;
        }

        .Maindiv{
            margin-bottom:100px
        }

        .AddCattable {
          border-collapse: separate;
          border-spacing: 0 15px;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {    
            $(".open").on("click", function () {
                $(".popup, .popup-content").addClass("active");
                return false;
            });
            AddCategory.BindCalendarToTextBoxes();
        });

        var AddCategory = {
            BindCalendarToTextBoxes: function () {
                $(".txtsdate").datepicker({
                    defaultDate: new Date(),
                    changeMonth: true,
                    onClose: function (selectedDate) {
                        $(".txtsdate").datepicker("option", "minDate", selectedDate);
                    }
                });

                $(".txtedate").datepicker({
                    defaultDate: new Date(),
                    changeMonth: true,
                    onClose: function (selectedDate) {
                        $(".txtedate").datepicker("option", "maxDate", selectedDate);
                    }

                });
            },
            clearErrorMessageFromPopup: function () {
            $(".ErrorMsg").html('');
        }
        }
    </script>

    <div class="container-fluid">
        <h1>Project Types</h1>
    </div>
     <div style="padding-left:15px">
            <asp:Label ID="lblProjectType" runat="server" Text="Project Type"/>
            <asp:DropDownList ID="lstProjectTypes" AutoPostBack="true" runat="server" CssClass="ListBoxCSS"></asp:DropDownList>
        </div>

    <br />
    <button type="button" onclick="AddCategory.clearErrorMessageFromPopup()" class="btn btn-info btn-lg modalbtn" data-toggle="modal" data-target="#AddCategoryModal">Add Category</button>
     <button type="button" class="btn btn-info btn-lg modalbtn temp" data-toggle="modal" data-target="#AddProjectModal">Add Project Code</button>

    <div id="AddCategoryModal" class="modal fade" role="dialog">
        <div class="modal-dialog">

            <div class="modal-content">
                <div class="modal-body">
                    <div>
                        <h1>Add Project Codes</h1>
                    </div>

                    <asp:Label ID="ErrorMsg" Text="" CssClass="ErrorMsg" runat="server"></asp:Label>
                    <table class="AddCattable">
                        <tr>
                            <td>Category
                            </td>                            
                            <td>
                                <asp:TextBox ID="txtCategory" runat="server" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td>Description
                            </td>
                            <td>
                                <asp:TextBox ID="txtCategoryDescription" MaxLength="255" runat="server"></asp:TextBox>
                            </td>
                        </tr>                                            
                        <tr>
                            <td colspan="2">
                                <asp:Button class="btn btn-primary btn-lg" runat="server" ID="btnAddCategory" Text="Add" OnClick="cmdAddCategory_Click" />
                                 <button type="button" class="btn btn-default btn-primary btn-lg" data-dismiss="modal">Close</button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>

    <div class="container-fluid Maindiv">
        <asp:ListView ID="lstCategory" runat="server" DataKeyNames="Id" OnItemCanceling="lstCategory_ItemCanceling" OnItemEditing="lstCategory_ItemEditing" OnItemUpdating="lstCategory_ItemUpdating" OnItemDataBound="lstCategory_ItemDataBound">
            <LayoutTemplate>
                <table cellpadding="2" width="400px" border="1" runat="server" id="itemPlaceholderContainer" style="background-color: #eeeeee" class="table">
                    <tr runat="server" style="">
                        <th runat="server" class="hide">ID</th>
                        <th runat="server">Category</th>
                        <th runat="server">Description</th>                          
                        <th runat="server">Update</th>
                    </tr>
                    <tr id="itemPlaceholder" runat="server">
                    </tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr runat="server">
                    <td class="hide">
                        <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblCategory" runat="server" Text='<%#Eval("project_code") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblCategoryDescription" runat="server" Text='<%#Eval("description") %>'></asp:Label>
                    </td>                                     
                    <td>
                        <asp:LinkButton ID="btnedit" runat="server" CommandName="Edit">Edit</asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>
            <EditItemTemplate>
                <tr>
                    <td class="hide">
                        <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCategory" runat="server" Text='<%#Bind("project_code") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="txtCategoryDescription" runat="server" Text='<%#Bind("description") %>' />
                    </td>  
                    <td>
                        <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" Text="Update" />&nbsp;
                    <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" />
                    </td>
                </tr>
            </EditItemTemplate>
        </asp:ListView>
    </div>
</asp:Content>