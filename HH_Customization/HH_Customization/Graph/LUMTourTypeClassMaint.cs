using System;
using PX.Data;
using HH_Customization.DAC;

namespace HH_Customization.Graph
{
    public class LUMTourTypeClassMaint : PXGraph<LUMTourTypeClassMaint, LUMTourTypeClass>
    {

        #region View
        public PXSelect<LUMTourTypeClass> ToryTypeClass;
        public PXSelect<LUMTourCostStructure,
            Where<LUMTourCostStructure.typeClassID, Equal<Current<LUMTourTypeClass.typeClassID>>>> CostStructures;
        #endregion

    }
}