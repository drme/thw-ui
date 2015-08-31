using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ThW.UI.Utils.Designer
{
    class PropertyGridEx : PropertyGrid
    {
        protected override PropertyTab CreatePropertyTab(Type tabType)
        {
            return new PropertyTabEx();
        }
    }
}
