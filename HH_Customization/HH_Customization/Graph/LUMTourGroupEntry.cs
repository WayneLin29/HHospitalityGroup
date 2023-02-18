using System.Collections;
using PX.Data;
using HH_Customization.DAC;

namespace HH_Customization.Graph
{
    public class LUMTourGroupEntry : PXGraph<LUMTourGroupEntry, LUMTourGroup>
    {
        #region View
        public PXSelect<LUMTourGroup> Group;
        public PXSelect<LUMTourGuset,
            Where<LUMTourGuset.tourGroupNbr, Equal<Current<LUMTourGroup.tourGroupNbr>>>> Gusets;
        public PXSelect<LUMTourGroupItem,
            Where<LUMTourGroupItem.tourGroupNbr, Equal<Current<LUMTourGroup.tourGroupNbr>>>> Items;
        #endregion

        #region Action
        public PXAction<LUMTourGroup> updateSO;

        [PXButton(CommitChanges = true), PXUIField(DisplayName = "Update SO", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        public IEnumerable UpdateSO(PXAdapter adapter)
        {
            throw new PXException($"Update SO");
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

        #region LUMTourGuset
        protected virtual void _(Events.FieldUpdated<LUMTourGuset, LUMTourGuset.birthDay> e)
        {
            if (e.Row == null) return;
            e.Cache.SetDefaultExt<LUMTourGuset.age>(e.Row);
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
                LUMTourGroupItem item = new LUMTourGroupItem();
                item.InventoryID = cost.InventoryID;
                item.CuryID = typeClass.CuryID;
                item.ExtCost = cost.ExtCost;
                item.AccountID = cost.AccountID;
                item.SubID = cost.SubID;
                item.PayAccountID = cost.PayAccountID;
                item.PaySubID = cost.PaySubID;
                Items.Insert(item);
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