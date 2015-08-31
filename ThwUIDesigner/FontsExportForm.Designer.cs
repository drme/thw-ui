namespace ThW.UI.Designer
{
	partial class FontsExportForm
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
			this.buttonClose = new System.Windows.Forms.Button();
			this.buttonExport = new System.Windows.Forms.Button();
			this.textBoxSaveLocation = new System.Windows.Forms.TextBox();
			this.buttonSelectFolder = new System.Windows.Forms.Button();
			this.labelSaveLocation = new System.Windows.Forms.Label();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.listBoxFonts = new System.Windows.Forms.ListBox();
			this.buttonAdd = new System.Windows.Forms.Button();
			this.buttonRemove = new System.Windows.Forms.Button();
			this.fontDialog = new System.Windows.Forms.FontDialog();
			this.SuspendLayout();
			// 
			// buttonClose
			// 
			this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonClose.Location = new System.Drawing.Point(397, 326);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(75, 23);
			this.buttonClose.TabIndex = 0;
			this.buttonClose.Text = "Close";
			this.buttonClose.UseVisualStyleBackColor = true;
			// 
			// buttonExport
			// 
			this.buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonExport.Enabled = false;
			this.buttonExport.Location = new System.Drawing.Point(316, 326);
			this.buttonExport.Name = "buttonExport";
			this.buttonExport.Size = new System.Drawing.Size(75, 23);
			this.buttonExport.TabIndex = 1;
			this.buttonExport.Text = "Export";
			this.buttonExport.UseVisualStyleBackColor = true;
			this.buttonExport.Click += new System.EventHandler(this.ExportFontsClick);
			// 
			// textBoxSaveLocation
			// 
			this.textBoxSaveLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxSaveLocation.Location = new System.Drawing.Point(93, 13);
			this.textBoxSaveLocation.Name = "textBoxSaveLocation";
			this.textBoxSaveLocation.ReadOnly = true;
			this.textBoxSaveLocation.Size = new System.Drawing.Size(298, 20);
			this.textBoxSaveLocation.TabIndex = 2;
			// 
			// buttonSelectFolder
			// 
			this.buttonSelectFolder.Location = new System.Drawing.Point(397, 12);
			this.buttonSelectFolder.Name = "buttonSelectFolder";
			this.buttonSelectFolder.Size = new System.Drawing.Size(75, 23);
			this.buttonSelectFolder.TabIndex = 3;
			this.buttonSelectFolder.Text = "Select";
			this.buttonSelectFolder.UseVisualStyleBackColor = true;
			this.buttonSelectFolder.Click += new System.EventHandler(this.SelectFolderClick);
			// 
			// labelSaveLocation
			// 
			this.labelSaveLocation.AutoSize = true;
			this.labelSaveLocation.Location = new System.Drawing.Point(12, 17);
			this.labelSaveLocation.Name = "labelSaveLocation";
			this.labelSaveLocation.Size = new System.Drawing.Size(75, 13);
			this.labelSaveLocation.TabIndex = 4;
			this.labelSaveLocation.Text = "Save location:";
			// 
			// listBoxFonts
			// 
			this.listBoxFonts.FormattingEnabled = true;
			this.listBoxFonts.Location = new System.Drawing.Point(12, 39);
			this.listBoxFonts.Name = "listBoxFonts";
			this.listBoxFonts.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
			this.listBoxFonts.Size = new System.Drawing.Size(460, 277);
			this.listBoxFonts.TabIndex = 5;
			// 
			// buttonAdd
			// 
			this.buttonAdd.Location = new System.Drawing.Point(12, 326);
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.Size = new System.Drawing.Size(29, 23);
			this.buttonAdd.TabIndex = 6;
			this.buttonAdd.Text = "+";
			this.buttonAdd.UseVisualStyleBackColor = true;
			this.buttonAdd.Click += new System.EventHandler(this.AddFontClick);
			// 
			// buttonRemove
			// 
			this.buttonRemove.Location = new System.Drawing.Point(47, 326);
			this.buttonRemove.Name = "buttonRemove";
			this.buttonRemove.Size = new System.Drawing.Size(29, 23);
			this.buttonRemove.TabIndex = 7;
			this.buttonRemove.Text = "-";
			this.buttonRemove.UseVisualStyleBackColor = true;
			this.buttonRemove.Click += new System.EventHandler(this.RemoveFontClick);
			// 
			// fontDialog
			// 
			this.fontDialog.AllowScriptChange = false;
			this.fontDialog.AllowSimulations = false;
			this.fontDialog.FontMustExist = true;
			this.fontDialog.ShowEffects = false;
			// 
			// FontsExportForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(484, 361);
			this.Controls.Add(this.buttonRemove);
			this.Controls.Add(this.buttonAdd);
			this.Controls.Add(this.listBoxFonts);
			this.Controls.Add(this.labelSaveLocation);
			this.Controls.Add(this.buttonSelectFolder);
			this.Controls.Add(this.textBoxSaveLocation);
			this.Controls.Add(this.buttonExport);
			this.Controls.Add(this.buttonClose);
			this.MinimumSize = new System.Drawing.Size(500, 400);
			this.Name = "FontsExportForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Export fonts";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.Button buttonExport;
		private System.Windows.Forms.TextBox textBoxSaveLocation;
		private System.Windows.Forms.Button buttonSelectFolder;
		private System.Windows.Forms.Label labelSaveLocation;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.ListBox listBoxFonts;
		private System.Windows.Forms.Button buttonAdd;
		private System.Windows.Forms.Button buttonRemove;
		private System.Windows.Forms.FontDialog fontDialog;
	}
}