using LeaveAndOvertimeCustomization.DAC;
using PX.Data;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using PX.Objects.EP;
using LeaveAndOvertimeCustomization;
using PX.Objects.FS;

public partial class Page_LM407000 : PX.Web.UI.PXPage
{
    public string holidayJson;

    protected void Page_Load(object sender, EventArgs e)
    {
        var pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        pageUrl = pageUrl.Substring(0, pageUrl.IndexOf("/LM/"));
        var result = SelectFrom<v_EmployeeHolidaySehedule>
                    .InnerJoin<EPEmployee>.On<v_EmployeeHolidaySehedule.requestEmployeeID.IsEqual<EPEmployee.bAccountID>>
                    .View.Select(new PXGraph());
        List<LumCalendaEntity> entity = new List<LumCalendaEntity>();
        foreach (PXResult<v_EmployeeHolidaySehedule, EPEmployee> item in result.Where(x => x.GetItem<v_EmployeeHolidaySehedule>().Catagory == "LEAVE"))
        {
            v_EmployeeHolidaySehedule schedule = (v_EmployeeHolidaySehedule)item;
            EPEmployee emp = (EPEmployee)item;
            entity.Add(new LumCalendaEntity()
            {
                title = $"{emp.LegalName} - {schedule.Type} - {(schedule.Duration)?.ToString("0.0")} hours",
                url = $"{pageUrl}/LM/LM301070.aspx?RefNbr={schedule.RefNbr}",
                start = schedule.StartDate.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                end = schedule.EndDate.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")
            });
        }
        holidayJson = JsonConvert.SerializeObject(entity);
        HttpResponse.RemoveOutputCacheItem("/caching/CacheForever.aspx");
    }
    public class LumCalendaEntity
    {
        public string title { get; set; }
        public string start { get; set; }
        public string url { get; set; }
        public string end { get; set; }
    }
}
