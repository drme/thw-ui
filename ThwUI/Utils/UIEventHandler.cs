using System;

namespace ThW.UI.Utils
{
    /// <summary>
    /// Event handler for control.
    /// </summary>
    /// <typeparam name="ControlType">Control type</typeparam>
    /// <param name="sender">control that raised event</param>
    /// <param name="args">event arguments</param>
    public delegate void UIEventHandler<ControlType>(ControlType sender, EventArgs args);
}
