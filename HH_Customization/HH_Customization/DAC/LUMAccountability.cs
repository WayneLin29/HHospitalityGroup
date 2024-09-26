using System;
using System.Linq;
using System.Collections.Generic;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.Data.ReferentialIntegrity.Attributes;

namespace HH_Customization.DAC
{
    [Serializable]
    [PXCacheName("LUMAccountability")]
    public class LUMAccountability : PXBqlTable, IBqlTable
    {
        #region Key
        public class PK : PrimaryKeyOf<LUMAccountability>.By<bAccountID, lineNbr>
        {
            public static LUMAccountability Find(PXGraph graph, int? bAccountID, int? lineNbr) => FindBy(graph, bAccountID, lineNbr);
        }
        #endregion

        #region BAccountID
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "EmployeeID")]
        [PXDBDefault(typeof(EPEmployee.bAccountID))]
        [PXParent(typeof(Select<EPEmployee, Where<EPEmployee.bAccountID, Equal<Current<bAccountID>>>>))]
        public virtual int? BAccountID { get; set; }
        public abstract class bAccountID : PX.Data.BQL.BqlInt.Field<bAccountID> { }
        #endregion

        #region LineNbr
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "Line Nbr")]
        [PXLineNbr(typeof(EPEmployeeHHExt.usrLineCntr))]
        public virtual int? LineNbr { get; set; }
        public abstract class lineNbr : PX.Data.BQL.BqlInt.Field<lineNbr> { }
        #endregion

        #region Type
        [PXDBString(10, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Type", Required = true)]
        [PXDefault]
        [AtTypeList]
        public virtual string Type { get; set; }
        public abstract class type : PX.Data.BQL.BqlString.Field<type> { }
        #endregion

        #region Field1
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Field1")]
        public virtual string Field1 { get; set; }
        public abstract class field1 : PX.Data.BQL.BqlString.Field<field1> { }
        #endregion

        #region Field2
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Field2")]
        public virtual string Field2 { get; set; }
        public abstract class field2 : PX.Data.BQL.BqlString.Field<field2> { }
        #endregion

        #region Field3
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Field3")]
        public virtual string Field3 { get; set; }
        public abstract class field3 : PX.Data.BQL.BqlString.Field<field3> { }
        #endregion

        #region Field4
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Field4")]
        public virtual string Field4 { get; set; }
        public abstract class field4 : PX.Data.BQL.BqlString.Field<field4> { }
        #endregion

        #region Field5
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Field5")]
        public virtual string Field5 { get; set; }
        public abstract class field5 : PX.Data.BQL.BqlString.Field<field5> { }
        #endregion

        #region Field6
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Field6")]
        public virtual string Field6 { get; set; }
        public abstract class field6 : PX.Data.BQL.BqlString.Field<field6> { }
        #endregion

        #region Field7
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Field7")]
        public virtual string Field7 { get; set; }
        public abstract class field7 : PX.Data.BQL.BqlString.Field<field7> { }
        #endregion

        #region Field8
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Field8")]
        public virtual string Field8 { get; set; }
        public abstract class field8 : PX.Data.BQL.BqlString.Field<field8> { }
        #endregion

        #region Tstamp
        [PXDBTimestamp()]
        [PXUIField(DisplayName = "Tstamp")]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
        #endregion

        #region CreatedByID
        [PXDBCreatedByID()]
        public virtual Guid? CreatedByID { get; set; }
        public abstract class createdByID : PX.Data.BQL.BqlGuid.Field<createdByID> { }
        #endregion

        #region CreatedByScreenID
        [PXDBCreatedByScreenID()]
        public virtual string CreatedByScreenID { get; set; }
        public abstract class createdByScreenID : PX.Data.BQL.BqlString.Field<createdByScreenID> { }
        #endregion

        #region CreatedDateTime
        [PXDBCreatedDateTime()]
        public virtual DateTime? CreatedDateTime { get; set; }
        public abstract class createdDateTime : PX.Data.BQL.BqlDateTime.Field<createdDateTime> { }
        #endregion

        #region LastModifiedByID
        [PXDBLastModifiedByID()]
        public virtual Guid? LastModifiedByID { get; set; }
        public abstract class lastModifiedByID : PX.Data.BQL.BqlGuid.Field<lastModifiedByID> { }
        #endregion

        #region LastModifiedByScreenID
        [PXDBLastModifiedByScreenID()]
        public virtual string LastModifiedByScreenID { get; set; }
        public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field<lastModifiedByScreenID> { }
        #endregion

        #region LastModifiedDateTime
        [PXDBLastModifiedDateTime()]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        public abstract class lastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime> { }
        #endregion

        #region Noteid
        [PXNote()]
        public virtual Guid? Noteid { get; set; }
        public abstract class noteid : PX.Data.BQL.BqlGuid.Field<noteid> { }
        #endregion

        #region AttributeCombo
        public class AtTypeList : PXStringListAttribute, IPXRowSelectedSubscriber
        {
            public void RowSelected(PXCache sender, PXRowSelectedEventArgs e)
            {

                List<CSAttributeDetail> datas = GetAttributeDetail(sender.Graph);
                PXStringListAttribute.SetList(sender, e.Row, _FieldName,
                    datas.Select(x => x.ValueID).ToArray(),
                    datas.Select(x => x.Description).ToArray());
            }

            public virtual List<CSAttributeDetail> GetAttributeDetail(PXGraph graph)
            {
                return SelectFrom<CSAttributeDetail>
                    .Where<CSAttributeDetail.attributeID.IsEqual<aTTYPE>>
                    .View.Select(graph)
                    .RowCast<CSAttributeDetail>()
                    .ToList();
            }
        }

        public const string ATTYPE = "ATTYPE";
        public class aTTYPE : PX.Data.BQL.BqlString.Constant<aTTYPE> { public aTTYPE() : base(ATTYPE) { } }
        #endregion
    }
}