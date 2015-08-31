using System;

namespace ThW.UI.Utils
{
    /// <summary>
    /// Parameters for passing to CreateControl, CreateWindow methods.
    /// </summary>
    public enum CreationFlag
    {
        FlagsNone = 0,
        SelectableInDesigner = 1,	// control with this flag set can be 
        NeedSaving = 2, // if not set, controls attributes are cleared after loading in order to save space
        NeedLoading = 4, // if not set NeedLoading and NeedSaving, controls attributes are not created, thus saving memory
        /// <summary>
        /// control is used in composition of another control (for example ComboBox button) and saving of this control is not required
        /// </summary>
        InternalControl = 8
    }
}
