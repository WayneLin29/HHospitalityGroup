using System;
using PX.Data;

namespace HH_Customization.Descriptor
{
    public class LUMStringList
    {
        public class LUMTourLevel : PXStringListAttribute
        {

            public LUMTourLevel() : base(values, labels) { }
            public static string[] labels = { "GROUP", "SO" };
            public static string[] values = { GROUP, SO };

            public const string GROUP = "GRP";
            public class group : PX.Data.BQL.BqlString.Constant<group> { public group() : base(GROUP) { } }

            public const string SO = "SO";
            public class so : PX.Data.BQL.BqlString.Constant<so> { public so() : base(SO) { } }
        }
    }
}
