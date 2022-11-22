using PX.Data;

namespace PX.Objects.IN
{
    public class PIGeneratorHHExt : PXGraphExtension<PIGenerator>
    {
        #region View
        public PXSetup<INPIClass>.Where<INPIClass.pIClassID.IsEqual<PIGeneratorSettings.pIClassID.FromCurrent>> INPIClassSetup;
        #endregion

        #region Event
        protected virtual void _(Events.FieldDefaulting<INPIDetail, INPIDetail.reasonCode> e, PXFieldDefaulting baseMethod)
        {
            if (e.Row == null) return;
            baseMethod?.Invoke(e.Cache, e.Args);
            var setup = INPIClassSetup.Current;
            var setupExt = setup?.GetExtension<INPIClassHHExt>();
            if (setupExt?.UsrReasonCode != null)
            {
                e.NewValue = setupExt.UsrReasonCode;
            }
        }
        #endregion
    }
}
