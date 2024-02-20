using HH_APICustomization.DAC;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_LM505006 : PX.Web.UI.PXPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void PaymentDetail_RowDataBound(object sender, PX.Web.UI.PXGridRowEventArgs e)
    {
        var _currentRefNbr = (string)e.Row.Cells["CurrentRefNbr"]?.Value;
        var _transID = (string)e.Row.Cells["TransactionID"]?.Value;
        var _toRemit = (bool?)e.Row.Cells["ToRemit"]?.Value;
        if (_toRemit ?? false)
            e.Row.Style.CssClass = "GridAquamarine";
        else if (IsExclude(_currentRefNbr, _transID))
            e.Row.Style.CssClass = "Gridlightgoldenrodyellow";
    }

    protected void ReservationTrans_RowDataBound(object sender, PX.Web.UI.PXGridRowEventArgs e)
    {
        var _pendingCount = (int?)e.Row.Cells["PendingCount"]?.Value;
        var _toRemit = (int?)e.Row.Cells["ToRemitCount"]?.Value;
        if ((_pendingCount ?? 0) != 0)
            e.Row.Cells["ReservationID"].Style.CssClass = "Gridlightgoldenrodyellow";
        if ((_toRemit ?? 0) != 0)
            e.Row.Cells["ReservationID"].Style.CssClass = "GridAquamarine";
    }

    protected void ReservationDetail_RowDataBound(object sender, PX.Web.UI.PXGridRowEventArgs e)
    {
        var _currentRefNbr = (string)e.Row.Cells["CurrentRefNbr"]?.Value;
        var _transID = (string)e.Row.Cells["TransactionID"]?.Value;
        var _toRemit = (bool?)e.Row.Cells["ToRemit"]?.Value;
        if (_toRemit ?? false)
            e.Row.Style.CssClass = "GridAquamarine";
        else if (IsExclude(_currentRefNbr, _transID))
            e.Row.Style.CssClass = "Gridlightgoldenrodyellow";
    }

    protected bool IsExclude(string refNbr, string transID)
        => SelectFrom<LUMRemitExcludeTransactions>
           .Where<LUMRemitExcludeTransactions.refNbr.IsEqual<P.AsString>
             .And<LUMRemitExcludeTransactions.transactionID.IsEqual<P.AsString>>>
           .View.Select(new PX.Data.PXGraph(), refNbr, transID).Count > 0;

}