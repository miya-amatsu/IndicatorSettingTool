using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndicatorSettingTool.Infrastructure.ViewModel
{
    public class TransferModelConditionModel
    {
        public IEnumerable<string> Equipment { set; get; }
        public IEnumerable<string> Chamber { set; get; }
        public IEnumerable<string> Recipe { set; get; }
        public TransferModelConditionModel()
        {
            this.Equipment = Enumerable.Empty<string>();
            this.Chamber = Enumerable.Empty<string>();
            this.Recipe = Enumerable.Empty<string>();
        }
    }
    public enum TransferModelConditionModelRequireEnum
    {
        Equipment, Chamber, Recipe
    }
}
