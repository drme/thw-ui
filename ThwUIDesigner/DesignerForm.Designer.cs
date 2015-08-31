namespace ThW.UI.Utils.Designer
{
    partial class DesignerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DesignerForm));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cacheFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.newButton = new System.Windows.Forms.ToolStripButton();
			this.openButton = new System.Windows.Forms.ToolStripButton();
			this.deleteButton = new System.Windows.Forms.ToolStripButton();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.panel1 = new System.Windows.Forms.Panel();
			this.controlsPanel = new System.Windows.Forms.Panel();
			this.radioButtonPointer = new System.Windows.Forms.RadioButton();
			this.label2 = new System.Windows.Forms.Label();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panelProperties = new System.Windows.Forms.Panel();
			this.propertiesPanel = new System.Windows.Forms.Panel();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.renderPanel = new ThW.UI.Utils.Designer.RenderPanel();
			this.menuStrip1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.controlsPanel.SuspendLayout();
			this.panelProperties.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(911, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.loadWindowToolStripMenuItem,
            this.saveWindowToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.newToolStripMenuItem.Text = "New";
			this.newToolStripMenuItem.Click += new System.EventHandler(this.NewWindowClick);
			// 
			// loadWindowToolStripMenuItem
			// 
			this.loadWindowToolStripMenuItem.Name = "loadWindowToolStripMenuItem";
			this.loadWindowToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.loadWindowToolStripMenuItem.Text = "Open...";
			this.loadWindowToolStripMenuItem.Click += new System.EventHandler(this.OpenWindow);
			// 
			// saveWindowToolStripMenuItem
			// 
			this.saveWindowToolStripMenuItem.Name = "saveWindowToolStripMenuItem";
			this.saveWindowToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.saveWindowToolStripMenuItem.Text = "Save";
			this.saveWindowToolStripMenuItem.Click += new System.EventHandler(this.SaveClick);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.saveAsToolStripMenuItem.Text = "Save as...";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsClick);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitClick);
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.cacheFontToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
			this.toolsToolStripMenuItem.Text = "Tools";
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.optionsToolStripMenuItem.Text = "Options...";
			this.optionsToolStripMenuItem.Click += new System.EventHandler(this.OptionsClick);
			// 
			// cacheFontToolStripMenuItem
			// 
			this.cacheFontToolStripMenuItem.Name = "cacheFontToolStripMenuItem";
			this.cacheFontToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.cacheFontToolStripMenuItem.Text = "Export fonts...";
			this.cacheFontToolStripMenuItem.Click += new System.EventHandler(this.CacheFontClicked);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "Help";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.aboutToolStripMenuItem.Text = "About";
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newButton,
            this.openButton,
            this.deleteButton});
			this.toolStrip1.Location = new System.Drawing.Point(0, 24);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(911, 25);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// newButton
			// 
			this.newButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.newButton.Image = ((System.Drawing.Image)(resources.GetObject("newButton.Image")));
			this.newButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.newButton.Name = "newButton";
			this.newButton.Size = new System.Drawing.Size(23, 22);
			this.newButton.Click += new System.EventHandler(this.NewWindowClick);
			// 
			// openButton
			// 
			this.openButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.openButton.Image = ((System.Drawing.Image)(resources.GetObject("openButton.Image")));
			this.openButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.openButton.Name = "openButton";
			this.openButton.Size = new System.Drawing.Size(23, 22);
			this.openButton.Click += new System.EventHandler(this.OpenWindow);
			// 
			// deleteButton
			// 
			this.deleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.deleteButton.Image = ((System.Drawing.Image)(resources.GetObject("deleteButton.Image")));
			this.deleteButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.deleteButton.Name = "deleteButton";
			this.deleteButton.Size = new System.Drawing.Size(23, 22);
			this.deleteButton.Click += new System.EventHandler(this.DeleteButtonClick);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Location = new System.Drawing.Point(0, 522);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(911, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.controlsPanel);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point(0, 49);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(145, 473);
			this.panel1.TabIndex = 3;
			// 
			// controlsPanel
			// 
			this.controlsPanel.Controls.Add(this.radioButtonPointer);
			this.controlsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.controlsPanel.Location = new System.Drawing.Point(0, 19);
			this.controlsPanel.Name = "controlsPanel";
			this.controlsPanel.Size = new System.Drawing.Size(145, 454);
			this.controlsPanel.TabIndex = 1;
			// 
			// radioButtonPointer
			// 
			this.radioButtonPointer.AutoSize = true;
			this.radioButtonPointer.Dock = System.Windows.Forms.DockStyle.Top;
			this.radioButtonPointer.Location = new System.Drawing.Point(0, 0);
			this.radioButtonPointer.Name = "radioButtonPointer";
			this.radioButtonPointer.Size = new System.Drawing.Size(145, 17);
			this.radioButtonPointer.TabIndex = 0;
			this.radioButtonPointer.TabStop = true;
			this.radioButtonPointer.Text = "Pointer";
			this.radioButtonPointer.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
			this.label2.Dock = System.Windows.Forms.DockStyle.Top;
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(145, 19);
			this.label2.TabIndex = 0;
			this.label2.Text = "Toolbox";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(145, 49);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 473);
			this.splitter1.TabIndex = 4;
			this.splitter1.TabStop = false;
			// 
			// panelProperties
			// 
			this.panelProperties.Controls.Add(this.propertiesPanel);
			this.panelProperties.Controls.Add(this.comboBox1);
			this.panelProperties.Controls.Add(this.label1);
			this.panelProperties.Dock = System.Windows.Forms.DockStyle.Right;
			this.panelProperties.Location = new System.Drawing.Point(689, 49);
			this.panelProperties.Name = "panelProperties";
			this.panelProperties.Size = new System.Drawing.Size(222, 473);
			this.panelProperties.TabIndex = 5;
			// 
			// propertiesPanel
			// 
			this.propertiesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertiesPanel.Location = new System.Drawing.Point(0, 40);
			this.propertiesPanel.Name = "propertiesPanel";
			this.propertiesPanel.Size = new System.Drawing.Size(222, 433);
			this.propertiesPanel.TabIndex = 2;
			// 
			// comboBox1
			// 
			this.comboBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(0, 19);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(222, 21);
			this.comboBox1.TabIndex = 1;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.ControlSelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(222, 19);
			this.label1.TabIndex = 0;
			this.label1.Text = "Properties";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// splitter2
			// 
			this.splitter2.Dock = System.Windows.Forms.DockStyle.Right;
			this.splitter2.Location = new System.Drawing.Point(686, 49);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(3, 473);
			this.splitter2.TabIndex = 6;
			this.splitter2.TabStop = false;
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.DefaultExt = "window.xml";
			this.openFileDialog1.FileName = "openFileDialog1";
			this.openFileDialog1.Filter = "Window files|*.window.xml";
			this.openFileDialog1.Title = "Select window";
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.DefaultExt = "window.xml";
			this.saveFileDialog1.Filter = "Window files|*.window.xml";
			this.saveFileDialog1.Title = "Save as...";
			// 
			// renderPanel
			// 
			this.renderPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.renderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.renderPanel.Location = new System.Drawing.Point(148, 49);
			this.renderPanel.Name = "renderPanel";
			this.renderPanel.Size = new System.Drawing.Size(538, 473);
			this.renderPanel.TabIndex = 7;
			// 
			// DesignerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.ClientSize = new System.Drawing.Size(911, 544);
			this.Controls.Add(this.renderPanel);
			this.Controls.Add(this.splitter2);
			this.Controls.Add(this.panelProperties);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.MinimumSize = new System.Drawing.Size(780, 460);
			this.Name = "DesignerForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ThW UI Designer";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClosingForm);
			this.Load += new System.EventHandler(this.InitControls);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.controlsPanel.ResumeLayout(false);
			this.controlsPanel.PerformLayout();
			this.panelProperties.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton newButton;
        private System.Windows.Forms.ToolStripButton openButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panelProperties;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Splitter splitter2;
        private RenderPanel renderPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel controlsPanel;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Panel propertiesPanel;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.RadioButton radioButtonPointer;
        private System.Windows.Forms.ToolStripButton deleteButton;
		private System.Windows.Forms.ToolStripMenuItem cacheFontToolStripMenuItem;
    }
}