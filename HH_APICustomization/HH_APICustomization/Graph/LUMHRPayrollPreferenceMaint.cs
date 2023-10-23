using HH_APICustomization.DAC;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH_APICustomization.Graph
{
    /// <summary>
    /// LM201003
    /// </summary>
    public class LUMHRPayrollPreferenceMaint : PXGraph<LUMHRPayrollPreferenceMaint>
    {
        public PXSave<LUMHRContributionTable> Save;
        public PXCancel<LUMHRContributionTable> Cancel;

        public SelectFrom<LUMHRContributionTable>.View Contribution;
        public SelectFrom<LUMHRPayrollAccountMapping>.View PayrollAccountMapping;

        #region Event
        public virtual void _(Events.RowSelected<LUMHRContributionTable> e)
        {
            var attrMethod = SelectFrom<PX.Objects.CS.CSAttributeDetail>
                            .Where<PX.Objects.CS.CSAttributeDetail.attributeID.IsEqual<P.AsString>>
                            .View.Select(this, "CALEMETHOD").RowCast<PX.Objects.CS.CSAttributeDetail>();

            PXStringListAttribute.SetList<LUMHRContributionTable.method>(base.Caches[typeof(LUMHRContributionTable)], null, attrMethod.Select(x => x?.ValueID).ToArray(), attrMethod.Select(x => x?.Description).ToArray());
        }
        #endregion
    }
}
