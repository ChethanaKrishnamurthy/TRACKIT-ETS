<%@ Page Title="AddRemoveAdmin" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddRemoveAdmin.aspx.cs" Inherits="TrackItWeb.AddRemoveAdmin" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .table{
            margin-top:20px;
        }
            .table tr:nth-child(odd) td {
            background-color: white;
        }

       .table th {
            background-color: lightblue;
        }

       td{
            padding-bottom: 20px;
            padding-right:10px;
       }       
    </style>

    <div class="Maindiv">
        <div>
            <h1> Add / Remove User as Admin</h1>
        </div>
        <br />
      <table>
          <tr>
              <td>
                  Select User
              </td>              
              <td >
                  <asp:DropDownList ID="ddlNonAdminUsers" runat="server"></asp:DropDownList>
              </td>
          </tr>   
          <tr>
              <td colspan="2">
                  <asp:Button class="btn btn-primary btn-lg" runat="server" ID="btnAddAdmin" Text="Add" OnClick="cmdAddAdmin_Click" />
              </td>
          </tr>
      </table>
        <div>
            <asp:ListView ID="lstAdminUsers" runat="server" OnItemDeleting="DeleteAdmin"
            GroupPlaceholderID="groupPlaceHolder1" ItemPlaceholderID="itemPlaceHolder1">
            <LayoutTemplate>
         <table  class="table">
            <tr>
                <th>
                    Username
                </th>
               
                <th>
                    Operation
                </th>
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
        <td>
            <asp:Label ID="lblUsername" Text='<%# Eval("username") %>' runat="server" />
        </td>      
        <td>
            <asp:LinkButton ID="lnkDelete" CommandName="Delete" runat="server" Text="Delete"></asp:LinkButton>
        </td>
    </ItemTemplate>
</asp:ListView>
        </div>
    </div>   
</asp:Content>