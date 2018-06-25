using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace AutoLead
{
	public class OfferOption : Form
	{
		public bool @add;

		public OfferOption.updateCombo UpdateCombo;

		public OfferOption.PassControl passControl;

		private IContainer components = null;

		private Label label1;

		private TextBox textBox1;

		private CheckBox checkBox1;

		private Label label2;

		private TextBox textBox2;

		private Label label3;

		private ComboBox comboBox1;

		private Label label4;

		private CheckBox checkBox2;

		private TextBox textBox4;

		private Button button1;

		private Button button2;

		private Button button3;

		private Label label5;

		private NumericUpDown numericUpDown1;

		private Label label7;

		private NumericUpDown numericUpDown2;

		private Label label8;

		private CheckBox checkBox3;

		private NumericUpDown numericUpDown3;

		private Label label9;

		private NumericUpDown numericUpDown4;

		private Label label10;

		private Label label11;

		private Label Referer;

		private TextBox textBox3;

		public OfferOption()
		{
			this.InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Uri uri;
			bool flag;
			if (this.textBox1.Text == "")
			{
				MessageBox.Show("Offer name is required!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else if (this.textBox2.Text != "")
			{
				if (!Uri.TryCreate(this.textBox2.Text, UriKind.Absolute, out uri))
				{
					flag = false;
				}
				else
				{
					flag = (uri.Scheme == Uri.UriSchemeHttp ? true : uri.Scheme == Uri.UriSchemeHttps);
				}
				if (!flag)
				{
					MessageBox.Show("Offer URL is invalid!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				else if (this.passControl != null)
				{
					offerItem _offerItem = new offerItem()
					{
						appName = this.comboBox1.Text,
						appID = this.comboBox1.SelectedIndex.ToString(),
						offerEnable = this.checkBox1.Checked,
						offerName = this.textBox1.Text,
						offerURL = this.textBox2.Text,
						timeSleepBefore = (int)this.numericUpDown2.Value,
						timeSleepBeforeRandom = this.checkBox3.Checked,
						range1 = (int)this.numericUpDown3.Value,
						range2 = (int)this.numericUpDown4.Value,
						timeSleep = (int)this.numericUpDown1.Value,
						useScript = this.checkBox2.Checked,
						script = this.textBox4.Text,
						referer = this.textBox3.Text
					};
					this.passControl(this.@add, _offerItem);
					base.Hide();
				}
			}
			else
			{
				MessageBox.Show("Offer URL is required!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			base.Hide();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			if (this.UpdateCombo != null)
			{
				this.UpdateCombo();
			}
		}

		private void checkBox3_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.checkBox3.Checked)
			{
				this.numericUpDown2.Enabled = true;
				this.numericUpDown3.Enabled = false;
				this.numericUpDown4.Enabled = false;
			}
			else
			{
				this.numericUpDown2.Enabled = false;
				this.numericUpDown3.Enabled = true;
				this.numericUpDown4.Enabled = true;
			}
		}

		public void disableButton3()
		{
			this.button3.Enabled = false;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(OfferOption));
			this.label1 = new Label();
			this.textBox1 = new TextBox();
			this.checkBox1 = new CheckBox();
			this.label2 = new Label();
			this.textBox2 = new TextBox();
			this.label3 = new Label();
			this.comboBox1 = new ComboBox();
			this.label4 = new Label();
			this.checkBox2 = new CheckBox();
			this.textBox4 = new TextBox();
			this.button1 = new Button();
			this.button2 = new Button();
			this.button3 = new Button();
			this.label5 = new Label();
			this.numericUpDown1 = new NumericUpDown();
			this.label7 = new Label();
			this.numericUpDown2 = new NumericUpDown();
			this.label8 = new Label();
			this.checkBox3 = new CheckBox();
			this.numericUpDown3 = new NumericUpDown();
			this.label9 = new Label();
			this.numericUpDown4 = new NumericUpDown();
			this.label10 = new Label();
			this.label11 = new Label();
			this.Referer = new Label();
			this.textBox3 = new TextBox();
			((ISupportInitialize)this.numericUpDown1).BeginInit();
			((ISupportInitialize)this.numericUpDown2).BeginInit();
			((ISupportInitialize)this.numericUpDown3).BeginInit();
			((ISupportInitialize)this.numericUpDown4).BeginInit();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(32, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(62, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Offer name:";
			this.textBox1.Location = new Point(110, 16);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(100, 20);
			this.textBox1.TabIndex = 1;
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new Point(304, 18);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(59, 17);
			this.checkBox1.TabIndex = 2;
			this.checkBox1.Text = "Enable";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(32, 44);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(58, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Offer URL:";
			this.textBox2.Location = new Point(37, 60);
			this.textBox2.Multiline = true;
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(378, 31);
			this.textBox2.TabIndex = 4;
			this.label3.AutoSize = true;
			this.label3.Location = new Point(32, 269);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(51, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Tên App:";
			this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new Point(89, 265);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(121, 21);
			this.comboBox1.Sorted = true;
			this.comboBox1.TabIndex = 6;
			this.label4.AutoSize = true;
			this.label4.Location = new Point(32, 304);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(119, 13);
			this.label4.TabIndex = 7;
			this.label4.Text = "Thời gian mở ứng dụng:";
			this.checkBox2.AutoSize = true;
			this.checkBox2.Location = new Point(35, 343);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(260, 17);
			this.checkBox2.TabIndex = 9;
			this.checkBox2.Text = "Sử dụng script sau khi hết thời gian mở ứng dụng:";
			this.checkBox2.UseVisualStyleBackColor = true;
			this.textBox4.Location = new Point(35, 366);
			this.textBox4.Multiline = true;
			this.textBox4.Name = "textBox4";
			this.textBox4.ScrollBars = ScrollBars.Vertical;
			this.textBox4.Size = new System.Drawing.Size(369, 99);
			this.textBox4.TabIndex = 10;
			this.textBox4.Text = "Touch(x,y)\r\nSwipe(x1,y1,x2,y2)\r\nSend(\"text\")\r\nWait(sec)";
			this.button1.Location = new Point(218, 483);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 11;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new EventHandler(this.button1_Click);
			this.button2.Location = new Point(340, 483);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 12;
			this.button2.Text = "Cancel";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new EventHandler(this.button2_Click);
			this.button3.Enabled = false;
			this.button3.Location = new Point(231, 269);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 23);
			this.button3.TabIndex = 13;
			this.button3.Text = "Get List App:";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new EventHandler(this.button3_Click);
			this.label5.AutoSize = true;
			this.label5.Location = new Point(228, 304);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(26, 13);
			this.label5.TabIndex = 14;
			this.label5.Text = "Sec";
			this.numericUpDown1.Location = new Point(167, 302);
			this.numericUpDown1.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(55, 20);
			this.numericUpDown1.TabIndex = 15;
			this.numericUpDown1.Value = new decimal(new int[] { 20, 0, 0, 0 });
			this.label7.AutoSize = true;
			this.label7.Location = new Point(32, 205);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(118, 13);
			this.label7.TabIndex = 17;
			this.label7.Text = "Thời gian mở AppStore:";
			this.numericUpDown2.Location = new Point(174, 203);
			this.numericUpDown2.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
			this.numericUpDown2.Name = "numericUpDown2";
			this.numericUpDown2.Size = new System.Drawing.Size(48, 20);
			this.numericUpDown2.TabIndex = 18;
			this.numericUpDown2.Value = new decimal(new int[] { 10, 0, 0, 0 });
			this.label8.AutoSize = true;
			this.label8.Location = new Point(228, 205);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(26, 13);
			this.label8.TabIndex = 19;
			this.label8.Text = "Sec";
			this.checkBox3.AutoSize = true;
			this.checkBox3.Location = new Point(35, 234);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new System.Drawing.Size(110, 17);
			this.checkBox3.TabIndex = 20;
			this.checkBox3.Text = "Ngẫu nhiên mở từ";
			this.checkBox3.UseVisualStyleBackColor = true;
			this.checkBox3.CheckedChanged += new EventHandler(this.checkBox3_CheckedChanged);
			this.numericUpDown3.Enabled = false;
			this.numericUpDown3.Location = new Point(147, 232);
			this.numericUpDown3.Name = "numericUpDown3";
			this.numericUpDown3.Size = new System.Drawing.Size(52, 20);
			this.numericUpDown3.TabIndex = 21;
			this.numericUpDown3.Value = new decimal(new int[] { 2, 0, 0, 0 });
			this.label9.AutoSize = true;
			this.label9.Location = new Point(237, 234);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(27, 13);
			this.label9.TabIndex = 22;
			this.label9.Text = "Đến";
			this.numericUpDown4.Enabled = false;
			this.numericUpDown4.Location = new Point(266, 231);
			this.numericUpDown4.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
			this.numericUpDown4.Name = "numericUpDown4";
			this.numericUpDown4.Size = new System.Drawing.Size(46, 20);
			this.numericUpDown4.TabIndex = 23;
			this.numericUpDown4.Value = new decimal(new int[] { 60, 0, 0, 0 });
			this.label10.AutoSize = true;
			this.label10.Location = new Point(205, 234);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(28, 13);
			this.label10.TabIndex = 24;
			this.label10.Text = "Giây";
			this.label11.AutoSize = true;
			this.label11.Location = new Point(318, 234);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(28, 13);
			this.label11.TabIndex = 25;
			this.label11.Text = "Giây";
			this.Referer.AutoSize = true;
			this.Referer.Location = new Point(32, 101);
			this.Referer.Name = "Referer";
			this.Referer.Size = new System.Drawing.Size(45, 13);
			this.Referer.TabIndex = 26;
			this.Referer.Text = "Referer:";
			this.textBox3.Location = new Point(35, 117);
			this.textBox3.Multiline = true;
			this.textBox3.Name = "textBox3";
			this.textBox3.ScrollBars = ScrollBars.Vertical;
			this.textBox3.Size = new System.Drawing.Size(378, 65);
			this.textBox3.TabIndex = 27;
			this.textBox3.Text = "https://google.com\r\nhttps://bing.com\r\nhttps://facebook.com\r\nhttps://yahoo.com";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(442, 524);
			base.Controls.Add(this.textBox3);
			base.Controls.Add(this.Referer);
			base.Controls.Add(this.label11);
			base.Controls.Add(this.label10);
			base.Controls.Add(this.numericUpDown4);
			base.Controls.Add(this.label9);
			base.Controls.Add(this.numericUpDown3);
			base.Controls.Add(this.checkBox3);
			base.Controls.Add(this.label8);
			base.Controls.Add(this.numericUpDown2);
			base.Controls.Add(this.label7);
			base.Controls.Add(this.numericUpDown1);
			base.Controls.Add(this.label5);
			base.Controls.Add(this.button3);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.textBox4);
			base.Controls.Add(this.checkBox2);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.comboBox1);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.textBox2);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.checkBox1);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.label1);
			base.Icon = (System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "OfferOption";
			this.Text = "OfferOption";
			((ISupportInitialize)this.numericUpDown1).EndInit();
			((ISupportInitialize)this.numericUpDown2).EndInit();
			((ISupportInitialize)this.numericUpDown3).EndInit();
			((ISupportInitialize)this.numericUpDown4).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			bool flag;
			if (keyData != Keys.Escape)
			{
				flag = base.ProcessCmdKey(ref msg, keyData);
			}
			else
			{
				base.Hide();
				flag = true;
			}
			return flag;
		}

		public void resetFormData()
		{
			this.@add = true;
			this.checkBox1.Checked = false;
			this.textBox1.Text = "";
			this.textBox2.Text = "";
			this.comboBox1.Text = "";
			this.checkBox2.Checked = false;
			this.checkBox3.Checked = false;
			this.numericUpDown3.Value = new decimal(2);
			this.numericUpDown4.Value = new decimal(5);
			this.numericUpDown2.Value = new decimal(2);
			this.numericUpDown3.Enabled = false;
			this.numericUpDown4.Enabled = false;
			this.textBox4.Text = "";
			this.numericUpDown1.Value = new decimal(25);
			this.textBox3.Text = "https://google.com\r\nhttps://yahoo.com\r\nhttps://bing.com\r\nhttps://facebook.com";
		}

		public void setButton3(bool getting)
		{
			if (!getting)
			{
				this.button3.Text = "Get list";
				this.button3.Enabled = true;
			}
			else
			{
				this.button3.Text = "Getting";
				this.button3.Enabled = false;
			}
		}

		public void setComboBoxItem(object appList)
		{
			this.comboBox1.Items.Clear();
			if (appList != null)
			{
				foreach (appDetail _appDetail in (List<appDetail>)appList)
				{
					this.comboBox1.Items.Add(_appDetail.appName);
				}
			}
		}

		public void setFormData(bool offerenable, string offername, string offerurl, string appname, int sleeptimebefore, bool sleeptimebeforeenable, int range1, int range2, int sleeptime, bool usescript, string script, string referer)
		{
			this.@add = false;
			this.checkBox1.Checked = offerenable;
			this.textBox1.Text = offername;
			this.textBox2.Text = offerurl;
			this.comboBox1.Text = appname;
			this.checkBox2.Checked = usescript;
			this.checkBox3.Checked = sleeptimebeforeenable;
			this.numericUpDown3.Value = range1;
			this.numericUpDown4.Value = range2;
			this.numericUpDown2.Value = sleeptimebefore;
			this.textBox4.Text = script;
			this.numericUpDown1.Value = sleeptime;
			this.textBox3.Text = referer;
		}

		public delegate void PassControl(bool add, object sender);

		public delegate void updateCombo();
	}
}