using LeaveAndOvertimeCustomization.DAC;
using PX.Data;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Objects.EP;
using PX.Data.EP;
using LeaveAndOvertimeCustomization.Descriptor;
using PX.Data.BQL;
using System.Collections;

namespace LeaveAndOvertimeCustomization.Graph
{
    public class LeaveRequestMaint : PXGraph<LeaveRequestMaint>
    {
        public PXFilter<LeaveRequestFilter> Filter;

        [PXFilterable]
        public SelectFrom<LumLeaveRequest>
               .Where<LumLeaveRequest.requestEmployeeID.IsEqual<LeaveRequestFilter.employeeID.FromCurrent>>
               .View document;

        public LeaveRequestMaint()
        {
            this.document.View.IsReadOnly = true;
        }

        public IEnumerable Document()
        {
            PXView select = new PXView(this, true, document.View.BqlSelect);
            Int32 totalrow = 0;
            Int32 startrow = PXView.StartRow;
            List<object> result = select.Select(PXView.Currents, PXView.Parameters,
                   PXView.Searches, PXView.SortColumns, PXView.Descendings,
                   PXView.Filters, ref startrow, PXView.MaximumRows, ref totalrow);
            PXView.StartRow = 0;
            foreach (LumLeaveRequest item in result)
            {
                EPEmployee employee = SelectFrom<EPEmployee>.Where<EPEmployee.bAccountID.IsEqual<P.AsInt>>.View.Select(this, item.RequestEmployeeID);
                if (employee != null)
                    item.Duration = new HRHelper().GetLeaveDuration(employee, item.LeaveStart, item.LeaveEnd, new HRHelper().GetLeaveTypeInfo(item.LeaveType)?.IsOnlyWorkDay);
            }
            return result;
        }

        #region Action

        public PXSave<LumLeaveRequest> Save;

        public PXAction<LeaveRequestFilter> createNew;
        [PXInsertButton]
        [PXUIField(DisplayName = "")]
        protected virtual void CreateNew()
        {
            using (new PXPreserveScope())
            {
                LeaveRequestEntry graph = (LeaveRequestEntry)PXGraph.CreateInstance(typeof(LeaveRequestEntry));
                graph.Clear(PXClearOption.ClearAll);
                LumLeaveRequest claim = (LumLeaveRequest)graph.document.Cache.CreateInstance();
                graph.document.Insert(claim);
                graph.document.Cache.IsDirty = false;
                PXRedirectHelper.TryRedirect(graph, PXRedirectHelper.WindowMode.InlineWindow);
            }
        }

        public PXAction<LeaveRequestFilter> EditDetail;
        [PXEditDetailButton]
        [PXUIField(DisplayName = "", MapEnableRights = PXCacheRights.Select)]
        protected virtual void editDetail()
        {
            if (document.Current == null) return;
            LumLeaveRequest row = PXSelect<LumLeaveRequest, Where<LumLeaveRequest.refNbr, Equal<Required<LumLeaveRequest.refNbr>>>>.SelectSingleBound(this, null, document.Current.RefNbr);
            PXRedirectHelper.TryRedirect(this, row, PXRedirectHelper.WindowMode.InlineWindow);
        }

        public PXAction<LeaveRequestFilter> delete;
        [PXDeleteButton]
        [PXUIField(DisplayName = "")]
        protected void Delete()
        {
            if (document.Current == null) return;

            if (document.Current.Status != LumLeaveRequestStatus.OnHold)
                throw new PXException("can not delete this document");

            LeaveRequestEntry graph = (LeaveRequestEntry)PXGraph.CreateInstance(typeof(LeaveRequestEntry));
            graph.Clear(PXClearOption.ClearAll);
            graph.document.Current = graph.document.Search<LumLeaveRequest.refNbr>(document.Current.RefNbr);
            graph.Delete.Press();
            this.document.View.RequestRefresh();
        }

        #endregion

    }

    [Serializable]
    [PXHidden]
    public partial class LeaveRequestFilter : PXBqlTable, IBqlTable
    {
        private int? _employeeId;

        #region EmployeeID
        public abstract class employeeID : PX.Data.BQL.BqlInt.Field<employeeID> { }

        [PXDBInt]
        [PXUIField(DisplayName = "Employee")]
        [PXDefault(typeof(Search<EPEmployee.bAccountID, Where<EPEmployee.userID, Equal<Current<AccessInfo.userID>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXSubordinateAndWingmenSelector()]
        [PXFieldDescription]
        public virtual Int32? EmployeeID
        {
            get { return _employeeId; }
            set { _employeeId = value; }
        }

        #endregion
    }
}
