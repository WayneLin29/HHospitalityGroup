using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Data.EP;
using PX.Data.Licensing;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PX.Objects.FA.FABookSettings.midMonthType;

namespace PX.Objects.AP
{
    public class APPaymentEntryExtension : PXGraphExtension<APPaymentEntry>
    {
        #region Release
        public delegate IEnumerable ReleaseDelegate(PXAdapter adapter);
        [PXOverride]
        public IEnumerable Release(PXAdapter adapter, ReleaseDelegate baseMethod)
        {
            baseMethod(adapter);
            var payment = Base.Document.Current;
            SendEmail();
            return adapter.Get();
        }
        #endregion

        private void SendEmail()
        {
            // Get Template
            Notification notification = PXSelect<Notification,
                             Where<Notification.name, Equal<Required<Notification.name>>>>
                             .Select(Base, "Payment Released");

            TemplateNotificationGenerator sender = TemplateNotificationGenerator.Create(Base, Base.Document.Current,
                        notification.NotificationID.Value);

            // Find All AP Creater
            var allPostNbr = Base.Adjustments.Select().RowCast<APAdjust>().Select(p => p.AdjdRefNbr).ToList();

            HashSet<Guid?> allCreatedByIDs = new HashSet<Guid?>();
            foreach (var refNbr in allPostNbr)
            {
                APInvoice aPInvoice = PXSelect<APInvoice,
                                        Where<APInvoice.refNbr, Equal<Required<APInvoice.refNbr>>>>
                                        .Select(Base, refNbr);
                if (aPInvoice != null && aPInvoice.CreatedByID != null)
                {
                    allCreatedByIDs.Add(aPInvoice.CreatedByID);
                }
            }

            foreach (var item in allCreatedByIDs)
            {
                string Mail = string.Empty;
                Contact contact = PXSelect<Contact,
                                Where<Contact.userID, Equal<Required<Contact.userID>>>>
                                .Select(Base, item)
                                .FirstOrDefault();

                if (contact?.EMail == null)
                {
                    Users user = PXSelect<Users,
                                Where<Users.pKID, Equal<Required<Users.pKID>>>>
                                .Select(Base, item)
                                .FirstOrDefault();
                    Mail = user?.Email;
                }
                else
                    Mail = contact?.EMail;

                if (!string.IsNullOrEmpty(Mail))
                {
                    sender.To = Mail;
                    sender.Send().Any();
                }
            }
        }
    }
}
