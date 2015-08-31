using System;
using ThW.UI.Design;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Composite panel with image, title and subtext.
    /// </summary>
	public class InfoPanel : Button
	{
        /// <summary>
        /// Creates info panel control.
        /// </summary>
        /// <param name="window">window it belongs to</param>
        /// <param name="creationFlags">creation flags</param>
		public InfoPanel(Window window, CreationFlag creationFlags) : base(window, creationFlags)
        {
            this.Type = TypeName;

            this.imageBorder = CreateControl<Button>();
            this.image = CreateControl<Button>();
            this.titleLabel = CreateControl<Button>();
            this.subTitleLabel = CreateControl<Button>();
            this.commentLabel = CreateControl<Button>();

			this.imageBorder.Bounds = new Rectangle(0, 0, 200, 120);
			this.imageBorder.BackColor = Colors.Black;
            this.imageBorder.Border = BorderStyle.None;
            this.imageBorder.BackImage = "";

            this.image.Bounds = new Rectangle(10, 10, 180, 100);
			this.image.BackImageLayout = ImageLayout.ImageLayoutStretch;
            this.image.Border = BorderStyle.None;

            this.titleLabel.FontInfo.SetFontInfo("Arial", 12, true, false);
            this.titleLabel.Bounds = new Rectangle(220, 0, 230, 20);
            this.titleLabel.TextAlignment = ContentAlignment.MiddleLeft;
            this.titleLabel.BackColor = Colors.None;
            this.titleLabel.Anchor = AnchorStyle.AnchorLeft | AnchorStyle.AnchorRight | AnchorStyle.AnchorTop;
            this.titleLabel.TextColor = this.TextColor;
            this.titleLabel.Border = BorderStyle.None;
            this.titleLabel.BackImage = "";

            this.subTitleLabel.FontInfo.SetFontInfo("Arial", 12, false, false);
            this.subTitleLabel.Bounds = new Rectangle(220, 20, 230, 20);
			this.subTitleLabel.TextAlignment = ContentAlignment.MiddleLeft;
            this.subTitleLabel.BackColor = Colors.None;
            this.subTitleLabel.Anchor = AnchorStyle.AnchorLeft | AnchorStyle.AnchorRight | AnchorStyle.AnchorTop;
            this.subTitleLabel.TextColor = this.TextColor;
            this.subTitleLabel.Border = BorderStyle.None;
            this.subTitleLabel.BackImage = "";

            this.commentLabel.FontInfo.SetFontInfo("Arial", 12, false, false);
            this.commentLabel.Bounds = new Rectangle(220, 40, 230, 20);
            this.commentLabel.TextAlignment = ContentAlignment.MiddleLeft;
			this.commentLabel.BackColor = Colors.None;
            this.commentLabel.Anchor = AnchorStyle.AnchorLeft | AnchorStyle.AnchorRight | AnchorStyle.AnchorTop;
            this.commentLabel.TextColor = this.TextColor;
            this.commentLabel.Border = BorderStyle.None;
            this.commentLabel.BackImage = "";

            this.MinSize = new Size2d(200, 100);
            this.Bounds = new Rectangle(0, 0, 470, 120);
            this.Border = BorderStyle.None;
            this.BackImage = "";

			this.AddControl(this.imageBorder);
			this.AddControl(this.titleLabel);
			this.AddControl(this.subTitleLabel);
			this.AddControl(this.commentLabel);

			this.imageBorder.AddControl(this.image);

            this.imageBorder.Clicked += this.ChildControlClicked;
            this.titleLabel.Clicked += this.ChildControlClicked;
            this.subTitleLabel.Clicked += this.ChildControlClicked;
            this.commentLabel.Clicked += this.ChildControlClicked;
            this.image.Clicked += this.ChildControlClicked;
        }

        /// <summary>
        /// Child control clicked.
        /// </summary>
        /// <param name="sender">clicked control</param>
        /// <param name="args">arguments</param>
        private void ChildControlClicked(Control sender, EventArgs args)
        {
            OnClick(-1, -1);
        }

        /// <summary>
        /// Mouse cursor for this control.
        /// </summary>
        public override MousePointers Cursor
        {
            get
            {
                return base.Cursor;
            }
            set
            {
                base.Cursor = value;

                this.imageBorder.Cursor = this.Cursor;
                this.image.Cursor = this.Cursor;
                this.titleLabel.Cursor = this.Cursor;
                this.subTitleLabel.Cursor = this.Cursor;
                this.commentLabel.Cursor = this.Cursor;
            }
        }

        /// <summary>
        /// Registers available control properties.
        /// </summary>
		protected override void AddProperties()
        {
			base.AddProperties();

            AddProperty(new PropertyString(this.Title, "infoTitle", "InfoPanel", "infoTitle", (x) => { this.Title = x; }, () => { return this.Title; }));
            AddProperty(new PropertyString(this.SubTitle, "infoSubTitle", "InfoPanel", "infoSubTitle", (x) => { this.SubTitle = x; }, () => { return this.SubTitle; }));
            AddProperty(new PropertyString(this.Comment, "infoComment", "InfoPanel", "infoComment", (x) => { this.Comment = x; }, () => { return this.Comment; }));
            AddProperty(new PropertyImage(this.InfoImage, "infoImage", "InfoPanel", "InfoImage", (x) => { this.InfoImage = x; }, () => { return this.InfoImage; }));
        }

        /// <summary>
        /// Controls name as serialized in a xml file.
        /// </summary>
        public new static String TypeName
        {
            get
            {
                return "infoPanel";
            }
        }

        /// <summary>
        /// Title
        /// </summary>
        public String Title
        {
            get
            {
                return this.titleLabel.Text;
            }
            set
            {
                this.titleLabel.Text = value;
            }
        }

        /// <summary>
        /// Sub Title
        /// </summary>
        public String SubTitle
        {
            get
            {
                return this.subTitleLabel.Text;
            }
            set
            {
                this.subTitleLabel.Text = value;
            }
        }

        /// <summary>
        /// Comment
        /// </summary>
        public String Comment
        {
            get
            {
                return this.commentLabel.Text;
            }
            set
            {
                this.commentLabel.Text = value;
            }
        }

        /// <summary>
        /// Information image
        /// </summary>
        public String InfoImage
        {
            get
            {
                return this.image.BackImage;
            }
            set
            {
                this.image.BackImage = value;
            }
        }

        protected Button imageBorder = null;
        protected Button image = null;
        protected Button titleLabel = null;
        protected Button subTitleLabel = null;
        protected Button commentLabel = null;
	}
}
