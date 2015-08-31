using ThW.UI.Controls;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Design
{
    /// <summary>
    /// Constructs control object, asigns container window to it.
    /// </summary>
    /// <typeparam name="ControlType">control type</typeparam>
    /// <param name="window">container window control belongs to</param>
    /// <param name="creationFlags">creation flags</param>
    /// <returns>created window</returns>
    public delegate ControlType ControlCreator<ControlType>(Window window, CreationFlag creationFlags) where ControlType : Control;

    /// <summary>
    /// Controls creator default implementation.
    /// </summary>
    /// <typeparam name="ControlType">control type</typeparam>
	public class ControlsCreator<ControlType> : IControlsCreator where ControlType : Control
	{
        public ControlsCreator(ControlCreator<ControlType> creator, bool showInDesigner)
        {
            this.creator = creator;
            this.showInDesigner = showInDesigner;
        }

        public ControlsCreator(ControlCreator<ControlType> creator) : this(creator, true)
        {
        }

        public virtual Control CreateControl(Window window, CreationFlag creationFlags)
        {
            return this.creator(window, creationFlags);
        }

		public virtual bool ShowInDesigner
		{
            get
            {
                return this.showInDesigner;
            }
		}

		private	bool showInDesigner = false;
        private ControlCreator<ControlType> creator = null;
	}
}
