using System;
using System.Windows.Forms;

namespace ThW.UI.Utils.Designer
{
    public static class StartUp
    {
        [STAThread()]
        public static void Main(String[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DesignerForm());
        }
    }
}
