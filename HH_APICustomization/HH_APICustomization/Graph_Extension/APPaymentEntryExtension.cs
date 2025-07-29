using HH_APICustomization.DAC;
using PX.Data;
using PX.Objects.CR;
using PX.Objects.EP;
using PX.SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
            if(Base.Document.Current?.DocType == APDocType.Check || Base.Document.Current?.DocType == APDocType.VoidCheck)
                SendEmail();
            return adapter.Get();
        }
        #endregion

        private void SendEmail()
        {
            APNotification apNotification = PXSelect<APNotification,
                                Where<APNotification.notificationCD, Equal<Required<APNotification.notificationCD>>>>
                                .Select(Base, "PYRelease");

            // Get Template
            Notification notification = PXSelect<Notification,
                             Where<Notification.notificationID, Equal<Required<Notification.notificationID>>>>
                             .Select(Base, apNotification?.NotificationID);

            // Find All AP
            var allPostNbr = Base.Adjustments.Select().RowCast<APAdjust>().Select(p => p.AdjdRefNbr).ToList();

            HashSet<string> allMailAddress = new HashSet<string>();

            foreach (var refNbr in allPostNbr)
            {
                APInvoice aPInvoice = PXSelect<APInvoice,
                                        Where<APInvoice.refNbr, Equal<Required<APInvoice.refNbr>>>>
                                        .Select(Base, refNbr);

                if (aPInvoice != null && aPInvoice.CreatedByID != null)
                {
                    Contact contact = PXSelect<Contact,
                                    Where<Contact.userID, Equal<Required<Contact.userID>>>>
                                    .Select(Base, aPInvoice.CreatedByID)
                                    .FirstOrDefault();

                    if (contact?.EMail == null)
                    {
                        Users user = PXSelect<Users,
                                    Where<Users.pKID, Equal<Required<Users.pKID>>>>
                                    .Select(Base, aPInvoice.CreatedByID)
                                    .FirstOrDefault();
                        allMailAddress.Add(user?.Email);
                    }
                    else
                        allMailAddress.Add(contact?.EMail);
                }

                // Find UDF Mail
                if (aPInvoice?.NoteID != null)
                {
                    APRegisterKvExt attributeMail = PXSelect<APRegisterKvExt,
                                        Where<APRegisterKvExt.recordID, Equal<Required<APRegisterKvExt.recordID>>,
                                        And<APRegisterKvExt.fieldName, Contains<Required<APRegisterKvExt.fieldName>>>>>
                                        .Select(Base, aPInvoice?.NoteID, "AttributeEMAILALSO");
                    allMailAddress.Add(attributeMail?.ValueString);
                }
            }

            // Get PY Attachment
            UploadFileMaintenance fileGraph = PXGraph.CreateInstance<UploadFileMaintenance>();
            Guid[] fileIDs = PXNoteAttribute.GetFileNotes(Base.Document.Cache, Base.Document.Current);
            var attachments = fileIDs.Select(id => fileGraph.GetFile(id)).Where(f => f != null).ToList();

            foreach (var item in allMailAddress)
            {
                TemplateNotificationGenerator sender = TemplateNotificationGenerator.Create(Base, Base.Document.Current,
                      notification.NotificationID.Value);

                sender.To = item;
                foreach (FileInfo file in attachments)
                {
                    sender.AddAttachment(file.FullName, file.BinData);
                }
                sender.Send().Any();
            }
        }
    }
}
