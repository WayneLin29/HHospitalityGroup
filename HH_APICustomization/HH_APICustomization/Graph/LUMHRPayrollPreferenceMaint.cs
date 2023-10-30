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
        public PXSave<EmptyFilter> Save;
        public PXCancel<EmptyFilter> Cancel;

        public PXFilter<EmptyFilter> MasterFilter;
        [PXImport]
        public SelectFrom<LUMHRContributionTable>.View Contribution;
        [PXImport]
        public SelectFrom<LUMHRPayrollAccountMapping>.View PayrollAccountMapping;

    }

    #region Table
    [Serializable]
    [PXCacheName("Empty Filter")]
    public class EmptyFilter : IBqlTable
    { }
    #endregion

}
