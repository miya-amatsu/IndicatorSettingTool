using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndicatorSettingTool.Infrastructure.ViewModel
{
    //Condition的選單順序 命名須與Form1ConditionModel的屬性名稱一樣
    public enum FormConditionEnum { Equipment, Chamber, Recipe }
    //Condition的選單順序
    public class FormConditionModel
    {
        public string Equipment { set; get; }
        public string Chamber { set; get; }
        public string Recipe { set; get; }
        public FormConditionModel()
        {
            this.Equipment = default(string);
            this.Chamber = default(string);
            this.Recipe = default(string);
        }
    }
 
    public class FormQueryConditionStateModel
    {
        //當前顯示的
        public FormConditionModel Current { set; get; }
        //當前已Query的
        public FormConditionModel Queried { set; get; }
        public FormQueryConditionStateModel()
        {
            this.Current = new FormConditionModel();
            this.Queried = new FormConditionModel();
        }
      
    }
}
