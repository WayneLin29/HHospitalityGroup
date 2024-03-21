using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Data;

namespace PX.Objects.EP
{
    public class EPEmployeeHHExt : PXCacheExtension<EPEmployee>
    {
        #region UsrLineCntr
        [PXDBInt()]
        [PXUIField(DisplayName = "Line Cntr")]
        [PXDefault(0, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual int? UsrLineCntr { get; set; }
        public abstract class usrLineCntr : PX.Data.BQL.BqlInt.Field<usrLineCntr> { }
        #endregion
    }
}
