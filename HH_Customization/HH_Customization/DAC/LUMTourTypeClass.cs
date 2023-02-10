using System;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.CM.Extensions;
using PX.Objects.GL;

namespace HH_Customization.DAC
{
    [Serializable]
    [PXCacheName("LUMTourTypeClass")]
    public class LUMTourTypeClass : IBqlTable
    {
        #region Key
        public class PK : PrimaryKeyOf<LUMTourTypeClass>.By<typeClassID>
        {
            public static LUMTourTypeClass Find(PXGraph graph, int? typeClassID) => FindBy(graph, typeClassID);
        }
        public class UK : PrimaryKeyOf<LUMTourTypeClass>.By<typeClassCD>
        {
            public static LUMTourTypeClass Find(PXGraph graph, string typeClassCD) => FindBy(graph, typeClassCD);
        }
        #endregion

        #region TypeClassID
        [PXDBIdentity()]
        public virtual int? TypeClassID { get; set; }
        public abstract class typeClassID : PX.Data.BQL.BqlInt.Field<typeClassID> { }
        #endregion

        #region TypeClassCD
        [PXDBString(25, IsUnicode = true, InputMask = "",IsKey = true)]
        [PXUIField(DisplayName = "Type CD",Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXSelector(typeof(Search<LUMTourTypeClass.typeClassCD>),
            typeof(LUMTourTypeClass.typeClassCD),
            typeof(LUMTourTypeClass.description),
            typeof(LUMTourTypeClass.branchID),
            typeof(LUMTourTypeClass.curyID),
            typeof(LUMTourTypeClass.baseRate),
            typeof(LUMTourTypeClass.targetRate)
            )]
        public virtual string TypeClassCD { get; set; }
        public abstract class typeClassCD : PX.Data.BQL.BqlString.Field<typeClassCD> { }
        #endregion

        #region BranchID
        [Branch()]
        [PXDefault(typeof(Current<AccessInfo.branchID>), PersistingCheck = PXPersistingCheck.NullOrBlank)]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }
        #endregion

        #region Description
        [PXDBString(255, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Description")]
        public virtual string Description { get; set; }
        public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
        #endregion

        #region CuryID
        [PXDBString(5, IsUnicode = true, InputMask = ">LLLLL")]
        [PXUIField(DisplayName = "Currency", Required = true)]
        [PXDefault(typeof(Current<AccessInfo.baseCuryID>),PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXSelector(typeof(Currency.curyID))]

        public virtual string CuryID { get; set; }
        public abstract class curyID : PX.Data.BQL.BqlString.Field<curyID> { }
        #endregion

        #region BaseRate
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Base Rate")]
        public virtual Decimal? BaseRate { get; set; }
        public abstract class baseRate : PX.Data.BQL.BqlDecimal.Field<baseRate> { }
        #endregion

        #region TargetRate
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Target Rate")]
        public virtual Decimal? TargetRate { get; set; }
        public abstract class targetRate : PX.Data.BQL.BqlDecimal.Field<targetRate> { }
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
    }
}