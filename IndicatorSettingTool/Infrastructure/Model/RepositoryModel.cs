using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndicatorSettingTool.Infrastructure.Model
{
    public class RepositoryModel
    {
        public string Equipment { get; set; }
        public string Chamber { get; set; }
        public string Recipe { get; set; }
        public string Parameter { get; set; }
        public string Step { get; set; }
        public double? HeadBegin { get; set; }
        public double? TailBegin { get; set; }
        public double? HeadEnd { get; set; }
        public double? TailEnd { get; set; }
        public string Statistic { get; set; }
        public double? ParameterValue { get; set; }//XML檔案用

        public double? Usl { get; set; }
        public double? Lsl { get; set; }
        public double? Ucl { get; set; }
        public double? Lcl { get; set; }
        public double? Uwl { get; set; }
        public double? Lwl { get; set; }

        //insert 用
        public int IndicatorDefInfoKey { get; set; }
        public string IndicatorName { get; set; }
        public DateTime UpdataTime { get; set; }
         

    }
    public class InsertFdcIndicatorDefInfoEventMessage
    {
        public int IsActiveChangeCount  { get; set; }
        public int InsertCount { get; set; }
        public bool IsSuccess { get; set; }


    }
}
