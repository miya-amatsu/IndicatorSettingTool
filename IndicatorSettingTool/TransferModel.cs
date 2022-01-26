using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IndicatorSettingTool.Infrastructure.Interface.Repository;
using IndicatorSettingTool.Infrastructure.Interface.View;
using IndicatorSettingTool.Infrastructure.ViewModel;

namespace IndicatorSettingTool
{
    public partial class TransferModel : Form, ITransferModel
    {
        private IIndicatorSettingToolRepository _repository;
        public IIndicatorSettingToolViewInterface _view;
        private IEnumerable<string> _checkedListBoxItems;
        private TransferModelConditionModel _form;
        private TransferModelSearchType _type;


    public TransferModel(IIndicatorSettingToolRepository repository, IIndicatorSettingToolViewInterface view, TransferModelSearchType type, TransferModelConditionModel form)
        {
            InitializeComponent();
            this._repository = repository;
            this._view = view;
            this._form = form;
            this._type = type;
            this.SetSearchType(type, form);
        }

      
        private void SetSearchType(TransferModelSearchType value, TransferModelConditionModel form)
        {
          
            switch (value)
            {
                case TransferModelSearchType.Equipment:
                    this._checkedListBoxItems = _repository.GetTransferModelEquipment(form);
                    break;
                case TransferModelSearchType.Chamber:
                    this._checkedListBoxItems = _repository.GetTransferModelChamber(form);
                    break;
                case TransferModelSearchType.Recipe:
                    this._checkedListBoxItems = _repository.GetTransferModelRecipe(form);
                    break;
                default:
                    break;
            }
            this.SetCheckedListBoxItems();
        }

        private void SetCheckedListBoxItems()
        {
            this.checkedListBoxItemDesearch.Items.AddRange(this._checkedListBoxItems.ToArray());
        }
        private void buttonSelectd_Click(object sender, EventArgs e)
        {
            foreach (var item in this.checkedListBoxItemDesearch.CheckedItems.OfType<string>().ToList())
            {
                this.checkedListBoxItemSearch.Items.Add(item);
                this.checkedListBoxItemDesearch.Items.Remove(item);
            }
            this.SetCondition();
        }

        private void buttonDeselectd_Click(object sender, EventArgs e)
        {
            foreach (var item in this.checkedListBoxItemSearch.CheckedItems.OfType<string>().ToList())
            {
                this.checkedListBoxItemDesearch.Items.Add(item);
                this.checkedListBoxItemSearch.Items.Remove(item);
            }
            this.SetCondition();
        }

        private void buttonAllSelectd_Click(object sender, EventArgs e)
        {
            foreach (var item in this.checkedListBoxItemDesearch.Items.OfType<string>().ToList())
            {
                this.checkedListBoxItemSearch.Items.Add(item);
                this.checkedListBoxItemDesearch.Items.Remove(item);
            }
            this.SetCondition();
        }

        private void buttonAllDeselectd_Click(object sender, EventArgs e)
        {
            foreach (var item in this.checkedListBoxItemSearch.Items.OfType<string>().ToList())
            {
                this.checkedListBoxItemDesearch.Items.Add(item);
                this.checkedListBoxItemSearch.Items.Remove(item);
            }
            this.SetCondition();
        }

        private void textBoxSearchItem_TextChanged( object sender, EventArgs e)
        {
            var text = ((TextBox)sender).Text;
            if (string.IsNullOrEmpty(text))
            {
                 
                this.checkedListBoxItemDesearch.Items.Clear();
                this.checkedListBoxItemDesearch.Items.AddRange(this._checkedListBoxItems.ToArray());
            }
            else
            {
                var items = this._checkedListBoxItems.Where(item => item.Contains(text)).ToArray();
                this.checkedListBoxItemDesearch.Items.Clear();
                this.checkedListBoxItemDesearch.Items.AddRange(items);
            }
           

        }

      

        private void buttonSetCondition_Click(object sender, EventArgs e)
        {
            this._view.SetTransferModelConditionModel(this._form);
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void SetCondition()
        {
             
            switch (this._type)
            {
                case TransferModelSearchType.Equipment:
                    this._form.Equipment = this.checkedListBoxItemSearch.Items.OfType<string>().ToList();
                    this._form.Chamber = Enumerable.Empty<string>();
                    this._form.Recipe = Enumerable.Empty<string>();

                    break;
                case TransferModelSearchType.Chamber:
                    this._form.Chamber = this.checkedListBoxItemSearch.Items.OfType<string>().ToList();
                    
                    this._form.Recipe = Enumerable.Empty<string>();
                    break;
                case TransferModelSearchType.Recipe:
                    this._form.Recipe = this.checkedListBoxItemSearch.Items.OfType<string>().ToList();
 
                    break;
                default:
                    break;
            }
        }
    }
}
