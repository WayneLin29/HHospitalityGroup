using HH_APICustomization.Graph;
using PX.Data;
using PX.Data.Webhooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using HH_APICustomization.APIHelper;
using Newtonsoft.Json;
using HHAPICustomization.DAC;
using PX.SM;
using PX.Data.BQL;

namespace HH_APICustomization.WebHook
{
    public class CloudBedWebHook : IWebhookHandler
    {
        [InjectDependency]
        internal ICurrentUserInformationProvider CurrentUserInformationProvider { get; set; }

        public async Task<System.Web.Http.IHttpActionResult> ProcessRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using (var scope = GetAdminScope())
            {
                if (request.Method == HttpMethod.Get)
                {
                    var code = HttpUtility.ParseQueryString(request.RequestUri.Query).Get("CODE");
                    // Get Access Token By Code
                    if (!string.IsNullOrEmpty(code))
                        CloudBedHelper.GetAccessToeknByCode(code);
                }
                else if (request.Method == HttpMethod.Post)
                {
                    // Cloudbed Block webhook data
                    var adminID = PXSelect<Users, Where<Users.username, Equal<Required<Users.username>>>>.Select(new PXGraph(), "admin").TopFirst?.PKID;
                    var body = request.Content.ReadAsStringAsync().Result;
                    HH_APICustomization.Entity.Blockroom.CloudBed_BlockroomEntity blockroomEntity = JsonConvert.DeserializeObject<HH_APICustomization.Entity.Blockroom.CloudBed_BlockroomEntity>(body);
                    using (PXTransactionScope sc = new PXTransactionScope())
                    {
                        int idx = 1;
                        switch (blockroomEntity?.Event?.ToUpper())
                        {
                            case "ROOMBLOCK/CREATED":
                                InsertRoomBlock(blockroomEntity, adminID);
                                idx = 1;
                                foreach (var room in blockroomEntity?.rooms)
                                    InsertRoomBlockDetails(blockroomEntity?.roomBlockID, blockroomEntity?.propertyID, idx++, room, adminID);
                                break;
                            case "ROOMBLOCK/REMOVED":
                                MarkRoomBlockDeleted(blockroomEntity?.roomBlockID, blockroomEntity?.propertyID);
                                break;
                            case "ROOMBLOCK/DETAILS_CHANGED":
                                DeleteAllRoomBlock(blockroomEntity?.roomBlockID, blockroomEntity?.propertyID);
                                InsertRoomBlock(blockroomEntity, adminID);
                                idx = 1;
                                foreach (var room in blockroomEntity?.rooms)
                                    InsertRoomBlockDetails(blockroomEntity?.roomBlockID, blockroomEntity?.propertyID, idx++, room, adminID);
                                break;
                        }
                        sc.Complete();
                    }
                }
            }
            return new OkResult(request);
        }

        private IDisposable GetAdminScope()
        {
            var userName = "admin";
            if (PXDatabase.Companies.Length > 0)
            {
                var company = PXAccess.GetCompanyName();
                if (string.IsNullOrEmpty(company))
                {
                    company = PXDatabase.Companies[0];
                }
                userName = userName + "@" + company;
            }
            return new PXLoginScope(userName);
        }

        /// <summary> Insert Data int LUMCloudBedRoomBlock </summary>
        private void InsertRoomBlock(HH_APICustomization.Entity.Blockroom.CloudBed_BlockroomEntity entity, Guid? adminID)
        {
            List<PXDataFieldAssign> assigns = new List<PXDataFieldAssign>();
            assigns.Add(new PXDataFieldAssign<LUMCloudBedRoomBlock.roomBlockID>(entity?.roomBlockID));
            assigns.Add(new PXDataFieldAssign<LUMCloudBedRoomBlock.propertyID>(entity?.propertyID));
            assigns.Add(new PXDataFieldAssign<LUMCloudBedRoomBlock.roomBlockType>(entity?.roomBlockType));
            assigns.Add(new PXDataFieldAssign<LUMCloudBedRoomBlock.roomBlockReason>(entity?.roomBlockReason));
            assigns.Add(new PXDataFieldAssign<LUMCloudBedRoomBlock.startDate>(entity?.startDate));
            assigns.Add(new PXDataFieldAssign<LUMCloudBedRoomBlock.endDate>(entity?.endDate));
            assigns.Add(new PXDataFieldAssign<LUMCloudBedRoomBlock.createdByID>(adminID));
            assigns.Add(new PXDataFieldAssign<LUMCloudBedRoomBlock.createdDateTime>(DateTime.Now));
            assigns.Add(new PXDataFieldAssign<LUMCloudBedRoomBlock.createdByScreenID>("SM304000"));
            assigns.Add(new PXDataFieldAssign<LUMCloudBedRoomBlock.lastModifiedByID>(adminID));
            assigns.Add(new PXDataFieldAssign<LUMCloudBedRoomBlock.lastModifiedDateTime>(DateTime.Now));
            assigns.Add(new PXDataFieldAssign<LUMCloudBedRoomBlock.lastModifiedByScreenID>("SM304000"));
            assigns.Add(new PXDataFieldAssign<LUMCloudBedRoomBlock.noteid>(Guid.NewGuid()));
            PXDatabase.Insert<LUMCloudBedRoomBlock>(assigns.ToArray());
        }

