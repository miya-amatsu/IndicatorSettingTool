using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IndicatorSettingTool
{
    public partial class TestControl : UserControl
    {
        public TestControl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {                       
            using (var form = new TestForm())
            {
                form.ShowDialog();
            }                                                    
        }                                                                                                               
    }
}
