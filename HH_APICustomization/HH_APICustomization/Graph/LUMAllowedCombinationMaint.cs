using HH_APICustomization.DAC;
using HHAPICustomization.DAC;
using PX.Data;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH_APICustomization.Graph
{
    public class LUMAllowedCombinationMaint : PXGraph<LUMAllowedCombinationMaint>
    {
        public PXSave<LUMHHSetup> Save;
        public PXCancel<LUMHHSetup> Cancel;

        public SelectFrom<LUMHHSetup>.View Setup;

        [PXImport(typeof(LUMAllowCombination))]
        public SelectFrom<LUMAllowCombination>.View AllowData;
    }
}
