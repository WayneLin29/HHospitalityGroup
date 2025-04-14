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
using HHAPICustomization.DAC;
using static PX.Objects.GL.ControlAccountModule;

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
                 .And<APTranExtension.usrORBranch2.IsEqual<ORMaintFilter.orBranch.FromCurrent>>
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
            if (filter.APBranch.HasValue)
                select.WhereAnd<Where<APTran.branchID, Equal<Current<ORMaintFilter.apBranch>>>>();

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
                row.UpdAccountID = null;
                row.UpdSubID = null;
                this.Filter.UpdateCurrent();

                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updORBranch>(e.Cache, null, false);
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updORDate>(e.Cache, null, false);
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updORVendor>(e.Cache, null, false);
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updORNumber>(e.Cache, null, false);
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updORStatus>(e.Cache, null, false);
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updORTaxZone>(e.Cache, null, false);
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
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updAccountID>(e.Cache, null, true);
                PXUIFieldAttribute.SetEnabled<ORMaintFilter.updSubID>(e.Cache, null, true);
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
                    PXUpdate<Set<APTranExtension.usrORBranch2, Required<APTranExtension.usrORBranch2>,
                             Set<APTranExtension.usrORDate, Required<APTranExtension.usrORDate>,
                             Set<APTranExtension.usrORVendor, Required<APTranExtension.usrORVendor>,
                             Set<APTranExtension.usrORNumber, Required<APTranExtension.usrORNumber>,
                             Set<APTranExtension.usrORStatus, Required<APTranExtension.usrORStatus>,
                             Set<APTranExtension.usrORTaxZone, Required<APTranExtension.usrORTaxZone>,
                             Set<APTran.accountID, Required<APTran.accountID>,
                             Set<APTran.subID, Required<APTran.subID>,
                             Set<APTran.taxCategoryID, Required<APTran.taxCategoryID>>>>>>>>>>,
                        APTran,
                        Where<APTran.tranType, Equal<Required<APTran.tranType>>,
                              And<APTran.refNbr, Equal<Required<APTran.refNbr>>,
                              And<APTran.lineNbr, Equal<Required<APTran.lineNbr>>>>>>
                    .Update(
                        baseGraph,
                        (filter.UpdCleanUp ?? false) ? null : filter.UpdORBranch ?? aptranExtensionInfo?.UsrORBranch2,
                        (filter.UpdCleanUp ?? false) ? null : filter.UpdORDate ?? aptranExtensionInfo?.UsrORDate,
                        (filter.UpdCleanUp ?? false) ? null : filter.UpdORVendor ?? aptranExtensionInfo?.UsrORVendor,
                        (filter.UpdCleanUp ?? false) ? null : filter.UpdORNumber ?? aptranExtensionInfo?.UsrOrNumber,
                        (filter.UpdCleanUp ?? false) ? null : filter.UpdORStatus ?? aptranExtensionInfo?.UsrORStatus,
                        (filter.UpdCleanUp ?? false) ? null : filter.UpdORTaxZone ?? aptranExtensionInfo?.UsrORTaxZone,
                        (filter.UpdCleanUp ?? false) || (selectedItem.Released ?? false) ? selectedItem?.AccountID : filter.UpdAccountID ?? selectedItem?.AccountID,
                        (filter.UpdCleanUp ?? false) || (selectedItem.Released ?? false) ? selectedItem?.SubID : filter.UpdSubID ?? selectedItem?.SubID,
                        (filter.UpdCleanUp ?? false) || (selectedItem.Released ?? false) ? selectedItem?.TaxCategoryID : filter.UpdTaxCategoryID ?? selectedItem?.TaxCategoryID,
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
            filter.UpdAccountID = null;
            filter.UpdSubID = null;
            this.Filter.UpdateCurrent();
            #endregion
        }

        #endregion

    }

    public class ORMaintFilter : PXBqlTable, IBqlTable
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
        [Branch(typeof(APRegister.branchID), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "AP Branch")]
        public virtual int? APBranch { get; set; }
        public abstract class apBranch : PX.Data.BQL.BqlInt.Field<apBranch> { }
        #endregion

        #region APHBranch
        [Branch(typeof(APRegister.branchID), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "APH Branch")]
        public virtual int? APHBranch { get; set; }
        public abstract class apHBranch : PX.Data.BQL.BqlInt.Field<apHBranch> { }
        #endregion

        #region ORStatus
        [PXDBString(1)]
        [PXSelector(typeof(SearchFor<CSAttributeDetail.valueID>
                           .Where<CSAttributeDetail.attributeID.IsEqual<ORSTATUSAttr>
                             .And<CSAttributeDetail.disabled.IsEqual<False>>>),
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
        [PXDBString(15)]
        [PXSelector(typeof(Search<CSAttributeDetail.valueID,
                           Where<CSAttributeDetail.attributeID, Equal<IVBRANCHAttr>,
                             And<CSAttributeDetail.disabled, NotEqual<True>>>>),
                    typeof(CSAttributeDetail.description))]
        [PXDefault]
        [PXUIField(DisplayName = "OR Branch")]
        public virtual string ORBranch { get; set; }
        public abstract class orBranch : PX.Data.BQL.BqlString.Field<orBranch> { }
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
        [PXDBString]
        [PXSelector(typeof(Search<CSAttributeDetail.valueID,
                          Where<CSAttributeDetail.attributeID, Equal<IVBRANCHAttr>,
                            And<CSAttributeDetail.disabled, NotEqual<True>>>>),
                   typeof(CSAttributeDetail.description))]
        [PXUIField(DisplayName = "OR Branch")]
        public virtual string UpdORBranch { get; set; }
        public abstract class updORBranch : PX.Data.BQL.BqlString.Field<updORBranch> { }
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
                           .Where<CSAttributeDetail.attributeID.IsEqual<ORSTATUSAttr>
                             .And<CSAttributeDetail.disabled.IsEqual<False>>>),
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

        #region UpdAccountID
        [Account(typeof(ORMaintFilter.updORBranch), DisplayName = "Account", Visibility = PXUIVisibility.Visible, Filterable = false, DescriptionField = typeof(Account.description))]
        public virtual int? UpdAccountID { get; set; }
        public abstract class updAccountID : PX.Data.BQL.BqlInt.Field<updAccountID> { }

        #endregion

        #region UpdSubID
        [SubAccount(typeof(ORMaintFilter.updAccountID), typeof(ORMaintFilter.updORBranch), true, DisplayName = "Subaccount", Visibility = PXUIVisibility.Visible, Filterable = true, TabOrder = 100)]
        public virtual int? UpdSubID { get; set; }
        public abstract class updSubID : PX.Data.BQL.BqlInt.Field<updSubID> { }

        #endregion

        #region UpdTaxCategoryID
        [PXDBString(TaxCategory.taxCategoryID.Length, IsUnicode = true)]
        [PXUIField(DisplayName = "Tax Category", Visibility = PXUIVisibility.Visible)]
        [PXSelector(typeof(TaxCategory.taxCategoryID), DescriptionField = typeof(TaxCategory.descr))]
        [PXRestrictor(typeof(Where<TaxCategory.active, Equal<True>>), PX.Objects.TX.Messages.InactiveTaxCategory, typeof(TaxCategory.taxCategoryID))]
        public virtual string UpdTaxCategoryID { get; set; }
        public abstract class updTaxCategoryID : PX.Data.BQL.BqlString.Field<updTaxCategoryID> { }

        #endregion

    }

    public class IVBRANCHAttr : PX.Data.BQL.BqlString.Constant<IVBRANCHAttr>
    {
        public IVBRANCHAttr() : base("IVBRANCH") { }
    }
}
