namespace IndicatorSettingTool
{
    partial class TransferModel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxSearchItem = new System.Windows.Forms.TextBox();
            this.checkedListBoxItemDesearch = new System.Windows.Forms.CheckedListBox();
            this.checkedListBoxItemSearch = new System.Windows.Forms.CheckedListBox();
            this.buttonSelectd = new System.Windows.Forms.Button();
            this.buttonDeselectd = new System.Windows.Forms.Button();
            this.buttonAllSelectd = new System.Windows.Forms.Button();
            this.buttonAllDeselectd = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSetCondition = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxSearchItem
            // 
            this.textBoxSearchItem.Location = new System.Drawing.Point(54, 31);
            this.textBoxSearchItem.Name = "textBoxSearchItem";
            this.textBoxSearchItem.Size = new System.Drawing.Size(194, 22);
            this.textBoxSearchItem.TabIndex = 4;
            this.textBoxSearchItem.TextChanged += new System.EventHandler(this.textBoxSearchItem_TextChanged);
            // 
            // checkedListBoxItemDesearch
            // 
            this.checkedListBoxItemDesearch.FormattingEnabled = true;
            this.checkedListBoxItemDesearch.Location = new System.Drawing.Point(15, 59);
            this.checkedListBoxItemDesearch.Name = "checkedListBoxItemDesearch";
            this.checkedListBoxItemDesearch.Size = new System.Drawing.Size(233, 276);
            this.checkedListBoxItemDesearch.TabIndex = 5;
            // 
            // checkedListBoxItemSearch
            // 
            this.checkedListBoxItemSearch.FormattingEnabled = true;
            this.checkedListBoxItemSearch.Location = new System.Drawing.Point(320, 59);
            this.checkedListBoxItemSearch.Name = "checkedListBoxItemSearch";
            this.checkedListBoxItemSearch.Size = new System.Drawing.Size(233, 276);
            this.checkedListBoxItemSearch.TabIndex = 6;
            // 
            // buttonSelectd
            // 
            this.buttonSelectd.Location = new System.Drawing.Point(273, 76);
            this.buttonSelectd.Name = "buttonSelectd";
            this.buttonSelectd.Size = new System.Drawing.Size(23, 23);
            this.buttonSelectd.TabIndex = 7;
            this.buttonSelectd.Text = ">";
            this.buttonSelectd.UseVisualStyleBackColor = true;
            this.buttonSelectd.Click += new System.EventHandler(this.buttonSelectd_Click);
            // 
            // buttonDeselectd
            // 
            this.buttonDeselectd.Location = new System.Drawing.Point(273, 105);
            this.buttonDeselectd.Name = "buttonDeselectd";
            this.buttonDeselectd.Size = new System.Drawing.Size(23, 23);
            this.buttonDeselectd.TabIndex = 8;
            this.buttonDeselectd.Text = "<";
            this.buttonDeselectd.UseVisualStyleBackColor = true;
            this.buttonDeselectd.Click += new System.EventHandler(this.buttonDeselectd_Click);
            // 
            // buttonAllSelectd
            // 
            this.buttonAllSelectd.Location = new System.Drawing.Point(264, 240);
            this.buttonAllSelectd.Name = "buttonAllSelectd";
            this.buttonAllSelectd.Size = new System.Drawing.Size(41, 23);
            this.buttonAllSelectd.TabIndex = 9;
            this.buttonAllSelectd.Text = ">>";
            this.buttonAllSelectd.UseVisualStyleBackColor = true;
            this.buttonAllSelectd.Click += new System.EventHandler(this.buttonAllSelectd_Click);
            // 
            // buttonAllDeselectd
            // 
            this.buttonAllDeselectd.Location = new System.Drawing.Point(264, 269);
            this.buttonAllDeselectd.Name = "buttonAllDeselectd";
            this.buttonAllDeselectd.Size = new System.Drawing.Size(41, 23);
            this.buttonAllDeselectd.TabIndex = 10;
            this.buttonAllDeselectd.Text = "<<";
            this.buttonAllDeselectd.UseVisualStyleBackColor = true;
            this.buttonAllDeselectd.Click += new System.EventHandler(this.buttonAllDeselectd_Click);
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(15, 3);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(233, 22);
            this.textBox2.TabIndex = 11;
            this.textBox2.Text = "Items";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "Search:";
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(320, 3);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(233, 22);
            this.textBox3.TabIndex = 14;
            this.textBox3.Text = "Selected Items";
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(394, 350);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 15;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonSetCondition
            // 
            this.buttonSetCondition.Location = new System.Drawing.Point(475, 350);
            this.buttonSetCondition.Name = "buttonSetCondition";
            this.buttonSetCondition.Size = new System.Drawing.Size(75, 23);
            this.buttonSetCondition.TabIndex = 16;
            this.buttonSetCondition.Text = "OK";
            this.buttonSetCondition.UseVisualStyleBackColor = true;
            this.buttonSetCondition.Click += new System.EventHandler(this.buttonSetCondition_Click);
            // 
            // TransferModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 385);
            this.Controls.Add(this.buttonSetCondition);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.buttonAllDeselectd);
            this.Controls.Add(this.buttonAllSelectd);
            this.Controls.Add(this.buttonDeselectd);
            this.Controls.Add(this.buttonSelectd);
            this.Controls.Add(this.checkedListBoxItemSearch);
            this.Controls.Add(this.checkedListBoxItemDesearch);
            this.Controls.Add(this.textBoxSearchItem);
            this.Name = "TransferModel";
            this.Text = "TransferModel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxSearchItem;
        private System.Windows.Forms.CheckedListBox checkedListBoxItemDesearch;
        private System.Windows.Forms.CheckedListBox checkedListBoxItemSearch;
        private System.Windows.Forms.Button buttonSelectd;
        private System.Windows.Forms.Button buttonDeselectd;
        private System.Windows.Forms.Button buttonAllSelectd;
        private System.Windows.Forms.Button buttonAllDeselectd;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSetCondition;
    }
}