using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PX.Objects.FA
{
    public class FADetailsExtension : PXCacheExtension<FADetails>
    {
        [PXDBText(IsUnicode = true)]
        [PXUIField(DisplayName = "Long Description")]
        public virtual string UsrLongDescription { get; set; }
        public abstract class usrLongDescription : PX.Data.BQL.BqlBool.Field<usrLongDescription> { }
    }
}
