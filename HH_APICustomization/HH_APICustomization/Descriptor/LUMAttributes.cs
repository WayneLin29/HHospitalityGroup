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
}
