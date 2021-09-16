<%@ Page Title="View User Entry"  Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewUserEntry.aspx.cs" Inherits="TrackItWeb.ViewUserEntry"%>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        tr:nth-child(even) td {
            background-color: lightgray;
        }

        .Maindiv th {
            background-color: darkgray;
        }

        .modal-content {
            background-color: lightgray;
            height: 100%;
        }

        .Maindiv {
            float: left;
            height: 80%;
            width: 80%;
        }

        .LeftFilter .ListBoxCSS {
            height: 150px !important;
            width: 210px;
        }

        .LeftFilter th {
            font-weight: bold;
        }

        .LeftFilter table {
            border-spacing: 25px;
            border-collapse: separate;
        }
        .SuccessMessage{
            color:red;
        }       
    </style>
 
    <div class="container-fluid">
        <h1>TrackIt - Enterprise Technology Services</h1>
    </div>
    <br />
    <div class="container-fluid Maindiv">
        <div>
            <asp:Label ID="lblPeriod" runat="server" Text="Month"/>
            <asp:DropDownList ID="lstPeriods" AutoPostBack="true" runat="server" OnTextChanged="lstPeriods_SelectedIndexChanged" CssClass="ListBoxCSS"></asp:DropDownList>
        </div>
        <br />
         <div>
            <asp:Label ID="lblPeriodSelection" runat="server" Text="Time Period"/>
            <asp:DropDownList ID="lstPeriodSelection" AutoPostBack="true" runat="server" OnTextChanged="lstPeriodSelection_SelectedIndexChanged" CssClass="ListBoxCSS"></asp:DropDownList>
        </div>
        <br />
        <asp:ListView ID="lstViewUserEntry" runat="server" 
            GroupPlaceholderID="groupPlaceHolder1"
            ItemPlaceholderID="itemPlaceHolder1">
            <LayoutTemplate>
                <table style="background-color: #eeeeee" class="table">
                    <thead class="thead-dark">
                        <tr>
                            <th scope="col">UserName</th>
                            <th scope="col">Category</th>
                            <th scope="col">Sub Category</th>
                            <th scope="col">Activity Type</th>                            
                        </tr>
                    </thead>
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
                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("tabempname") %>' ToolTip='<%# Eval("srt") %>' Visible="true"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblDay1" runat="server" Text='<%# Eval("P1Display") %>' ToolTip='<%# Eval("P1ProjectDescr") %>' Visible="true"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblDay2" runat="server" Text='<%# Eval("P2Display") %>' ToolTip='<%# Eval("P2ProjectDescr") %>' Visible="true"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblDay3" runat="server" Text='<%# Eval("P3Display") %>' ToolTip='<%# Eval("P3ProjectDescr") %>' Visible="true"></asp:Label>                </td>
               
            </ItemTemplate>
            <EmptyDataTemplate>
                <table class="emptyTable">
                    <tr>
                        <td>No records available.
                        </td>
                    </tr>
                </table>
            </EmptyDataTemplate>
        </asp:ListView>  
        </div>
    <br />
    <br />     
</asp:Content>