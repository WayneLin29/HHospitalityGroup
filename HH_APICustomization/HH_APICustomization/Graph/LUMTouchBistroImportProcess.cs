using System;
using System.Collections.Generic;
using System.Text;
using HH_APICustomization.DAC;
using HH_APICustomization.Descriptor;
using HH_APICustomization.Entity;
using PX.Data;
using PX.SM;

namespace HH_APICustomization.Graph
{
    public class LUMTouchBistroImportProcess : PXGraph<LUMTouchBistroImportProcess>
    {

        public LUMTouchBistroImportProcess()
        {
            LUMTouchBistroImportProcess self = this;
            Receiveds.SetProcessDelegate(list => DoProcess(self, list));
            Receiveds.SetProcessCaption("Process");
            Receiveds.SetProcessAllCaption("Process All");
        }

        const string CSV_FOOTER = "REPORT SUMMARY";

        #region View
        public PXFilter<ImportFilter> Filter;
        public PXFilteredProcessing<LUMTouchBistroDataReceivedV, ImportFilter,
            Where<IsNull<LUMTouchBistroDataReceivedV.isImported, False>, Equal<IsNull<Current2<ImportFilter.isImported>, False>>>> Receiveds;
        public PXSelect<LUMTBTransactionSummary> Imports;
        #endregion

        #region Method
        public static void DoProcess(LUMTouchBistroImportProcess self, List<LUMTouchBistroDataReceivedV> datas)
        {
            //因為Selected 的關係 LUMTouchBistroDataReceivedV 會被判定為異動，存檔會出事
            datas.ForEach(data => self.Caches<LUMTouchBistroDataReceivedV>().SetStatus(data, PXEntryStatus.Notchanged));

            for (int i = 0; i < datas.Count; i++)
            {
                var data = datas[i];
                PXProcessing.SetCurrentItem(datas);
                try
                {
                    using (PXTransactionScope ts = new PXTransactionScope())
                    {
                        //分析檔名
                        TouchBistro_DataReceivedEntity entity = self.ParsingFileName(data);
                        //讀取Csv
                        using (CSVReader reader = self.GetCSV(data.FileID))
                        {
                            if (entity.Type == LUMTBTransactionSummaryType.SALES_BY_MENUITEM)
                                self.DoSalesByMenuItem(reader, entity);
                            else if (entity.Type == LUMTBTransactionSummaryType.ACCOUNTS_SUMMARY)
                                self.DoAccountsSummary(reader, entity);
                            else if (entity.Type == LUMTBTransactionSummaryType.PAY_INS)
                                self.DoPayOutsIns(reader, entity);
                            else if (entity.Type == LUMTBTransactionSummaryType.PAY_OUTS)
                                self.DoPayOutsIns(reader, entity);
                        }
                        PXUpdate<Set<UploadFileHHExt.usrIsImported, True>,
                            UploadFile,
                            Where<UploadFile.fileID, Equal<Required<UploadFile.fileID>>>>
                            .Update(self, entity.FileID);
                        self.Persist();
                        ts.Complete();
                    }
                }
                catch (Exception e)
                {
                    PXProcessing.SetError<LUMTouchBistroDataReceivedV>(i, e.Message);
                }
            }

        }

        protected virtual TouchBistro_DataReceivedEntity ParsingFileName(LUMTouchBistroDataReceivedV data)
        {
            try
            {
                string ss = data.FileName.Replace(".csv", "");
                var sarray = ss.Split('-');
                int i = 0;
                string restaurantCD = sarray[i++];
                string type = sarray[i++];
                string yyyyFrom = sarray[i++];
                string mmFrom = sarray[i++];
                string ddFrom = sarray[i++];
                string yyyyTo = sarray[i++];
                string mmTo = sarray[i++];
                string ddTo = sarray[i++];

                if ("SalesByMenuItem".Equals(type))
                    type = LUMTBTransactionSummaryType.SALES_BY_MENUITEM;
                else if ("Accounts(Summary)".Equals(type))
                    type = LUMTBTransactionSummaryType.ACCOUNTS_SUMMARY;
                else if ("PayIns".Equals(type))
                    type = LUMTBTransactionSummaryType.PAY_INS;
                else if ("PayOuts".Equals(type))
                    type = LUMTBTransactionSummaryType.PAY_OUTS;
                else
                    throw new PXException("Parsing filename failed(Type):{0}", type);

                DateTime dateFrom = DateTime.Parse(String.Format("{0}-{1}-{2}", yyyyFrom, mmFrom, ddFrom));
                DateTime dateTo = DateTime.Parse(String.Format("{0}-{1}-{2}", yyyyTo, mmTo, ddTo));

                return new TouchBistro_DataReceivedEntity()
                {
                    FileID = data.FileID,
                    FileName = data.FileName,
                    RestaurantCD = restaurantCD,
                    Type = type,
                    DateFrom = dateFrom,
                    DateTo = dateTo
                };
            }
            catch (PXException e) { throw e; }
            catch (Exception)
            {
                throw new PXException("Parsing filename failed:{0}", data.FileName);
            }
        }

