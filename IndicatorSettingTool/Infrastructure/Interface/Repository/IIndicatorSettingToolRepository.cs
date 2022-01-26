using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndicatorSettingTool.Infrastructure.Interface.View;
using IndicatorSettingTool.Infrastructure.ViewModel;
using IndicatorSettingTool.Infrastructure.Model;
namespace IndicatorSettingTool.Infrastructure.Interface.Repository
{
    public interface IIndicatorSettingToolRepository
    {
        void SetView(IIndicatorSettingToolViewInterface form);
        IEnumerable<string> GetForm1Equipment(FormConditionModel value);
        IEnumerable<string> GetForm1Chamber(FormConditionModel value);
        IEnumerable<string> GetForm1Recipe(FormConditionModel value);
        IEnumerable<RepositoryModel> GetForm1ConditionQueryData(FormConditionModel value);
        bool InsertIndicatorDefInfoData(IEnumerable<RepositoryModel> value);


        IEnumerable<string> GetTransferModelEquipment(TransferModelConditionModel value);
        IEnumerable<string> GetTransferModelChamber(TransferModelConditionModel value);

        IEnumerable<string> GetTransferModelRecipe(TransferModelConditionModel value);

    }
}
