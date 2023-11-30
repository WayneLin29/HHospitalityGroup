using System;
using PX.Data;

namespace HH_APICustomization.DAC
{
    [Serializable]
    [PXCacheName("vHHRemitReservationCheck")]
    public class vHHRemitReservationCheck : IBqlTable
    {
        #region PropertyID
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Property ID")]
        public virtual string PropertyID { get; set; }
        public abstract class propertyID : PX.Data.BQL.BqlString.Field<propertyID> { }
        #endregion

        #region Type
        [PXDBString(1, InputMask = "")]
        [PXUIField(DisplayName = "Type")]
        public virtual string Type { get; set; }
        public abstract class type : PX.Data.BQL.BqlString.Field<type> { }
        #endregion

        #region ReservationID
        [PXDBString(269, IsUnicode = true, InputMask = "", IsKey = true)]
        [PXUIField(DisplayName = "Reservation ID")]
        public virtual string ReservationID { get; set; }
        public abstract class reservationID : PX.Data.BQL.BqlString.Field<reservationID> { }
        #endregion

        #region Message
        [PXDBString(1149, IsUnicode = true, InputMask = "", IsKey = true)]
        [PXUIField(DisplayName = "Message")]
        public virtual string Message { get; set; }
        public abstract class message : PX.Data.BQL.BqlString.Field<message> { }
        #endregion
    }
}