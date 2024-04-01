using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.GL;

namespace HH_APICustomization.Descriptor
{
    public class HHHelper
    {
        public Ledger GetLedgerInfo(string cd)
            => SelectFrom<Ledger>
              .Where<Ledger.ledgerCD.IsEqual<P.AsString>>
              .View.Select(new PX.Data.PXGraph(), cd).TopFirst;
    }
}