        /// <summary> Insert Data int LUMCloudBedRoomBlockDetails </summary>
        private void InsertRoomBlockDetails(string blockID, string propertyID, int idx, Entity.Blockroom.Room room, Guid? adminID)
        {
            List<PXDataFieldAssign> assigns = new List<PXDataFieldAssign>();
            assigns.Add(new PXDataFieldAssign<LUMCloudBedRoomBlockDetails.roomBlockID>(blockID));
            assigns.Add(new PXDataFieldAssign<LUMCloudBedRoomBlockDetails.propertyID>(propertyID));
            assigns.Add(new PXDataFieldAssign<LUMCloudBedRoomBlockDetails.lineNbr>(idx));
            assigns.Add(new PXDataFieldAssign<LUMCloudBedRoomBlockDetails.roomid>(room?.roomID));
            assigns.Add(new PXDataFieldAssign<LUMCloudBedRoomBlockDetails.roomTypeID>(room?.roomTypeID));
            assigns.Add(new PXDataFieldAssign<LUMCloudBedRoomBlockDetails.createdByID>(adminID));
            assigns.Add(new PXDataFieldAssign<LUMCloudBedRoomBlockDetails.createdDateTime>(DateTime.Now));
            assigns.Add(new PXDataFieldAssign<LUMCloudBedRoomBlockDetails.createdByScreenID>("SM304000"));
            assigns.Add(new PXDataFieldAssign<LUMCloudBedRoomBlockDetails.lastModifiedByID>(adminID));
            assigns.Add(new PXDataFieldAssign<LUMCloudBedRoomBlockDetails.lastModifiedDateTime>(DateTime.Now));
            assigns.Add(new PXDataFieldAssign<LUMCloudBedRoomBlockDetails.lastModifiedByScreenID>("SM304000"));
            assigns.Add(new PXDataFieldAssign<LUMCloudBedRoomBlockDetails.noteid>(Guid.NewGuid()));
            PXDatabase.Insert<LUMCloudBedRoomBlockDetails>(assigns.ToArray());
        }

        /// <summary> Delete data from LUMCloudBedRoomBlock & LUMCloudBedRoomBlockDetails </summary>
        private void DeleteAllRoomBlock(string blockID, string propertyID)
        {
            // Delete LUMCloudBedRoomBlock
            PXDatabase.Delete<LUMCloudBedRoomBlock>(
                   new PXDataFieldRestrict<LUMCloudBedRoomBlock.roomBlockID>(blockID),
                   new PXDataFieldRestrict<LUMCloudBedRoomBlock.propertyID>(propertyID));

            // Delete LUMCloudBedRoomBlockDetails
            PXDatabase.Delete<LUMCloudBedRoomBlockDetails>(
                  new PXDataFieldRestrict<LUMCloudBedRoomBlockDetails.roomBlockID>(blockID),
                  new PXDataFieldRestrict<LUMCloudBedRoomBlockDetails.propertyID>(propertyID));
        }

        private void MarkRoomBlockDeleted(string blockID, string propertyID)
        {
            PXDatabase.Update<LUMCloudBedRoomBlock>(
                new PXDataFieldAssign<LUMCloudBedRoomBlock.isRemoved>(true),
                new PXDataFieldRestrict<LUMCloudBedRoomBlockDetails.roomBlockID>(blockID),
                new PXDataFieldRestrict<LUMCloudBedRoomBlockDetails.propertyID>(propertyID));
        }
    }
}
