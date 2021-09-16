<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TrackItWeb._Default" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" />
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
    <style type="text/css">
        tr:nth-child(even) td {
            background-color: gainsboro;
        }

        .Maindiv th {
            background-color: steelblue;
            color:white;
            font-size:medium;
        }

        .modal-content {
            background-color: lightgrey;
            height: 100%;
            width:100%;            
        }

        .pjtCode {
            background-color: lightgrey;
            height: 100%;
            width:100%; 
            text-align:left;
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
            border-collapse:separate;                
        }

        .pjtcontent
        {
            text-align:left;
            padding-left: 5px;
        }
        .SuccessMessage {
            color: red;
        }

        table, th, td {
            border: 1px solid white;
            text-align:center;            
            width: 50px;
            
         }

          .body-content {
            margin-left: 450px           
        }
            .hiddenbtn {
            display:none;
            visibility:hidden;
        }
            .txtsdate {
            border: 1px solid black;  
             -moz-border-radius: 8px;
            border-radius: 8px;
            -webkit-border-radius: 8px;
            border:solid 1px black;
            padding:5px;
        }     
             .txtsImport {
            border: 1px solid black;
            -moz-border-radius: 8px;
            border-radius: 8px;
            -webkit-border-radius: 8px;
            border:solid 1px black;
            padding:5px;              
        } 
             ::-webkit-input-placeholder { 
              color: #0066FF;
            }  

    </style>

    <script type="text/javascript">          
        
        var ProjectTime = {            
            getEditedRecords: function (elem) {
                debugger;
                var ElementId = elem.id;
                var UserLabelId = ElementId.split("_")[0] + "_" + ElementId.split("_")[1] + "_" + ElementId.split("_")[2] + "_lblEntryUserId_" + ElementId.split("_")[4];
                var userId = $("#" + UserLabelId).html();
                if (ProjectTime.listOfIdsEdited.indexOf(userId) === -1) {
                    ProjectTime.listOfIdsEdited.push(userId)
                    var AllUserId;
                    if ($("#MainContent_ModifiedIds").val().length != 0) {
                        AllUserId = $("#MainContent_ModifiedIds").val() + "," + userId;
                    }
                    else {
                        AllUserId = userId;
                    }
                    $("#MainContent_ModifiedIds").val(AllUserId);
                }
            },
            listOfIdsEdited: []            
        }

         $(document).ready(function ()
        {
          try {
              if (document.getElementById('<%= dataFirstTime.ClientID %>').value == "yes")
              {
                document.getElementById('<%= cmdFirst.ClientID %>').click();
                document.getElementById('<%= dataFirstTime.ClientID %>').value = "no";
               }
          }
          catch (e) {
              alert("JS Error "  + e );
          }
      });

         //Date Picker
        $(document).ready(function () {

            HomePageDate.BindCalendarToTextBoxes();            
            if ($(".table").html()) {
                $(".header").show();
                HomePageDate.changeRowColorBasedOnStatus();
            }
            else {
                $(".header").hide();
            }

            ImportEntriesDate.BindCalendarToTextBoxes();            
            if ($(".table").html()) {
                $(".header").show();
                ImportEntriesDate.changeRowColorBasedOnStatus();
            }
            else {
                $(".header").hide();
            }
        });
        
        var HomePageDate = {            
            BindCalendarToTextBoxes: function () {
                $(".txtsImport").datepicker({
                    defaultDate: new Date(),
                    changeMonth: true,
                    minDate: new Date('2020-10-1')                                    
                });               
            },
            changeRowColorBasedOnStatus: function () {
                $(".False").each(function (index) {
                    $(this).parent().addClass("inactive");
                });
            
            
            $(".txtsdate").on("dp.change", function () {
                __doPostBack();
                });
            }
        }  

        var ImportEntriesDate = {            
            BindCalendarToTextBoxes: function () {
                $(".txtsdate").datepicker({
                    defaultDate: new Date(),
                    changeMonth: true,
                    minDate: new Date('2020-10-1')                 

                });               
            },
            changeRowColorBasedOnStatus: function () {
                $(".False").each(function (index) {
                    $(this).parent().addClass("inactive");
                });
            
            
            $(".txtsdate").on("dp.change", function () {
                __doPostBack();
                });
            }
        }
   </script>
  

    <div id =" " style ="display:none; visibility:hidden">
        <input type="hidden" runat="server" name="dataFirstTime" id ="dataFirstTime" value ="yes" />
        <asp:Button  class="hiddenbtn" runat="server" ID="cmdFirst" Text="First" OnClick="firstTime_Load" UseSubmitBehavior="False"/>
    </div>


    <div class="container-fluid">        
        <h1>TrackIt - Enterprise Technology Services</h1>
    </div>
    <br />
       
    <asp:HiddenField ID="ModifiedIds"
        Value=""
        runat="server" />
    <div id="projectCodeModal" class="modal fade" role="dialog">
         <div class="modal-dialog">
            <div class="modal-content">      
                <div class="modal-body" >
                    <asp:GridView class="pjtCode" style ="text-align:left" ID="tblProjects" runat="server" AutoGenerateColumns="False">
                        <Columns>
                             <asp:BoundField DataField="code" HeaderText="Project Code" ItemStyle-Wrap="false" ItemStyle-CssClass="pjtcontent" />
                            <asp:BoundField DataField="descr" HeaderText="Project Description" ItemStyle-CssClass="pjtcontent" />
                        </Columns>
                    </asp:GridView>

                </div>
             </div>
        </div>
    </div>
    <div class="container-fluid Maindiv">
        <div style="padding-bottom: 5px" >
            <asp:Label ID="lblPeriod" style ="font-weight:bold" runat="server" Text="Date " Font-Size="15pt"/>
            &nbsp;<asp:TextBox style="text-align:center" CssClass="txtsdate" ID="txtsdate" runat="server" AutoCompleteType="Disabled" AutoPostBack="true" OnTextChanged="txtsdate_TextChanged" BorderStyle="Solid" BorderWidth="2px" Font-Bold="True" Height="32px" Font-Size="13pt" Width="142px"></asp:TextBox>     
             
        </div>
           <div style="padding-bottom: 25px">
            <asp:Label ID="StrMessage" runat="server" CssClass="SuccessMessage" Font-Bold="True" Font-Size="25px"></asp:Label><br />
            <asp:Button class="btn btn-primary btn-lg save" runat="server" ID="cmdSave" Text="Save" OnClientClick="javascript: ProjectTime.listOfIdsEdited.length=0;" OnClick="cmdSave_Click" />
            <button type="button" class="btn btn-info btn-lg modalbtn" data-toggle="modal" data-target="#projectCodeModal">Project Code Info</button>
                          
                       
            
            <asp:Button  style="float:right;" class="btn btn-primary btn-lg" runat="server" ID="btnImportEntries" Text="Import Entries" OnClick="cmdImportEntries_Click" BackColor="#0099FF"/>
            <asp:TextBox style="margin-right:5px;float:right;" class="float-md-left" CssClass="txtsImport" ID="txtImportEntries" runat="server" Placeholder="Import Date" Autocomplete="off" AutoPostBack="true" OnTextChanged="txtImportEntries_TextChanged" BorderStyle="Solid" Font-Bold="False" Font-Size="13pt" Height="45px" Width="110px" BorderColor="#0066FF" ForeColor="#0066FF"></asp:TextBox> 

        </div>
          
        
 <asp:ListView ID="ListView1" runat="server" OnItemDataBound="OnItemDataBound" GroupPlaceholderID="groupPlaceHolder1" ItemPlaceholderID="itemPlaceHolder1">
    <LayoutTemplate>
       <table id="table1" class="table">
            <thead  class="thead-dark">
                <tr>
                   <th scope="col">#</th>
                    <th scope="col">
                       <asp:Label ID="ProjectHeader" runat="server" Text="Project"></asp:Label></th> 
                   <th scope="col">
                       <asp:Label ID="CategoryHeader" runat="server" Text="Category"></asp:Label></th>                       
                   <th scope="col">
                       <asp:Label ID="SubCategoryHeader" runat="server" Text="Sub Category" ></asp:Label></th>                     
                    <th scope="col">
                       <asp:Label ID="ActivityTypeHeader" runat="server" Text="Activity Type"></asp:Label></th>                           
                    <th scope="col">
                       <asp:Label ID="Comment" runat="server" Text="Comment"></asp:Label></th>   
                    <th scope="col">
                       <asp:Label ID="Percentage" runat="server" Text="Percentage"></asp:Label></th>
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
                <td style="display: none">                    
                    <asp:Label ID="lblEntryUserId" runat="server" Text='<%# Eval("UserId") %>'></asp:Label>                                
                </td> 
               <td>                    
                    <asp:Label ID="Record" runat="server" Text='<%# Eval("SNo") %>'></asp:Label>                                
                </td>
                 <td> 
                    <asp:DropDownList ID="lstProject1" style="height:23px" runat="server" onchange="ProjectTime.getEditedRecords(this)">
                    </asp:DropDownList>
                    <asp:Label ID="lblPjt1" runat="server" Text='<%# Eval("Project") %>' Visible="false"></asp:Label>
                </td>
                <td> 
                    <asp:DropDownList ID="lstProject2" style="height:23px" runat="server" onchange="ProjectTime.getEditedRecords(this)">
                    </asp:DropDownList>
                    <asp:Label ID="lblPjt2" runat="server" Text='<%# Eval("Category") %>' Visible="false"></asp:Label>
                </td>
                <td>   
                    <asp:DropDownList ID="lstProject3"  style="height:23px" runat="server" onchange="ProjectTime.getEditedRecords(this)">     
                    </asp:DropDownList>
                    <asp:Label ID="lblPjt3" runat="server" Text='<%# Eval("SubCategory") %>' Visible="false"></asp:Label>                    
                </td>
                 <td>   
                    <asp:DropDownList ID="lstProject4" style="height:23px" runat="server" onchange="ProjectTime.getEditedRecords(this)">   
                    </asp:DropDownList>
                    <asp:Label ID="lblPjt4" runat="server" Text='<%# Eval("ActivityType") %>' Visible="false"></asp:Label>                    
                </td> 
                <td style="white-space: nowrap">   
                    <asp:TextBox ID="txtComment" style="height:23px"  value='<%# Eval("Comment")  %>' runat="server" onchange="ProjectTime.getEditedRecords(this)" Columns="2"  width="250px" Height="23px"></asp:TextBox> 
                </td>
              <td style="white-space: nowrap">   
                    <asp:TextBox ID="txtAllocatedPer" style="height:23px"  value='<%# Eval("AllocatedPer")  %>' runat="server" onchange="javascript:return CalculateValue(this);" Columns="2"  MaxLength="3" width="50px" type="number" Height="20px" min="0" max="100"></asp:TextBox>% 
                </td> 

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

       <div style="float:right;margin-right:50px;">
            <asp:Label ID="lblTotalPer" style="margin-right:5px" runat="server" Text="Total "/>
            <asp:TextBox ID="txtTotalPer" runat="server" onchange="javascript:return CalculateValue(this);" Columns="2"  MaxLength="3" width="50px" Height="20px" type="number" ReadOnly="true" Enabled="false"></asp:TextBox>%          
        </div>
        <!-- Display SNOW Entries  -->  
    <br />
    <asp:Label ID="lblSNOW" style ="font-weight:bold" runat="server" Text="ServiceNow Entries " Font-Size="15pt"/>  <br /> 
    <asp:Label ID="lblSNOWRecords" runat="server" Text="" CssClass="SuccessMessage"></asp:Label>

    <asp:Label ID="lblTotalTimeMin" style ="font-weight:bold;margin-left:1300px;"  runat="server" Visible="true"></asp:Label>
    <asp:Label ID="lblTotalTimeHrs" style ="font-weight:bold;margin-left:150px" runat="server" Visible="true"></asp:Label>

    <asp:ListView ID="lstViewSNOWEntries" runat="server" OnItemDataBound="lstViewSNOWEntries_ItemDataBound" GroupPlaceholderID="groupPlaceHolder1" ItemPlaceholderID="itemPlaceHolder1" >
            <LayoutTemplate>
                <table style="background-color: #eeeeee" class="table">
                    <thead style="background-color: #D3D3D3">                      
                            <th style="background-color:#A5A5A5;color:black" scope="col">#</th>
                            <th style="background-color:#A5A5A5;color:black" scope="col">Task Number</th>
                            <th style="background-color:#A5A5A5;color:black" scope="col">Short Description</th>  
                            <th style="background-color:#A5A5A5;color:black" scope="col">Time Worked: <span id="total-worked-min"> </span> Minutes</th>        
                            <th style="background-color:#A5A5A5;color:black" scope="col">Time Worked: <span id="total-worked-hrs"> </span> Hours</th>
                            <th style="background-color:#A5A5A5;color:black" scope="col">Task Type</th> 
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
                     <td style="width:4%"> 
                    <asp:Label ID="lblNo" runat="server"  Text='<%# Eval("#")%>' Visible="true"></asp:Label>
                </td>
                 <td> 
                    <asp:Label ID="lblTaskNo" runat="server"  Text='<%# Eval("tasknumber")%>' Visible="true"></asp:Label>
                </td>              
                <td style="text-align:left;width:36%">
                    <asp:Label ID="lblShrtDescription" runat="server" style="text-align:left" Text='<%# Eval("short_description") %>' Visible="true"></asp:Label>
                </td>                
                <td>
                    <asp:Label ID="lblTimeWorked" runat="server" style="text-align:left" Text='<%# Eval("timeworked") %>' Visible="true"></asp:Label>
                </td>
                  <td>
                    <asp:Label ID="lblTimeWrkdhrs" runat="server" style="text-align:left" Text='<%# Eval("timeworkedhrs") %>' Visible="true"></asp:Label>
                </td>
                  <td>
                    <asp:Label ID="lblTaskType" runat="server" style="text-align:left" Text='<%# Eval("tasktype")%>' Visible="true"></asp:Label>
                </td>                                
            </ItemTemplate>
            <EmptyDataTemplate>
                <table class="emptyTable">
                    <tr>
                        <td>
                        </td>                        
                    </tr>
                </table>
            </EmptyDataTemplate>
        </asp:ListView>      
    <br />    <br />    <br />    <br />

    <!-- Display SNOW Entries End -->
   
    </div> 
    
         <script type="text/javascript">
             function CalculateValue(elem) {
                var table = document.getElementById("table1"), sumVal = 0;            
                for (var i = 1; i < table.rows.length; i++){                
                    var numberValue = table.rows[i].cells[7].getElementsByTagName("INPUT")[0].value;                    
                    sumVal = sumVal + parseInt(numberValue);
                } 
                var _txttotper = document.getElementById('<%= txtTotalPer.ClientID %>');                
                 _txttotper.value = parseInt(sumVal);                
             } 

             //Display ServiceNow Entries Total hrs and Minutes worked
             const Minelement = document.getElementById('MainContent_lblTotalTimeMin');
             if (Minelement.innerText != "") {
                 document.getElementById('total-worked-min').innerText = Minelement.innerText;
             }
             Minelement.style.display= 'none';

             const Hrselement = document.getElementById('MainContent_lblTotalTimeHrs');
             if (Hrselement.innerText != "") {
                 document.getElementById('total-worked-hrs').innerText = Hrselement.innerText;
             }
             Hrselement.style.display = 'none';
          </script>   
    <br />
    <br />
</asp:Content> 