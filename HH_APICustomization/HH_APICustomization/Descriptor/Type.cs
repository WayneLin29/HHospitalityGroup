using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH_APICustomization.Descriptor
{
    public class LUMTBTransactionSummaryType {
        /// <summary> Sales By MenuItem </summary>
        public const string SALES_BY_MENUITEM = "SM";
        /// <summary> Sales By MenuItem </summary>
        public class salesByMenuItem : PX.Data.BQL.BqlString.Constant<salesByMenuItem> { public salesByMenuItem() : base(SALES_BY_MENUITEM) { } }

        /// <summary> Accounts Summary </summary>
        public const string ACCOUNTS_SUMMARY = "AS";
        /// <summary> Accounts Summary </summary>
        public class accountsSummary : PX.Data.BQL.BqlString.Constant<accountsSummary> { public accountsSummary() : base(ACCOUNTS_SUMMARY) { } }

        /// <summary> Pay Outs </summary>
        public const string PAY_OUTS = "PO";
        /// <summary> Pay Outs </summary>
        public class payOuts : PX.Data.BQL.BqlString.Constant<payOuts> { public payOuts() : base(PAY_OUTS) { } }

        /// <summary> Pay ins </summary>
        public const string PAY_INS = "PI";
        /// <summary> Pay Outs </summary>
        public class payIns : PX.Data.BQL.BqlString.Constant<payIns> { public payIns() : base(PAY_INS) { } }
    }

    public class LUMTBTransactionSummaryDirection
    {
        /// <summary> IN </summary>
        public const string IN = "IN";
        /// <summary> Sales By MenuItem </summary>
        public class iN : PX.Data.BQL.BqlString.Constant<iN> { public iN() : base(IN) { } }

        /// <summary> OT </summary>
        public const string OT = "OT";
        /// <summary> Accounts Summary </summary>
        public class oT : PX.Data.BQL.BqlString.Constant<oT> { public oT() : base(OT) { } }
    }
}
