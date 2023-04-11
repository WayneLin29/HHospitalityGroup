using System.Collections;
using System.Linq;
using PX.Data;
using HH_Customization.DAC;
using static HH_Customization.Descriptor.LUMStringList;
using PX.Objects.SO;
using PX.Objects.CR;
using PX.Objects.PM;
using PX.Objects.IN;
using CM = PX.Objects.CM;
using PX.Objects.GL;
using System.Collections.Generic;
using PX.Objects.AP;
using System;
using HH_Customization.Descriptor;

namespace HH_Customization.Graph
{
    public class LUMTourGroupEntry : PXGraph<LUMTourGroupEntry, LUMTourGroup>
    {
        #region Message
        public const string CUST_GROUP_NOT_FOUND = "Customer 'GROUP' not found";
        public const string INVENTORY_PACKAGE_NOT_FOUND = "InventoryItem 'PACKAGE' not found";
        public const string PLZ_CHECK_ASSIGN = "Assignment exists for the guest, please unassign all before delete";
        public const string PLZ_SAVE_FIRST = "Please save first";
        #endregion

        #region const
        public const string DATE_FORMAT = "MMddyyyy";
        #endregion

        #region View
        public PXSelect<LUMTourGroup> Group;
        public PXSelect<LUMTourGuest,
            Where<LUMTourGuest.tourGroupNbr, Equal<Current<LUMTourGroup.tourGroupNbr>>>> Guests;
        public PXSelect<LUMTourGroupItem,
            Where<LUMTourGroupItem.tourGroupNbr, Equal<Current<LUMTourGroup.tourGroupNbr>>>> Items;
        #endregion

        #region Action
        #region Update SO
        public PXAction<LUMTourGroup> updateSO;

