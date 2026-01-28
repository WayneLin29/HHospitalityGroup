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
            PXTrace.WriteVerbose("[PYEMAIL] ========== [PY Release Email] START ==========");
            PXTrace.WriteVerbose($"[PYEMAIL] Payment: {Base.Document.Current?.DocType} {Base.Document.Current?.RefNbr}");
            PXTrace.WriteVerbose($"[PYEMAIL] Vendor ID: {Base.Document.Current?.VendorID}");
            PXTrace.WriteVerbose($"[PYEMAIL] Payment Amount: {Base.Document.Current?.CuryOrigDocAmt}");

            // Get notification configuration
            APNotification apNotification = PXSelect<APNotification,
                                Where<APNotification.notificationCD, Equal<Required<APNotification.notificationCD>>>>
                                .Select(Base, "PYRelease");

            if (apNotification == null)
            {
                PXTrace.WriteWarning("[PYEMAIL] PYRelease notification not configured");
                return;
            }
            PXTrace.WriteVerbose($"[PYEMAIL] Notification CD: PYRelease, Notification ID: {apNotification.NotificationID}");

            // Get Template
            Notification notification = PXSelect<Notification,
                             Where<Notification.notificationID, Equal<Required<Notification.notificationID>>>>
                             .Select(Base, apNotification.NotificationID);

            if (notification == null)
            {
                PXTrace.WriteWarning("[PYEMAIL] PYRelease notification template not found");
                return;
            }
            PXTrace.WriteVerbose($"[PYEMAIL] Email Template: {notification.Name}, From Account ID: {notification.NFrom}");

            // Validate Mail Account exists
            if (!notification.NFrom.HasValue)
            {
                PXTrace.WriteError($"[PYEMAIL] Notification '{notification.Name}' has no Mail Account configured (NFrom is null)");
                PXTrace.WriteError($"[PYEMAIL] Please configure Mail Account in System → Email Templates (SM204003) → Notification ID: {notification.NotificationID}");
                return;
            }

            // Check if Mail Account exists in the system
            PX.SM.EMailAccount mailAccount = PXSelect<PX.SM.EMailAccount,
                                            Where<PX.SM.EMailAccount.emailAccountID, Equal<Required<PX.SM.EMailAccount.emailAccountID>>>>
                                            .Select(Base, notification.NFrom.Value);

            if (mailAccount == null)
            {
                PXTrace.WriteError($"[PYEMAIL] Mail Account ID '{notification.NFrom.Value}' does not exist in the system!");
                PXTrace.WriteError($"[PYEMAIL] Available solutions:");
                PXTrace.WriteError($"[PYEMAIL]   1. Create Mail Account ID '{notification.NFrom.Value}' in System → Email Accounts (SM204002)");
                PXTrace.WriteError($"[PYEMAIL]   2. Or update Notification Template to use a valid Mail Account in System → Email Templates (SM204003)");

                // List available mail accounts for reference
                var availableAccounts = PXSelect<PX.SM.EMailAccount>.Select(Base);
                if (availableAccounts.Count > 0)
                {
                    PXTrace.WriteError($"[PYEMAIL] Available Mail Account IDs in system:");
                    foreach (PX.SM.EMailAccount acc in availableAccounts)
                    {
                        PXTrace.WriteError($"[PYEMAIL]   - ID: {acc.EmailAccountID}, Address: {acc.Address}, Description: {acc.Description}");
                    }
                }
                else
                {
                    PXTrace.WriteError($"[PYEMAIL] No Mail Accounts found in the system. Please create one first.");
                }
                return;
            }

            PXTrace.WriteVerbose($"[PYEMAIL] Mail Account validated: ID {mailAccount.EmailAccountID}, Address: {mailAccount.Address}");

            // Get Payment's Vendor emails (once, reuse for all invoices)
            List<string> vendorEmails = GetVendorEmails(Base.Document.Current?.VendorID);
            PXTrace.WriteVerbose($"[PYEMAIL] Vendor Emails Retrieved: {vendorEmails.Count} email(s) - [{string.Join(", ", vendorEmails)}]");

            // Get Payment Attachments (once, reuse for all invoices)
            UploadFileMaintenance fileGraph = PXGraph.CreateInstance<UploadFileMaintenance>();
            Guid[] fileIDs = PXNoteAttribute.GetFileNotes(Base.Document.Cache, Base.Document.Current);
            var attachments = fileIDs.Select(id => fileGraph.GetFile(id)).Where(f => f != null).ToList();
            PXTrace.WriteVerbose($"[PYEMAIL] Payment Attachments: {attachments.Count} file(s)");
            foreach (var file in attachments)
            {
                PXTrace.WriteVerbose($"[PYEMAIL]   - Attachment: {file.FullName} ({file.BinData?.Length ?? 0} bytes)");
            }

            // Count total invoices to process
            var allAdjustments = Base.Adjustments.Select().RowCast<APAdjust>().ToList();
            PXTrace.WriteVerbose($"[PYEMAIL] Total Invoices to Process: {allAdjustments.Count}");
            PXTrace.WriteVerbose("[PYEMAIL] ---------- Processing Invoices ----------");

            int invoiceCounter = 0;
            // Process each APAdjust (Invoice) separately - one email per invoice
            foreach (APAdjust apAdjust in allAdjustments)
            {
                if (apAdjust == null) continue;
                invoiceCounter++;

                PXTrace.WriteVerbose($"[PYEMAIL] \n[Invoice {invoiceCounter}] Processing: {apAdjust.AdjdDocType} {apAdjust.AdjdRefNbr}");
                PXTrace.WriteVerbose($"[PYEMAIL]   Adjusted Amount: {apAdjust.CuryAdjdAmt}");

                // Get the Invoice record
                APInvoice aPInvoice = PXSelect<APInvoice,
                                        Where<APInvoice.refNbr, Equal<Required<APInvoice.refNbr>>,
                                        And<APInvoice.docType, Equal<Required<APInvoice.docType>>>>>
                                        .Select(Base, apAdjust.AdjdRefNbr, apAdjust.AdjdDocType);

                if (aPInvoice == null)
                {
                    PXTrace.WriteWarning($"[PYEMAIL]   [Invoice {invoiceCounter}] APInvoice record not found, skipping");
                    continue;
                }

                PXTrace.WriteVerbose($"[PYEMAIL]   Invoice Amount: {aPInvoice.CuryOrigDocAmt}, CreatedBy: {aPInvoice.CreatedByID}");

                // Collect recipients for THIS invoice
                HashSet<string> recipients = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                PXTrace.WriteVerbose($"[PYEMAIL]   Collecting recipients for Invoice {aPInvoice.RefNbr}:");

                // 1. Add Invoice CreatedBy Email
                string createdByEmail = GetCreatedByEmail(aPInvoice.CreatedByID);
                if (!string.IsNullOrWhiteSpace(createdByEmail))
                {
                    recipients.Add(createdByEmail.Trim());
                    PXTrace.WriteVerbose($"[PYEMAIL]     [Source: Invoice CreatedBy] {createdByEmail}");
                }
                else
                {
                    PXTrace.WriteVerbose($"[PYEMAIL]     [Source: Invoice CreatedBy] (No email found)");
                }

                // 2. Add Vendor Emails (from Payment)
                if (vendorEmails.Count > 0)
                {
                    foreach (string vendorEmail in vendorEmails)
                    {
                        if (!string.IsNullOrWhiteSpace(vendorEmail))
                        {
                            bool isNew = recipients.Add(vendorEmail.Trim());
                            PXTrace.WriteVerbose($"[PYEMAIL]     [Source: Vendor] {vendorEmail} {(isNew ? "" : "(duplicate, ignored)")}");
                        }
                    }
                }
                else
                {
                    PXTrace.WriteVerbose($"[PYEMAIL]     [Source: Vendor] (No vendor emails)");
                }

                // 3. Add Email Also attribute (from Invoice)
                string emailAlso = GetEmailAlsoAttribute(aPInvoice.NoteID);
                if (!string.IsNullOrWhiteSpace(emailAlso))
                {
                    bool isNew = recipients.Add(emailAlso.Trim());
                    PXTrace.WriteVerbose($"[PYEMAIL]     [Source: Email Also Attribute] {emailAlso} {(isNew ? "" : "(duplicate, ignored)")}");
                }
                else
                {
                    PXTrace.WriteVerbose($"[PYEMAIL]     [Source: Email Also Attribute] (No email found)");
                }

                // Skip if no valid recipients
                if (recipients.Count == 0)
                {
                    PXTrace.WriteWarning($"[PYEMAIL]   [Invoice {invoiceCounter}] No valid recipients, skipping email");
                    continue;
                }

                PXTrace.WriteVerbose($"[PYEMAIL]   Total Unique Recipients: {recipients.Count}");

                // Send email to all recipients (concatenated in To field)
                string allRecipients = string.Join(";", recipients);

                PXTrace.WriteVerbose($"[PYEMAIL]   Preparing to send email:");
                PXTrace.WriteVerbose($"[PYEMAIL]     To: {allRecipients}");
                PXTrace.WriteVerbose($"[PYEMAIL]     Template: {notification.Name} (ID: {notification.NotificationID})");
                PXTrace.WriteVerbose($"[PYEMAIL]     Attachments: {attachments.Count} file(s)");
                PXTrace.WriteVerbose($"[PYEMAIL]     Activity will be linked to Invoice NoteID: {aPInvoice.NoteID}");

                try
                {
                    TemplateNotificationGenerator sender = TemplateNotificationGenerator.Create(
                        Base,
                        Base.Document.Current,
                        notification.NotificationID.Value);

                    sender.MailAccountId = notification.NFrom.Value;
                    sender.To = allRecipients;
                    sender.RefNoteID = aPInvoice.NoteID;  // Attach Activity to Invoice

                    // Attach files
                    foreach (FileInfo file in attachments)
                    {
                        sender.AddAttachment(file.FullName, file.BinData);
                    }

                    // Send email - automatically creates Activity attached to Invoice
                    sender.Send().Any();

                    PXTrace.WriteVerbose($"[PYEMAIL]   ✓ Email sent successfully for Invoice {aPInvoice.RefNbr}");
                }
                catch (Exception ex)
                {
                    PXTrace.WriteError($"[PYEMAIL]   ✗ Failed to send email for Invoice {aPInvoice.RefNbr}: {ex.Message}");
                    PXTrace.WriteError($"[PYEMAIL]     Exception Type: {ex.GetType().Name}");
                    if (ex.InnerException != null)
                    {
                        PXTrace.WriteError($"[PYEMAIL]     Inner Exception: {ex.InnerException.Message}");
                    }
                    PXTrace.WriteError($"[PYEMAIL]     Mail Account ID used: {notification.NFrom}");
                    PXTrace.WriteError($"[PYEMAIL]     Recipients: {allRecipients}");
                    PXTrace.WriteError($"[PYEMAIL]     Stack Trace: {ex.StackTrace}");
                }
            }

            PXTrace.WriteVerbose($"[PYEMAIL] \n========== [PY Release Email] COMPLETED - Processed {invoiceCounter} invoice(s) ==========");
        }

        /// <summary>
        /// Get Vendor Account Email and Primary Contact Email from Payment's Vendor
        /// </summary>
        private List<string> GetVendorEmails(int? vendorID)
        {
            PXTrace.WriteVerbose($"[PYEMAIL] [GetVendorEmails] Looking up emails for Vendor ID: {vendorID}");
            List<string> emails = new List<string>();

            if (vendorID == null)
            {
                PXTrace.WriteVerbose($"[PYEMAIL] [GetVendorEmails] Vendor ID is null, returning empty list");
                return emails;
            }

            // Get Vendor record
            Vendor vendor = PXSelect<Vendor,
                            Where<Vendor.bAccountID, Equal<Required<Vendor.bAccountID>>>>
                            .Select(Base, vendorID);

            if (vendor == null)
            {
                PXTrace.WriteWarning($"[PYEMAIL] [GetVendorEmails] Vendor record not found for ID: {vendorID}");
                return emails;
            }

            PXTrace.WriteVerbose($"[PYEMAIL] [GetVendorEmails] Vendor: {vendor.AcctCD} - {vendor.AcctName}");
            PXTrace.WriteVerbose($"[PYEMAIL] [GetVendorEmails] PrimaryContactID: {vendor.PrimaryContactID}, DefContactID: {vendor.DefContactID}");

            // Get Vendor's Primary Contact
            if (vendor.PrimaryContactID.HasValue)
            {
                Contact primaryContact = PXSelect<Contact,
                                        Where<Contact.contactID, Equal<Required<Contact.contactID>>>>
                                        .Select(Base, vendor.PrimaryContactID);

                if (!string.IsNullOrWhiteSpace(primaryContact?.EMail))
                {
                    emails.Add(primaryContact.EMail);
                    PXTrace.WriteVerbose($"[PYEMAIL] [GetVendorEmails] Added Primary Contact Email: {primaryContact.EMail}");
                }
                else
                {
                    PXTrace.WriteVerbose($"[PYEMAIL] [GetVendorEmails] Primary Contact has no email");
                }
            }
            else
            {
                PXTrace.WriteVerbose($"[PYEMAIL] [GetVendorEmails] No Primary Contact configured");
            }

            // Get Vendor's Account Email (DefContactID - default contact for the account)
            if (vendor.DefContactID.HasValue)
            {
                Contact accountContact = PXSelect<Contact,
                                        Where<Contact.contactID, Equal<Required<Contact.contactID>>>>
                                        .Select(Base, vendor.DefContactID);

                if (!string.IsNullOrWhiteSpace(accountContact?.EMail))
                {
                    // Avoid duplicate if primary contact = account contact
                    if (!emails.Contains(accountContact.EMail, StringComparer.OrdinalIgnoreCase))
                    {
                        emails.Add(accountContact.EMail);
                        PXTrace.WriteVerbose($"[PYEMAIL] [GetVendorEmails] Added Account Contact Email: {accountContact.EMail}");
                    }
                    else
                    {
                        PXTrace.WriteVerbose($"[PYEMAIL] [GetVendorEmails] Account Contact Email is duplicate of Primary Contact: {accountContact.EMail}");
                    }
                }
                else
                {
                    PXTrace.WriteVerbose($"[PYEMAIL] [GetVendorEmails] Account Contact has no email");
                }
            }
            else
            {
                PXTrace.WriteVerbose($"[PYEMAIL] [GetVendorEmails] No Account Contact configured");
            }

            PXTrace.WriteVerbose($"[PYEMAIL] [GetVendorEmails] Total emails found: {emails.Count}");
            return emails;
        }

        /// <summary>
        /// Get CreatedBy user's email address
        /// </summary>
        private string GetCreatedByEmail(Guid? createdByID)
        {
            PXTrace.WriteVerbose($"[PYEMAIL] [GetCreatedByEmail] Looking up email for User ID: {createdByID}");

            if (createdByID == null)
            {
                PXTrace.WriteVerbose($"[PYEMAIL] [GetCreatedByEmail] User ID is null, returning null");
                return null;
            }

            // Try to get from Contact first
            Contact contact = PXSelect<Contact,
                            Where<Contact.userID, Equal<Required<Contact.userID>>>>
                            .Select(Base, createdByID)
                            .FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(contact?.EMail))
            {
                PXTrace.WriteVerbose($"[PYEMAIL] [GetCreatedByEmail] Found email from Contact: {contact.EMail}");
                return contact.EMail;
            }

            PXTrace.WriteVerbose($"[PYEMAIL] [GetCreatedByEmail] Contact not found or has no email, trying Users table");

            // Fallback: get from Users table
            Users user = PXSelect<Users,
                        Where<Users.pKID, Equal<Required<Users.pKID>>>>
                        .Select(Base, createdByID)
                        .FirstOrDefault();

            if (user != null && !string.IsNullOrWhiteSpace(user.Email))
            {
                PXTrace.WriteVerbose($"[PYEMAIL] [GetCreatedByEmail] Found email from Users: {user.Email}");
                return user.Email;
            }

            PXTrace.WriteVerbose($"[PYEMAIL] [GetCreatedByEmail] No email found for User ID: {createdByID}");
            return null;
        }

        /// <summary>
        /// Get Email Also attribute from Invoice
        /// </summary>
        private string GetEmailAlsoAttribute(Guid? noteID)
        {
            PXTrace.WriteVerbose($"[PYEMAIL] [GetEmailAlsoAttribute] Looking up Email Also for NoteID: {noteID}");

            if (noteID == null)
            {
                PXTrace.WriteVerbose($"[PYEMAIL] [GetEmailAlsoAttribute] NoteID is null, returning null");
                return null;
            }

            APRegisterKvExt attributeMail = PXSelect<APRegisterKvExt,
                                Where<APRegisterKvExt.recordID, Equal<Required<APRegisterKvExt.recordID>>,
                                And<APRegisterKvExt.fieldName, Equal<Required<APRegisterKvExt.fieldName>>>>>
                                .Select(Base, noteID, "AttributeEMAILALSO");

            if (attributeMail != null && !string.IsNullOrWhiteSpace(attributeMail.ValueString))
            {
                PXTrace.WriteVerbose($"[PYEMAIL] [GetEmailAlsoAttribute] Found Email Also: {attributeMail.ValueString}");
                return attributeMail.ValueString;
            }

            PXTrace.WriteVerbose($"[PYEMAIL] [GetEmailAlsoAttribute] No Email Also attribute found");
            return null;
        }
    }
}