        protected virtual void DoSalesByMenuItem(CSVReader reader, TouchBistro_DataReceivedEntity entity)
        {
            reader.Reset();
            SalesByMenuItemColumn column = new SalesByMenuItemColumn(reader.IndexKeyPairs);
            while (reader.MoveNext())
            {
                string firstColumn = reader.GetValue(0);
                //最後一筆不處理
                if (firstColumn.Contains(CSV_FOOTER)) break;

                var item = CreateBaseImportData(entity);
                item.MenuItem = reader.GetValue(column.Get(SalesByMenuItemColumn.MenuItem));
                item.SalesCategory = reader.GetValue(column.Get(SalesByMenuItemColumn.SalesCategory));
                item.MenuGroup = reader.GetValue(column.Get(SalesByMenuItemColumn.MenuGroup));
                item.MenuItemVoidQty = GetDec(reader.GetValue(column.Get(SalesByMenuItemColumn.MenuItemVoidQuantity)));
                item.Voids = GetDec(reader.GetValue(column.Get(SalesByMenuItemColumn.Voids)));
                item.MenuItemQty = GetDec(reader.GetValue(column.Get(SalesByMenuItemColumn.MenuItemQuantity)));
                item.GrossSales = GetDec(reader.GetValue(column.Get(SalesByMenuItemColumn.GrossSales)));
                item.Discounts = GetDec(reader.GetValue(column.Get(SalesByMenuItemColumn.Disconunts)));
                item.NetSales = GetDec(reader.GetValue(column.Get(SalesByMenuItemColumn.NetSales)));
                item.Tax1 = GetDec(reader.GetValue(column.Get(SalesByMenuItemColumn.Tax1)));
                item.Tax2 = GetDec(reader.GetValue(column.Get(SalesByMenuItemColumn.Tax2)));
                item.Tax3 = GetDec(reader.GetValue(column.Get(SalesByMenuItemColumn.Tax3)));

                //Get Account & Sub
                var mapping = LUMTouchBistroPreferenceMaint.GetSalesByMenuItemAcct(this, item);
                item.AccountID = mapping?.AccountID;
                item.SubID = mapping?.SubAcctID;

                this.Imports.Insert(item);
            }

        }

        protected virtual void DoAccountsSummary(CSVReader reader, TouchBistro_DataReceivedEntity entity)
        {
            reader.Reset();
            AccountsSummaryColumn column = new AccountsSummaryColumn(reader.IndexKeyPairs);
            while (reader.MoveNext())
            {
                string firstColumn = reader.GetValue(0);
                //最後一筆不處理
                if (firstColumn.Contains(CSV_FOOTER)) break;

                var item = CreateBaseImportData(entity);
                item.AccountName = reader.GetValue(column.Get(AccountsSummaryColumn.AccountName));
                item.Payments = GetDec(reader.GetValue(column.Get(AccountsSummaryColumn.Payments)));
                item.Deposits = GetDec(reader.GetValue(column.Get(AccountsSummaryColumn.Deposits)));
                item.ChargedToAccount = GetDec(reader.GetValue(column.Get(AccountsSummaryColumn.ChargedToAccount)));
                item.Subtotal = GetDec(reader.GetValue(column.Get(AccountsSummaryColumn.Subtotal)));
                item.Tips = GetDec(reader.GetValue(column.Get(AccountsSummaryColumn.Tips)));
                item.Total = GetDec(reader.GetValue(column.Get(AccountsSummaryColumn.Total)));

                //Get Account & Sub
                var mapping = LUMTouchBistroPreferenceMaint.GetAccountsSummaryAcct(this, item);
                item.AccountID = mapping?.AccountID;
                item.SubID = mapping?.SubAcctID;

                this.Imports.Insert(item);
            }
        }

