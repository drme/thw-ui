using System;
using System.Collections.Generic;
using System.Text;

namespace ThW.UI.Controls
{
    public enum ControlState
    {
        None = 0,
        ControlDragging = 1 << 0,
        ControlResizing = 1 << 1,
        ControlResizeVert = 1 << 2,
        ControlResizeHoriz = 1 << 3,
        ControlResizeTop = 1 << 4,
        ControlResizeLeft = 1 << 5
        //				ControlMousePressed = 1 << 6
        //				ControlHasMouseOver = 1 << 6,
    };
}
