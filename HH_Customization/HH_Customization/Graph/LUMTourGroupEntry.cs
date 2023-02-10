using System;
using PX.Data;
using HH_Customization.DAC;

namespace HH_Customization.Graph
{
    public class LUMTourGroupEntry : PXGraph<LUMTourGroupEntry, LUMTourGroup>
    {

        public PXFilter<LUMTourGroup> Group;
        public PXFilter<LUMTourGuset> Gusets;
        public PXFilter<LUMTourGroupItem> Items;


    }
}