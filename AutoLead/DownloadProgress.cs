using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace AutoLead
{
	public class DownloadProgress : Form
	{
		private IContainer components = null;

		public ProgressBar progressBar1;

		public DownloadProgress()
		{
			this.InitializeComponent();
		}

		protected override void Dispose(bool disposing)
		{
			if ((!disposing ? false : this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DownloadProgress));
			this.progressBar1 = new ProgressBar();
			base.SuspendLayout();
			this.progressBar1.Location = new Point(39, 26);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(284, 23);
			this.progressBar1.TabIndex = 0;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(362, 81);
			base.Controls.Add(this.progressBar1);
			base.Icon = (System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "DownloadProgress";
			this.Text = "DownloadProgress";
			base.ResumeLayout(false);
		}
	}
}