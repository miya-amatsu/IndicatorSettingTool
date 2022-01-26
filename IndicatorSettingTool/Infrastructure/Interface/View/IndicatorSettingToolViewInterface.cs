using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndicatorSettingTool.Infrastructure.ViewModel;
using IndicatorSettingTool.Repository;
using IndicatorSettingTool.Infrastructure.Interface.Repository;
namespace IndicatorSettingTool.Infrastructure.Interface.View
{
    public interface IIndicatorSettingToolViewInterface
    {
         void SetMessage(string message, TextStyleEnum type);
         void SetRepository(IIndicatorSettingToolRepository Repository);
         void SetTransferModelConditionModel(TransferModelConditionModel value);

        
    }
}
