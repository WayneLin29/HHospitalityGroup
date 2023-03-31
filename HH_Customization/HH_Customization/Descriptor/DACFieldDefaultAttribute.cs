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

        /// <summary>
        /// default = true , 是否在RowSelected觸發刷新
        /// </summary>
        public bool IsRowSelected = true;

        /// <summary>
        /// default = false , 是否每次RowSelected都刷新值，否則只會在null時刷新
        /// </summary>
        public bool IsAllowRefresh = false;

        /// <summary>
        /// default = true , 是否在RowUpdated觸發刷新
        /// </summary>
        public bool IsRowUpdated = true;

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
            if (e.Row == null || !IsRowSelected) return;
            if (IsAllowRefresh || sender.GetValue(e.Row, _FieldName) != null) return;
            sender.SetDefaultExt(e.Row, _FieldName);
        }

        public void RowUpdated(PXCache sender, PXRowUpdatedEventArgs e)
        {
            if (e.Row == null || !IsRowUpdated) return;
            sender.SetDefaultExt(e.Row, _FieldName);
        }
    }
}
