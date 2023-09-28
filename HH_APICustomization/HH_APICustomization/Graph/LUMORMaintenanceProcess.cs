using PX.CS;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Data.EP;
using PX.Objects.AP;
using PX.Objects.GL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Objects.TX;
using System.Collections;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.PM;
using PX.Data.BQL;
using PX.Objects.CA;

namespace HH_APICustomization.Graph
{
    public class LUMORMaintenanceProcess : PXGraph<LUMORMaintenanceProcess>
    {
        public PXSave<ORMaintFilter> Save;
        public PXCancel<ORMaintFilter> Cancel;
        public PXFilter<ORMaintFilter> Filter;

        public SelectFrom<APTran>
               .InnerJoin<APInvoice>.On<APInvoice.docType.IsEqual<APTran.tranType>
                     .And<APInvoice.refNbr.IsEqual<APTran.refNbr>>>
               .Where<APInvoice.docDate.IsBetween<ORMaintFilter.apStartDate.FromCurrent, ORMaintFilter.apEndDate.FromCurrent>
                 .And<APTran.branchID.IsEqual<ORMaintFilter.apBranch.FromCurrent>>
                 .And<APInvoice.branchID.IsEqual<ORMaintFilter.apHBranch.FromCurrent>.Or<ORMaintFilter.apHBranch.FromCurrent.IsNull>>>.ProcessingView.FilteredBy<ORMaintFilter> Transactions;

        public IEnumerable transactions()
        {
            var filter = this.Filter.Current;
            PXView select = new PXView(this, true, this.Transactions.View.BqlSelect);
            Int32 totalrow = 0;
            Int32 startrow = PXView.StartRow;

            if (!string.IsNullOrEmpty(filter.APRefNbrFrom) && !string.IsNullOrEmpty(filter.APRefNbrTo))
                select.WhereAnd<Where<APInvoice.refNbr, Between<Current<ORMaintFilter.apRefNbrFrom>, Current<ORMaintFilter.apRefNbrTo>>>>();
            if (filter.APVendor.HasValue)
                select.WhereAnd<Where<APInvoice.vendorID, Equal<Current<ORMaintFilter.apVendor>>>>();
            if (!string.IsNullOrEmpty(filter.ORStatus))
                select.WhereAnd<Where<APTranExtension.usrORStatus, Equal<Current<ORMaintFilter.orStatus>>>>();
            if (filter.ORDateFrom.HasValue && filter.ORDateTo.HasValue)
                select.WhereAnd<Where<APTranExtension.usrORDate, Between<Current<ORMaintFilter.orDateFrom>, Current<ORMaintFilter.orDateTo>>>>();
            if (!string.IsNullOrEmpty(filter.ORNumberFrom) && !string.IsNullOrEmpty(filter.ORNumberTo))
                select.WhereAnd<Where<APTranExtension.usrORNumber, Between<Current<ORMaintFilter.orNumberFrom>, Current<ORMaintFilter.orNumberTo>>>>();
            if (filter.ORVendor.HasValue)
                select.WhereAnd<Where<APTranExtension.usrORVendor, Equal<Current<ORMaintFilter.orVendor>>>>();
            if (filter.ORBranch.HasValue)
                select.WhereAnd<Where<APTranExtension.usrORBranch, Equal<Current<ORMaintFilter.orBranch>>>>();

            List<object> result = select.Select(PXView.Currents, PXView.Parameters,
                PXView.Searches, PXView.SortColumns, PXView.Descendings,
                PXView.Filters, ref startrow, PXView.MaximumRows, ref totalrow);
            PXView.StartRow = 0;
            return result;
        }

        public LUMORMaintenanceProcess()
        {
            //PXUIFieldAttribute.SetEnabled<LUMCloudBedTransactions.isImported>(Transaction.Cache, null, true);
            var filter = this.Filter.Current;
            Transactions.SetProcessDelegate(delegate (List<APTran> list)
            {
                GoProcessing(list, filter);
            });
        }

        #region Event

        [PXUIField(DisplayName = "APH Branch")]
        [PXMergeAttributes(Method = MergeMethod.Merge)]
        public virtual void _(Events.CacheAttached<APInvoice.branchID> e) { }
        
