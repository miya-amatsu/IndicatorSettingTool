using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndicatorSettingTool
{
    class IndicatorSettingToolXMLDataModel
    {
        public List<string> Parameter;
        public string EQID;
        public string ChamberID;
        public string Recipe;
        public string RawTrace;
        public string WaferInfo;
    }

     public enum BeginTypeEnum {Head,Tail };
    public enum EndTypeEnum { Head, Tail };
    public enum StatisticsEnum { MAX, MIN,MEAN, STDDEV , PROCESSTIME };

    public class IndicatorSettingToolGridDataColumnModel
    {
        public bool IsSelect { get; set; }
        public string Step { get; set; }
        public string BeginType { get; set; }
        public string EndType { get; set; }
        public string Statistics { get; set; }
        public double? BeginValue { get; set; }
        public double? EndValue { get; set; }
        public double? Usl { get; set; }
        public double? Lsl { get; set; }
        public double? Ucl { get; set; }
        public double? Lcl { get; set; }
        public double? Uwl { get; set; }
        public double? Lwl { get; set; }

        public IndicatorSettingToolGridDataColumnModel( )
        {
            this.IsSelect = false;
            this.Step = default(string);
            this.BeginType = BeginTypeEnum.Head.ToString();
            this.BeginValue = default(double?); 
            this.EndType = EndTypeEnum.Head.ToString();
            this.EndValue = default(double?);
            this.Statistics = StatisticsEnum.MEAN.ToString();
            this.Usl = default(double?);
            this.Lsl = default(double?);
            this.Ucl = default(double?);
            this.Lcl = default(double?);
            this.Uwl = default(double?);
            this.Lwl = default(double?);
        }
    }
    public class IndicatorSettingToolGridDataModel: IndicatorSettingToolGridDataColumnModel
    {
        public int RowIndex { get; set; }
    }
}
