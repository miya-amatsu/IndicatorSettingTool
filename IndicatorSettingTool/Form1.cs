using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using System.Reflection;
using System.Dynamic;
using Oracle.ManagedDataAccess.Types;
using IndicatorSettingTool.Repository;
using Oracle.ManagedDataAccess.Client;
using IndicatorSettingTool.Infrastructure.ViewModel;
using IndicatorSettingTool.Infrastructure.Model;
using IndicatorSettingTool.Infrastructure.Interface.Repository;
using IndicatorSettingTool.Infrastructure.Interface.View;
namespace IndicatorSettingTool
{
    public partial class Form1 : Form, IIndicatorSettingToolViewInterface
    {
        private IEnumerable<string> _indicatorSettingToolParameterFilter = new List<string>();
        //grid的顯示資料
        private DataTable _dataTableTemp;
        //Xml的資料暫存
        private IEnumerable<RepositoryModel> _dataTableSourceXml;
        //Db import的資料暫存
        private IEnumerable<RepositoryModel> _dataTableSourceResult;
        //Db import的QueryForm，有當前的跟已Query的兩種不同狀態的Condition
        private FormQueryConditionStateModel _queryFormGroup;

        //連DB使用的Repository 
        private IIndicatorSettingToolRepository _repository;

        //Db import的多選 QueryForm
        private TransferModelConditionModel _transferModelCondition;
        // 多選 QueryForm的必填項目
        private TransferModelConditionModelRequireEnum _transferModelRequireEnum;

        public Form1()
        {
            InitializeComponent();
        }
        public void SetRepository(IIndicatorSettingToolRepository Repository)
        {
            this._repository = Repository;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //XML有 Parameter， 設定要過濾的Parameter，會將過濾後的Parameter呈現在 combobox 上
            this._indicatorSettingToolParameterFilter = ConfigurationManager.AppSettings["IndicatorSettingToolParameterFilter"].Split(',');
            //設定Data Grid的 combobox 選項
            this.BeginType.Items.AddRange(Enum.GetNames(typeof(BeginTypeEnum)));
            this.EndType.Items.AddRange(Enum.GetNames(typeof(EndTypeEnum)));
            this.Statistics.Items.AddRange(Enum.GetNames(typeof(StatisticsEnum)));

            //初始化
            this._queryFormGroup = new FormQueryConditionStateModel();
            this._dataTableSourceXml = Enumerable.Empty<RepositoryModel>();
            this._dataTableSourceResult = Enumerable.Empty<RepositoryModel>();
            this._transferModelCondition = new TransferModelConditionModel();
        }

        // File Load Button Click 觸發行為
        private void XMLFileLoadButton_Click(object sender, EventArgs e)
        {
            // 開啟檔案選擇器 ，Filter檔案類型 (*.xml)
            this.XMLOpenFileDialog.ShowDialog();
        }

        //寫入Message
        public void SetMessage(string s, TextStyleEnum type)
        {
            switch (type)
            {
                case TextStyleEnum.Normal:
                    this.richTextBox1.SelectionColor = Color.Black;
                    this.richTextBox1.AppendText(s + "\r\n");
                    break;
                case TextStyleEnum.Warning:
                    this.richTextBox1.SelectionColor = Color.Blue;
                    this.richTextBox1.AppendText(s + "\r\n");
                    break;
                case TextStyleEnum.Danger:
                    this.richTextBox1.SelectionColor = Color.Red;
                    this.richTextBox1.AppendText(s + "\r\n");
                    break;
                default:
                    break;
            }
 
        }

