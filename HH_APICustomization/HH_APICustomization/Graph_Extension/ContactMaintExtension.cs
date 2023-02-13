using HH_APICustomization.Graph;
using HHAPICustomization.DAC;
using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using HH_APICustomization.DAC;

namespace PX.Objects.CR
{
    public class ContactMaintExtension : PXGraphExtension<ContactMaint>
    {
        public SelectFrom<LUMContactReview>
               .LeftJoin<LUMCloudBedReservations>.On<LUMContactReview.reservationID.IsEqual<LUMCloudBedReservations.reservationID>>
               .LeftJoin<LUMCloudBedPreference>.On<LUMCloudBedReservations.propertyID.IsEqual<LUMCloudBedPreference.cloudBedPropertyID>>
               .LeftJoin<LUMCloudBedRoomAssignment>.On<LUMCloudBedRoomAssignment.reservationID.IsEqual<LUMContactReview.reservationID>
                    .And<LUMCloudBedRoomAssignment.roomid.IsEqual<LUMContactReview.roomID>>>
               .Where<LUMContactReview.contactID.IsEqual<Contact.contactID.FromCurrent>>
               .View ReviewCurrent;

        #region Delegate Methods

        public delegate void PersistDelegate();
        [PXOverride]
        public void Persist(PersistDelegate baseMethod)
        {
            var doc = Base.Contact.Current;
            var reviewRow = this.ReviewCurrent.Current;
            if (doc != null && reviewRow != null && reviewRow.ReviewDate.HasValue)
            {
                if (string.IsNullOrEmpty(reviewRow?.ReservationID))
                    this.ReviewCurrent.Cache.RaiseExceptionHandling<LUMContactReview.reservationID>(reviewRow, reviewRow.ReservationID,
                        new PXSetPropertyException<LUMContactReview.reservationID>("ReservationID can not be empty!", PXErrorLevel.Error));
                if (string.IsNullOrEmpty(reviewRow?.RoomID))
                    this.ReviewCurrent.Cache.RaiseExceptionHandling<LUMContactReview.roomID>(reviewRow, reviewRow.RoomID,
                        new PXSetPropertyException<LUMContactReview.roomID>("RoomID can not be empty!", PXErrorLevel.Error));
                if (!reviewRow.Score.HasValue)
                    this.ReviewCurrent.Cache.RaiseExceptionHandling<LUMContactReview.score>(reviewRow, reviewRow.Score,
                        new PXSetPropertyException<LUMContactReview.score>("Score can not be empty!", PXErrorLevel.Error));
            }
            baseMethod();
        }

        #endregion

    }
}
