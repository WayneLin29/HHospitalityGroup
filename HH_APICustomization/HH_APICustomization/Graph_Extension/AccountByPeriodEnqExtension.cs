using PX.Data;
using PX.Objects.CS;
using PX.Objects.GL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PX.Objects.FA.FAAccrualTran;

namespace PX.Objects.GL
{
    public class AccountByPeriodEnqExtension : PXGraphExtension<AccountByPeriodEnq>
    {
        
    }

    public class GLTranRExt : PXCacheExtension<GLTranR>
    {
        #region UsrNetAmount
        [PXDecimal]
        [PXUIField(DisplayName = "Net Amount", Enabled = false)]
        [PXFormula(typeof(Sub<GLTranR.debitAmt, GLTranR.creditAmt>))]
        public virtual decimal? UsrNetAmount { get; set; }
        public abstract class usrNetAmount : PX.Data.BQL.BqlDecimal.Field<usrNetAmount> { }
        #endregion

        #region UsrAbsAmount
        [PXDecimal]
        [PXFormula(
                typeof(Switch<
                    Case<Where<Sub<GLTranR.debitAmt, GLTranR.creditAmt>, Less<decimal0>>,
                        Minus<Sub<GLTranR.debitAmt, GLTranR.creditAmt>>
                    >,
                    Sub<GLTranR.debitAmt, GLTranR.creditAmt>
                >)
            )]
        [PXUIField(DisplayName = "Abs Amount", Enabled = false)]
        public virtual decimal? UsrAbsAmount { get; set; }
        public abstract class usrAbsAmount : PX.Data.BQL.BqlDecimal.Field<usrAbsAmount> { }
        #endregion
    }
}
