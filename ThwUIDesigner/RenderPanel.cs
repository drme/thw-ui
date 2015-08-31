using System.Windows.Forms;

namespace ThW.UI.Utils.Designer
{
    class RenderPanel : Panel
    {
        public RenderPanel() : base()
        {
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }
    }
}
