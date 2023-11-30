using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH_APICustomization.DAC
{
    [Serializable]
    [PXCacheName("vRemitBlockCheck")]
    public class vRemitBlockCheck : IBqlTable
    {
        #region RoomBlockID
        [PXDBString(50, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Room Block ID")]
        public virtual string RoomBlockID { get; set; }
        public abstract class roomBlockID : PX.Data.BQL.BqlString.Field<roomBlockID> { }
        #endregion

        #region PropertyID
        [PXDBString(50, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Property ID")]
        public virtual string PropertyID { get; set; }
        public abstract class propertyID : PX.Data.BQL.BqlString.Field<propertyID> { }
        #endregion

        #region Message
        [PXDBString(1024, IsUnicode = true)]
        [PXUIField(DisplayName = "Message")]
        public virtual string Message { get; set; }
        public abstract class message : PX.Data.BQL.BqlString.Field<message> { }
        #endregion
    }
}
