using HH_APICustomization.DAC;
using PX.Data;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH_APICustomization.Graph
{
    public class LUMCloudbedAccountMappingMaint : PXGraph<LUMCloudbedAccountMappingMaint>
    {
        public PXSave<LUMCloudBedAccountMapping> Save;
        public PXCancel<LUMCloudBedAccountMapping> Cancel;

        public PXFilter<AccountMappingFilter> AcctFilter;

        [PXImport(typeof(LUMCloudBedAccountMapping))]
        public SelectFrom<LUMCloudBedAccountMapping>
             .Where<LUMCloudBedAccountMapping.cloudBedPropertyID.IsEqual<AccountMappingFilter.cloudBedPropertyID.AsOptional>
                                                                .Or<AccountMappingFilter.cloudBedPropertyID.AsOptional.IsNull>>
             .OrderBy<LUMCloudBedAccountMapping.cloudBedPropertyID.Asc,
                      LUMCloudBedAccountMapping.type.Asc,
                      LUMCloudBedAccountMapping.accountID.Asc,
                      LUMCloudBedAccountMapping.subAccountID.Asc,
                      LUMCloudBedAccountMapping.description.Asc>.View AccountMapping;
    }
}
