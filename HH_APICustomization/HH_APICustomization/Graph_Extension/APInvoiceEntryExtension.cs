using HH_APICustomization.Graph;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.CR;
using PX.Objects.GL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PX.Objects.CS.BranchMaint;

namespace PX.Objects.AP
{
    public class APInvoiceEntryExtension : PXGraphExtension<APInvoiceEntry>
    {

        public virtual void _(Events.FieldDefaulting<APTran, APTranExtension.usrORBranch2> e)
        {
            if (e.Row != null)
            {
                var branchInfo = Branch.PK.Find(Base, e.Row?.BranchID);
                var acctInfo = BAccount.PK.Find(Base, branchInfo?.BAccountID);
                var locationInfo = Location.PK.Find(Base, acctInfo?.BAccountID, acctInfo?.DefLocationID);
                e.NewValue = locationInfo?.CAvalaraExemptionNumber;
            }
        }

    }
}