        protected virtual void DoPayOutsIns(CSVReader reader, TouchBistro_DataReceivedEntity entity)
        {
            reader.Reset();
            PayOutsInsColumn column = new PayOutsInsColumn(reader.IndexKeyPairs);
            while (reader.MoveNext())
            {
                string firstColumn = reader.GetValue(0);
                //最後一筆不處理
                if (firstColumn.Contains(CSV_FOOTER)) break;

                var item = CreateBaseImportData(entity);
                item.Server = reader.GetValue(column.Get(PayOutsInsColumn.Server));
                var _dateTimestamp = reader.GetValue(column.Get(PayOutsInsColumn.DateTimestamp));
                if (_dateTimestamp != null && _dateTimestamp != "")
                {
                    item.DateTimestamp = DateTime.Parse(_dateTimestamp);
                }
                item.Reason = reader.GetValue(column.Get(PayOutsInsColumn.Reason));
                item.Amount = GetDec(reader.GetValue(column.Get(PayOutsInsColumn.Amount)));
                item.Register = reader.GetValue(column.Get(PayOutsInsColumn.Register));
                item.Comment = reader.GetValue(column.Get(PayOutsInsColumn.Comment));

                //Get Account & Sub
                var mapping = LUMTouchBistroPreferenceMaint.GetPayOutsInsAcct(this, item);
                item.AccountID = mapping?.AccountID;
                item.SubID = mapping?.SubAcctID;

                this.Imports.Insert(item);
            }
        }

        protected virtual LUMTBTransactionSummary CreateBaseImportData(TouchBistro_DataReceivedEntity entity)
        {
            var preference = LUMTouchBistroPreference.UK.Find(this, entity.RestaurantCD);
            if (preference == null) throw new PXException("[{0}]RestaurantID not found.", entity.RestaurantCD);
            return new LUMTBTransactionSummary()
            {
                RestaurantID = preference.RestaurantID,
                FileID = entity.FileID,
                FileName = entity.FileName,
                Date = entity.DateFrom,
                DataType = entity.Type,
                IsImported = false
            };
        }

        protected decimal? GetDec(string str)
        {
            if (str == null) return null;
            return Decimal.Parse(str.Replace("$", "").Trim());
        }

        protected virtual CSVReader GetCSV(Guid? fileID)
        {
            UploadFileRevision file = PXSelectJoin<UploadFileRevision,
                InnerJoin<UploadFile, On<UploadFileRevision.fileID, Equal<UploadFile.fileID>,
                And<UploadFileRevision.fileRevisionID, Equal<UploadFile.lastRevisionID>>>>,
                Where<UploadFile.fileID, Equal<Required<UploadFile.fileID>>>>.Select(this, fileID);
            return new CSVReader(file.Data, Encoding.UTF8.CodePage);
        }

        #endregion

        #region Table
        [Serializable]
        [PXCacheName("Import Filter")]
        public class ImportFilter : IBqlTable
        {
            #region IsImported
            [PXBool()]
            [PXUIField(DisplayName = "Is Imported")]
            [PXUnboundDefault(false)]
            public virtual bool? IsImported { get; set; }
            public abstract class isImported : PX.Data.BQL.BqlBool.Field<isImported> { }
            #endregion
        }
        #endregion

        #region CSV Column Type
        protected class BaseColumn
        {
            readonly IDictionary<string, int> column;

            public BaseColumn(IDictionary<int, string> header)
            {
                column = new Dictionary<string, int>();
                foreach (var index in header.Keys)
                {
                    column.Add(header[index], index);
                }
            }

            public int Get(string columnName)
            {
                return column[columnName];
            }
        }


        protected class SalesByMenuItemColumn : BaseColumn
        {
            public SalesByMenuItemColumn(IDictionary<int, string> header) : base(header) { }
            public const string MenuItem = "Menu Item";
            public const string SalesCategory = "Sales Category";
            public const string MenuGroup = "Menu Group";
            public const string MenuItemVoidQuantity = "Menu Item Void Quantity";
            public const string Voids = "Voids";
            public const string MenuItemQuantity = "Menu Item Quantity";
            public const string GrossSales = "Gross Sales";
            public const string Disconunts = "Discounts";
            public const string NetSales = "Net Sales";
            public const string SalesPercentage = "Sales Percentage";
            public const string Tax1 = "Tax 1";
            public const string Tax2 = "Tax 2";
            public const string Tax3 = "Tax 3";
        }

        protected class AccountsSummaryColumn : BaseColumn
        {
            public AccountsSummaryColumn(IDictionary<int, string> header) : base(header) { }
            public const string AccountName = "Account Name";
            public const string Payments = "Payments";
            public const string Deposits = "Deposits";
            public const string ChargedToAccount = "Charged to Account";
            public const string Subtotal = "Subtotal";
            public const string Tips = "Tips";
            public const string Total = "Total";
        }

        protected class PayOutsInsColumn : BaseColumn
        {
            public PayOutsInsColumn(IDictionary<int, string> header) : base(header) { }
            public const string Server = "Server";
            public const string DateTimestamp = "Date/Timestamp";
            public const string Reason = "Reason";
            public const string Amount = "Amount";
            public const string Register = "Register";
            public const string Comment = "Comment";
        }
        #endregion

        #region BQL
        #endregion
    }
}