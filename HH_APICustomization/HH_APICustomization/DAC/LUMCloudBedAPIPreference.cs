using System;
using PX.Data;

namespace HH_APICustomization.DAC
{
    [Serializable]
    [PXCacheName("LUMCloudBedAPIPreference")]
    public class LUMCloudBedAPIPreference : IBqlTable
    {
        #region ClientID
        [PXDBString(256, IsUnicode = true)]
        [PXUIField(DisplayName = "Client_ID")]
        public virtual string ClientID { get; set; }
        public abstract class clientID : PX.Data.BQL.BqlString.Field<clientID> { }
        #endregion

        #region ClientSecret
        [PXDBString(256, IsUnicode = true)]
        [PXUIField(DisplayName = "Client_Secret")]
        public virtual string ClientSecret { get; set; }
        public abstract class clientSecret : PX.Data.BQL.BqlString.Field<clientSecret> { }
        #endregion

        #region OauthUrl
        [PXDBString(1024, IsUnicode = true)]
        [PXUIField(DisplayName = "OAuth URL")]
        public virtual string OauthUrl { get; set; }
        public abstract class oauthUrl : PX.Data.BQL.BqlString.Field<oauthUrl> { }
        #endregion

        #region OauthCODE
        [PXDBString(256, IsUnicode = true)]
        [PXUIField(DisplayName = "CODE")]
        public virtual string OauthCODE { get; set; }
        public abstract class oauthCODE : PX.Data.BQL.BqlString.Field<oauthCODE> { }
        #endregion

        #region AccessToken
        [PXDBString(1024,IsUnicode = true)]
        [PXUIField(DisplayName = "Access_token")]
        public virtual string AccessToken { get; set; }
        public abstract class accessToken : PX.Data.BQL.BqlString.Field<accessToken> { }
        #endregion

        #region RefreshToken
        [PXDBString(1024, IsUnicode = true)]
        [PXUIField(DisplayName = "Refresh_token")]
        public virtual string RefreshToken { get; set; }
        public abstract class refreshToken : PX.Data.BQL.BqlString.Field<refreshToken> { }
        #endregion

        #region RefreshTokenExpiresTime
        [PXDBDateAndTime]
        [PXUIField(DisplayName = "Refresh_token ExpiresTime", Enabled = false)]
        public virtual DateTime? RefreshTokenExpiresTime { get; set; }
        public abstract class refreshTokenExpiresTime : PX.Data.BQL.BqlDateTime.Field<refreshTokenExpiresTime> { }
        #endregion

        #region WebHookUrl
        [PXDBString(1024,IsUnicode = true)]
        [PXUIField(DisplayName = "Acumatica WebHook Url")]
        public virtual string WebHookUrl { get;set;}
        public abstract class webHookUrl : PX.Data.BQL.BqlString.Field<webHookUrl> { }
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

        #region Tstamp
        [PXDBTimestamp()]
        [PXUIField(DisplayName = "Tstamp")]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
        #endregion
    }
}