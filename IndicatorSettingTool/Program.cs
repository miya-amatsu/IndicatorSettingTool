using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using IndicatorSettingTool.Repository;
using IndicatorSettingTool.Infrastructure.Interface.Repository;
using IndicatorSettingTool.Infrastructure.Interface.View;

namespace IndicatorSettingTool
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 form = new Form1();
            Form1Repository repository = new Form1Repository();
            form.SetRepository(repository);
            repository.SetView(form);
            Application.Run(form);
             
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            ////Repository-First模式,先建Form1Repository 後View(Form1)注入到Repository(Repository)中  
            //Form view = new Form1Repository(IIndicatorSettingToolViewInterface);
            //Application.Run(repository._view);
        }
    }
}
