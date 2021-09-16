using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SysComponents;

namespace TrackItWeb
{
    public partial class ViewUserEntry : Page
    {
        public CUserInfo LoggedInUser;
        public String
            logMsgPageInfo = "",
            logMsgPageData = "",
            pageTitle = "View User Entry",
            userMessage = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            LoggedInUser = (CUserInfo)Page.Session["UserData"];
            if (!LoggedInUser.IsValidUser) CWeb.NotAuthorizedRedirect(LoggedInUser, Page.Title, Response);
            Page.Title = String.IsNullOrEmpty(pageTitle) ? LoggedInUser.WebAppTitle : String.Format("{0}-{1}", LoggedInUser.WebAppTitle, pageTitle);
            if (!IsPostBack)  PageSetup();            
        }

        #region Page Methods
        private void PageSetup()
        {
            try
            {
                CLog.WebEvent("PageSetup Start", Page.Title, LoggedInUser);
                DateTime
                    StartDate = new DateTime(LoggedInUser.EntryStartDate.Year, LoggedInUser.EntryStartDate.Month, 1)
                    , CurrentDate = new DateTime(LoggedInUser.EntryCurrentDate.Year, LoggedInUser.EntryCurrentDate.Month, 1);
                while (StartDate <= CurrentDate.AddMonths(+1))
                {
                    lstPeriods.Items.Add(new ListItem(String.Format("{0,20}", StartDate.ToString("y")), StartDate.ToString("yyyy-MM-dd")));
                    StartDate = StartDate.AddMonths(1);
                }
                lstPeriods.SelectedIndex = lstPeriods.Items.IndexOf(lstPeriods.Items.FindByValue(CurrentDate.ToString("yyyy-MM-dd")));
                CLog.WebEvent("PageSetup Completed", Page.Title, LoggedInUser);

               
                // Time Period Selection
                DateTime startOfWeek = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek));
                startOfWeek = startOfWeek.AddDays(1);               
                String SelectedValue = startOfWeek.ToString("dd/MMM/yyyy") + " - " + startOfWeek.AddDays(6).ToString("dd/MMM/yyyy");
                lstPeriodSelection.Items.Add(new ListItem("Daily", DateTime.Now.ToString("yyyy-MM-dd")));
                lstPeriodSelection.Items.Add(new ListItem("Weekly", startOfWeek.ToString("dd/MMM/yyyy") + " - " + startOfWeek.AddDays(6).ToString("dd/MMM/yyyy")));
                lstPeriodSelection.Items.Add(new ListItem("Monthly", LoggedInUser.EntryCurrentDate.ToString("yyyy-MM-dd")));
            }

            catch (Exception ex)
            {
                CLog.WebError("PageSetup Error", Page.Title, LoggedInUser, ex);
            }
        }
        #endregion

        #region User Event Handlers 
        private void DisplayEntries(DateTime EntryDate)
        {
            DataTable EntryList = CWeb.UserViewEntries(LoggedInUser, LoggedInUser.UserID, EntryDate.ToString());
            lstViewUserEntry.DataSource = EntryList;
            lstViewUserEntry.DataBind();
        }
        protected void lstPeriods_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {               
                DisplayEntries(DateTime.Parse(lstPeriods.SelectedValue));
            }
            catch (Exception ex)
            {
                CLog.WebError("Error in Dropdown Date Change", Page.Title, LoggedInUser, ex);
            }
        }


         private void DisplayTimePeriodEntries(DateTime EntryDate)
         {
             DataTable ViewEntryList = CWeb.UserViewEntriesByTime(LoggedInUser, LoggedInUser.UserID, EntryDate.ToString());
             lstViewUserEntry.DataSource = ViewEntryList;
             lstViewUserEntry.DataBind();
         }
        protected void lstPeriodSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DisplayTimePeriodEntries(DateTime.Parse(lstPeriodSelection.SelectedValue));
            }
            catch (Exception ex)
            {
               CLog.WebError("Error in Dropdown Date Change", Page.Title, LoggedInUser, ex);
            }
        }
        #endregion
    }
}
 