using PX.CS;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH_APICustomization.Descriptor
{
    public class LUMDDLAttribute : PXStringListAttribute
    {
        private string _attributeID = string.Empty;
        public LUMDDLAttribute() : base() { }
        public LUMDDLAttribute(string _id)
        {
            this._attributeID = _id;
        }

        public override void CacheAttached(PXCache sender)
        {
            base.CacheAttached(sender);
            var data = SelectFrom<CSAttributeDetail>
                       .Where<CSAttributeDetail.attributeID.IsEqual<P.AsString>>
                       .View.Select(new PXGraph(), this._attributeID).RowCast<CSAttributeDetail>();
            if (data != null)
            {
                this._AllowedLabels = data.Select(x => x.Description).ToArray();
                this._AllowedValues = data.Select(x => x.ValueID).ToArray();
            }
        }
    }

    public class LUMRemitStatus
    {
        public class ListAttribute : PXStringListAttribute
        {
            public ListAttribute() : base(
                new[]
                {
                    Pair(Hold, "On Hold"),
                    Pair(PendingApproval, "Pending Approval"),
                    Pair(Open, "Open"),
                    Pair(Released, "Rleased"),
                    Pair(Voided, "Voided"),
                    Pair(Rejected, "Rejected"),
                })
            { }
        }

        public class ListWithoutOrdersAttribute : PXStringListAttribute
        {
            public ListWithoutOrdersAttribute() : base(
                new[]
                {

                    Pair(Hold, "On Hold"),
                    Pair(PendingApproval, "Pending Approval"),
                    Pair(Open, "Open"),
                    Pair(Released, "Rleased"),
                    Pair(Voided, "Voided"),
                    Pair(Rejected, "Rejected"),
                })
            { }
        }

        public const string Hold = "H";
        public const string PendingApproval = "P";
        public const string Voided = "V";
        public const string Open = "O";
        public const string Released = "R";
        public const string Rejected = "J";

        public class voided : PX.Data.BQL.BqlString.Constant<voided>
        {
            public voided() : base(Voided) { }
        }
        public class pendingApproval : PX.Data.BQL.BqlString.Constant<pendingApproval>
        {
            public pendingApproval() : base(PendingApproval) { }
        }
        public class hold : PX.Data.BQL.BqlString.Constant<hold>
        {
            public hold() : base(Hold) { }
        }
        public class open : PX.Data.BQL.BqlString.Constant<open>
        {
            public open() : base(Open) { }
        }
        public class released : PX.Data.BQL.BqlString.Constant<released>
        {
            public released() : base(Released) { }
        }
        public class rejected : PX.Data.BQL.BqlString.Constant<rejected>
        {
            public rejected() : base(Rejected) { }
        }
    }
}
