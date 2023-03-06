using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PX.Data;
using PX.Objects.IN;
using PX.Objects.GL;
using HH_Customization.DAC;
using HH_APICustomization.DAC;
using static HH_Customization.Descriptor.LUMStringList;

namespace PX.Objects.SO
{
    public class SOOrderEntryHHExt : PXGraphExtension<SOOrderEntry>
    {
        #region isActive
        public static bool IsActive()
        {
            return true;
        }
        #endregion

        #region View
        public PXSelect<LUMTourFlight,
            Where<LUMTourFlight.sOOrderNbr, Equal<Current<SOOrder.orderNbr>>,
            And<LUMTourFlight.sOOrderType, Equal<Current<SOOrder.orderType>>>>> Flights;

        public PXSelect<LUMTourReservation,
            Where<LUMTourReservation.sOOrderNbr, Equal<Current<SOOrder.orderNbr>>,
            And<LUMTourReservation.sOOrderType, Equal<Current<SOOrder.orderType>>>>> Reservations;

        public PXSelectReadonly<LUMTourRoomReservations,
            Where<LUMTourRoomReservations.reservationID, Equal<Current<LUMTourReservation.reservationID>>>> RoomReservations;

        public PXSelect<LUMTourItem,
            Where<LUMTourItem.sOOrderNbr, Equal<Current<SOOrder.orderNbr>>,
            And<LUMTourItem.sOOrderType, Equal<Current<SOOrder.orderType>>>>> Items;

        #region Dialog
        public PXFilter<DailogControl> DialogCtrl;

        public PXSelect<LUMTourGuestLinkDetail,
            Where<LUMTourGuestLinkDetail.sOOrderNbr, Equal<Current<SOOrder.orderNbr>>,
                And<LUMTourGuestLinkDetail.sOOrderType, Equal<Current<SOOrder.orderType>>>>> GuestLinks;

        public PXSelect<LUMTourGuestLinkItem,
            Where<LUMTourGuestLinkItem.linkType, Equal<LUMTourLinkType.item>,
                And<LUMTourGuestLinkItem.linkID, Equal<Current<LUMTourItem.itemID>>>>> LinkItemTypes;
        public PXSelect<LUMTourGuestLinkFlight,
            Where<LUMTourGuestLinkItem.linkType, Equal<LUMTourLinkType.flight>,
                And<LUMTourGuestLinkItem.linkID, Equal<Current<LUMTourFlight.fligthID>>>>> LinkFlightTypes;
        public PXSelect<LUMTourGuestLink> Links;
        #endregion

        #endregion

