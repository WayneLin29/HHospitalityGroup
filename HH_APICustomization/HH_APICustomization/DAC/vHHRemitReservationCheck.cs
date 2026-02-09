using System;
using PX.Data;

namespace HH_APICustomization.DAC
{
    [Serializable]
    [PXCacheName("vHHRemitReservationCheck")]
    public class vHHRemitReservationCheck : PXBqlTable, IBqlTable
    {
        #region CheckID
        [PXDBString(320, IsUnicode = true, IsKey = true)]
        [PXUIField(DisplayName = "Check ID", Visible = false)]
        public virtual string CheckID { get; set; }
        public abstract class checkID : PX.Data.BQL.BqlString.Field<checkID> { }
        #endregion

        #region PropertyID
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Property ID")]
        public virtual string PropertyID { get; set; }
        public abstract class propertyID : PX.Data.BQL.BqlString.Field<propertyID> { }
        #endregion

        #region Type
        [PXDBString(2, InputMask = "")]
        [PXUIField(DisplayName = "Type")]
        public virtual string Type { get; set; }
        public abstract class type : PX.Data.BQL.BqlString.Field<type> { }
        #endregion

        #region ReservationID
        [PXDBString(269, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Reservation ID")]
        public virtual string ReservationID { get; set; }
        public abstract class reservationID : PX.Data.BQL.BqlString.Field<reservationID> { }
        #endregion

        #region Message
        [PXDBString(1149, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Message")]
        public virtual string Message { get; set; }
        public abstract class message : PX.Data.BQL.BqlString.Field<message> { }
        #endregion

        #region SortBy
        [PXDBInt]
        [PXUIField(DisplayName = "Sort By")]
        public virtual int? SortBy { get; set; }
        public abstract class sortBy : PX.Data.BQL.BqlInt.Field<sortBy> { }
        #endregion
    }
}