using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH_APICustomization.Entity
{
    public class CloudBed_TransactionEntity
    {
        public bool success { get; set; }
        public List<Transaction> data { get; set; }
        public int count { get; set; }
        public int total { get; set; }
    }

    public class Transaction
    {
        public string propertyID { get; set; }
        public string reservationID { get; set; }
        public string subReservationID { get; set; }
        public int? houseAccountID { get; set; }
        public string houseAccountName { get; set; }
        public string guestID { get; set; }
        public string propertyName { get; set; }
        public string transactionDateTime { get; set; }
        public string transactionDateTimeUTC { get; set; }
        public string transactionModifiedDateTime { get; set; }
        public string transactionModifiedDateTimeUTC { get; set; }
        public string guestCheckIn { get; set; }
        public string guestCheckOut { get; set; }
        public string roomTypeID { get; set; }
        public string roomTypeName { get; set; }
        public string roomName { get; set; }
        public string guestName { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public string transactionCode { get; set; }
        public string notes { get; set; }
        public string quantity { get; set; }
        public double amount { get; set; }
        public string currency { get; set; }
        public string userName { get; set; }
        public string transactionType { get; set; }
        public string transactionCategory { get; set; }
        public string itemCategoryName { get; set; }
        public string transactionID { get; set; }
        public string parentTransactionID { get; set; }
        public string cardType { get; set; }
        public bool isDeleted { get; set; }
    }

   

}
