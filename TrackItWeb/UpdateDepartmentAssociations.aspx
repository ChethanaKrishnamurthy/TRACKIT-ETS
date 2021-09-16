<%@ Page Title="UpdateDepartmentAssociations" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UpdateDepartmentAssociations.aspx.cs" Inherits="TrackItWeb.UpdateDepartmentAssociations" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">   
    <style type="text/css">
        .table{
            margin-top:20px;
        }
            .table tr:nth-child(odd) td {
            background-color: white;
        }

       .table th {
            background-color: darkgray;
        }

       td{
            padding-bottom: 20px;
            padding-right:10px;
       }      
       .btn-group button {
          background-color: lightgray; 
          border: 1px solid black; 
          color: black; 
         
          cursor: pointer; 
          width: 100%; 
          display: block; 

       }

        .btn-group button:not(:last-child) {
          border-bottom: none; 
        }
                
        .btn-group button:hover {
          background-color: darkgray;
        }

         .SuccessMessage {
            color: red;
        }

    </style>     
       

    <div class="Maindiv">
        <div style="padding-left: 50px">
            <h1> Department Associations</h1>
        </div>
        <br />    
        
        <div style="padding-left: 50px">
           <asp:Label ID="lblDepartment" style ="font-weight:bold" runat="server" Text=" Department "/>
           <asp:DropDownList ID="ddlDepartment" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged"></asp:DropDownList>
        </div>  
        <br />

        <div style="padding-left: 50px">
           <asp:Label ID="lblProjectTypes" style ="font-weight:bold" runat="server" Text=" ProjectType "/>
           <asp:DropDownList ID="ddlProjectTypes" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlProjectTypes_SelectedIndexChanged" style="height: 22px"></asp:DropDownList>
        </div> 
        
        <br />
          <div style="padding-left:50px">
            <asp:Button  runat="server" ID="btnAllocate" Text="Refresh" OnClick="btnAllocate_Click" />
              </div>
        <br />

         <div style=""float: right">
            <asp:Label ID="lblDepPjtType" style ="font-weight:bold;padding-left:120px" runat="server" Text="Project Type"/>
            <asp:Label ID="lblDepAllocatedPjtType" style ="font-weight:bold;padding-left:220px" runat="server" Text="Associated Project Type"/>
          </div>

        <div style="padding-left: 50px"; >
           <asp:ListBox style="height: 300px;width:250px" ID="lstDepPjtTypes" runat="server" SelectionMode="Multiple" CssClass="ListBoxCSS"></asp:ListBox>    
          
            <div style="vertical-align:130px;font-weight:bold" class="btn-group">
                <asp:Button ID="btnLeft" Text="<<" runat="server" OnClick="LeftClick" />
                <asp:Button ID="btnRight" Text=">>" runat="server" OnClick="RightClick" />      
            </div>

           <asp:ListBox style="height: 300px;width:250px" ID="lstCategoryWithAsscociation" SelectionMode="Multiple"  runat="server" CssClass="ListBoxCSS"></asp:ListBox> 
            <input type="hidden" id="hdnSelectedCategories" runat="server" />
        </div>

        <br />          
        <div style="padding-left:515px">
            <asp:Button  runat="server" ID="btnsave" Text="Save" OnClick="btnsave_Click" />
            <asp:Button  runat="server" ID="btnCancel" Text="Cancel" /><br />
            <asp:Label ID="StrSaveMsg" runat="server" Text="" CssClass="SuccessMessage"></asp:Label>
        </div>
        </div>
</asp:Content>