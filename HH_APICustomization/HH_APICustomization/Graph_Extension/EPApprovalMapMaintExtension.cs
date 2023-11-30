using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PX.Objects.EP
{
    public class EPApprovalMapMaintExtension : PXGraphExtension<EPApprovalMapMaint>
    {
        #region Overrides

        public delegate IEnumerable<string> GetEntityTypeScreensDel();

        [PXOverride]
        public virtual IEnumerable<string> GetEntityTypeScreens(GetEntityTypeScreensDel del)
        {
            foreach (string s in del())
            {
                yield return s;
            }
            yield return "LM505006";
        }
        #endregion
    }
}
