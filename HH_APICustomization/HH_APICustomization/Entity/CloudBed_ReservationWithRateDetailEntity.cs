using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH_APICustomization.Entity
{
    public class CloudBed_ReservationWithRateDetailEntity
    {
        public bool success { get; set; }
        public List<ReservationRateDetail> data { get; set; }
        public int count { get; set; }
        public int total { get; set; }
    }

    public class BalanceDetailed
    {
        public string suggestedDeposit { get; set; }
        public double subTotal { get; set; }
        public string additionalItems { get; set; }
        public string taxesFees { get; set; }
        public string grandTotal { get; set; }
        public string paid { get; set; }
    }

    public class ReservationRateDetail
    {
        public string reservationID { get; set; }
        public bool isDeleted { get; set; }
        public string dateCreated { get; set; }
        public string dateCreatedUTC { get; set; }
        public string dateModified { get; set; }
        public string dateModifiedUTC { get; set; }
        public string dateCancelled { get; set; }
        public string dateCancelledUTC { get; set; }
        public string status { get; set; }
        public string reservationCheckIn { get; set; }
        public string reservationCheckOut { get; set; }
        public string guestID { get; set; }
        public string guestName { get; set; }
        public string guestCountry { get; set; }
        public string sourceName { get; set; }
        public Source source { get; set; }
        public string sourceCategory { get; set; }
        public string sourceReservationID { get; set; }
        public string propertyCurrency { get; set; }
        public BalanceDetailed balanceDetailed { get; set; }
        public JObject detailedRates { get; set; }
        public List<Room> rooms { get; set; }
    }

    public class Room
    {
        public string roomTypeID { get; set; }
        public string roomTypeName { get; set; }
        public string subReservationID { get; set; }
        public string guestID { get; set; }
        public string guestName { get; set; }
        public int rateID { get; set; }
        public string rateName { get; set; }
        public string adults { get; set; }
        public string children { get; set; }
        public string roomID { get; set; }
        public string roomCheckIn { get; set; }
        public string roomCheckOut { get; set; }
        public string roomStatus { get; set; }
        public JObject detailedRoomRates { get; set; }
        public object detailedRoomRateNames { get; set; }
        public string roomName { get; set; }
    }

    public class Source
    {
        public string name { get; set; }
        public string paymentCollect { get; set; }
        public string category { get; set; }
    }

}