        public virtual void _(Events.FieldUpdated<ORMaintFilter.updCleanUp> e)
        {
            if (((bool?)e.NewValue) ?? false)
            {
                var row = e.Row as ORMaintFilter;
                row.UpdORBranch = null;
                row.UpdORDate = null;
                row.UpdORVendor = null;
                row.UpdORNumber = null;
                row.UpdORStatus = null;
                row.UpdORTaxZone = null;
                row.UpdProjectID = null;
                row.UpdTaskID = null;
                row.UpdAccountID = null;
                row.UpdSubID = null;
                this.Filter.UpdateCurrent();

                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updORBranch>(e.Cache, null, false);
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updORDate>(e.Cache, null, false);
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updORVendor>(e.Cache, null, false);
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updORNumber>(e.Cache, null, false);
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updORStatus>(e.Cache, null, false);
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updORTaxZone>(e.Cache, null, false);
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updProjectID>(e.Cache, null, false);
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updTaskID>(e.Cache, null, false);
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updAccountID>(e.Cache, null, false);
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updSubID>(e.Cache, null, false);
            }
            else
            {
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updORBranch>(e.Cache, null, true);
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updORDate>(e.Cache, null, true);
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updORVendor>(e.Cache, null, true);
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updORNumber>(e.Cache, null, true);
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updORStatus>(e.Cache, null, true);
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updORTaxZone>(e.Cache, null, true);
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updProjectID>(e.Cache, null, true);
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updTaskID>(e.Cache, null, true);
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updAccountID>(e.Cache, null, true);
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updSubID>(e.Cache, null, true);
            }
        }

        public virtual void _(Events.FieldUpdated<ORMaintFilter.updProjectID> e)
        {
            if (e.NewValue != null)
            {
                object newTaskID;
                e.Cache.RaiseFieldDefaulting<ORMaintFilter.updTaskID>(e.Row, out newTaskID);
                ((ORMaintFilter)e.Row).UpdTaskID = (int?)newTaskID;
            }
        }

        public virtual void _(Events.FieldDefaulting<ORMaintFilter.updTaskID> e)
        {
            var row = e.Row as ORMaintFilter;
            if (e.Row != null)
            {
                var newTask = SelectFrom<PMTask>
                              .Where<PMTask.projectID.IsEqual<P.AsInt>
                                .And<PMTask.isDefault.IsEqual<True>>>
                              .View.Select(this, row?.UpdProjectID).TopFirst;
                e.NewValue = newTask?.TaskID;
            }
        }
        #endregion

        #region Method
        public static void GoProcessing(List<APTran> selectedData, ORMaintFilter filter)
        {
            var baseGraph = PXGraph.CreateInstance<LUMORMaintenanceProcess>();
            baseGraph.UpdateORData(selectedData, filter, baseGraph);
        }

