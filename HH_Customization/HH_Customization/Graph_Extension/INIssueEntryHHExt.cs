using PX.Data;

namespace PX.Objects.IN
{
    public class INIssueEntryHHExt : PXGraphExtension<INIssueEntry>
    {
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
