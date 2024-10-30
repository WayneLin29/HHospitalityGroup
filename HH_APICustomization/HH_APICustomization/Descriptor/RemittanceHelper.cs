using HH_APICustomization.APIHelper;
using HH_APICustomization.DAC;
using PX.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Management;

namespace HH_APICustomization.Descriptor
{
    public class RemittanceHelper
    {

        public IEnumerable<T> GetBqlCommand<T>(PXGraph graph, BqlCommand queryCommand, params object[] parms) where T : PXBqlTable, IBqlTable
        {
            PXView view = new PXView(graph, false, queryCommand);
            return view.SelectMulti(parms).RowCast<T>();
        }

        /// <summary> 取得 待處理 Transactions </summary>
        public IEnumerable<LUMCloudBedTransactions> GetPendingTransactions(PXGraph graph, BqlCommand queryCommand, string propertyID, string refNbr)
        {
            PXView view = new PXView(graph, false, queryCommand);
            return view.SelectMulti(propertyID, refNbr).RowCast<LUMCloudBedTransactions>();
        }

        /// <summary> 取得 被排除 Transactions </summary>
        public IEnumerable<LUMRemitExcludeTransactions> GetExcludedTransactions(PXGraph graph, BqlCommand queryCommand)
        {
            PXView view = new PXView(graph, false, queryCommand);
            return view.SelectMulti().RowCast<LUMRemitExcludeTransactions>();
        }

        /// <summary> 過濾被排除的 Transactions </summary>
        public IEnumerable<LUMCloudBedTransactions> FilterExcludedTransactions(IEnumerable<LUMCloudBedTransactions> pendingTransactions, IEnumerable<LUMRemitExcludeTransactions> excludedTransactions)
        {
            return pendingTransactions.Where(x => !excludedTransactions.Any(y => y.TransactionID == x.TransactionID));
        }

        /// <summary> 取得 符合 ReservationCheck的 Transactions </summary>
        public List<LUMCloudBedTransactions> GetMatchReservationCheckTransactions(IEnumerable<LUMCloudBedTransactions> allTransactions, IEnumerable<vHHRemitReservationCheck> reservationCheckData, IEnumerable<LUMRemitReservation> existsRemitReservations)
        {
            var matchTransactions = new List<LUMCloudBedTransactions>();

            foreach (var checkResv in reservationCheckData)
            {
                if (existsRemitReservations.Any(x => x.ReservationID == checkResv.ReservationID && (x.IsOutOfScope ?? false)))
                    continue;

                // 如果有 '-' 透過HousAccountID/ReservationID尋找未處理的Transactions
                if (checkResv.ReservationID.Contains("-"))
                {
                    var houseAccountId = checkResv.ReservationID.Substring(0, checkResv.ReservationID.IndexOf('-'));
                    matchTransactions.AddRange(allTransactions.Where(x => x.HouseAccountID?.ToString() == houseAccountId));
                }
                else
                {
                    matchTransactions.AddRange(allTransactions.Where(x => x.ReservationID == checkResv.ReservationID));
                }
            }

            return matchTransactions;
        }
    }
}
