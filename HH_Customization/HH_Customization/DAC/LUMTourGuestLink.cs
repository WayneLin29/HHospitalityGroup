using System;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using static HH_Customization.Descriptor.LUMStringList;

namespace HH_Customization.DAC
{
    [Serializable]
    [PXCacheName("LUMTourGuestLink")]
    public class LUMTourGuestLink : IBqlTable
    {
        #region Key
        public class PK : PrimaryKeyOf<LUMTourGuestLink>.By<linkType, linkID, guestID>
        {
            public static LUMTourGuestLink Find(PXGraph graph, string linkType, int? linkID, int? guestID) => FindBy(graph, linkType, linkID, guestID);
        }
        #endregion

        #region LinkType
        [PXDBString(6, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Link Type")]
        [LUMTourLinkType]
        public virtual string LinkType { get; set; }
        public abstract class linkType : PX.Data.BQL.BqlString.Field<linkType> { }
        #endregion

        #region LinkID
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "LinkID")]
        public virtual int? LinkID { get; set; }
        public abstract class linkID : PX.Data.BQL.BqlInt.Field<linkID> { }
        #endregion

        #region GuestID
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "Guest ID")]
        public virtual int? GuestID { get; set; }
        public abstract class guestID : PX.Data.BQL.BqlInt.Field<guestID> { }
        #endregion
    }

    [PXCacheName("LUMTourGuestLinkItem")]
    public class LUMTourGuestLinkItem : LUMTourGuestLink
    {
        [PXDBString(6, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Link Type")]
        [LUMTourLinkType]
        [PXDefault(LUMTourLinkType.ITEM)]
        public override string LinkType { get; set; }

        #region LinkID
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "LinkID")]
        [PXDBDefault(typeof(LUMTourItem.itemID))]
        [PXParent(typeof(Select<LUMTourItem, Where<LUMTourItem.itemID, Equal<Current<linkID>>>>))]
        public override int? LinkID { get; set; }
        #endregion

    }

    [PXCacheName("LUMTourGuestLinkFlight")]
    public class LUMTourGuestLinkFlight : LUMTourGuestLink
    {
        [PXDBString(6, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Link Type")]
        [LUMTourLinkType]
        [PXDefault(LUMTourLinkType.FLIGHT)]
        public override string LinkType { get; set; }

        #region LinkID
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "LinkID")]
        [PXDBDefault(typeof(LUMTourFlight.fligthID))]
        [PXParent(typeof(Select<LUMTourFlight, Where<LUMTourFlight.fligthID, Equal<Current<linkID>>>>))]
        public override int? LinkID { get; set; }
        #endregion

    }
}