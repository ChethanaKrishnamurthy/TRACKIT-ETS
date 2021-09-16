<%@ Page Title="EditProjectCode"  Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="EditProjectCode.aspx.cs" Inherits="TrackItWeb.EditProjectCode" %>

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

        .AddPjttable {
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
            AddProjectType.BindCalendarToTextBoxes();
        });

        var AddProjectType = {
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
        <h1>Project Codes</h1>
    </div>

    <br />
    <button type="button" onclick="AddProjectType.clearErrorMessageFromPopup()" class="btn btn-info btn-lg modalbtn" data-toggle="modal" data-target="#AddProjectModal">Add Project Code</button>
     <button type="button" class="btn btn-info btn-lg modalbtn temp" data-toggle="modal" data-target="#AddProjectModal">Add Project Code</button>

    <div id="AddProjectModal" class="modal fade" role="dialog">
        <div class="modal-dialog">

            <div class="modal-content">
                <div class="modal-body">
                    <div>
                        <h1>Add Project Codes</h1>
                    </div>

                    <asp:Label ID="ErrorMsg" Text="" CssClass="ErrorMsg" runat="server"></asp:Label>
                    <table class="AddPjttable">
                        <tr>
                            <td>Project Code
                            </td>                            
                            <td>
                                <asp:TextBox ID="txtProjectCode" runat="server" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td>Description
                            </td>
                            <td>
                                <asp:TextBox ID="txtProjectDescription" MaxLength="255" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Sub Product
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlProjectOrg">
                                    <asp:ListItem Enabled="true" Text="Select Sub Project Code" Value="NA"></asp:ListItem>                                   
                                </asp:DropDownList>                                
                            </td>
                        </tr>                       
                        <tr>
                            <td colspan="2">
                                <asp:Button class="btn btn-primary btn-lg" runat="server" ID="btnAddProjectCodes" Text="Add" OnClick="cmdAddProjectCode_Click" />
                                 <button type="button" class="btn btn-default btn-primary btn-lg" data-dismiss="modal">Close</button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>

    <div class="container-fluid Maindiv">
        <asp:ListView ID="lstProjectCode" runat="server" DataKeyNames="Id" OnItemCanceling="lstProjectCode_ItemCanceling" OnItemEditing="lstProjectCode_ItemEditing" OnItemUpdating="lstProjectCode_ItemUpdating" OnItemDataBound="lstProjectCode_ItemDataBound">
            <LayoutTemplate>
                <table cellpadding="2" width="400px" border="1" runat="server" id="itemPlaceholderContainer" style="background-color: #eeeeee" class="table">
                    <tr runat="server" style="">
                        <th runat="server" class="hide">ID</th>
                        <th runat="server">Code</th>
                        <th runat="server">Description</th>
                        <th runat="server">Sub Product</th>  
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
                        <asp:Label ID="lblProjectCode" runat="server" Text='<%#Eval("project_code") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblProjectDescription" runat="server" Text='<%#Eval("description") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblProjectOrg" runat="server" Text='<%#Eval("project_org") %>'></asp:Label>
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
                        <asp:TextBox ID="txtProjectCode" runat="server" Text='<%#Bind("project_code") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="txtProjectDescription" runat="server" Text='<%#Bind("description") %>' />
                    </td>
                    <td>
                         <asp:DropDownList ID="ddlGridProjectOrg" runat="server"  > </asp:DropDownList>
                         <asp:Label ID="lblGridProjectOrg" runat="server" Text='<%#Bind("project_org") %>' Visible="false"></asp:Label> 
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