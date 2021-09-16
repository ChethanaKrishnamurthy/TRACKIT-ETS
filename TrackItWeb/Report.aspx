<%@ Page Title="Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="TrackItWeb.Report" %>
<%@ MasterType VirtualPath="Site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" />
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>

    <style type="text/css">
        .inactive, .inactive td {
            background-color: lightsalmon !important;
        }
     
        .table tr:nth-child(even) td {
            background-color: lightgray;
        }

        .table th {
            background-color: darkgray;
        }

        .FilterTable {
            background-color: darkgrey;
            width: 100%
        }

            .FilterTable h1 {
                margin-left: 20px;
            }

            .FilterTable td {
                padding: 0 0 0 20px;
            }

        .ListBoxCSS {
            height: 150px !important;
            width: 210px;
        }

        .MainDiv {
            padding-top: 50px;
            padding-bottom:100px;
        }

        .export {
            Margin-top: 20px !important;
        }

        .selectWrapper {
            border-radius: 36px;
            display: inline-block;
            overflow: hidden;
            background: #cccccc;
            border: 1px solid #cccccc;
        }

        .txtedate, .txtsdate {
            border: 1px solid black;
        }

        select {
            width: 250px;
            border: 1px solid black;
            -webkit-border-top-right-radius: 15px;
            -webkit-border-bottom-right-radius: 15px;
            -moz-border-radius-topright: 15px;
            -moz-border-radius-bottomright: 15px;
            border-top-right-radius: 15px;
            border-bottom-right-radius: 15px;
            padding: 2px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {

            Reports.BindCalendarToTextBoxes();
            if ($(".table").html()) {
                $(".header").show();
                Reports.changeRowColorBasedOnStatus();
            }
            else {
                $(".header").hide();
            }
        })
        var Reports = {            
            BindCalendarToTextBoxes: function () {
                $(".txtsdate").datepicker({
                    defaultDate: new Date(),
                    changeMonth: true,
                    onClose: function (selectedDate) {
                        $(".txtedate").datepicker("option", "minDate", selectedDate);
                    }
                });

                $(".txtedate").datepicker({
                    defaultDate: new Date(),
                    changeMonth: true,
                    onClose: function (selectedDate) {
                        $(".txtsdate").datepicker("option", "maxDate", selectedDate);
                    }

                });
            },

            changeRowColorBasedOnStatus: function () {
                $(".False").each(function (index) {                    
                    $(this).parent().addClass("inactive");
                });
            }
        }
    </script>


    <h2>Reports</h2>
    <div class="container-fluid Maindiv">

        <table class="FilterTable">
            <tr>
                <th colspan="3">
                    <h1>Filters</h1>
                </th>
            </tr>
            <tr>
                <td>Start Date: 
                    <asp:TextBox CssClass="txtsdate" ID="txtsdate" runat="server" Autocomplete="off"></asp:TextBox>
                </td>
                <td>End Date:     
                    <asp:TextBox CssClass="txtedate" ID="txtedate" runat="server" Autocomplete="off"></asp:TextBox>
                </td>
                <td>User: 
                  <asp:DropDownList ID="ddlPeople" runat="server"></asp:DropDownList>
                </td>
                <td>Project: 
                    <asp:DropDownList ID="ddlProject" runat="server"></asp:DropDownList>
                </td>
                <td>Department: 
                    <asp:DropDownList ID="ddlDepartment" runat="server"></asp:DropDownList>
                </td> 
                  <td>Status: 
                    <asp:DropDownList ID="ddlStatus" runat="server"></asp:DropDownList>
                </td> 
                  <td>Type: 
                    <asp:DropDownList ID="ddlType" runat="server"></asp:DropDownList>
                </td> 
            </tr>
            <tr>
                <td colspan="3" style="padding-top: 20px;">
                    <asp:Button class="btn btn-primary btn-lg" runat="server" ID="btnFilter" Text="Generate Report" OnClick="cmdFilter_Click" />
                    <asp:Button class="btn btn-primary btn-lg" runat="server" ID="btnClear" Text="Clear Filter" OnClick="cmdClear_Click" />
                    <asp:Label ID="lblErrorMsgReport" Text='' runat="server" />
                </td>
            </tr>
        </table>
        <div class="MainDiv">
            <div class="header">
                <div style="float: left">
                    <h1>Results
                    </h1>
                </div>
                <div style="float: Right">                  
                     <asp:Button class="btn btn-primary btn-lg" runat="server" ID="ExportToExcel" Text="Export" OnClick="cmdExport_Click" />
                </div>
            </div>
            <asp:ListView ID="lstReports" runat="server"
                GroupPlaceholderID="groupPlaceHolder1"
                ItemPlaceholderID="itemPlaceHolder1">
                <LayoutTemplate>
                    <table style="background-color: #eeeeee" class="table" id="exporttable">
                        <thead class="thead-dark">
                            <tr>
                                <th scope="col">Name</th> 
                                <th scope="col">Department</th>
                                <th scope="col">Status</th>
                                <th scope="col">Type</th>
                                <th scope="col">Project </th>
                                <th scope="col">Category </th>
                                <th scope="col">Sub Category </th>
                                <th scope="col">Activity Type </th>
                                <th scope="col">Date</th>
                                <th scope="col">Allocated</th>
                                <th scope="col">Comment</th>                                
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
                        <asp:Label ID="lblAlias" runat="server" Text='<%# Eval("empname") %>' ToolTip='<%# Eval("srt") %>'></asp:Label>
                    </td> 
                     <td >
                        <asp:Label ID="lblDepartment" runat="server" Text='<%# Eval("cchlvl1") %>'></asp:Label>
                    </td>
                     <td >
                        <asp:Label ID="lblEmpState" runat="server" Text='<%# Eval("empstate") %>'></asp:Label>
                    </td>
                    <td >
                        <asp:Label ID="lblType" runat="server" Text='<%# Eval("emp_type") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblProjectCode" runat="server" Text='<%# Eval("code") %>' ToolTip='<%# Eval("descr") %>' ></asp:Label>
                    </td>
                     <td>
                        <asp:Label ID="lblCat" runat="server" Text='<%# Eval("CategoryCode") %>' ToolTip='<%# Eval("descr") %>' ></asp:Label>
                    </td>
                     <td>
                        <asp:Label ID="lblSubCat" runat="server" Text='<%# Eval("SubCategoryCode") %>' ToolTip='<%# Eval("descr") %>' ></asp:Label>
                    </td>
                     <td>
                        <asp:Label ID="lblActType" runat="server" Text='<%# Eval("ActivityTypeCode") %>' ToolTip='<%# Eval("descr") %>' ></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("EntryDate") %>'></asp:Label>
                    </td>
                    <td >
                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Allocated") %>'></asp:Label>
                    </td>
                     <td >
                        <asp:Label ID="lblComment" runat="server" Text='<%# Eval("Comment") %>'></asp:Label>
                    </td>
                   
                </ItemTemplate>
                <EmptyDataTemplate>
                    <div>
                        <asp:Label ID="lblFetchStatus" runat="server"></asp:Label>                        
                    </div>
                </EmptyDataTemplate>
            </asp:ListView>
        </div>
    </div>
</asp:Content>