        /// <summary> Update APTran "OR" Data </summary>
        public void UpdateORData(List<APTran> selectedData, ORMaintFilter filter, LUMORMaintenanceProcess baseGraph)
        {
            foreach (var selectedItem in selectedData)
            {
                PXProcessing.SetCurrentItem(selectedItem);
                try
                {
                    var aptranExtensionInfo = selectedItem.GetExtension<APTranExtension>();
                    PXUpdate<Set<APTranExtension.usrORBranch, Required<APTranExtension.usrORBranch>,
                             Set<APTranExtension.usrORDate, Required<APTranExtension.usrORDate>,
                             Set<APTranExtension.usrORVendor, Required<APTranExtension.usrORVendor>,
                             Set<APTranExtension.usrORNumber, Required<APTranExtension.usrORNumber>,
                             Set<APTranExtension.usrORStatus, Required<APTranExtension.usrORStatus>,
                             Set<APTranExtension.usrORTaxZone, Required<APTranExtension.usrORTaxZone>,
                             Set<APTran.projectID, Required<APTran.projectID>,
                             Set<APTran.taskID, Required<APTran.taskID>,
                             Set<APTran.accountID,Required<APTran.accountID>,
                             Set<APTran.subID,Required<APTran.subID>>>>>>>>>>>,
                        APTran,
                        Where<APTran.tranType, Equal<Required<APTran.tranType>>,
                              And<APTran.refNbr, Equal<Required<APTran.refNbr>>,
                              And<APTran.lineNbr, Equal<Required<APTran.lineNbr>>>>>>
                    .Update(
                        baseGraph,
                        (filter.UpdCleanUp ?? false) ? null : filter.UpdORBranch ?? aptranExtensionInfo?.UsrORBranch,
                        (filter.UpdCleanUp ?? false) ? null : filter.UpdORDate ?? aptranExtensionInfo?.UsrORDate,
                        (filter.UpdCleanUp ?? false) ? null : filter.UpdORVendor ?? aptranExtensionInfo?.UsrORVendor,
                        (filter.UpdCleanUp ?? false) ? null : filter.UpdORNumber ?? aptranExtensionInfo?.UsrOrNumber,
                        (filter.UpdCleanUp ?? false) ? null : filter.UpdORStatus ?? aptranExtensionInfo?.UsrORStatus,
                        (filter.UpdCleanUp ?? false) ? null : filter.UpdORTaxZone ?? aptranExtensionInfo?.UsrORTaxZone,
                        (filter.UpdCleanUp ?? false) || (selectedItem.Released ?? false) ? selectedItem?.ProjectID : filter.UpdProjectID ?? selectedItem?.ProjectID,
                        (filter.UpdCleanUp ?? false) || (selectedItem.Released ?? false) ? selectedItem?.TaskID : filter.UpdTaskID ?? selectedItem?.TaskID,
                        (filter.UpdCleanUp ?? false) || (selectedItem.Released ?? false) ? selectedItem?.AccountID : filter.UpdAccountID ?? selectedItem?.AccountID,
                        (filter.UpdCleanUp ?? false) || (selectedItem.Released ?? false) ? selectedItem?.SubID : filter.UpdSubID ?? selectedItem?.SubID,
                        selectedItem.TranType,
                        selectedItem.RefNbr,
                        selectedItem.LineNbr
                        );

                }
                catch (Exception ex)
                {
                    PXProcessing.SetError<APInvoice>(ex.Message);
                }
            }
            #region Clean up Update Filter
            filter.UpdORBranch = null;
            filter.UpdORDate = null;
            filter.UpdORVendor = null;
            filter.UpdORNumber = null;
            filter.UpdORStatus = null;
            filter.UpdORTaxZone = null;
            filter.UpdProjectID = null;
            filter.UpdTaskID = null;
            filter.UpdAccountID = null;
            filter.UpdSubID = null;
            this.Filter.UpdateCurrent();
            #endregion
        }

        #endregion

    }

    public class ORMaintFilter : IBqlTable
    {
        #region APRefNbrFrom
        [PXDBString(15, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "AP Reference Number From", Visibility = PXUIVisibility.SelectorVisible, TabOrder = 1)]
        [PXSelector(typeof(Search<APRegister.refNbr>), Filterable = true)]
        [PXFieldDescription]
        public virtual string APRefNbrFrom { get; set; }
        public abstract class apRefNbrFrom : PX.Data.BQL.BqlString.Field<apRefNbrFrom> { }
        #endregion

        #region APRefNbrTo
        [PXDBString(15, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "AP Reference Number To", Visibility = PXUIVisibility.SelectorVisible, TabOrder = 1)]
        [PXSelector(typeof(Search<APRegister.refNbr>), Filterable = true)]
        [PXFieldDescription]
        public virtual string APRefNbrTo { get; set; }
        public abstract class apRefNbrTo : PX.Data.BQL.BqlString.Field<apRefNbrTo> { }
        #endregion

        #region APStartDate
        [PXDBDate]
        [PXDefault()]
        [PXUIField(DisplayName = "AP Date From")]
        public virtual DateTime? APStartDate { get; set; }
        public abstract class apStartDate : PX.Data.BQL.BqlDateTime.Field<apStartDate> { }
        #endregion

        #region APEndDate
        [PXDBDate]
        [PXDefault(typeof(AccessInfo.businessDate))]
        [PXUIField(DisplayName = "AP Date To")]
        public virtual DateTime? APEndDate { get; set; }
        public abstract class apEndDate : PX.Data.BQL.BqlDateTime.Field<apEndDate> { }
        #endregion