        [PXButton(CommitChanges = true), PXUIField(DisplayName = "Update SO", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        public IEnumerable UpdateSO(PXAdapter adapter)
        {
            this.Persist();
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
                        string soDescDate = header.DateFrom?.ToString(DATE_FORMAT) + "-" + (header.DateTo?.ToString(DATE_FORMAT) ?? "");
                        SOOrder so = new SOOrder()
                        {
                            OrderType = "IN",
                            BranchID = header.BranchID,
                            CustomerID = groupCustomer.BAccountID,
                            OrderDate = header.DateFrom,
                            RequestDate = header.DateFrom,
                            ProjectID = ProjectDefaultAttribute.NonProject()
                        };
                        so.OrderDesc = $"{header.TourGroupNbr}-{soDescDate}-{groupDate.Key.SubGroupID}";
                        entry.Document.Current = entry.Document.Insert(so);
                        isNewSO = true;
                    }
                    #endregion
                    #region 更新幣別
                    CM.CurrencyInfo curyInfo = entry.currencyinfo.Current;
                    entry.currencyinfo.Cache.SetValueExt<CM.CurrencyInfo.curyID>(curyInfo, groupDate.Key.CuryID);
                    entry.currencyinfo.Update(curyInfo);
                    entry.Document.Cache.SetValueExt<SOOrder.curyID>(entry.Document.Current, groupDate.Key.CuryID);
                    entry.Document.UpdateCurrent();
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
                            line.TranDesc = $"{header.TourGroupNbr}-{guest.SubGroupID}-{guest.NameCH}-{guest.Remark}";
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
                                item.Pax = groupList.Count;
                                item.ExtCost = item.UnitPrice * item.Pax;
                                item.AccountID = cost.AccountID;
                                item.SubID = cost.SubID;
                                item.VendorID = cost.VendorID;
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
            ReloadAmt();
            return adapter.Get();
        }
        #endregion

        #region Check AP Link
        public PXAction<LUMTourGroup> checkAPLink;
        [PXButton(CommitChanges = true), PXUIField(DisplayName = "Check AP Bill Link", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        public IEnumerable CheckAPLink(PXAdapter adapter)
        {
            using (PXTransactionScope ts = new PXTransactionScope())
            {
                var apList = Items.Select().RowCast<LUMTourGroupItem>().ToList()
                    .FindAll(d => d.APRefNbr != null && d.APDocType != null && d.APLineNbr != null);
                foreach (var item in apList)
                {
                    APTran tran = APTran.PK.Find(this, item.APDocType, item.APRefNbr, item.APLineNbr);
                    if (tran == null)
                    {
                        item.APDocType = null;
                        item.APRefNbr = null;
                        item.APLineNbr = null;
                        Items.Update(item);
                    }
                }
                this.Persist();
                ts.Complete();
            }
            ReloadAmt();
            return adapter.Get();
        }
        #endregion

        #region Create AP
        public PXAction<LUMTourGroup> createAP;

        [PXButton(CommitChanges = true), PXUIField(DisplayName = "Create AP Bill", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        public IEnumerable CreateAP(PXAdapter adapter)
        {
            ValidatInsertByGroupItem();
            LUMTourGroup header = Group.Current;
            using (PXTransactionScope ts = new PXTransactionScope())
            {
                var groupby = Items.Select().RowCast<LUMTourGroupItem>().ToList()
                                .FindAll(d => d.Selected ?? false)
                                .GroupBy(d => new { d.VendorID, d.CuryID });
                string soDescDate = header.DateFrom?.ToString(DATE_FORMAT) + "-" + (header.DateTo?.ToString(DATE_FORMAT) ?? "");
                foreach (var group in groupby)
                {
                    List<LUMTourGroupItem> groupList = group.ToList();
                    APInvoiceEntry entry = PXGraph.CreateInstance<APInvoiceEntry>();
                    APInvoice doc = new APInvoice()
                    {
                        DocType = APDocType.Invoice,
                        BranchID = header.BranchID,
                        VendorID = group.Key.VendorID,
                        InvoiceNbr = header.TourGroupNbr,
                        DocDesc = $"{header.TourGroupNbr}-{soDescDate}-{header.Description}"
                    };
                    doc = entry.Document.Current = entry.Document.Insert(doc);

                    #region 更新幣別
                    CM.Extensions.CurrencyInfo curyInfo = entry.currencyinfo.Current;
                    entry.currencyinfo.Cache.SetValueExt<CM.Extensions.CurrencyInfo.curyID>(curyInfo, group.Key.CuryID);
                    entry.currencyinfo.Update(curyInfo);
                    entry.Document.Cache.SetValueExt<APInvoice.curyID>(doc, group.Key.CuryID);
                    doc = entry.Document.Update(doc);
                    #endregion
                    entry.Save.Press();
                    foreach (LUMTourGroupItem groupItem in groupList)
                    {
                        APTran tran = entry.Transactions.Insert(new APTran());
                        tran.InventoryID = groupItem.InventoryID;
                        tran.AccountID = groupItem.AccountID;
                        tran.SubID = groupItem.SubID;
                        tran.CuryLineAmt = groupItem.ExtCost;
                        tran.TranDesc = $"{header.TourGroupNbr}-{groupItem.Date?.ToString(DATE_FORMAT) ?? ""}-{groupItem.Description}";
                        entry.Transactions.Update(tran);

                        groupItem.APRefNbr = doc.RefNbr;
                        groupItem.APDocType = doc.DocType;
                        groupItem.APLineNbr = tran.LineNbr;
                        Items.Update(groupItem);
                    }
                    entry.Save.Press();
                }
                this.Persist();
                ts.Complete();
            }
            ReloadAmt();
            Items.Cache.Clear();
            return adapter.Get();
        }
        #endregion
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

        protected virtual void _(Events.FieldUpdated<LUMTourGroup, LUMTourGroup.dateFrom> e)
        {
            if (e.Row == null) return;
            foreach (var guest in Guests.Select())
            {
                Guests.Cache.SetDefaultExt<LUMTourGuest.age>(guest);
            }
        }

        #region LUMTourGuest
        protected virtual void _(Events.RowDeleting<LUMTourGuest> e)
        {
            if (e.Row == null) return;
            int assignCount = GetGuestLink(e.Row.TourGuestID).Count;
            if (assignCount > 0)
                throw new PXException(PLZ_CHECK_ASSIGN, PXErrorLevel.RowError);
        }

        protected virtual void _(Events.RowPersisted<LUMTourGuest> e)
        {
            if (e.Row == null) return;
            if (e.Operation == PXDBOperation.Delete)
                DeleteLink(e.Row);
            Group.Cache.SetDefaultExt<LUMTourGroup.revenuePHP>(Group.Current);
        }

        protected virtual void _(Events.FieldUpdated<LUMTourGuest, LUMTourGuest.birthDay> e)
        {
            if (e.Row == null) return;
            e.Cache.SetDefaultExt<LUMTourGuest.age>(e.Row);
        }
        #endregion

        #region LUMTourGroupItem
        protected virtual void _(Events.RowSelected<LUMTourGroupItem> e)
        {
            if (e.Row == null) return;
            bool hasAP = e.Row.APDocType != null && e.Row.APRefNbr != null && e.Row.APLineNbr != null;
            PXUIFieldAttribute.SetEnabled<LUMTourGroupItem.selected>(e.Cache, e.Row, !hasAP);
            PXUIFieldAttribute.SetEnabled<LUMTourGroupItem.extCost>(e.Cache, e.Row, !hasAP);
        }

        protected virtual void _(Events.RowDeleting<LUMTourGroupItem> e)
        {
            if (e.Row == null) return;
            if (e.Row.APRefNbr != null && e.Row.APDocType != null && e.Row.APLineNbr != null)
                throw new PXException($"Please delete AP Bill before delete the item", PXErrorLevel.RowError);
        }

        protected virtual void _(Events.FieldUpdated<LUMTourGroupItem, LUMTourGroupItem.inventoryID> e)
        {
            if (e.Row == null) return;
            e.Cache.SetDefaultExt<LUMTourGroupItem.description>(e.Row);
        }
        #endregion
        #endregion

        #region Method

        public virtual void ValidatInsertByGroupItem()
        {
            foreach (LUMTourGroupItem item in Items.Select())
            {
                if (Items.Cache.GetStatus(item) == PXEntryStatus.Inserted)
                    throw new PXException(PLZ_SAVE_FIRST);
            }
        }

        public virtual void ReloadAmt()
        {
            LUMTourGroup current = Group.Current;
            Group.Cache.SetDefaultExt<LUMTourGroup.revenuePHP>(current);
            Group.Cache.SetDefaultExt<LUMTourGroup.costPHP>(current);
            Group.Cache.SetDefaultExt<LUMTourGroup.grossProfitPHP>(current);
            Group.Cache.SetDefaultExt<LUMTourGroup.grossProfitPer>(current);
        }

        public virtual void DeleteLink(LUMTourGuest row)
        {
            if (row.SOOrderNbr == null || row.SOOrderType == null || row.SOLineNbr == null) return;
            SOOrderEntry entry = PXGraph.CreateInstance<SOOrderEntry>();
            entry.Document.Current = SOOrder.PK.Find(entry, row.SOOrderType, row.SOOrderNbr);
            SOLine line = SOLine.PK.Find(this, row.SOOrderType, row.SOOrderNbr, row.SOLineNbr);
            entry.Transactions.Delete(line);
            entry.Save.Press();
        }

        protected virtual void SetUI(LUMTourGroup row)
        {
            updateSO.SetEnabled(false);
            checkAPLink.SetEnabled(false);
            createAP.SetEnabled(false);
            if (Group.Cache.GetStatus(row) != PXEntryStatus.Inserted)
            {
                PXUIFieldAttribute.SetEnabled<LUMTourGroup.tourTypeClassID>(Group.Cache, row, false);
                PXUIFieldAttribute.SetEnabled<LUMTourGroup.branchID>(Group.Cache, row, false);
                updateSO.SetEnabled(true);
                checkAPLink.SetEnabled(true);
                createAP.SetEnabled(true);
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
        protected virtual List<LUMTourItem> GetItem(string orderNbr, string orderType)
        {
            return PXSelect<LUMTourItem,
               Where<LUMTourItem.sOOrderNbr, Equal<Required<LUMTourItem.sOOrderNbr>>,
               And<LUMTourItem.sOOrderType, Equal<Required<LUMTourItem.sOOrderType>>>>>
           .Select(this, orderNbr, orderType).RowCast<LUMTourItem>().ToList();
        }

        protected virtual List<LUMTourReservation> GetReservation(string orderNbr, string orderType)
        {
            return PXSelect<LUMTourReservation,
               Where<LUMTourReservation.sOOrderNbr, Equal<Required<LUMTourReservation.sOOrderNbr>>,
               And<LUMTourReservation.sOOrderType, Equal<Required<LUMTourReservation.sOOrderType>>>>>
           .Select(this, orderNbr, orderType).RowCast<LUMTourReservation>().ToList();
        }

        protected virtual List<LUMTourFlight> GetFlight(string orderNbr, string orderType)
        {
            return PXSelect<LUMTourFlight,
               Where<LUMTourFlight.sOOrderNbr, Equal<Required<LUMTourFlight.sOOrderNbr>>,
               And<LUMTourFlight.sOOrderType, Equal<Required<LUMTourFlight.sOOrderType>>>>>
           .Select(this, orderNbr, orderType).RowCast<LUMTourFlight>().ToList();
        }

        protected virtual List<LUMTourGroupItem> GetGroupItem(string groupNbr)
        {
            return PXSelect<LUMTourGroupItem,
                Where<LUMTourGroupItem.tourGroupNbr, Equal<Required<LUMTourGroupItem.tourGroupNbr>>>>
           .Select(this, groupNbr).RowCast<LUMTourGroupItem>().ToList();
        }

        protected virtual List<LUMTourGuest> GetGuest(string groupNbr)
        {
            return PXSelect<LUMTourGuest,
                Where<LUMTourGuest.tourGroupNbr, Equal<Required<LUMTourGroup.tourGroupNbr>>>>
            .Select(this, groupNbr).RowCast<LUMTourGuest>().ToList();
        }

        protected virtual PXResultset<LUMTourCostStructure> GetCostStructure(int? typeClassID)
        {
            return PXSelect<LUMTourCostStructure
                , Where<LUMTourCostStructure.typeClassID, Equal<Required<LUMTourCostStructure.typeClassID>>>>
                .Select(this, typeClassID);
        }

        protected virtual List<LUMTourGuestLink> GetGuestLink(int? guestID)
        {
            return PXSelect<LUMTourGuestLink,
                Where<LUMTourGuestLink.guestID, Equal<Required<LUMTourGuestLink.guestID>>>>
                .Select(this, guestID).RowCast<LUMTourGuestLink>().ToList();
        }
        #endregion
    }
}