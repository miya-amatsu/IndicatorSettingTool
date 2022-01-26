using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndicatorSettingTool.Infrastructure.Interface.SqlTemp;
using System.IO;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Types;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Dapper;
using IndicatorSettingTool.Infrastructure.ViewModel;
using IndicatorSettingTool.Infrastructure.Model;
using IndicatorSettingTool.Infrastructure.Interface.Repository;
using IndicatorSettingTool.Infrastructure.Interface.View;

namespace IndicatorSettingTool.Repository
{
    public class Form1Repository : IIndicatorSettingToolRepository

    {
        public IIndicatorSettingToolViewInterface _view;

        public DBSetting _dbSetting;
        public SqlTemplate _sqlTemplate;
        public Form1Repository( )
        {
            this.GetAppSettings();
          
        }
        public  void SetView(IIndicatorSettingToolViewInterface view)
        {
            this._view = view;
        }

        /// <summary>  
        /// 取得 config
        /// </summary>  

        private void GetAppSettings()
        {
            try
            {
                using (StreamReader r = new StreamReader("../../AppSettings.json"))
                {
                    string json = r.ReadToEnd();
                    AppSettings items = JsonConvert.DeserializeObject<AppSettings>(json);
                    this._dbSetting = items.DBSetting;
                    this._sqlTemplate = items.SqlTemplate;
                }
            }
            catch (Exception ex)
            {
                this._view.SetMessage(ex.Message, TextStyleEnum.Danger);
                throw;
            }
        }


        public IEnumerable<string> GetForm1Equipment(FormConditionModel value)
        {
            try
            {
                var result = Enumerable.Empty<string>();
                using (OracleConnection conn = new OracleConnection(this._dbSetting.ConnectionStrings))
                {
                    result = conn.Query<string>(this._sqlTemplate.GetForm1Equipment);
                }
                return result;
            }
            catch (Exception ex)
            {
                this._view.SetMessage(ex.Message, TextStyleEnum.Danger);
                throw;
            }
        }

        public IEnumerable<string> GetForm1Chamber(FormConditionModel value)
        {
            try
            {
                var result = Enumerable.Empty<string>();
                using (OracleConnection conn = new OracleConnection(this._dbSetting.ConnectionStrings))
                {
                    result = conn.Query<string>(this._sqlTemplate.GetForm1Chamber, value);
                }
                return result;
            }
            catch (Exception ex)
            {
                this._view.SetMessage(ex.Message, TextStyleEnum.Danger);

                throw;
            }
        }
        public IEnumerable<string> GetForm1Recipe(FormConditionModel value)
        {
            try
            {
                var result = Enumerable.Empty<string>();
                using (OracleConnection conn = new OracleConnection(this._dbSetting.ConnectionStrings))
                {
                    result = conn.Query<string>(this._sqlTemplate.GetForm1Recipe, value);
                }
                return result;
            }
            catch (Exception ex)
            {
                this._view.SetMessage(ex.Message, TextStyleEnum.Danger);
                throw;
            }
        }


        public IEnumerable<RepositoryModel> GetForm1ConditionQueryData(FormConditionModel value)
        {
            try
            {
                var result = Enumerable.Empty<RepositoryModel>();
                using (OracleConnection conn = new OracleConnection(this._dbSetting.ConnectionStrings))
                {
                    result = conn.Query<RepositoryModel>(this._sqlTemplate.GetForm1ConditionQueryData, value);
                }
                return result;
            }
            catch (Exception ex)
            {
                this._view.SetMessage(ex.Message, TextStyleEnum.Danger);
                throw;
            }
        }


        public bool InsertIndicatorDefInfoData(IEnumerable<RepositoryModel> value)
        {
            try
            {
                var result = true;
                using (OracleConnection conn = new OracleConnection(this._dbSetting.ConnectionStrings))
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                        conn.Open();
                    using (var trans = conn.BeginTransaction())
                    {
                        try
                        {
                            var keySql = this._sqlTemplate.GetFdcIndicatorDefInfoNextval;
                            var updataSql = this._sqlTemplate.UpDataFdcIndicatorDefInfoHasIndicatorName;
                            var insertSql = this._sqlTemplate.InsertFdcIndicatorDefInfo;

                            foreach (var item in value)
                            {
                                item.IndicatorDefInfoKey = conn.QueryFirst<int>(keySql);
                                conn.Execute(updataSql, item);
                                conn.Execute(insertSql, item);
                            }
                        }
                        catch (Exception ex)
                        {
                            this._view.SetMessage(ex.Message, TextStyleEnum.Danger);
                            trans.Rollback();
                            conn.Close();
                            result = false;
                        }
                        finally
                        {
                            trans.Commit();
                            conn.Close();
                        }
                    }   
                }
                return result;
            }
            catch (Exception ex)
            {
                this._view.SetMessage(ex.Message, TextStyleEnum.Danger);
                throw;
            }
        }

        public IEnumerable<string> GetTransferModelEquipment(TransferModelConditionModel value)
        {
            try
            {
                var result = Enumerable.Empty<string>();
                using (OracleConnection conn = new OracleConnection(this._dbSetting.ConnectionStrings))
                {
                    result = conn.Query<string>(this._sqlTemplate.GetTransferModelEquipment, value);
                }
                return result;
            }
            catch (Exception ex)
            {
                this._view.SetMessage(ex.Message, TextStyleEnum.Danger);
                throw;
            }

        }

        public IEnumerable<string> GetTransferModelChamber(TransferModelConditionModel value)
        {
            try
            {
                var result = Enumerable.Empty<string>();
                using (OracleConnection conn = new OracleConnection(this._dbSetting.ConnectionStrings))
                {
                    result = conn.Query<string>(this._sqlTemplate.GetTransferModelChamber, value);
                }
                return result;
            }
            catch (Exception ex)
            {
                this._view.SetMessage(ex.Message, TextStyleEnum.Danger);
                throw;
            }

        }

        public IEnumerable<string> GetTransferModelRecipe(TransferModelConditionModel value)
        {
            try
            {
                var result = Enumerable.Empty<string>();
                using (OracleConnection conn = new OracleConnection(this._dbSetting.ConnectionStrings))
                {
                    result = conn.Query<string>(this._sqlTemplate.GetTransferModelRecipe, value);
                }
                return result;
            }
            catch (Exception ex)
            {
                this._view.SetMessage(ex.Message, TextStyleEnum.Danger);
                throw;
            }

        }
    }
}


 