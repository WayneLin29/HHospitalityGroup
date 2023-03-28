using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH_Customization.Descriptor
{
    public class DACFieldDefaultAttribute : PXEventSubscriberAttribute, IPXRowSelectedSubscriber, IPXRowUpdatedSubscriber, IPXFieldDefaultingSubscriber
    {
        public delegate void FieldDefaultDelegate(PXCache sender, PXFieldDefaultingEventArgs e);

        protected FieldDefaultDelegate _defaultFun;

        public DACFieldDefaultAttribute(FieldDefaultDelegate defaultFun)
        {
            this._defaultFun = defaultFun;
        }

        public DACFieldDefaultAttribute(Type delegateType, string delegateName)
        {
            this._defaultFun = (FieldDefaultDelegate)Delegate.CreateDelegate(typeof(FieldDefaultDelegate), delegateType, delegateName);
        }


        public void FieldDefaulting(PXCache sender, PXFieldDefaultingEventArgs e)
        {
            if (e.Row == null) return;
            _defaultFun(sender, e);
        }

        public void RowSelected(PXCache sender, PXRowSelectedEventArgs e)
        {
            if (e.Row == null || sender.GetValue(e.Row, _FieldName) != null) return;
            sender.SetDefaultExt(e.Row, _FieldName);
        }

        public void RowUpdated(PXCache sender, PXRowUpdatedEventArgs e)
        {
            sender.SetDefaultExt(e.Row, _FieldName);
        }
    }
}