        #region APVendor
        [VendorActive(Visibility = PXUIVisibility.SelectorVisible,
                      DescriptionField = typeof(Vendor.acctName),
                      CacheGlobal = true,
                      Filterable = true)]
        [PXDefault(typeof(APInvoice.vendorID), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "AP Vendor")]
        public virtual int? APVendor { get; set; }
        public abstract class apVendor : PX.Data.BQL.BqlInt.Field<apVendor> { }
        #endregion

        #region APBranch
        [Branch(typeof(APRegister.branchID))]
        [PXUIField(DisplayName = "AP Branch")]
        public virtual int? APBranch { get; set; }
        public abstract class apBranch : PX.Data.BQL.BqlInt.Field<apBranch> { }
        #endregion

        #region APHBranch
        [Branch(typeof(APRegister.branchID),PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "APH Branch")]
        public virtual int? APHBranch { get; set; }
        public abstract class apHBranch : PX.Data.BQL.BqlInt.Field<apHBranch> { }
        #endregion

        #region ORStatus
        [PXDBString(1)]
        [PXSelector(typeof(SearchFor<CSAttributeDetail.valueID>
                           .Where<CSAttributeDetail.attributeID.IsEqual<ORSTATUSAttr>>),
                    DescriptionField = typeof(CSAttributeDetail.description))]
        [PXUIField(DisplayName = "OR Status", Visibility = PXUIVisibility.SelectorVisible)]
        public virtual string ORStatus { get; set; }
        public abstract class orStatus : PX.Data.BQL.BqlString.Field<orStatus> { }
        #endregion

        #region ORNumberFrom
        [PXDBString]
        [PXUIField(DisplayName = "OR Number From")]
        public virtual string ORNumberFrom { get; set; }
        public abstract class orNumberFrom : PX.Data.BQL.BqlInt.Field<orNumberFrom> { }
        #endregion

        #region ORNumberTo
        [PXDBString]
        [PXUIField(DisplayName = "OR Number To")]
        public virtual string ORNumberTo { get; set; }
        public abstract class orNumberTo : PX.Data.BQL.BqlInt.Field<orNumberTo> { }
        #endregion

        #region ORBranch
        [Branch(typeof(APRegister.branchID), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "OR Branch")]
        public virtual int? ORBranch { get; set; }
        public abstract class orBranch : PX.Data.BQL.BqlInt.Field<orBranch> { }
        #endregion

        #region ORDateFrom
        [PXDBDate]
        [PXDefault(typeof(APInvoice.docDate), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "OR Date From")]
        public virtual DateTime? ORDateFrom { get; set; }
        public abstract class orDateFrom : PX.Data.BQL.BqlDateTime.Field<orDateFrom> { }
        #endregion

        #region ORDateTo
        [PXDBDate]
        [PXDefault(typeof(APInvoice.docDate), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "OR Date To")]
        public virtual DateTime? ORDateTo { get; set; }
        public abstract class orDateTo : PX.Data.BQL.BqlDateTime.Field<orDateTo> { }
        #endregion

        #region ORVendor
        [VendorActive(Visibility = PXUIVisibility.SelectorVisible,
                      DescriptionField = typeof(Vendor.acctName),
                      CacheGlobal = true,
                      Filterable = true)]
        [PXUIField(DisplayName = "OR Vendor")]
        public virtual int? ORVendor { get; set; }
        public abstract class orVendor : PX.Data.BQL.BqlInt.Field<orVendor> { }
        #endregion

        #region UpdORNumber
        [PXDBString]
        [PXUIField(DisplayName = "OR Number")]
        public virtual string UpdORNumber { get; set; }
        public abstract class updORNumber : PX.Data.BQL.BqlString.Field<updORNumber> { }
        #endregion

        #region UpdORBranch
        [Branch(typeof(APRegister.branchID), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "OR Branch")]
        public virtual int? UpdORBranch { get; set; }
        public abstract class updORBranch : PX.Data.BQL.BqlInt.Field<updORBranch> { }
        #endregion

        #region UpdORDate
        [PXDBDate]
        [PXDefault(typeof(APInvoice.docDate), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "OR Date")]
        public virtual DateTime? UpdORDate { get; set; }
        public abstract class updORDate : PX.Data.BQL.BqlDateTime.Field<updORDate> { }
        #endregion

