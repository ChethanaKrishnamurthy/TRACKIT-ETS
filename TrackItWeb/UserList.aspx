<%@ Page Title="UserList"  Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="TrackItWeb.AddRemoveRoles" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .FilterTable {
            background-color: darkgrey;
            width: 100%;
            margin-bottom: 20px;
        }

            .FilterTable h1 {
                margin-left: 20px;
            }

            .FilterTable td {
                padding: 20px 20px 20px 20px;
            }
              .Maindiv{
            margin-bottom:100px
        }
    </style>
    <div class="Maindiv">

        <div>
            <h1>User List</h1>
        </div>
        
        <table class="FilterTable">
            <tr>
                <td>Name: 
                    <asp:DropDownList ID="ddlName" AutoPostBack="true" runat="server" OnTextChanged="ddlFilterChanged"></asp:DropDownList></td>                 
                <td>Department:
                    <asp:DropDownList ID="ddlDepartment" AutoPostBack="true" runat="server" OnTextChanged="ddlFilterChanged"></asp:DropDownList></td>
                <td>Manager Name:
                    <asp:DropDownList ID="ddlManagerName" AutoPostBack="true" runat="server" OnTextChanged="ddlFilterChanged"></asp:DropDownList></td>
                <td>Employee Type:
                    <asp:DropDownList ID="ddlEmployeeType" AutoPostBack="true" runat="server" OnTextChanged="ddlFilterChanged"></asp:DropDownList></td>
                <td>Status:
                    <asp:DropDownList ID="ddlStatus" AutoPostBack="true" runat="server" OnTextChanged="ddlFilterChanged"></asp:DropDownList></td>               
            </tr>
            <tr>
                <td colspan="3" style="padding-top: 20px;">   
                    <asp:Button class="btn btn-primary btn-lg" runat="server" ID="btnClear" Text="Clear Filter" OnClick="cmdClear_Click" />
                    <asp:Button class="btn btn-primary btn-lg" runat="server" ID="ExportToExcel" Text="Export" OnClick="cmdExport_Click" />  
                    <asp:Label ID="lblErrorMsg" Text='' runat="server" />
                </td>
                
            </tr>
        </table>
        <div>
            <asp:ListView ID="lstAllUsers" runat="server" GroupPlaceholderID="groupPlaceHolder1" ItemPlaceholderID="itemPlaceHolder1">
                <LayoutTemplate>
                    <table style="background-color: #eeeeee" class="table">
                        <tr>
                            <th>Name</th>
                            <th>Employee ID</th>
                            <th>Department</th>
                            <th>Manager Name</th>
                            <th>Employee Type</th>
                            <th>Active/ Inactive Status</th>                            
                        </tr>
                        <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
                    </table>
                </LayoutTemplate>
                <GroupTemplate>
                    <tr>
                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                    </tr>
                </GroupTemplate>
                <ItemTemplate>
                    <td><asp:Label ID="lblempname" Text='<%# Eval("empname") %>' runat="server" /></td>   
                    <td><asp:Label ID="lblEmpID" Text='<%# Eval("emp_id") %>' runat="server" /></td>
                    <td><asp:Label ID="lbldepartment" Text='<%# Eval("department") %>' runat="server" /></td>
                    <td><asp:Label ID="lblmgr" Text='<%# Eval("mgr") %>' runat="server" /></td>
                    <td><asp:Label ID="lblemptyp" Text='<%# Eval("emp_type") %>' runat="server" /></td>
                    <td><asp:Label ID="lblempstate" Text='<%# Eval("empstate") %>' runat="server" /></td>                    
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>
</asp:Content>