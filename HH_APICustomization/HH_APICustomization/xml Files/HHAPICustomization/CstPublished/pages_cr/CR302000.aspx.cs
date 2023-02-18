using System;
using System.Drawing;
using System.Web.UI.WebControls;
using PX.Web.UI;
using PX.Data;
using PX.Objects.CR;
using PX.Objects.CR.Extensions.CRDuplicateEntities;

public partial class Page_CR302000 : PX.Web.UI.PXPage
{
	protected void Page_Init(object sender, EventArgs e)
	{
		this.Master.PopupHeight = 700;
		this.Master.PopupWidth = 920;
	}

	protected void Duplicates_RowDataBound(object sender, PXGridRowEventArgs e)
	{
		if (e.Row.DataItem == null)
			return;

		var dedupExt = this.ds.DataGraph.FindImplementation<ContactMaint.CRDuplicateEntitiesForContactGraphExt>();

		dedupExt.Highlight(e.Row.Cells, e.Row.DataItem as CRDuplicateResult);
	}
}
