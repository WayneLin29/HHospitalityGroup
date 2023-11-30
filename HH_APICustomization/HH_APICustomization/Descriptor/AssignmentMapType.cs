using HH_APICustomization.DAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH_APICustomization.Descriptor
{
    public static class AssignmentMapType
    {
        public class AssignmentMapTypeRemit : PX.Data.BQL.BqlString.Constant<AssignmentMapTypeRemit>
        {
            public AssignmentMapTypeRemit() : base(typeof(LUMRemittance).FullName) { }
        }
    }
}