        #region Actions
        public PXAction<LUMTourFlight> editLinkByFlight;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "EditLink By Flight")]
        public virtual IEnumerable EditLinkByFlight(PXAdapter adapter)
        {
            LUMTourFlight current = Flights.Current;
            ShowLinkEdit(true, current.FligthID);
            return adapter.Get();
        }

        public PXAction<LUMTourItem> editLinkByItem;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "EditLink By Item")]
        public virtual IEnumerable EditLinkByItem(PXAdapter adapter)
        {
            var current = Items.Current;
            ShowLinkEdit(false, current.ItemID);
            return adapter.Get();
        }

        #endregion

        #region Event
        protected void _(Events.RowDeleting<SOLine> e)
        {
            if (e.Row == null) return;
            //find Guest
            LUMTourGuest guest = GetGuestBySO(e.Row.OrderNbr, e.Row.OrderType, e.Row.LineNbr);
            if (guest != null)
                throw new PXSetPropertyException($"Please check Tour Group(LM201000):{guest.TourGroupNbr}", PXErrorLevel.RowError);
        }

        protected void _(Events.FieldUpdated<LUMTourReservation, LUMTourReservation.reservationID> e)
        {
            if (e.Row == null) return;
            e.Cache.SetDefaultExt<LUMTourReservation.extCostCB>(e.Row);
        }

        #endregion

        #region Method
        public virtual void ShowLinkEdit(bool isFlight, int? linkID)
        {
            if (DialogCtrl.Current.IsShow != true)
            {
                SetGuestLinksSelect(isFlight, linkID);
                DialogCtrl.Current.IsShow = true;
            }
            if (GuestLinks.AskExt() == WebDialogResult.OK)
            {
                EditLink(isFlight, linkID);
            }
            GuestLinks.Cache.Clear();
            DialogCtrl.Current.IsShow = false;
        }

        public virtual void EditLink(bool isFlight, int? linkID)
        {
            var linkType = isFlight ? LUMTourLinkType.FLIGHT : LUMTourLinkType.ITEM;
            foreach (LUMTourGuestLink delete in GetLinks(linkType, linkID))
            {
                Links.Delete(delete);
            }

            List<LUMTourGuestLinkDetail> selected = GuestLinks.Cache.Cached.
                RowCast<LUMTourGuestLinkDetail>().ToList().FindAll(d => d.Selected ?? false);
            if (isFlight)
            {
                Flights.Current.Pax = selected.Count;
                Flights.UpdateCurrent();
            }
            else
            {
                Items.Current.Pax = selected.Count;
                Items.UpdateCurrent();
            }
            foreach (LUMTourGuestLinkDetail linkItem in selected)
            {
                if (isFlight)
                {
                    LUMTourGuestLinkFlight link = new LUMTourGuestLinkFlight()
                    {
                        GuestID = linkItem.TourGuestID
                    };
                    LinkFlightTypes.Insert(link);
                }
                else
                {
                    LUMTourGuestLinkItem link = new LUMTourGuestLinkItem()
                    {
                        GuestID = linkItem.TourGuestID
                    };
                    LinkItemTypes.Insert(link);
                }
            }
        }

        public virtual void SetGuestLinksSelect(bool isFlight, int? linkID)
        {
            var linkType = isFlight ? LUMTourLinkType.FLIGHT : LUMTourLinkType.ITEM;
            foreach (LUMTourGuestLinkDetail detail in GuestLinks.Select())
            {
                var link = Links.Select().RowCast<LUMTourGuestLink>().ToList()
                    .Find(d => d.GuestID == detail.TourGuestID && d.LinkID == linkID && d.LinkType == linkType);
                detail.Selected = link != null;
                GuestLinks.Update(detail);
            }
        }

        #endregion

        #region BQL
        public PXResultset<LUMTourGuestLink> GetLinks(string linkType, int? linkID)
        {
            return PXSelect<LUMTourGuestLink,
                Where<LUMTourGuestLink.linkType, Equal<Required<LUMTourGuestLink.linkType>>,
                And<LUMTourGuestLink.linkID, Equal<Required<LUMTourGuestLink.linkID>>>>>
                .Select(Base, linkType, linkID);
        }

        public PXResultset<LUMTourGuest> GetGuestBySO(string orderNbr, string orderType, int? lineNbr)
        {
            return PXSelect<LUMTourGuest,
                Where<LUMTourGuest.sOOrderNbr, Equal<Required<LUMTourGuest.sOOrderNbr>>,
                And<LUMTourGuest.sOOrderType, Equal<Required<LUMTourGuest.sOOrderType>>,
                And<LUMTourGuest.sOLineNbr, Equal<Required<LUMTourGuest.sOLineNbr>>>>>>
                .Select(Base, orderNbr, orderType, lineNbr);
        }
        #endregion

        #region Table
        [PXHidden]
        public class DailogControl : IBqlTable
        {
            #region IsShow
            [PXBool()]
            [PXUnboundDefault(false)]
            public virtual bool? IsShow { get; set; }
            public abstract class isShow : PX.Data.BQL.BqlBool.Field<isShow> { }
            #endregion
        }

        #region LUMTourGuestLinkDetail
        [PXCacheName("Tour Guest Link Detail")]
        [PXProjection(typeof(
        Select2<LUMTourGuest,
            InnerJoin<SOLine, On<LUMTourGuest.sOOrderNbr, Equal<SOLine.orderNbr>,
                And<LUMTourGuest.sOOrderType, Equal<SOLine.orderType>,
                And<LUMTourGuest.sOLineNbr, Equal<SOLine.lineNbr>>>>>
        >), Persistent = true)]
        public class LUMTourGuestLinkDetail : IBqlTable
        {
            #region Selected
            [PXBool()]
            [PXUIField(DisplayName = "Selected")]
            [PXUnboundDefault]
            public virtual bool? Selected { get; set; }
            public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
            #endregion

            #region SOOrderNbr
            [PXDBString(IsKey = true, BqlField = typeof(LUMTourGuest.sOOrderNbr))]
            [PXUIField(DisplayName = "SOOrder Nbr", IsReadOnly = true)]
            [PXSelector(typeof(SOOrder.orderNbr))]
            public virtual string SOOrderNbr { get; set; }
            public abstract class sOOrderNbr : PX.Data.BQL.BqlString.Field<sOOrderNbr> { }
            #endregion

            #region SOOrderType
            [PXDBString(IsKey = true, BqlField = typeof(LUMTourGuest.sOOrderType))]
            [PXUIField(DisplayName = "SOOrder Type", IsReadOnly = true)]
            [PXSelector(typeof(Search<SOOrderType.orderType>),
                SubstituteKey = typeof(SOOrderType.descr)
                )]

            public virtual string SOOrderType { get; set; }
            public abstract class sOOrderType : PX.Data.BQL.BqlString.Field<sOOrderType> { }
            #endregion

            #region TourGuestID
            [PXDBInt(IsKey = true, BqlField = typeof(LUMTourGuest.tourGuestID))]
            public virtual int? TourGuestID { get; set; }
            public abstract class tourGuestID : PX.Data.BQL.BqlInt.Field<tourGuestID> { }
            #endregion

            #region NameCH
            [PXDBString(BqlField = typeof(LUMTourGuest.nameCH), IsUnicode = true)]
            [PXUIField(DisplayName = "Chinese Name", IsReadOnly = true)]
            public virtual string NameCH { get; set; }
            public abstract class nameCH : PX.Data.BQL.BqlString.Field<nameCH> { }
            #endregion

            #region NameEN
            [PXDBString(BqlField = typeof(LUMTourGuest.nameEN), IsUnicode = true)]
            [PXUIField(DisplayName = "English Name", IsReadOnly = true)]
            public virtual string NameEN { get; set; }
            public abstract class nameEN : PX.Data.BQL.BqlString.Field<nameEN> { }
            #endregion


        }
        #endregion

        #region LUMTourRoomReservations
        [PXCacheName("Tour Room Reservations")]
        [PXProjection(typeof(
        Select2<LUMCloudBedReservations,
            InnerJoin<LUMCloudBedRoomRateDetails,
                On<LUMCloudBedReservations.reservationID, Equal<LUMCloudBedRoomRateDetails.reservationID>>,
            LeftJoin<LUMCloudBedRoomAssignment,
                On<LUMCloudBedRoomRateDetails.reservationID, Equal<LUMCloudBedRoomAssignment.reservationID>,
                And<LUMCloudBedRoomRateDetails.roomid, Equal<LUMCloudBedRoomAssignment.roomid>>>>>
        >))]
        public class LUMTourRoomReservations : IBqlTable
        {
            #region Key
            #region ReservationID
            [PXDBString(IsKey = true, BqlField = typeof(LUMCloudBedReservations.reservationID))]
            [PXUIField(DisplayName = "ReservationID")]
            public virtual string ReservationID { get; set; }
            public abstract class reservationID : PX.Data.BQL.BqlString.Field<reservationID> { }
            #endregion

            #region RoomID
            [PXDBString(IsKey = true, BqlField = typeof(LUMCloudBedRoomRateDetails.roomid))]
            [PXUIField(DisplayName = "Room ID")]
            public virtual string RoomID { get; set; }
            public abstract class roomID : PX.Data.BQL.BqlString.Field<roomID> { }
            #endregion

            #region RateDate
            [PXDBDate(IsKey = true, BqlField = typeof(LUMCloudBedRoomRateDetails.rateDate))]
            [PXUIField(DisplayName = "Rate Date")]
            public virtual DateTime? RateDate { get; set; }
            public abstract class rateDate : PX.Data.BQL.BqlDateTime.Field<rateDate> { }
            #endregion
            #endregion

            #region PropertyID
            [PXDBString(BqlField = typeof(LUMCloudBedReservations.propertyID))]
            [PXUIField(DisplayName = "PropertyID")]
            public virtual string PropertyID { get; set; }
            public abstract class propertyID : PX.Data.BQL.BqlString.Field<propertyID> { }
            #endregion

            #region Room
            [PXDBString(BqlField = typeof(LUMCloudBedRoomAssignment.roomName))]
            [PXUIField(DisplayName = "Room")]
            public virtual string Room { get; set; }
            public abstract class room : PX.Data.BQL.BqlString.Field<room> { }
            #endregion

            #region Type
            [PXDBString(BqlField = typeof(LUMCloudBedRoomAssignment.roomTypeName))]
            [PXUIField(DisplayName = "Type")]
            public virtual string Type { get; set; }
            public abstract class type : PX.Data.BQL.BqlString.Field<type> { }
            #endregion

            #region ExtCost
            [PXDBDecimal(BqlField = typeof(LUMCloudBedRoomRateDetails.rate))]
            [PXUIField(DisplayName = "Ext Cost")]
            public virtual decimal? ExtCost { get; set; }
            public abstract class extCost : PX.Data.BQL.BqlDecimal.Field<extCost> { }
            #endregion

            #region CheckIn
            [PXDBDate(BqlField = typeof(LUMCloudBedRoomAssignment.checkin))]
            [PXUIField(DisplayName = "Check In")]
            public virtual DateTime? CheckIn { get; set; }
            public abstract class checkIn : PX.Data.BQL.BqlDateTime.Field<checkIn> { }
            #endregion

            #region CheckOut
            [PXDBDate(BqlField = typeof(LUMCloudBedRoomAssignment.checkout))]
            [PXUIField(DisplayName = "Check Out")]
            public virtual DateTime? CheckOut { get; set; }
            public abstract class checkout : PX.Data.BQL.BqlDateTime.Field<checkout> { }
            #endregion

        }
        #endregion

        #endregion
    }
}
