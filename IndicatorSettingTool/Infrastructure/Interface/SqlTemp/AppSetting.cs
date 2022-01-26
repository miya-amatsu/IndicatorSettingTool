using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndicatorSettingTool.Infrastructure.Interface.SqlTemp
{
    public class AppSettings
    {
        public DBSetting DBSetting { get; set; }
        public SqlTemplate SqlTemplate { get; set; }
    }

    public class DBSetting
    {
        public string DBType { get; set; }
      
        public string ConnectionStrings { get; set; }
    }
    public class SqlTemplate
    {
        //取得 FDC_INDICATOR_DEF_INFO 的 所有 EQUIPMENT
        public string GetForm1Equipment { get; set; }
        public string GetForm1Chamber { get; set; }
        public string GetForm1Recipe { get; set; }
        public string GetForm1ConditionQueryData { get; set; }
        public string GetFdcIndicatorDefInfoNextval { get; set; }
        public string UpDataFdcIndicatorDefInfoHasIndicatorName { get; set; }
        public string InsertFdcIndicatorDefInfo { get; set; }
        public string GetTransferModelEquipment { get; set; }
        public string GetTransferModelChamber { get; set; }
        public string GetTransferModelRecipe { get; set; }





    }
}
