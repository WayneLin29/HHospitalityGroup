using System.Collections;
using System.Linq;
using PX.Data;
using HH_Customization.DAC;
using static HH_Customization.Descriptor.LUMStringList;
using PX.Objects.SO;
using PX.Objects.CR;
using PX.Objects.PM;
using PX.Objects.IN;
using PX.Objects.CM;
using PX.Objects.GL;

namespace HH_Customization.Graph
{
    public class LUMTourGroupEntry : PXGraph<LUMTourGroupEntry, LUMTourGroup>
    {
        #region Message
        public const string CUST_GROUP_NOT_FOUND = "Customer 'GROUP' not found";
        public const string INVENTORY_PACKAGE_NOT_FOUND = "InventoryItem 'PACKAGE' not found";
        #endregion

        #region View
        public PXSelect<LUMTourGroup> Group;
        public PXSelect<LUMTourGuest,
            Where<LUMTourGuest.tourGroupNbr, Equal<Current<LUMTourGroup.tourGroupNbr>>>> Guests;
        public PXSelect<LUMTourGroupItem,
            Where<LUMTourGroupItem.tourGroupNbr, Equal<Current<LUMTourGroup.tourGroupNbr>>>> Items;
        #endregion

        #region Action
        public PXAction<LUMTourGroup> updateSO;

