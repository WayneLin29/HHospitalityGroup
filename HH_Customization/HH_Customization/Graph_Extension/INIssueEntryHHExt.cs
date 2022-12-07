using PX.Data;
using System.Collections;
using PX.Objects.GL;

namespace PX.Objects.IN
{
    public class INIssueEntryHHExt : PXGraphExtension<INIssueEntry>
    {
        #region Action
        public PXAction<INRegister> viewProjectBatch;
        [PXUIField(DisplayName = "", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXButton]
        public virtual IEnumerable ViewProjectBatch(PXAdapter adapter)
        {
            EntityHelper helper = new EntityHelper(Base);
            var doc = Base.issue.Current;
            string projectBatchNbr = doc.GetExtension<INRegisterHHExt>().UsrProjectBatchNbr;
            var projectBatch = Batch.PK.Find(Base, BatchModule.GL, projectBatchNbr);
            if (projectBatch != null)
            {
                helper.NavigateToRow(projectBatch.NoteID.Value, PXRedirectHelper.WindowMode.InlineWindow);
            }
            return adapter.Get();
        }

        #endregion

        #region Event
        protected virtual void _(Events.RowSelected<INRegister> e)
        {
            if (e.Row == null) return;
            var rowExt = e.Row.GetExtension<INRegisterHHExt>();
            PXUIFieldAttribute.SetReadOnly<INTran.reasonCode>(Base.transactions.Cache, null, rowExt.UsrReasonCode != null);
        }

        protected virtual void _(Events.FieldUpdated<INRegister, INRegisterHHExt.usrReasonCode> e)
        {
            if (e.Row == null) return;
            //var rowExt = e.Row.GetExtension<INRegisterHHExt>();
            foreach (var item in Base.transactions.Select())
            {
                Base.transactions.Cache.SetDefaultExt<INTran.reasonCode>(item);
            }
        }

        #endregion

        #region CacheAttached
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXDefault(typeof(INRegisterHHExt.usrReasonCode), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void _(Events.CacheAttached<INTran.reasonCode> e) { }
        #endregion
    }
}