        #region UpdORVendor
        [VendorActive(Visibility = PXUIVisibility.SelectorVisible,
                      DescriptionField = typeof(Vendor.acctName),
                      CacheGlobal = true,
                      Filterable = true)]
        [PXUIField(DisplayName = "OR Vendor")]
        public virtual int? UpdORVendor { get; set; }
        public abstract class updORVendor : PX.Data.BQL.BqlInt.Field<updORVendor> { }
        #endregion

        #region UpdORTaxZone
        [PXDBString(10, IsUnicode = true)]
        [PXUIField(DisplayName = "OR TaxZone", Visibility = PXUIVisibility.Visible)]
        [PXSelector(typeof(TaxZone.taxZoneID), DescriptionField = typeof(TaxZone.descr), Filterable = true)]
        public virtual string UpdORTaxZone { get; set; }
        public abstract class updORTaxZone : PX.Data.BQL.BqlString.Field<updORTaxZone> { }
        #endregion

        #region UpdORStatus
        [PXDBString]
        [PXSelector(typeof(SearchFor<CSAttributeDetail.valueID>
                           .Where<CSAttributeDetail.attributeID.IsEqual<ORSTATUSAttr>>),
                    SubstituteKey = typeof(CSAttributeDetail.description))]
        [PXUIField(DisplayName = "OR Status", Visibility = PXUIVisibility.SelectorVisible)]
        public virtual string UpdORStatus { get; set; }
        public abstract class updORStatus : PX.Data.BQL.BqlString.Field<updORStatus> { }
        #endregion

        #region UpdCleanUp
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Clear OR Information")]
        public virtual bool? UpdCleanUp { get; set; }
        public abstract class updCleanUp : PX.Data.BQL.BqlBool.Field<updCleanUp> { }
        #endregion

        #region UpdProjectID
        [ProjectDefault(BatchModule.AP, typeof(Search<PX.Objects.CR.Location.vDefProjectID, Where<PX.Objects.CR.Location.bAccountID, Equal<Current<APInvoice.vendorID>>, And<PX.Objects.CR.Location.locationID, Equal<Current<APInvoice.vendorLocationID>>>>>), typeof(APTran.accountID),PersistingCheck = PXPersistingCheck.Nothing)]
        [APActiveProject]
        [PXForeignReference(typeof(Field<updProjectID>.IsRelatedTo<PMProject.contractID>))]
        public virtual Int32? UpdProjectID { get; set; }
        public abstract class updProjectID : PX.Data.BQL.BqlInt.Field<updProjectID> { }
        #endregion

        #region UpdTaskID
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXSelector(typeof(SelectFrom<PMTask>
                          .Where<PMTask.projectID.IsEqual<ORMaintFilter.updProjectID.FromCurrent>
                            .And<PMTask.isActive.IsEqual<True>>>
                          .SearchFor<PMTask.taskID>),
            typeof(PMTask.description),
            typeof(PMTask.isActive),
            SubstituteKey = typeof(PMTask.taskCD))]
        [PXUIField(DisplayName = "Project Task")]
        public virtual Int32? UpdTaskID { get; set; }
        public abstract class updTaskID : PX.Data.BQL.BqlInt.Field<updTaskID> { }
        #endregion

        #region UpdAccountID
        [Account(typeof(ORMaintFilter.updORBranch),DisplayName = "Account", Visibility = PXUIVisibility.Visible, Filterable = false, DescriptionField = typeof(Account.description))]
        public virtual int? UpdAccountID { get;set;}
        public abstract class updAccountID : PX.Data.BQL.BqlInt.Field<updAccountID> { }

        #endregion

        #region UpdSubID
        [SubAccount(typeof(ORMaintFilter.updAccountID), typeof(ORMaintFilter.updORBranch), true, DisplayName = "Subaccount", Visibility = PXUIVisibility.Visible, Filterable = true, TabOrder = 100)]
        public virtual int? UpdSubID { get;set;}
        public abstract class updSubID : PX.Data.BQL.BqlInt.Field<updSubID> { }

        #endregion

    }
}
