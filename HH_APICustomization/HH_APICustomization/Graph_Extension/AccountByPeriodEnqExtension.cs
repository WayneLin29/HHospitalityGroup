using PX.Data;
using PX.Objects.GL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PX.Objects.GL
{
    public class AccountByPeriodEnqExtension : PXGraphExtension<AccountByPeriodEnq>
    {
        public virtual void _(Events.RowSelected<GLTranR> e, PXRowSelected baseMethod)
        {
            baseMethod?.Invoke(e.Cache, e.Args);
            var row = e.Row;
            var netAmount = (row?.DebitAmt ?? 0) - (row?.CreditAmt ?? 0);
            var absAmount = Math.Abs(netAmount);

            e.Cache.SetValueExt<GLTranRExt.usrNetAmount>(e.Row, netAmount);
            e.Cache.SetValueExt<GLTranRExt.usrAbsAmount>(e.Row, absAmount);
        }
    }

    public class GLTranRExt : PXCacheExtension<GLTranR>
    {
        #region UsrNetAmount
        [PXDecimal]
        [PXUIField(DisplayName = "Net Amount", Enabled = false)]
        public virtual decimal? UsrNetAmount { get; set; }
        public abstract class usrNetAmount : PX.Data.BQL.BqlDecimal.Field<usrNetAmount> { }
        #endregion

        #region UsrAbsAmount
        [PXDecimal]
        [PXUIField(DisplayName = "Abs Amount", Enabled = false)]
        public virtual decimal? UsrAbsAmount { get; set; }
        public abstract class usrAbsAmount : PX.Data.BQL.BqlDecimal.Field<usrAbsAmount> { }
        #endregion
    }
}