        [PXButton(CommitChanges = true), PXUIField(DisplayName = "Update SO", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        public IEnumerable UpdateSO(PXAdapter adapter)
        {
            var header = Group.Current;
            var groupCustomer = BAccount.UK.Find(this, "GROUP");
            var inventoryPACKAGE = InventoryItem.UK.Find(this, "PACKAGE");
            if (groupCustomer == null) throw new PXException(CUST_GROUP_NOT_FOUND);
            if (inventoryPACKAGE == null) throw new PXException(INVENTORY_PACKAGE_NOT_FOUND);
            using (PXTransactionScope ts = new PXTransactionScope())
            {
                //群組化：依據subGroupID & curyID 建立SO單
                var groupBy = Guests.Select().RowCast<LUMTourGuest>().GroupBy(data => new { data.SubGroupID, data.CuryID });
                foreach (var groupDate in groupBy)
                {
                    var groupList = groupDate.ToList();
                    LUMTourGuest soItem = groupList.Find(data => data.SOOrderNbr != null && data.SOOrderType != null);
                    SOOrderEntry entry = PXGraph.CreateInstance<SOOrderEntry>();
                    SOOrderEntryHHExt entryExt = entry.GetExtension<SOOrderEntryHHExt>();
                    bool isNewSO = false;
                    #region 取得SO
                    if (soItem != null)
                    {
                        entry.Document.Current = SOOrder.PK.Find(entry, soItem.SOOrderType, soItem.SOOrderNbr);
                    }
                    else
                    {
                        SOOrder so = new SOOrder()
                        {
                            OrderType = "IN",
                            BranchID = header.BranchID,
                            CustomerID = groupCustomer.BAccountID,
                            OrderDate = header.DateFrom,
                            RequestDate = header.DateFrom,
                            ProjectID = ProjectDefaultAttribute.NonProject()
                        };
                        entry.Document.Current = entry.Document.Insert(so);
                        isNewSO = true;
                    }
                    #endregion
                    #region 更新幣別
                    CurrencyInfo curyInfo = entry.currencyinfo.Current;
                    entry.currencyinfo.Cache.SetValueExt<CurrencyInfo.curyID>(curyInfo,groupDate.Key.CuryID);
                    entry.currencyinfo.Update(curyInfo);
                    #endregion
                    entry.Save.Press();
                    #region Guest link SO
                    foreach (LUMTourGuest guest in groupList)
                    {
                        if (guest.SOOrderNbr != null && guest.SOOrderType != null && guest.SOLineNbr != null)
                        {
                            SOLine line = SOLine.PK.Find(entry, guest.SOOrderType, guest.SOOrderNbr, guest.SOLineNbr);
                            line.CuryExtPrice = guest.Total;
                            entry.Transactions.Update(line);
                        }
                        else
                        {
                            SOLine line = entry.Transactions.Insert(new SOLine());
                            line.InventoryID = inventoryPACKAGE.InventoryID;
                            line.Qty = 1;
                            line.UnitPrice = guest.Total;
                            line.CuryExtPrice = guest.Total;
                            line = entry.Transactions.Update(line);
                            guest.SOLineNbr = line.LineNbr;
                            guest.SOOrderNbr = entry.Document.Current.OrderNbr;
                            guest.SOOrderType = entry.Document.Current.OrderType;
                            Guests.Update(guest);
                        }
                    }
                    #endregion

                    if (isNewSO)
                    {
                        foreach (LUMTourCostStructure cost in GetCostStructure(header.TourTypeClassID))
                        {
                            if (cost.Level == LUMTourLevel.SO)
                            {
                                LUMTourItem item = entryExt.Items.Insert(new LUMTourItem());
                                item.InventoryID = cost.InventoryID;
                                Branch branch = Branch.PK.Find(this, header.BranchID);
                                var inventory = InventoryItemCurySettings.PK.Find(this, cost.InventoryID, branch.BaseCuryID);
                                item.UnitPrice = inventory.BasePrice;
                                item.ExtCost = cost.ExtCost;
                                item.AccountID = cost.AccountID;
                                item.SubID = cost.SubID;
                                item.VendorID = cost.VendorID;
                                item.Pax = groupList.Count;
                                entryExt.Items.Update(item);

                                #region Link Item
                                foreach (LUMTourGuest guest in groupList)
                                {
                                    entryExt.LinkItemTypes.Insert(new LUMTourGuestLinkItem()
                                    {
                                        GuestID = guest.TourGuestID,
                                    });
                                }
                                #endregion
                            }
                        }
                    }
                    entry.Save.Press();
                }
                this.Persist();
                ts.Complete();
            }
            return adapter.Get();
        }
        #endregion

        #region Event
        protected virtual void _(Events.RowPersisting<LUMTourGroup> e)
        {
            if (e.Row == null) return;
            if (e.Operation == PXDBOperation.Insert) LinkTypeItem2GroupItem(e.Row);
        }

        protected virtual void _(Events.RowSelected<LUMTourGroup> e)
        {
            if (e.Row == null) return;
            SetUI(e.Row);
        }

        #region LUMTourGuest
        protected virtual void _(Events.FieldUpdated<LUMTourGuest, LUMTourGuest.birthDay> e)
        {
            if (e.Row == null) return;
            e.Cache.SetDefaultExt<LUMTourGuest.age>(e.Row);
        }
        #endregion

        #region LUMTourGroupItem
        protected virtual void _(Events.FieldUpdated<LUMTourGroupItem, LUMTourGroupItem.inventoryID> e)
        {
            if (e.Row == null) return;
            e.Cache.SetDefaultExt<LUMTourGroupItem.description>(e.Row);
        }
        #endregion
        #endregion

        #region Method
        protected virtual void SetUI(LUMTourGroup row)
        {
            if (Group.Cache.GetStatus(row) != PXEntryStatus.Inserted)
            {
                PXUIFieldAttribute.SetEnabled<LUMTourGroup.tourTypeClassID>(Group.Cache, row, false);
                PXUIFieldAttribute.SetEnabled<LUMTourGroup.branchID>(Group.Cache, row, false);
            }
        }

        protected virtual void LinkTypeItem2GroupItem(LUMTourGroup group)
        {
            LUMTourTypeClass typeClass = LUMTourTypeClass.PK.Find(this, group.TourTypeClassID);
            foreach (LUMTourCostStructure cost in GetCostStructure(group.TourTypeClassID))
            {
                if (cost.Level == LUMTourLevel.GROUP)
                {
                    LUMTourGroupItem item = new LUMTourGroupItem();
                    item.InventoryID = cost.InventoryID;
                    item.CuryID = typeClass.CuryID;
                    item.ExtCost = cost.ExtCost;
                    item.AccountID = cost.AccountID;
                    item.SubID = cost.SubID;
                    item.VendorID = cost.VendorID;
                    Items.Insert(item);
                }
            }
        }
        #endregion

        #region BQL
        protected virtual PXResultset<LUMTourCostStructure> GetCostStructure(int? typeClassID)
        {
            return PXSelect<LUMTourCostStructure
                , Where<LUMTourCostStructure.typeClassID, Equal<Required<LUMTourCostStructure.typeClassID>>>>
                .Select(this, typeClassID);
        }
        #endregion
    }
}