        //檔案選擇 確定按鈕點擊後觸發
        private void XMLOpenFileDialog_FileOk(object sender, CancelEventArgs cancelEvent)
        {
            try
            {
                string filePath = this.XMLOpenFileDialog.FileName.ToString();
                this.XMLFileLoadTextBox.Text = filePath;
                XmlReader reader = XmlReader.Create(filePath);
                //保存XML資料
                var IndicatorSettingToolXMLData = new IndicatorSettingToolXMLDataModel();
                this.SetMessage("XML file = " + filePath, TextStyleEnum.Normal);
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name.ToString())
                        {
                            case "Parameter":
                                this.ParameterComboBox.Items.Clear();
                                var source = reader.ReadElementContentAsString().Replace("\n", "").Replace("\t", "");
                                IndicatorSettingToolXMLData.Parameter = source.Split(',').ToList();
                                break;
                            case "EQID":
                                IndicatorSettingToolXMLData.EQID = (reader.ReadElementContentAsString()).ToString();
                                this.IndicatorSettingToolLabelEqpValue.Text = IndicatorSettingToolXMLData.EQID;
                                break;
                            case "ChamberID":
                                IndicatorSettingToolXMLData.ChamberID = reader.ReadElementContentAsString().ToString();
                                this.IndicatorSettingToolLabelChamberValue.Text = IndicatorSettingToolXMLData.ChamberID;
                                break;
                            case "Recipe":
                                IndicatorSettingToolXMLData.Recipe = reader.ReadElementContentAsString().ToString();
                                this.IndicatorSettingToolLabelRecipeValue.Text = IndicatorSettingToolXMLData.Recipe;
                                break;
                            case "RawTrace":
                                IndicatorSettingToolXMLData.RawTrace = reader.ReadElementContentAsString().ToString();
                                // this._richTextBox1StringList.Add("\r\n" + "The " + $"{reader.Name.ToString()}" + " of the FDC is " + IndicatorSettingToolXMLData.RawTrace);
                                break;
                        }
                    }
                }
                this.GetXMLGridData(IndicatorSettingToolXMLData);
            }
            catch (Exception ex)
            {
                this.SetMessage(ex.Message, TextStyleEnum.Danger);
                throw;
            }
        }


        /// <summary>
        /// 將XML處理成 RepositoryModel
        /// </summary>
        /// <param name="source"></param>
        private void GetXMLGridData(IndicatorSettingToolXMLDataModel source)
        {
            var rawTrace = source.RawTrace.Split(new string[] { "\n\t\t" }, StringSplitOptions.RemoveEmptyEntries);
            var stepIndex = source.Parameter.IndexOf(ConfigurationManager.AppSettings["IndicatorSettingToolParameterStepUse"].ToString());
            var parameValuetList = new List<string>();
            if (stepIndex > -1)
            {
                var dataRowSource = new List<RepositoryModel>();
                var dataRow = new List<RepositoryModel>();
                this._dataTableSourceXml = Enumerable.Empty<RepositoryModel>();
                foreach (var item in rawTrace)
                {
                    var itemArray = item.Split(',').ToArray();
                    //過濾不要進入 Parameter 下拉的屬性
                    var parameterComboBoxItems = source.Parameter.Except(this._indicatorSettingToolParameterFilter).ToArray();
                    var ParameterStratIndex = itemArray.Count() - parameterComboBoxItems.Count();
                    for (int i = 0; i < parameterComboBoxItems.Count(); i++)
                    {
                        double isNum;
                        double? parameterValue = default(double?);
                        var value = itemArray[i + ParameterStratIndex];
                        if (double.TryParse(value, out isNum))
                        {
                            parameterValue = isNum;
                        }
                        dataRowSource.Add(new RepositoryModel()
                        {
                            Equipment = source.EQID,
                            Chamber = source.ChamberID,
                            Recipe = source.Recipe,
                            Parameter = parameterComboBoxItems[i],
                            Step = itemArray[stepIndex],
                            ParameterValue = parameterValue,
                            Statistic = StatisticsEnum.MEAN.ToString(),
                        });
                    }
                }

                foreach (var item in dataRowSource.GroupBy(e => new { e.Step, e.Parameter }))
                {
                    dataRow.Add(new RepositoryModel()
                    {
                        Equipment = source.EQID,
                        Chamber = source.EQID,
                        Recipe = source.EQID,
                        Parameter = item.Key.Parameter,
                        Step = item.Key.Step,
                        ParameterValue = default(double),
                        Statistic = StatisticsEnum.MEAN.ToString(),
                    });
                }
                this._dataTableSourceXml = dataRow;
                this.SetParameterComboBoxOption();
                this.RepositoryModelToRowData(this._dataTableSourceXml);
            }
            else
            {
                this.SetMessage($"Can't find parameter { ConfigurationManager.AppSettings["IndicatorSettingToolParameterStepUse"]} in data", TextStyleEnum.Danger);
            }
        }

        //複製點擊欄位
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;

            //確認點擊欄位格為image 且為第一個column,
            if (e.RowIndex > -1 && grid[e.ColumnIndex, e.RowIndex] is DataGridViewImageCell && e.ColumnIndex == 0)
            {
                var tr = ((DataRowView)((DataGridViewRow)grid.Rows[e.RowIndex]).DataBoundItem).Row.ItemArray;
                DataRow inserDataRow = _dataTableTemp.NewRow();
                PropertyInfo[] props = typeof(IndicatorSettingToolGridDataColumnModel).GetProperties();
                for (int i = 0; i < tr.Length; i++)
                {
                    inserDataRow[props[i].Name] = tr[i];
                }
                this._dataTableTemp.Rows.InsertAt(inserDataRow, e.RowIndex);
                this.dataGridView1.DataSource = new DataView(this._dataTableTemp);
            }
        }

        //非數字被輸入時阻止行為
        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if ((this.dataGridView1.CurrentCell.ValueType).Equals(typeof(double)))
            {
                double isNum = default(double);
                var value = e.FormattedValue.ToString();
                if (double.TryParse(value, out isNum))
                {
                    e.Cancel = false;
                }
                else if (string.IsNullOrEmpty(value))
                {
                    e.Cancel = false;
                }
                else
                {
                    this.SetMessage("Please insert number", TextStyleEnum.Danger);
                    e.Cancel = true;
                }
            }


        }

        /// <summary>
        /// 全選
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxAllSelect_Click(object sender, EventArgs e)
        {
            if (this._dataTableTemp != null && this._dataTableTemp.Rows.Count > 0)
            {
                foreach (DataRow dr in this._dataTableTemp.Rows)
                {
                    List<string> props = (typeof(IndicatorSettingToolGridDataColumnModel).GetProperties()).Select(prop => prop.Name).ToList();
                    var Index = props.IndexOf("IsSelect");
                    dr["IsSelect"] = true;
                }
            }
            this.dataGridView1.DataSource = new DataView(this._dataTableTemp);
        }

        /// <summary>
        /// 全部不選
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxAllDeselect_Click(object sender, EventArgs e)
        {
            if (this._dataTableTemp != null && this._dataTableTemp.Rows.Count > 0)
            {
                foreach (DataRow dr in this._dataTableTemp.Rows)
                {
                    List<string> props = (typeof(IndicatorSettingToolGridDataColumnModel).GetProperties()).Select(prop => prop.Name).ToList();
                    var Index = props.IndexOf("IsSelect");
                    dr["IsSelect"] = false;
                }
            }
        }


        /// <summary>
        ///  Parameter 變化時重新整理 grid 資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParameterComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.ParameterComboBox.SelectedIndex > -1)
            {
                switch (this.TabControl.SelectedIndex)
                {
                    case 0:
                        if (this._dataTableSourceXml != null)
                            this.RepositoryModelToRowData(this._dataTableSourceXml);
                        break;
                    case 1:
                        if (this._dataTableSourceResult != null)
                            this.RepositoryModelToRowData(this._dataTableSourceResult);
                        break;

                    default:
                        break;
                }
            }

        }

        /// <summary>
        /// DB撈Equipment
        /// </summary>
        private void GetForm1Equipment()
        {
            string[] result = this._repository.GetForm1Equipment(_queryFormGroup.Current).ToArray();
            this.ComboBoxEqp.Items.Clear();
            this.ComboBoxEqp.Items.AddRange(result);
            if (result.Any())
            {
                this.ComboBoxEqp.SelectedIndex = 0;
            }
            else
            {
                this.ComboBoxEqp.SelectedIndex = -1;
            }
        }
        /// <summary>
        /// 用 Equipment 去DB撈Chamber
        /// </summary>
        private void GetForm1Chamber()
        {
            string[] result = this._repository.GetForm1Chamber(_queryFormGroup.Current).ToArray();
            this.ComboBoxChamber.Items.Clear();
            this.ComboBoxChamber.Items.AddRange(result);
            if (result.Any())
            {
                this.ComboBoxChamber.SelectedIndex = 0;
            }
            else
            {
                this.ComboBoxChamber.SelectedIndex = -1;
            }
        }
        /// <summary>
        /// 用 Equipment Chamber 去DB撈Chamber
        /// </summary>
        private void GetForm1Recipe()
        {
            string[] result = this._repository.GetForm1Recipe(_queryFormGroup.Current).ToArray();
            this.ComboBoxRecipe.Items.Clear();
            this.ComboBoxRecipe.Items.AddRange(result);
            if (result.Any())
            {
                this.ComboBoxRecipe.SelectedIndex = 0;
            }
            else
            {
                this.ComboBoxRecipe.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Tab變化時處理相應的資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.TabControl.SelectedIndex)
            {
                case 0:
                    this.panel2.Visible = false;
                    this.panel3.Visible = false;
                    if (this._dataTableSourceXml.Any())
                    {
                        this.SetParameterComboBoxOption();
                    }
                    else
                    {
                        this.ParameterComboBox.Items.Clear();
                    }
                    this.RepositoryModelToRowData(this._dataTableSourceXml);

                    break;
                case 1:
                    this.panel2.Visible = true;
                    this.radioButtonOriginaQueryCondition.Checked = true;

                    if (this._dataTableSourceResult.Any())
                    {
                        this.SetParameterComboBoxOption();
                    }
                    else
                    {
                        this.ParameterComboBox.Items.Clear();
                    }
                    this.RepositoryModelToRowData(this._dataTableSourceResult);
                    this.GetForm1Equipment();
                    break;

                default:
                    break;
            }
        }


        /// <summary>
        /// ComboBoxEqp變化時Form,ComboBox 狀態更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxEqp_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._queryFormGroup.Current.Equipment = ComboBoxEqp.Text;
            this.Form1ConditionChanged(FormConditionEnum.Equipment);
        }
        /// <summary>
        /// ComboBoxEqp變化時Form,ComboBox 狀態更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxChamber_SelectedIndexChanged(object sender, EventArgs e)
        {

            this._queryFormGroup.Current.Chamber = ComboBoxChamber.Text;
            this.Form1ConditionChanged(FormConditionEnum.Chamber);
        }

        /// <summary>
        /// ComboBoxEqp變化時Form,ComboBox 狀態更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxRecipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._queryFormGroup.Current.Recipe = ComboBoxRecipe.Text;
            this.Form1ConditionChanged(FormConditionEnum.Recipe);
        }


        /// <summary>
        /// Query點擊觸發
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ConditionQuery_Click(object sender, EventArgs eventArgs)
        {
            this.SubmitFormCondition();
        }

        /// <summary>
        /// 以Form 查詢資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void SubmitFormCondition()
        {
            if (this.ConditionValid())
            {
                PropertyInfo[] props = typeof(FormConditionModel).GetProperties();
                foreach (var item in props)
                {
                    item.SetValue(this._queryFormGroup.Queried, item.GetValue(this._queryFormGroup.Current, null), null);
                }

                var result = this._repository.GetForm1ConditionQueryData(this._queryFormGroup.Queried);
                this._dataTableSourceResult = result;
                
                if (result.Any())
                {
                    this.SetParameterComboBoxOption();
                    this.RepositoryModelToRowData(result);
                }
                else
                {
                    this.SetMessage("No data", TextStyleEnum.Danger);
                }
            }
        }

        ///<summary>
        /// WriteToDb點擊觸發 判斷過濾後的資料 判斷有無重複Row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void BottonWriteToDb_Click(object sender, EventArgs eventArgs)
        {
            IEnumerable<IndicatorSettingToolGridDataModel> gridDataModelArray = this.FliterRowData();
          
            if (gridDataModelArray.Any())
            {
                var valid = false;
                valid = this.HasDuplicateRow(gridDataModelArray);
                if (valid)
                {
                    this.GridDataModelToRepository(gridDataModelArray);
                }
                else
                {
                    this.SetMessage("Has duplicate row", TextStyleEnum.Danger);
                }
            }

        }

        private bool HasDuplicateRow(IEnumerable<IndicatorSettingToolGridDataModel>  gridDataModelArray )
        {
            bool valid = true;
            int rowCount = dataGridView1.Rows.Count;
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                this.dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Empty;
            }
                foreach (var item in gridDataModelArray.GroupBy(e => new { e.Step, e.BeginType, e.BeginValue, e.EndType, e.EndValue, e.Statistics }))
                {
                    if (item.Count() > 1)
                    {
                        valid = false;
                        var grid = (DataGridView)this.dataGridView1;
                        foreach (var rowIndex in item.Select(e => e.RowIndex))
                        {
                            this.dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Red;
                        }
                    }
                }
            return valid;
        }
        /// <summary>
        /// ComboBox變化時的主要動作
        /// </summary>
        /// <param name="ConditionControl"></param>
        private void Form1ConditionChanged(FormConditionEnum ConditionControl)
        {
            PropertyInfo[] props = typeof(FormConditionModel).GetProperties();
            var Form1ConditionModelPropIndex = (props.Select(e => e.Name)).Where(e => e == ConditionControl.ToString());
            switch (ConditionControl)
            {
                case FormConditionEnum.Equipment:
                    this.GetForm1Chamber();

                    break;

                case FormConditionEnum.Chamber:
                    this.GetForm1Recipe();

                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// 判斷Form是否都有選
        /// </summary>
        /// <returns></returns>
        private bool ConditionValid()
        {
            PropertyInfo[] props = typeof(FormConditionModel).GetProperties();
            bool valid = true;
            // var Form1ConditionModelPropIndex = (props.Select(e => e.Name)).Where(e => e == ConditionControl.ToString());
            if (ComboBoxEqp.SelectedIndex < 0)
            {
                this.SetMessage("Eqp is required", TextStyleEnum.Danger);
                valid = false;
            }
            if (ComboBoxChamber.SelectedIndex < 0)
            {
                this.SetMessage("Chamber is required", TextStyleEnum.Danger);
                valid = false;
            }
            if (ComboBoxRecipe.SelectedIndex < 0)
            {
                this.SetMessage("Recipe is required", TextStyleEnum.Danger);
                valid = false;
            }
            return valid;
        }

        /// <summary>
        /// GridData轉Repository
        /// </summary>
        /// <param name="GridDataIEnumerable"></param>
        private void GridDataModelToRepository(IEnumerable<IndicatorSettingToolGridDataColumnModel> GridDataIEnumerable)
        {
         
            //要存入DB的資料
            List<RepositoryModel> RepositoryList = new List<RepositoryModel>();
            //DB設置的資料時間
            DateTime now = DateTime.Now;
            //DB的資料基本的equipment,chamber,recipe,parameter
            IEnumerable<RepositoryModel> RepositoryCondition;
                
           RepositoryCondition = this.SetGridDataModelCondition();
            if (RepositoryCondition.Any())
            {
                foreach (var condition in RepositoryCondition)
                {
                    foreach (var item in GridDataIEnumerable)
                    {
                        string BeginType = (item.BeginType == BeginTypeEnum.Head.ToString()) ? "Hb" : "Tb";
                        string EndType = (item.EndType == EndTypeEnum.Head.ToString()) ? "He" : "Te";
                        string IndicatorName = $"{condition.Parameter}-S{item.Step}{BeginType}{((double)item.BeginValue).ToString("F0")}{EndType}{((double)item.EndValue).ToString("F0")}_{item.Statistics}";
                        RepositoryList.Add(new RepositoryModel
                        {
                            Equipment = condition.Equipment,
                            Chamber = condition.Chamber,
                            Recipe = condition.Recipe,
                            Parameter = condition.Parameter,
                            Step = item.Step,
                            IndicatorName = IndicatorName,
                            HeadBegin = (item.BeginType == BeginTypeEnum.Head.ToString()) ? item.BeginValue : null,
                            TailBegin = (item.BeginType == BeginTypeEnum.Tail.ToString()) ? item.BeginValue : null,
                            HeadEnd = (item.EndType == EndTypeEnum.Head.ToString()) ? item.EndValue : null,
                            TailEnd = (item.EndType == EndTypeEnum.Tail.ToString()) ? item.EndValue : null,
                            Statistic = item.Statistics,
                            Usl = item.Usl,
                            Lsl = item.Lsl,
                            Ucl = item.Ucl,
                            Lcl = item.Lcl,
                            Uwl = item.Uwl,
                            Lwl = item.Lwl,
                            UpdataTime = now
                        });
                    }

                }
                this.InsertIndicatorDefInfoData(RepositoryList);
            }
            else
            {
                this.SetMessage("Condition Error", TextStyleEnum.Danger);
            }
        }
        private IEnumerable<RepositoryModel> SetGridDataModelCondition( )
        {
            var condition = new List<RepositoryModel>();
            if (this.TabControl.SelectedIndex == 0)
            {
                condition.Add(new RepositoryModel
                {
                    Equipment = this.IndicatorSettingToolLabelEqpValue.Text,
                    Chamber = this.IndicatorSettingToolLabelChamberValue.Text,
                    Recipe = this.IndicatorSettingToolLabelRecipeValue.Text,
                    Parameter = this.ParameterComboBox.Text,
                });
            }else if (this.TabControl.SelectedIndex == 1)
            {
                if (this.radioButtonOriginaQueryCondition.Checked)
                {
                    condition.Add(new RepositoryModel
                    {
                        Equipment = this._queryFormGroup.Queried.Equipment,
                        Chamber = this._queryFormGroup.Queried.Chamber,
                        Recipe = this._queryFormGroup.Queried.Recipe,
                        Parameter = this.ParameterComboBox.Text,
                    });

                }
                else if(this.radioButtonAnotherQueryCondition.Checked)
                {
                    bool valid = true;
                    switch (this._transferModelRequireEnum)
                    {
                        case TransferModelConditionModelRequireEnum.Equipment:
                            if (!this._transferModelCondition.Equipment.Any())
                            {
                                this.SetMessage("Please select eqp", TextStyleEnum.Danger);
                                valid = false;
                            }
                            break;
                        case TransferModelConditionModelRequireEnum.Chamber:
                            if (!this._transferModelCondition.Chamber.Any())
                            {
                                this.SetMessage("Please select Chamber", TextStyleEnum.Danger);
                                valid = false;
                            }
                            break;
                        case TransferModelConditionModelRequireEnum.Recipe:
                            if (!this._transferModelCondition.Recipe.Any())
                            {
                                this.SetMessage("Please select Recipe", TextStyleEnum.Danger);
                                valid = false;
                            }
                            break;
                        default:
                            break;
                    }
                    if (valid)
                    {
                        foreach (string equipment in this._transferModelCondition.Equipment )
                        {
                            foreach (var chamber in this._transferModelCondition.Chamber)
                            {
                                foreach (var recipe in this._transferModelCondition.Recipe)
                                {
                                    condition.Add(new RepositoryModel
                                    {
                                        Equipment = equipment,
                                        Chamber = chamber,
                                        Recipe = recipe,
                                        Parameter = this.ParameterComboBox.Text,

                                    });
                                }
                            }
                        }
                    }
                }
            };
            return condition;
        }
        private void InsertIndicatorDefInfoData(IEnumerable<RepositoryModel> value)
        {
            this.SetMessage("Start write to db ", TextStyleEnum.Normal);
            this._repository.InsertIndicatorDefInfoData(value);
            this.SetMessage("Complete", TextStyleEnum.Normal);
            if (this.TabControl.SelectedIndex==1 )
            {
                this.SubmitFormCondition();
            }
         
        }

        /// <summary>
        /// 過濾未選擇的Grid Data資料
        /// </summary>
        /// <returns></returns>
        private List<IndicatorSettingToolGridDataModel> FliterRowData ()
        {
            var grid = (DataGridView)this.dataGridView1;
            List<IndicatorSettingToolGridDataModel> gridDataModelArray = new List<IndicatorSettingToolGridDataModel> { };
            PropertyInfo[] props = typeof(IndicatorSettingToolGridDataColumnModel).GetProperties();
            var selectIndex = -1;
            var beginValueIndex = -1;
            var endValueIndex = -1;
            double isNum = default(double);

            for (int propsIndex = 0; propsIndex < props.Length; propsIndex++)
            {
                if (props[propsIndex].Name == "IsSelect")
                {
                    selectIndex = propsIndex;
                    continue;
                }
                else if (props[propsIndex].Name == "BeginValue")
                {
                    beginValueIndex = propsIndex;
                    continue;
                }
                else if (props[propsIndex].Name == "EndValue")
                {
                    endValueIndex = propsIndex;
                    continue;
                }

            }
            if (selectIndex > -1 && grid.RowCount > 0)
            {
                for (int rowIndex = 0; rowIndex < grid.RowCount; rowIndex++)
                {
                    object[] rowItem = ((DataRowView)((DataGridViewRow)grid.Rows[rowIndex]).DataBoundItem).Row.ItemArray;
                    if ((bool)rowItem[selectIndex] == false)
                    {
                        //過濾IsSelect =flase的row
                        continue;
                    }
                    if (beginValueIndex < 0 || endValueIndex < 0)
                    {
                        this.SetMessage("Can't find begin or end column", TextStyleEnum.Danger);
                        return new List<IndicatorSettingToolGridDataModel> { }; ;
                    }
                    if (!double.TryParse(rowItem[beginValueIndex].ToString(), out isNum))
                    {
                        this.SetMessage("Begin is required", TextStyleEnum.Danger);
                        return new List<IndicatorSettingToolGridDataModel> { }; ;
                    }
                    if (!double.TryParse(rowItem[endValueIndex].ToString(), out isNum))
                    {
                        this.SetMessage("End is required", TextStyleEnum.Danger);
                        return new List<IndicatorSettingToolGridDataModel> { }; ;
                    }

                    var gridDataModeItem = new IndicatorSettingToolGridDataModel();
                    for (int propsIndex = 0; propsIndex < props.Length; propsIndex++)
                    {
                        var itemValueTemp = rowItem[propsIndex];

                        if (props[propsIndex].PropertyType == typeof(double?))
                        {

                            //double isNum = default(double);

                            if (double.TryParse(itemValueTemp.ToString(), out isNum))
                            {
                                props[propsIndex].SetValue(gridDataModeItem, rowItem[propsIndex], null);
                            }
                            else
                            {
                                props[propsIndex].SetValue(gridDataModeItem, null, null);
                            }
                        }
                        else
                        {
                            props[propsIndex].SetValue(gridDataModeItem, rowItem[propsIndex], null);
                        }
                    }
                    gridDataModeItem.RowIndex = rowIndex;
                    gridDataModelArray.Add(gridDataModeItem);
                }
            }
            if (!gridDataModelArray.Any())
            {
                this.SetMessage("Not select row ", TextStyleEnum.Danger);
            }
            return gridDataModelArray;
        }

        /// <summary>
        /// Repository 轉 RowData
        /// </summary>
        /// <param name="value"></param>
        private void RepositoryModelToRowData(IEnumerable<RepositoryModel> value)
        {
            //DataGridView1專用的Column
            List<IndicatorSettingToolGridDataColumnModel> dataRow = new List<IndicatorSettingToolGridDataColumnModel>();
            var slelctParameter = this.ParameterComboBox.Text;
            var dataRowsource = value.Where(e => e.Parameter == slelctParameter);
            foreach (var item in dataRowsource)
            {
                dataRow.Add(new IndicatorSettingToolGridDataColumnModel()
                {
                    BeginType = item.TailBegin != default(double?) ? BeginTypeEnum.Tail.ToString() : BeginTypeEnum.Head.ToString(),
                    EndType = item.TailEnd != default(double?) ? EndTypeEnum.Tail.ToString() : EndTypeEnum.Head.ToString(),
                    Step = item.Step,
                    BeginValue = item.HeadBegin != default(double?) ? item.HeadBegin : item.TailBegin,
                    EndValue = item.HeadEnd != default(double?) ? item.HeadEnd : item.TailEnd,
                    Statistics = item.Statistic,
                    Usl = item.Usl,
                    Lsl = item.Lsl,
                    Ucl = item.Ucl,
                    Lcl = item.Lcl,
                    Uwl = item.Uwl,
                    Lwl = item.Lwl,
                }); ;
            }
            this.SetDataGridView1Source(dataRow);
        }


        /// <summary>
        /// 將Grid Data 呈現在畫面上
        /// </summary>
        /// <param name="dataRow"></param>
        private void SetDataGridView1Source(List<IndicatorSettingToolGridDataColumnModel> dataRow)
        {
            try
            {
                using (DataTable table = new DataTable())
                {
                    this._dataTableTemp = new DataTable();
                    PropertyInfo[] props = typeof(IndicatorSettingToolGridDataColumnModel).GetProperties();
                    foreach (var prop in props)
                    {
                        var name = prop.Name;
                        //解決DataSet 不支援 System.Nullable<>
                        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                    }
                    foreach (var item in dataRow)
                    {
                        DataRow row = table.NewRow();
                        foreach (var prop in props)
                        {
                            var value = prop.GetValue(item, null);
                            if (value == null)
                            {
                                row[prop.Name] = DBNull.Value;
                            }
                            else
                            {
                                row[prop.Name] = value;
                            }
                        }
                        table.Rows.Add(row);
                    }
                    this._dataTableTemp = table;
                    // 用DataTable建出DataView後當作DataSource綁訂在dataGridView1
                    this.dataGridView1.DataSource = new DataView(this._dataTableTemp);
                }
            }
            catch (Exception ex)
            {
                this.SetMessage($"{ex.Message}   ", TextStyleEnum.Danger);
                throw;
            }
        }



        private void radioButtonOriginaQueryCondition_CheckedChanged(object sender, EventArgs e)
        {
            this.panel3.Visible = false;
        }

        private void radioButtonAnotherQueryCondition_CheckedChanged(object sender, EventArgs e)
        {
            this.panel3.Visible = true;
        }

        private void SetParameterComboBoxOption()
        {
            String[] ParameterComboBoxItems = new String[0];
            switch (this.TabControl.SelectedIndex)
            {
                case 0:
                    ParameterComboBoxItems = this._dataTableSourceXml.Select(e => e.Parameter).Distinct().OrderBy(e => e).ToArray();
                    break;
                case 1:
                    ParameterComboBoxItems = this._dataTableSourceResult.Select(e => e.Parameter).Distinct().OrderBy(e => e).ToArray();
                    break;
                default:
                    break;
            }
            this.ParameterComboBox.Items.Clear();
            this.ParameterComboBox.Items.AddRange(ParameterComboBoxItems);
            if (ParameterComboBoxItems.Any())
            {
                this.ParameterComboBox.SelectedIndex = 0;
            }
            else
            {
                this.ParameterComboBox.SelectedIndex = -1;
                this.SetMessage("Can't find parameter", TextStyleEnum.Danger);
            }

        }

        public void SetTransferModelConditionModel(TransferModelConditionModel value)
        {
            this._transferModelCondition = value;
            this.textBoxAnotherConditionEqp.Text = String.Join(",", this._transferModelCondition.Equipment);
            this.textBoxAnotherConditionChamber.Text = String.Join(",", this._transferModelCondition.Chamber);
            this.textBoxAnotherConditionRecipe.Text = String.Join(",", this._transferModelCondition.Recipe);
        }
        private void buttonTransferModelEqp_Click(object sender, EventArgs e)
        {
            var form =new TransferModelConditionModel();
            PropertyInfo[] props = typeof(TransferModelConditionModel).GetProperties();
            foreach (var item in props)
            {
                item.SetValue(form, item.GetValue(this._transferModelCondition, null), null);
            }

            TransferModel transferModel = new TransferModel(this._repository,this, TransferModelSearchType.Equipment, form);
            transferModel.ShowDialog();
        }

        private void buttonTransferModelChamber_Click(object sender, EventArgs e)
        {
            if (this._transferModelCondition.Equipment.Any())
            {
                var form = new TransferModelConditionModel();
                PropertyInfo[] props = typeof(TransferModelConditionModel).GetProperties();
                foreach (var item in props)
                {
                    item.SetValue(form, item.GetValue(this._transferModelCondition, null), null);
                }

                TransferModel transferModel = new TransferModel(this._repository, this, TransferModelSearchType.Chamber, form);
                transferModel.ShowDialog();
            }
            else
            {
                this.SetMessage("Please select Equipment", TextStyleEnum.Danger);
            }
            
        }

        private void buttonTransferModelRecipe_Click(object sender, EventArgs e)
        {

            if (this._transferModelCondition.Equipment.Any()&& this._transferModelCondition.Chamber.Any())
            {
                var form = new TransferModelConditionModel();
                PropertyInfo[] props = typeof(TransferModelConditionModel).GetProperties();
                foreach (var item in props)
                {
                    item.SetValue(form, item.GetValue(this._transferModelCondition, null), null);
                }
                TransferModel transferModel = new TransferModel(this._repository, this, TransferModelSearchType.Recipe, form);
                transferModel.ShowDialog();
            }
            else
            {
                this.SetMessage("Please select eqp and chamber", TextStyleEnum.Danger);
            }
           
        }
    }
}
