using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH_APICustomization.Entity
{
    public class CloudBed_ReservationEntity
    {
        public bool success { get; set; }
        public List<Reservation> data { get; set; }
        public int count { get; set; }
        public int total { get; set; }
    }

    public class Reservation
    {
        public string propertyID { get; set; }
        public string reservationID { get; set; }
        public string dateCreated { get; set; }
        public string dateModified { get; set; }
        public string status { get; set; }
        public int guestID { get; set; }
        public string guestName { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string adults { get; set; }
        public string children { get; set; }
        public double balance { get; set; }
        public string sourceName { get; set; }
        public string thirdPartyIdentifier { get; set; }
    }

}
