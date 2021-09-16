<%@ Page Title="Admin" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="TrackItWeb.Admin" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">   
    <style type="text/css">
        li{
            padding:10px;
        }
        .heading{
            padding:20px 0 50px 0px;
        }
    </style>
    <div class="Maindiv"> 
        <div class="heading">
            <h1>
                Admin
            </h1>
        </div>

        <div>
            <h4> Operations</h4>
        </div>
        <ul>
             <li>
               <a href="EditProjectCode.aspx">Add/ View/ Update Project Code</a>
            </li>                        
             <li>
               <a href="AddRemoveAdmin.aspx"> Add/ Remove user as Admin</a>
            </li>              
             <li>
               <a href="UserList.aspx"> List Of Users</a>
            </li>
              <li>
               <a href="UpdateUserEntry.aspx"> Update User Entry</a>
            </li>
            <li>
               <a href="UpdateDepartmentAssociations.aspx"> Department Associations</a>
            </li>
        </ul>
    </div>   
</asp:Content>