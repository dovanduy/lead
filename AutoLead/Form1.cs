using AutoLead.Properties;
using IPAddressControlLib;
using Renci.SshNet;
using RNCryptor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Globalization;

namespace AutoLead
{
    public partial class Form1 : Form
	{
	    //public object currentSeletectItem { get; private set; }

        static Form1()
		{
			Form1.getrandom = new Random();
			Form1.syncLock = new object();
		}

		public Form1(string param)
		{
			Random random = new Random();
			this.listcommand = Resources.scriptcommand.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList<string>();
			this.usformat = new CultureInfo(this.usCulture, false);
			this.listfirstname = Resources.firstname.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList<string>();
			this.listlastname = Resources.lastname.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList<string>();
			this.listemaildomain = Resources.emaildomain.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList<string>();
			this.dataleft = "";
			this.removeandreplace();
			this.InitializeComponent();
			this.numericUpDown1.Value = random.Next(1000, 65000);
			this.DeviceIpControl.Text = Settings.Default.ipaddress;
			this.loadlangcode();
			this.loadtimezone();
			this.loadcarrier();
			this.loadgeo();
            this.loadlumi();
			this.loadcountrycodeiOS();
			this.lvwColumnSorter = new ListViewColumnSorter();
			this.listView4.ListViewItemSorter = this.lvwColumnSorter;
			this.listView4.OwnerDraw = true;
			this.ipAddressControl1.Text = Form1.IPAddresses();
			this.optionform.passControl = new OfferOption.PassControl(this.passlistview);
			this.optionform.UpdateCombo = new OfferOption.updateCombo(this.getApp);
			this.optionform.StartPosition = FormStartPosition.CenterScreen;
			this.loadcountrycode();
			ImageList imageList = new ImageList()
			{
				ImageSize = new System.Drawing.Size(1, 50)
			};
			this.listView1.SmallImageList = imageList;
			this.proxytool.Text = "SSH";
			this.cmd.sendControl = new command.SendControl(this.Send);
			//this.checkversion();
			Process[] processesByName = Process.GetProcessesByName("AutoLead");
			List<string> strs = new List<string>();
			Process[] processArray = processesByName;
			for (int i = 0; i < (int)processArray.Length; i++)
			{
				string mainWindowTitle = processArray[i].MainWindowTitle;
				if (mainWindowTitle != "")
				{
					strs.Add(mainWindowTitle);
				}
			}
			if (param == "start")
			{
				string[] strArrays = File.ReadAllLines(string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "//iplist.txt"));
				for (int j = 0; j < (int)strArrays.Length; j++)
				{
					string str = strArrays[j].Split(new string[] { "." }, StringSplitOptions.None)[3];
					if (strs.FirstOrDefault<string>((string x) => x == str) == null)
					{
						Process process = new Process();
						process.StartInfo.FileName = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "AutoLead.exe");
						process.StartInfo.Arguments = strArrays[j];
						process.Start();
						Thread.Sleep(500);
					}
				}
				Application.Exit();
				Environment.Exit(0);
			}
			if (param != "none")
			{
				this.DeviceIpControl.Text = param;
				this.button2_Click(null, null);
			}
		}



		private void _disable()
		{
			this.button29.Enabled = false;
			this.button28.Enabled = false;
			this.button27.Enabled = false;
			this.proxytool.Enabled = false;
			this.comboBox5.Enabled = false;
			this.ipAddressControl1.Enabled = false;
			this.numericUpDown1.Enabled = false;
			this.button20.Enabled = false;
			this.button9.Enabled = false;
			this.button30.Enabled = false;
			this.button11.Enabled = false;
			this.button12.Enabled = false;
			this.button10.Enabled = false;			

			this.button13.Enabled = false;
			this.wipecombo.Enabled = false;
			this.button18.Enabled = false;
		}

		private void _enable()
		{
			this.button27.Enabled = true;
			this.button29.Enabled = true;
			this.proxytool.Enabled = true;
			this.comboBox5.Enabled = true;
			this.ipAddressControl1.Enabled = true;
			this.numericUpDown1.Enabled = true;
			this.button20.Enabled = true;
			this.button9.Enabled = true;
			this.button11.Enabled = true;
			this.button30.Enabled = true;
			this.button12.Enabled = true;
			this.button10.Enabled = true;            
            this.button13.Enabled = true;
			this.wipecombo.Enabled = true;
			this.button18.Enabled = true;
			this.button28.Enabled = true;
		}

		private void AnalyData(string text)
		{
			base.Invoke(new MethodInvoker(() => {
				string[] strArrays = string.Concat(this.dataleft, text).Split(new string[] { "{[|]}" }, StringSplitOptions.None);
				if (text != "")
				{
					for (int i = 0; i < strArrays.Count<string>() - 1; i++)
					{
						string[] strArrays1 = strArrays[i].Split(new string[] { "=" }, StringSplitOptions.None);
						if (strArrays[i] != "")
						{
							string str = strArrays[i].Substring(strArrays1[0].Length + 1);
							string str1 = strArrays1[0];
							if (str1 == "getinfo")
							{
								this.label1.Text = "Connected";
								this.button7.Enabled = true;
								this.button19.Enabled = true;
								this.setInfo(str);
							}
							else if (str1 == "Applist")
							{
								this.optionform.setButton3(false);
								this.getListApp(str);
							}
							else if (str1 == "getapp-front")
							{
								if ((str == "" ? false : str != "none"))
								{
									string[] strArrays2 = str.Split(new string[] { ";" }, StringSplitOptions.None);
									this.cmdResult.frontAppByID = strArrays2[1];
									this.cmdResult.frontAppByName = strArrays2[0];
								}
								else
								{
									this.cmdResult.frontAppByID = "none";
									this.cmdResult.frontAppByName = "none";
								}
							}
							else if (str1 == "checkwipe")
							{
								this.cmdResult.wipe = true;
							}
							else if (str1 == "randomtouch")
							{
								this.cmdResult.touchrandom = true;
							}
							else if (str1 == "checkbackup")
							{
								this.cmdResult.backup = true;
							}
							else if (str1 == "checkip")
							{
								if (str != "true")
								{
									this.cmdResult.checkip = 2;
								}
								else
								{
									this.cmdResult.checkip = 1;
								}
							}
							else if (str1 == "version")
							{
								this.cmdResult.version = str;
							}
							else if (str1 == "checkrestore")
							{
								this.cmdResult.restore = true;
							}
							else if (str1 == "savecomment")
							{
								this.cmdResult.savecomment = true;
							}
							else if (str1 == "sendtext")
							{
								this.cmdResult.sendtext = Convert.ToBoolean(str);
							}
							else if (str1 == "touch")
							{
								this.cmdResult.touch = Convert.ToBoolean(str);
							}
							else if (str1 == "proxy")
							{
								if (str != "notfound")
								{
									string[] strArrays3 = str.Split(new string[] { ":" }, StringSplitOptions.None);
									this.ipAddressControl1.Text = strArrays3[0];
									this.numericUpDown1.Value = Convert.ToInt32(strArrays3[1]);
									this.oriadd = this.ipAddressControl1.Text;
									this.oriport = (int)this.numericUpDown1.Value;
									this.ipAddressControl1.Refresh();
									this.numericUpDown1.Refresh();
									if (strArrays3.Count<string>() > 2)
									{
										if (strArrays3[2] != "enable")
										{
											this.button23.Text = "Enable Proxy";
										}
										else
										{
											this.button23.Text = "Disable Proxy";
											this.button23.BackColor = Color.Red;
										}
										this.button23.Refresh();
										if (this.button23.Text.Contains("Enable"))
										{
                                            MessageBox.Show("Change Proxy from AnalyData!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);

                                            this.button23_Click(null, null);
										}
									}
								}
							}
							else if (str1 == "swipe")
							{
								this.cmdResult.swipe = Convert.ToBoolean(str);
							}
							else if (str1 == "open")
							{
								string str2 = str;
								if (str2 == "true")
								{
									this.cmdResult.openApp = 1;
								}
								else if (str2 == "notfound")
								{
									this.cmdResult.openApp = 2;
								}
								else
								{
									this.cmdResult.openApp = 3;
								}
							}
							else if (str1 == "openurl")
							{
								this.cmdResult.openURL = Convert.ToBoolean(str);
							}
							else if (str1 == "backuplist")
							{
								this.button15.Enabled = true;
								this.getListBackUp(str);
								this.cmdResult.getbackup = true;
							}
							else if (str1 == "wipe")
							{
								if (str == "done")
								{
									this.cmdResult.wipe = true;
								}
							}
							else if (str1 == "setsocks")
							{
								this.cmdResult.changeport = true;
							}
							else if (str1 == "setProxy")
							{
								if (str == "done")
								{
									this.cmdResult.changeport = true;
								}
							}
							else if (str1 == "restore")
							{
								this.cmdResult.restore = true;
							}
							else if (str1 != "backup")
							{
								if (str1 == "backupfull")
								{
									if (str == "done")
									{
										this.cmdResult.backup = true;
									}
								}
							}
							else if (str == "done")
							{
								this.cmdResult.backup = true;
							}
						}
					}
					this.dataleft = strArrays[strArrays.Count<string>() - 1];
				}
			}));
		}

		private string anaURL(string URL)
		{
			MatchCollection matchCollections = (new Regex("{randomnum\\((.*?)\\)}")).Matches(URL);
			int length = 0;
			foreach (Match match in matchCollections)
			{
				string value = match.Groups[1].Value;
				string[] strArrays = value.Replace(" ", string.Empty).Split(new string[] { "," }, StringSplitOptions.None);
				if (strArrays.Count<string>() == 2)
				{
					Random random = new Random();
					int num = random.Next(Convert.ToInt32(strArrays[0]), Convert.ToInt32(strArrays[1]) + 1);
					Random random1 = new Random();
					string str = new string((
						from s in Enumerable.Repeat<string>("123456789", num)
						select s[random1.Next(s.Length)]).ToArray<char>());
					URL = URL.Remove(match.Index - length, match.Length);
					URL = URL.Insert(match.Index - length, str);
					length = length + (match.Length - str.Length);
					Thread.Sleep(10);
				}
			}
			matchCollections = (new Regex("{randomtext\\((.*?)\\)}")).Matches(URL);
			length = 0;
			foreach (Match match1 in matchCollections)
			{
				string value1 = match1.Groups[1].Value;
				string[] strArrays1 = value1.Replace(" ", string.Empty).Split(new string[] { "," }, StringSplitOptions.None);
				if (strArrays1.Count<string>() == 2)
				{
					Random random2 = new Random();
					int num1 = random2.Next(Convert.ToInt32(strArrays1[0]), Convert.ToInt32(strArrays1[1]) + 1);
					Random random3 = new Random();
					string str1 = new string((
						from s in Enumerable.Repeat<string>("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz", num1)
						select s[random3.Next(s.Length)]).ToArray<char>());
					URL = URL.Remove(match1.Index - length, match1.Length);
					URL = URL.Insert(match1.Index - length, str1);
					length = length + (match1.Length - str1.Length);
					Thread.Sleep(10);
				}
			}
			return URL;
		}


	    private void backupthread()
		{
			string str = "";
			this.label1.Invoke(new MethodInvoker(() => {
				this.label1.Text = "Backing Up the application...";
				str = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
				this.cmd.backupfull(this.AppList[this.wipecombo.SelectedIndex].appID, str, string.Concat("[]", this.comboBox5.Text), "", "");
			}));
			this.cmdResult.backup = false;
			this.button2.Invoke(new MethodInvoker(() => this.maxwait = (int)this.numericUpDown10.Value));
			DateTime now = DateTime.Now;
			while (true)
			{
				if (!this.cmdResult.backup)
				{
					Thread.Sleep(500);
					if ((DateTime.Now - now).TotalSeconds <= (double)this.maxwait)
					{
						this.cmd.checkbackup(str);
					}
					else
					{
						this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "Request timeout..."));
						break;
					}
				}
				else
				{
					this.label1.Invoke(new MethodInvoker(() => {
						this.label1.Text = "Backup done";
						this.button13.Enabled = true;
					}));
					break;
				}
			}
		}

	    private void checkversion()
		{
			if (!File.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Routrek.granados.dll")))
			{
				WebClient webClient = new WebClient();
				webClient.DownloadFile("http://5.249.146.35/Routrek.granados.dll", string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Routrek.granados.dll"));
			}
			string productVersion = Application.ProductVersion;
			string str = "";
			try
			{
				str = (new WebClient()).DownloadString("http://5.249.146.35/version.txt");
			}
			catch (Exception exception)
			{
				MessageBox.Show("Can't conenct to server, please try again", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				//Application.Exit();
				//Environment.Exit(0);
			}
			if (productVersion == str.Replace("\r\n", ""))
			{
				System.Windows.Forms.DialogResult dialogResult = MessageBox.Show("Đã có phiên bản mới, bạn có muốn update không?", Application.ProductName, MessageBoxButtons.YesNo);
				if (dialogResult == System.Windows.Forms.DialogResult.Yes)
				{
					using (WebClient defaultNetworkCredentials = new WebClient())
					{
						try
						{
							defaultNetworkCredentials.Credentials = CredentialCache.DefaultNetworkCredentials;
							defaultNetworkCredentials.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.client_DownloadProgressChanged);
							defaultNetworkCredentials.DownloadFileCompleted += new AsyncCompletedEventHandler(this.downloadcompleted);
							defaultNetworkCredentials.DownloadFileAsync(new Uri("http://autoleadios.com/AutoLead.txt"), string.Concat(AppDomain.CurrentDomain.BaseDirectory, "_AutoLead.exe"));
						}
						catch (Exception exception1)
						{
						}
						this.downloadform.ShowDialog();
					}
				}
				else if (dialogResult == System.Windows.Forms.DialogResult.No)
				{
				}
			}
		}

	    private void DeviceIpControl_Click(object sender, EventArgs e)
		{
			Settings.Default.ipaddress = this.DeviceIpControl.Text;
		}

	    private void disableAll()
		{
			this.checkBox6.Enabled = false;
			this.checkBox7.Enabled = false;
			this.comboBox3.Enabled = false;
			this.listView7.Enabled = false;
			this.button33.Enabled = false;
			this.listView6.Enabled = false;
			this.button34.Enabled = false;
			this.button32.Enabled = false;
			this.checkBox1.Enabled = false;
			this.button3.Enabled = false;
			this.button4.Enabled = false;
			this.button5.Enabled = false;
			this.button6.Enabled = false;
			this.proxytool.Enabled = false;
			this.label5.Enabled = false;
			this.checkBox2.Enabled = false;
			this.checkBox3.Enabled = false;
			this.Reset.Enabled = false;
			this.ipAddressControl1.Enabled = false;
			this.numericUpDown1.Enabled = false;
		}

		private void disconnect()
		{
			this.dataleft = "";
			this.Text = "AutoLead for iOS";
			this.settingtime.Stop();
			this.listView7.Items.Clear();
			this.listView6.Items.Clear();
			this.listView7.Items.Add(new ListViewItem("All Script"));
			this.listscript.Clear();
			this._disable();
			this.rrsdisableall();
			this.disableAll();
			foreach (ListViewItem item in this.listView1.Items)
			{
				item.SubItems[0].ResetStyle();
				item.SubItems[1].ResetStyle();
				item.SubItems[2].ResetStyle();
				item.SubItems[3].ResetStyle();
				item.SubItems[4].ResetStyle();
				this.listView1.Refresh();
			}
			this.label12.Text = "Date Expired:";
			this.button7.Text = "START";
			this.button19.Enabled = false;
			this.button19.Refresh();
			this.button19.Text = "START";
			this.button27.Enabled = false;
			this.button29.Enabled = false;
			this.button7.Enabled = false;
			this.button7.Refresh();
			this.safariX.Enabled = false;
			this.safariY.Enabled = false;
			if (this.oThread != null)
			{
				if (this.oThread.ThreadState != System.Threading.ThreadState.Unstarted)
				{
					if (this.oThread.ThreadState == System.Threading.ThreadState.Suspended)
					{
						this.oThread.Resume();
						Thread.Sleep(100);
					}
					try
					{
						this.oThread.Abort();
					}
					catch (Exception exception)
					{
					}
				}
			}
			if (this.bkThread != null)
			{
				if (this.bkThread.ThreadState != System.Threading.ThreadState.Unstarted)
				{
					if (this.bkThread.ThreadState == System.Threading.ThreadState.Suspended)
					{
						this.bkThread.Resume();
						Thread.Sleep(100);
					}
					try
					{
						this.bkThread.Abort();
					}
					catch (Exception exception1)
					{
					}
				}
			}
			this.optionform.disableButton3();
			this.label1.Text = "Disconnected to iDevice";
			this.DeviceIpControl.Enabled = true;
			this.button2.Text = "Connect";
			this.labelSerial.Text = "Serial:";
			this.button7.Enabled = false;
			this.Reset.Enabled = false;
			this.isconnected = false;
			this.getListApp("");
			try
			{
				this._clientSocket.Shutdown(SocketShutdown.Both);
				this._clientSocket.Close();
			}
			catch (Exception exception2)
			{
			}
			if (this.autoreconnect.Checked)
			{
				(new Thread(new ThreadStart(this.reconnect))).Start();
			}
		}

		protected override void Dispose(bool disposing)
		{
			if ((!disposing ? false : this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void doscript(string marco)
		{
		}

	    private void enableAll()
		{
			this.checkBox6.Enabled = true;
			this.checkBox7.Enabled = this.checkBox6.Checked;
			this.comboBox3.Enabled = this.checkBox6.Checked;
			this.listView7.Enabled = true;
			this.button33.Enabled = true;
			this.listView6.Enabled = true;
			this.button34.Enabled = true;
			this.button32.Enabled = true;
			this.checkBox1.Enabled = true;
			this.button3.Enabled = this.button7.Text == "START";
			this.checkBox2.Enabled = true;
			this.button4.Enabled = true;
			this.button5.Enabled = this.button7.Text == "START";
			this.button6.Enabled = this.button7.Text == "START";
			this.proxytool.Enabled = true;
			this.label5.Enabled = true;
			this.checkBox3.Enabled = this.checkBox1.Checked;
			this.Reset.Enabled = this.button7.Text == "RESUME";
			this.ipAddressControl1.Enabled = true;
			this.numericUpDown1.Enabled = true;
		}

		private void excuteScript(string script)
		{
			DateTime now;
			int num;
			string str = "";
			string[] strArrays = script.Split(new string[] { "\n" }, StringSplitOptions.None);
			List<textvar> textvars = new List<textvar>();
			string[] strArrays1 = strArrays;
			for (int i = 0; i < (int)strArrays1.Length; i++)
			{
				string str1 = strArrays1[i];
				if (str1 != "")
				{
					Match match = (new Regex("(.*?)\\(")).Match(str1);
					Match match1 = (new Regex("\\((.*?)\\)")).Match(str1);
					if ((!match.Success ? false : match1.Success))
					{
						string value = match1.Groups[1].Value;
						string[] strArrays2 = value.Split(new string[] { "," }, StringSplitOptions.None);
						string value1 = "";
						string[] strArrays3 = match.Groups[1].Value.ToLower().Split(new string[] { "=" }, StringSplitOptions.None);
						string str2 = "";
						str2 = (strArrays3.Count<string>() != 2 ? match.Groups[1].Value.ToLower() : strArrays3[1]);
						string str3 = str2;
						switch (str3)
						{
							case "touchrandom":
							{
								this.label1.Invoke(new MethodInvoker(() => this.label1.Text = string.Concat("Command:", str1)));
								if (strArrays2.Count<string>() == 7)
								{
									this.cmdResult.touchrandom = false;
									Random random = new Random();
									int num1 = random.Next(Convert.ToInt32(strArrays2[4]), Convert.ToInt32(strArrays2[5]));
									this.cmd.touchRandom((double)Convert.ToInt32(strArrays2[0]), (double)Convert.ToInt32(strArrays2[1]), (double)Convert.ToInt32(strArrays2[2]), (double)Convert.ToInt32(strArrays2[3]), (double)num1, Math.Pow(10, (double)Convert.ToInt32(strArrays2[6])));
									Thread.Sleep(num1 * 1000);
									now = DateTime.Now;
									while (!this.cmdResult.touchrandom)
									{
										if ((DateTime.Now - now).Seconds < 5)
										{
											Thread.Sleep(100);
										}
										else
										{
											break;
										}
									}
								}
								break;
							}
							case "touch":
							{
								this.label1.Invoke(new MethodInvoker(() => this.label1.Text = string.Concat("Command:", str1)));
								if (strArrays2.Count<string>() == 2)
								{
									this.cmdResult.touch = false;
									this.cmd.touch(Convert.ToDouble(strArrays2[0], this.usformat), Convert.ToDouble(strArrays2[1], this.usformat));
									str = string.Concat(new string[] { str, "touch=", strArrays2[0], " ", strArrays2[1] });
									str = string.Concat(str, "|");
									now = DateTime.Now;
									while (!this.cmdResult.touch)
									{
										if ((DateTime.Now - now).Seconds < 5)
										{
											Thread.Sleep(100);
										}
										else
										{
											break;
										}
									}
								}
								break;
							}
							case "wait":
							{
								if (strArrays2.Count<string>() != 0)
								{
									Thread.Sleep((int)(Convert.ToDouble(strArrays2[0], this.usformat) * 1000));
									str = string.Concat(str, "wait=", strArrays2[0]);
									str = string.Concat(str, "|");
								}
								break;
							}
							case "swipe":
							{
								this.label1.Invoke(new MethodInvoker(() => this.label1.Text = string.Concat("Command:", str1)));
								if (strArrays2.Count<string>() == 5)
								{
									double num2 = Convert.ToDouble(strArrays2[0], this.usformat);
									double num3 = Convert.ToDouble(strArrays2[1], this.usformat);
									double num4 = Convert.ToDouble(strArrays2[2], this.usformat);
									double num5 = Convert.ToDouble(strArrays2[3], this.usformat);
									double num6 = Convert.ToDouble(strArrays2[4], this.usformat);
									double num7 = num6 / 0.01;
									double num8 = (num4 - num2) / num7;
									double num9 = (num5 - num3) / num7;
									for (int j = 0; j < (int)num7; j++)
									{
										this.cmd.mousedown((int)num2, (int)num3);
										num2 += num8;
										num3 += num9;
										Thread.Sleep(10);
										str = string.Concat(new string[] { str, "mousedown=", ((int)num2).ToString(), " ", ((int)num3).ToString() });
										str = string.Concat(str, "|");
									}
									this.cmdResult.touch = false;
									this.cmd.touch((double)((int)num4), (double)((int)num5));
									now = DateTime.Now;
									while (!this.cmdResult.touch)
									{
										if ((DateTime.Now - now).Seconds < 5)
										{
											Thread.Sleep(100);
										}
										else
										{
											break;
										}
									}
									str = string.Concat(new string[] { str, "touch=", ((int)num4).ToString(), " ", ((int)num4).ToString() });
									str = string.Concat(str, "|");
								}
								else if (strArrays2.Count<string>() == 6)
								{
									double num10 = Convert.ToDouble(strArrays2[0], this.usformat);
									double num11 = Convert.ToDouble(strArrays2[1], this.usformat);
									double num12 = Convert.ToDouble(strArrays2[2], this.usformat);
									double num13 = Convert.ToDouble(strArrays2[3], this.usformat);
									double num14 = Convert.ToDouble(strArrays2[4], this.usformat);
									double num15 = Convert.ToDouble(strArrays2[5], this.usformat);
									Random random1 = new Random();
									double num16 = (double)random1.Next((int)(num14 * 100), (int)(num15 * 100));
									double num17 = num16;
									double num18 = (num12 - num10) / num17;
									double num19 = (num13 - num11) / num17;
									for (int k = 0; k < (int)num17; k++)
									{
										this.cmd.mousedown((int)num10, (int)num11);
										num10 += num18;
										num11 += num19;
										Thread.Sleep(10);
										str = string.Concat(new string[] { str, "mousedown=", ((int)num10).ToString(), " ", ((int)num11).ToString() });
										str = string.Concat(str, "|");
									}
									this.cmdResult.touch = false;
									this.cmd.touch((double)((int)num12), (double)((int)num13));
									now = DateTime.Now;
									while (!this.cmdResult.touch)
									{
										if ((DateTime.Now - now).Seconds < 5)
										{
											Thread.Sleep(100);
										}
										else
										{
											break;
										}
									}
									str = string.Concat(new string[] { str, "touch=", ((int)num12).ToString(), " ", ((int)num12).ToString() });
									str = string.Concat(str, "|");
								}
								break;
							}
							case "close":
							{
								this.label1.Invoke(new MethodInvoker(() => this.label1.Text = string.Concat("Command:", str1)));
								Match match2 = (new Regex("'(.*?)'")).Match(str1);
								if (match2.Success)
								{
									this.cmd.close(match2.Groups[1].Value);
									str = string.Concat(str, "close=", match2.Groups[1].Value);
									str = string.Concat(str, "|");
								}
								break;
							}
							case "open":
							{
								this.label1.Invoke(new MethodInvoker(() => this.label1.Text = string.Concat("Command:", str1)));
								Match match3 = (new Regex("'(.*?)'")).Match(str1);
								if (match3.Success)
								{
									this.cmd.openApp(match3.Groups[1].Value);
									str = string.Concat(str, "open=", match3.Groups[1].Value);
									str = string.Concat(str, "|");
								}
								break;
							}
							case "send":
							{
								this.label1.Invoke(new MethodInvoker(() => this.label1.Text = string.Concat("Command:", str1)));
								Match match4 = (new Regex("'(.*?)'")).Match(str1);
								if (match4.Success)
								{
									this.cmdResult.sendtext = false;
									this.cmd.sendtext(match4.Groups[1].Value);
									value1 = match4.Groups[1].Value;
									str = string.Concat(str, "send=", match4.Groups[1].Value);
									str = string.Concat(str, "|");
									now = DateTime.Now;
									while (!this.cmdResult.sendtext)
									{
										if ((DateTime.Now - now).Seconds < 5)
										{
											Thread.Sleep(100);
										}
										else
										{
											break;
										}
									}
								}
								break;
							}
							case "waitrandom":
							{
								if (strArrays2.Count<string>() == 2)
								{
									Random random2 = new Random();
									int num20 = random2.Next(Convert.ToInt32(strArrays2[0]) * 1000, Convert.ToInt32(strArrays2[1]) * 1000);
									Thread.Sleep(num20);
									num = num20 / 1000;
									str = string.Concat(str, "wait=", num.ToString());
									str = string.Concat(str, "|");
								}
								break;
							}
							case "randomtext":
							{
								this.label1.Invoke(new MethodInvoker(() => this.label1.Text = string.Concat("Command:", str1)));
								if (strArrays2.Count<string>() == 2)
								{
									this.cmdResult.sendtext = false;
									Random random3 = new Random();
									int num21 = random3.Next(Convert.ToInt32(strArrays2[0]), Convert.ToInt32(strArrays2[1]) + 1);
									Random random4 = new Random();
									string str4 = new string((
										from s in Enumerable.Repeat<string>("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz", num21)
										select s[random4.Next(s.Length)]).ToArray<char>());
									this.cmd.sendtext(str4);
									str = string.Concat(str, "send=", str4);
									str = string.Concat(str, "|");
									value1 = str4;
									now = DateTime.Now;
									while (!this.cmdResult.sendtext)
									{
										if ((DateTime.Now - now).Seconds < 5)
										{
											Thread.Sleep(100);
										}
										else
										{
											break;
										}
									}
								}
								break;
							}
							case "randomfromfiledel":
							{
								this.label1.Invoke(new MethodInvoker(() => this.label1.Text = string.Concat("Command:", str1)));
								string str5 = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "TextFile");
								if (File.Exists(string.Concat(str5, "\\", strArrays2[0])))
								{
									string[] array = File.ReadAllLines(string.Concat(str5, "\\", strArrays2[0]));
									Random random5 = new Random();
									int num22 = random5.Next(0, array.Count<string>());
									string str6 = array[num22];
									List<string> list = array.ToList<string>();
									list.RemoveAt(num22);
									array = list.ToArray();
									File.WriteAllLines(string.Concat(str5, "\\", strArrays2[0]), array);
									this.cmd.sendtext(str6);
									str = string.Concat(str, "send=", str6);
									str = string.Concat(str, "|");
									value1 = str6;
									now = DateTime.Now;
									while (!this.cmdResult.sendtext)
									{
										if ((DateTime.Now - now).Seconds < 5)
										{
											Thread.Sleep(100);
										}
										else
										{
											break;
										}
									}
								}
								break;
							}
							case "randomfromfile":
							{
								this.label1.Invoke(new MethodInvoker(() => this.label1.Text = string.Concat("Command:", str1)));
								string str7 = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "TextFile");
								if (File.Exists(string.Concat(str7, "\\", strArrays2[0])))
								{
									string[] strArrays4 = File.ReadAllLines(string.Concat(str7, "\\", strArrays2[0]));
									Random random6 = new Random();
									string str8 = strArrays4[random6.Next(0, strArrays4.Count<string>())];
									this.cmdResult.sendtext = false;
									this.cmd.sendtext(str8);
									str = string.Concat(str, "send=", str8);
									str = string.Concat(str, "|");
									value1 = str8;
									now = DateTime.Now;
									while (!this.cmdResult.sendtext)
									{
										if ((DateTime.Now - now).Seconds < 5)
										{
											Thread.Sleep(100);
										}
										else
										{
											break;
										}
									}
								}
								break;
							}
							case "randomnumber":
							{
								this.label1.Invoke(new MethodInvoker(() => this.label1.Text = string.Concat("Command:", str1)));
								if (strArrays2.Count<string>() == 2)
								{
									this.cmdResult.sendtext = false;
									Random random7 = new Random();
									int num23 = random7.Next(Convert.ToInt32(strArrays2[0]), Convert.ToInt32(strArrays2[1]) + 1);
									Random random8 = new Random();
									string str9 = new string((
										from s in Enumerable.Repeat<string>("123456789", num23)
										select s[random8.Next(s.Length)]).ToArray<char>());
									this.cmd.sendtext(str9);
									str = string.Concat(str, "send=", str9);
									str = string.Concat(str, "|");
									value1 = str9;
									now = DateTime.Now;
									while (!this.cmdResult.sendtext)
									{
										if ((DateTime.Now - now).Seconds < 5)
										{
											Thread.Sleep(100);
										}
										else
										{
											break;
										}
									}
								}
								break;
							}
							case "randomfirstname":
							{
								this.label1.Invoke(new MethodInvoker(() => this.label1.Text = string.Concat("Command:", str1)));
								this.cmdResult.sendtext = false;
								Random random9 = new Random();
								string str10 = this.listfirstname.ElementAt<string>(random9.Next(0, this.listfirstname.Count));
								this.cmd.sendtext(str10);
								value1 = str10;
								str = string.Concat(str, "send=", str10);
								str = string.Concat(str, "|");
								now = DateTime.Now;
								while (!this.cmdResult.sendtext)
								{
									if ((DateTime.Now - now).Seconds < 5)
									{
										Thread.Sleep(100);
									}
									else
									{
										break;
									}
								}
								break;
							}
							case "randomlastname":
							{
								this.label1.Invoke(new MethodInvoker(() => this.label1.Text = string.Concat("Command:", str1)));
								this.cmdResult.sendtext = false;
								Random random10 = new Random();
								string str11 = this.listlastname.ElementAt<string>(random10.Next(0, this.listfirstname.Count));
								this.cmd.sendtext(str11);
								str = string.Concat(str, "send=", str11);
								str = string.Concat(str, "|");
								value1 = str11;
								now = DateTime.Now;
								while (!this.cmdResult.sendtext)
								{
									if ((DateTime.Now - now).Seconds < 5)
									{
										Thread.Sleep(100);
									}
									else
									{
										break;
									}
								}
								break;
							}
							case "randomemaildomain":
							{
								this.label1.Invoke(new MethodInvoker(() => this.label1.Text = string.Concat("Command:", str1)));
								this.cmdResult.sendtext = false;
								Random random11 = new Random();
								string str12 = this.listemaildomain.ElementAt<string>(random11.Next(0, this.listemaildomain.Count));
								this.cmd.sendtext(string.Concat("@", str12));
								str = string.Concat(str, "send=@", str12);
								str = string.Concat(str, "|");
								value1 = string.Concat("@", str12);
								now = DateTime.Now;
								while (!this.cmdResult.sendtext)
								{
									if ((DateTime.Now - now).Seconds < 5)
									{
										Thread.Sleep(100);
									}
									else
									{
										break;
									}
								}
								break;
							}
							case "randomemail":
							{
								this.label1.Invoke(new MethodInvoker(() => this.label1.Text = string.Concat("Command:", str1)));
								this.cmdResult.sendtext = false;
								Random random12 = new Random();
								string lower = this.listlastname.ElementAt<string>(random12.Next(0, this.listlastname.Count)).ToLower();
								lower = string.Concat(lower, this.listfirstname.ElementAt<string>(random12.Next(0, this.listfirstname.Count)).ToLower());
								if (random12.Next(0, 2) == 0)
								{
									num = random12.Next(0, 3000);
									lower = string.Concat(lower, num.ToString());
								}
								this.cmd.sendtext(lower);
								str = string.Concat(str, "send=", lower);
								str = string.Concat(str, "|");
								value1 = lower;
								now = DateTime.Now;
								while (!this.cmdResult.sendtext)
								{
									if ((DateTime.Now - now).Seconds < 5)
									{
										Thread.Sleep(100);
									}
									else
									{
										break;
									}
								}
								break;
							}
							case "sendvar":
							{
								this.label1.Invoke(new MethodInvoker(() => this.label1.Text = string.Concat("Command:", str1)));
								textvar _textvar = textvars.FirstOrDefault<textvar>((textvar x) => x.varname == strArrays2[0]);
								if (_textvar != null)
								{
									value1 = _textvar.varvalue;
									this.cmdResult.sendtext = false;
									this.cmd.sendtext(value1);
									str = string.Concat(str, "send=", value1);
									str = string.Concat(str, "|");
									now = DateTime.Now;
									while (!this.cmdResult.sendtext)
									{
										if ((DateTime.Now - now).Seconds < 5)
										{
											Thread.Sleep(100);
										}
										else
										{
											break;
										}
									}
								}
								break;
							}
						}
						if (strArrays3.Count<string>() == 2)
						{
							textvar _textvar1 = textvars.FirstOrDefault<textvar>((textvar x) => x.varname == strArrays3[0]);
							if (_textvar1 != null)
							{
								_textvar1.varvalue = value1;
							}
							else
							{
								textvar _textvar2 = new textvar()
								{
									varname = strArrays3[0],
									varvalue = value1
								};
								textvars.Add(_textvar2);
							}
						}
					}
				}
			}
		}

		private void excutescriptthread()
		{
			string text = "";
			this.label1.Invoke(new MethodInvoker(() => {
				this.label1.Text = "Running Script...";
				text = this.textBox2.Text;
			}));
			this.excuteScript(text);
			this.label1.Invoke(new MethodInvoker(() => {
				this.IPThread.Abort();
				this.button18.BackgroundImage = Resources.Play_icon__1_;
				this.scriptstatus = "stop";
				this.pausescript.Enabled = false;
				this.label1.Text = "Script done...";
				this.button18.Enabled = true;
			}));
		}

		private void excutescriptthread1()
		{
			this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "Running Script..."));
			this.excuteScript(this.textBox6.Text);
			this.label1.Invoke(new MethodInvoker(() => {
				this.label1.Text = "Script done...";
				this.button32.Enabled = true;
			}));
		}

		private void exportchanges()
		{
			if (this.checkBox16.Checked)
			{
				string str = string.Concat(new string[] { this.changeslocal.ToString(), "|", this.c_listofflocal.ToString(), "|", this.c_othersettinglocal.ToString(), "|", this.c_sshlocal.ToString(), "|", this.c_viplocal.ToString(), "|", this.c_startalllocal.ToString(), "|", this.c_stopalllocal.ToString(), "|", this.c_resetalllocal.ToString() });
				File.WriteAllText(string.Concat(this.documentfolder, "changes.dat"), str);
			}
			else
			{
				string str1 = string.Concat(new string[] { this.changes.ToString(), "|", this.c_listoff.ToString(), "|", this.c_othersetting.ToString(), "|", this.c_ssh.ToString(), "|", this.c_vip.ToString(), "|", this.c_startall.ToString(), "|", this.c_stopall.ToString(), "|", this.c_resetall.ToString() });
				File.WriteAllText(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting\\changes.dat"), str1);
			}
		}

	    private void fakeGPS(string IpAddress, double range)
		{
		}

		private void fakelang_CheckedChanged(object sender, EventArgs e)
		{
			this.comboBox1.Enabled = (!this.fakelang.Checked ? false : !this.checkBox20.Checked);
			this.saveothersetting();
		}

		private void fakeLocationByIP(string IP)
		{
			ipData pData = this.getIPData(IP);
			if (pData != null)
			{
				this.ltimezone.Text = pData.timezone;
				if (this.listcountrycodeiOS.FirstOrDefault<countrycodeiOS>((countrycodeiOS x) => x.countrycode == pData.countryCode) != null)
				{
					this.comboBox2.Text = this.listcountrycodeiOS.FirstOrDefault<countrycodeiOS>((countrycodeiOS x) => x.countrycode == pData.countryCode).countryname;
				}
				double randomNumber = (double)Form1.GetRandomNumber(-10000, 10000) / 100000;
				double num = (double)Form1.GetRandomNumber(-10000, 10000) / 100000;
				this.latitude.Value = (decimal)(pData.lat + randomNumber);
				this.longtitude.Value = (decimal)(pData.lon + num);
			}
		}

	    private void getApp()
		{
			this.optionform.setButton3(true);
			this.cmd.getAppList();
		}

		public ipData getIPData(string IpAddress)
		{
			ipData ipDatum;
			try
			{
				string str = (new WebClient()).DownloadString(string.Concat("http://pro.ip-api.com/json/", IpAddress, "?key=iUMpTiybEvs8wJl"));
				DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ipData));
				MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(str));
				ipDatum = (ipData)dataContractJsonSerializer.ReadObject(memoryStream);
			}
			catch (Exception exception)
			{
				ipDatum = null;
			}
			return ipDatum;
		}

		private void getListApp(string text)
		{
			this.AppList.Clear();
			this.AppList1.Clear();
			this.wipecombo.Items.Clear();
			string[] strArrays = text.Split(new string[] { ";" }, StringSplitOptions.None);
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				string str = strArrays[i];
				string[] strArrays1 = str.Split(new string[] { "|" }, StringSplitOptions.None);
				if (strArrays1.Count<string>() > 1)
				{
					appDetail _appDetail = new appDetail()
					{
						appID = strArrays1[0],
						appName = strArrays1[1]
					};
					this.AppList.Add(_appDetail);
					_appDetail.appName = strArrays1[1];
					this.AppList1.Add(_appDetail);
					this.wipecombo.Items.Add(_appDetail.appName);
				}
			}
			this.AppList = (
				from x in this.AppList
				orderby x.appName
				select x).ToList<appDetail>();
			this.AppList1 = (
				from x in this.AppList1
				orderby x.appName
				select x).ToList<appDetail>();
			if ((this.AppList.Count <= 0 ? false : this.wipecombo.Text == ""))
			{
				this.wipecombo.SelectedIndex = 0;
			}
			this.button28.Enabled = true;
			this.optionform.setComboBoxItem(this.AppList);
		}

		private void getListBackUp(string text)
		{
			this._sshssh = true;
			string str = string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber, "\\checkssh.dat");
			List<string> strs = new List<string>();
			if (File.Exists(str))
			{
				strs = File.ReadAllLines(str).ToList<string>();
			}
			string[] strArrays = text.Split(new string[] { "|" }, StringSplitOptions.None);
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				string str1 = strArrays[i];
				string[] strArrays1 = str1.Split(new string[] { "=" }, StringSplitOptions.None);
				if (strArrays1.Count<string>() > 1)
				{
					backup _backup = new backup()
					{
						filename = strArrays1[0]
					};
					string[] strArrays2 = strArrays1[0].Split(new string[] { "_" }, StringSplitOptions.None);
					_backup.timecreate = new DateTime(Convert.ToInt32(strArrays2[0]), Convert.ToInt32(strArrays2[1]), Convert.ToInt32(strArrays2[2]), Convert.ToInt32(strArrays2[3]), Convert.ToInt32(strArrays2[4]), Convert.ToInt32(strArrays2[5]));
					string[] strArrays3 = strArrays1[1].Split(new string[] { ";" }, StringSplitOptions.None);
					List<string> strs1 = new List<string>();
					string[] strArrays4 = strArrays3;
					for (int j = 0; j < (int)strArrays4.Length; j++)
					{
						string str2 = strArrays4[j];
						if (str2 != "")
						{
							strs1.Add(str2);
						}
					}
					_backup.appList = strs1;
					_backup.comment = "";
					if (strArrays1.Count<string>() > 4)
					{
						string[] strArrays5 = strArrays1[2].Split(new string[] { "[]" }, StringSplitOptions.None);
						_backup.country = "";
						try
						{
							_backup.comment = strArrays5[0];
							_backup.country = strArrays5[1];
						}
						catch (Exception exception)
						{
						}
						if (strArrays1[3] != "")
						{
							_backup.timemod = DateTime.Parse(strArrays1[3].Replace("CH", "PM").Replace("SA", "AM"), this.usformat);
						}
						else
						{
							_backup.timemod = _backup.timecreate;
						}
						if (strArrays1[4] != "")
						{
							_backup.runtime = Convert.ToInt32(strArrays1[4]);
						}
						else
						{
							_backup.runtime = 0;
						}
					}
					string[] str3 = new string[] { "", null, null, null, null, null, null, null };
					DateTime dateTime = _backup.timecreate;
					str3[1] = dateTime.ToString("MM/dd/yyyy HH:mm:ss");
					dateTime = _backup.timemod;
					str3[2] = dateTime.ToString("MM/dd/yyyy HH:mm:ss");
					int num = _backup.runtime;
					str3[3] = num.ToString();
					str3[4] = strArrays1[1];
					str3[5] = _backup.comment;
					str3[6] = _backup.country;
					str3[7] = _backup.filename;
					ListViewItem listViewItem = new ListViewItem(str3);
					if (strs.FirstOrDefault<string>((string x) => x == _backup.filename) != null)
					{
						listViewItem.Checked = true;
					}
					this.listView4.Items.Add(listViewItem);
					this.listbackup.Add(_backup);
					num = Convert.ToInt32(this.label34.Text.Replace("Total RRS:", "")) + 1;
					this.label34.Text = string.Concat("Total RRS:", num.ToString());
				}
			}
			this._sshssh = false;
		}

		private string getrandomdevice()
		{
			string str;
			bool flag;
			if (!this.fakemodel.Checked)
			{
				flag = true;
			}
			else
			{
				flag = (!this.fakemodel.Checked || this.ipad.Checked || this.iphone.Checked ? false : !this.ipod.Checked);
			}
			if (!flag)
			{
				string str1 = "abcxyz";
				string str2 = "abcxyz";
				string str3 = "abcxyz";
				if (this.ipad.Checked)
				{
					str1 = "iPad";
				}
				if (this.iphone.Checked)
				{
					str2 = "iPhone";
				}
				if (this.ipod.Checked)
				{
					str3 = "iPod";
				}
				List<string> list = Resources.iDevice.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList<string>();
				Random random = new Random();
				int num = random.Next(0, list.Count);
				while (true)
				{
					if ((list.ElementAt<string>(num).Contains(str1) || list.ElementAt<string>(num).Contains(str2) ? true : list.ElementAt<string>(num).Contains(str3)))
					{
						break;
					}
					num = random.Next(0, list.Count);
				}
				str = list.ElementAt<string>(num).Substring(0, list.ElementAt<string>(num).Length - 3);
			}
			else
			{
				str = "";
			}
			return str;
		}

		public static int GetRandomNumber(int min, int max)
		{
			int num;
			lock (Form1.syncLock)
			{
				num = Form1.getrandom.Next(min, max);
			}
			return num;
		}

		private void getuseragentfordevice()
		{
		}

	    private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Form1));
			ListViewItem listViewItem = new ListViewItem("All Script");
			this.groupBox1 = new GroupBox();
			this.button20 = new Button();
			this.button23 = new Button();
			this.proxytool = new ComboBox();
			this.label21 = new Label();
			this.label5 = new Label();
			this.label20 = new Label();
			this.ipAddressControl1 = new IPAddressControl();
			this.label3 = new Label();
			this.numericUpDown1 = new NumericUpDown();
			this.comboBox5 = new ComboBox();
			this.label6 = new Label();
			this.button1 = new Button();
			this.label1 = new Label();
			this.label2 = new Label();
			this.button2 = new Button();
			this.DeviceIpControl = new IPAddressControl();
			this.label4 = new Label();
			this.labelSerial = new Label();
			this.label12 = new Label();
			this.imageList1 = new ImageList(this.components);
			this.autoreconnect = new CheckBox();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.deleteToolStripMenuItem = new ToolStripMenuItem();
			this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.deleteToolStripMenuItem1 = new ToolStripMenuItem();
			this.moveToSlotToolStripMenuItem = new ToolStripMenuItem();
			this.toolStripMenuItem1 = new ToolStripMenuItem();
			this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.bảoVệDữLiệuToolStripMenuItem = new ToolStripMenuItem();
			this.contextMenuStrip4 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.deleteToolStripMenuItem2 = new ToolStripMenuItem();
			this.tabPage5 = new TabPage();
			this.label25 = new Label();
			this.label24 = new Label();
			this.label23 = new Label();
			this.label13 = new Label();
			this.pictureBox5 = new PictureBox();
			this.pictureBox4 = new PictureBox();
			this.pictureBox3 = new PictureBox();
			this.pictureBox2 = new PictureBox();
			this.tabPage10 = new TabPage();
			this.button54 = new Button();
			this.checkBox16 = new CheckBox();
			this.button53 = new Button();
			this.button52 = new Button();
			this.button51 = new Button();
			this.button50 = new Button();
			this.button49 = new Button();
			this.button48 = new Button();
			this.button47 = new Button();
			this.button46 = new Button();
			this.button45 = new Button();
			this.button44 = new Button();
			this.button43 = new Button();
			this.button42 = new Button();
			this.button41 = new Button();
			this.button40 = new Button();
			this.button39 = new Button();
			this.button38 = new Button();
			this.button37 = new Button();
			this.tabPage9 = new TabPage();
			this.groupBox8 = new GroupBox();
			this.napcodestt = new Label();
			this.deviceseri = new TextBox();
			this.button36 = new Button();
			this.label36 = new Label();
			this.code = new TextBox();
			this.label37 = new Label();
			this.Script = new TabPage();
			this.textBox9 = new TextBox();
			this.label33 = new Label();
			this.button34 = new Button();
			this.button33 = new Button();
			this.button32 = new Button();
			this.listView7 = new ListView();
			this.columnHeader14 = new ColumnHeader();
			this.textBox6 = new TextBox();
			this.listView6 = new ListView();
			this.columnHeader13 = new ColumnHeader();
			this.tabPage6 = new TabPage();
			this.textBox5 = new TextBox();
			this.listView5 = new ListView();
			this.groupBox6 = new GroupBox();
			this.checkBox20 = new CheckBox();
			this.longtitude = new NumericUpDown();
			this.ltimezone = new Label();
			this.checkBox19 = new CheckBox();
			this.checkBox13 = new CheckBox();
			this.label41 = new Label();
			this.checkBox5 = new CheckBox();
			this.label40 = new Label();
			this.checkBox9 = new CheckBox();
			this.latitude = new NumericUpDown();
			this.carrierBox = new ComboBox();
			this.fakelang = new CheckBox();
			this.comboBox2 = new ComboBox();
			this.fakeregion = new CheckBox();
			this.comboBox1 = new ComboBox();
			this.groupBox7 = new GroupBox();
			this.label63 = new Label();
			this.label43 = new Label();
			this.numericUpDown10 = new NumericUpDown();
			this.label31 = new Label();
			this.label32 = new Label();
			this.numericUpDown5 = new NumericUpDown();
			this.checkBox4 = new CheckBox();
			this.numericUpDown4 = new NumericUpDown();
			this.label30 = new Label();
			this.groupBox9 = new GroupBox();
			this.fakeversion = new CheckBox();
			this.iphone = new CheckBox();
			this.ipad = new CheckBox();
			this.ipod = new CheckBox();
			this.checkBox11 = new CheckBox();
			this.fakemodel = new CheckBox();
			this.fileofname = new Label();
			this.fakedevice = new CheckBox();
			this.checkBox15 = new CheckBox();
			this.checkBox14 = new CheckBox();
			this.tabPage8 = new TabPage();
			this.pausescript = new Button();
			this.textBox2 = new RichTextBox();
			this.button64 = new Button();
			this.button35 = new Button();
			this.button30 = new Button();
			this.trackBar1 = new TrackBar();
			this.groupBox4 = new GroupBox();
			this.button11 = new Button();
			this.textBox1 = new TextBox();
			this.groupBox3 = new GroupBox();
			this.button26 = new Button();
			this.textBox7 = new TextBox();
			this.label18 = new Label();
			this.label19 = new Label();
			this.textBox8 = new TextBox();
			this.textBox3 = new TextBox();
			this.button18 = new Button();
			this.groupBox5 = new GroupBox();
			this.button28 = new Button();
			this.label11 = new Label();
			this.button13 = new Button();
			this.button12 = new Button();

			this.button10 = new Button();			

			this.wipecombo = new ComboBox();
			this.pictureBox1 = new PictureBox();
			this.tabPage7 = new TabPage();
			this.label35 = new Label();
			this.label34 = new Label();
			this.checkBox7 = new CheckBox();
			this.comboBox3 = new ComboBox();
			this.checkBox6 = new CheckBox();
			this.button31 = new Button();
			this.label26 = new Label();
			this.textBox4 = new TextBox();
			this.button29 = new Button();
			this.button27 = new Button();
			this.label9 = new Label();
			this.rsswaitnum = new NumericUpDown();
			this.label8 = new Label();
			this.bkreset = new Button();
			this.button19 = new Button();
			this.button17 = new Button();
			this.button16 = new Button();
			this.button15 = new Button();
			this.listView4 = new ListView();
			this.columnHeader10 = new ColumnHeader();
			this.columnHeader7 = new ColumnHeader();
			this.columnHeader11 = new ColumnHeader();
			this.columnHeader12 = new ColumnHeader();
			this.columnHeader8 = new ColumnHeader();
			this.columnHeader9 = new ColumnHeader();
			this.columnHeader15 = new ColumnHeader();
			this.columnHeader16 = new ColumnHeader();
			this.tabPage2 = new TabPage();
			this.tabControl2 = new TabControl();
			this.tabPage3 = new TabPage();
			this.checkBox17 = new CheckBox();
			this.ss_dead = new Label();
			this.ssh_used = new Label();
			this.ssh_live = new Label();
			this.ssh_uncheck = new Label();
			this.numericUpDown2 = new NumericUpDown();
			this.label14 = new Label();
			this.labeltotalssh = new Label();
			this.button25 = new Button();
			this.button24 = new Button();
			this.button22 = new Button();
			this.button8 = new Button();
			this.button14 = new Button();
			this.button9 = new Button();
			this.importfromfile = new Button();
			this.listView2 = new ListView();
			this.columnHeader1 = new ColumnHeader();
			this.columnHeader2 = new ColumnHeader();
			this.columnHeader3 = new ColumnHeader();
			this.columnHeader4 = new ColumnHeader();


			this.tabPage4 = new TabPage();
			this.sameVip = new CheckBox();
			this.button57 = new Button();
			this.groupBox2 = new GroupBox();
			this.vippassword = new TextBox();
			this.vipid = new TextBox();
			this.label10 = new Label();
			this.label7 = new Label();
			this.vipadd = new Button();
			this.vipdelete = new Button();
			this.listView3 = new ListView();
			this.columnHeader5 = new ColumnHeader();
			this.columnHeader6 = new ColumnHeader();

		    this.tabPageQuan4 = new TabPage();		   
		    this.groupBoxQuan2 = new GroupBox();
		    this.lumipassword = new TextBox();
		    this.lumiid = new TextBox();
		    this.lumizone = new TextBox();

		    this.labelQuan10 = new Label();
		    this.labelQuan7 = new Label();
		    this.labelQuanZone = new Label();
		    this.lumiadd = new Button();
		    this.lumidelete = new Button();
		    this.listViewQuan3 = new ListView();
		    this.columnHeaderQuan5 = new ColumnHeader();
		    this.columnHeaderQuan6 = new ColumnHeader();
		    this.columnHeaderQuanZone = new ColumnHeader();

            this.tabPage1 = new TabPage();
			this.textBox11 = new TextBox();
			this.comment = new TextBox();
			this.label44 = new Label();
			this.checkBox18 = new CheckBox();
			this.checkBox12 = new CheckBox();
			this.numericUpDown6 = new NumericUpDown();
			this.checkBox10 = new CheckBox();
			this.backuprate = new Label();
			this.backupoftime = new Label();
			this.runoftime = new Label();
			this.label29 = new Label();
			this.label28 = new Label();
			this.numericUpDown3 = new NumericUpDown();
			this.label27 = new Label();
			this.itunesY = new NumericUpDown();
			this.label22 = new Label();
			this.itunesX = new NumericUpDown();
			this.label17 = new Label();
			this.safariY = new NumericUpDown();
			this.label16 = new Label();
			this.safariX = new NumericUpDown();
			this.label15 = new Label();
			this.button21 = new Button();
			this.Reset = new Button();
			this.checkBox3 = new CheckBox();
			this.checkBox2 = new CheckBox();
			this.button7 = new Button();
			this.checkBox1 = new CheckBox();
			this.button6 = new Button();
			this.button5 = new Button();
			this.button4 = new Button();
			this.button3 = new Button();
			this.listView1 = new ListView();
			this.offername = new ColumnHeader();
			this.offerurl = new ColumnHeader();
			this.appname = new ColumnHeader();
			this.timedelay = new ColumnHeader();
			this.usescript = new ColumnHeader();
			this.Contact = new TabControl();
			this.l_autover = new Label();
			this.groupBox1.SuspendLayout();
			((ISupportInitialize)this.numericUpDown1).BeginInit();
			this.contextMenuStrip1.SuspendLayout();
			this.contextMenuStrip2.SuspendLayout();
			this.contextMenuStrip3.SuspendLayout();
			this.tabPage5.SuspendLayout();
			((ISupportInitialize)this.pictureBox5).BeginInit();
			((ISupportInitialize)this.pictureBox4).BeginInit();
			((ISupportInitialize)this.pictureBox3).BeginInit();
			((ISupportInitialize)this.pictureBox2).BeginInit();
			this.tabPage10.SuspendLayout();
			this.tabPage9.SuspendLayout();
			this.groupBox8.SuspendLayout();
			this.Script.SuspendLayout();
			this.tabPage6.SuspendLayout();
			this.groupBox6.SuspendLayout();
			((ISupportInitialize)this.longtitude).BeginInit();
			((ISupportInitialize)this.latitude).BeginInit();
			this.groupBox7.SuspendLayout();
			((ISupportInitialize)this.numericUpDown10).BeginInit();
			((ISupportInitialize)this.numericUpDown5).BeginInit();
			((ISupportInitialize)this.numericUpDown4).BeginInit();
			this.groupBox9.SuspendLayout();
			this.tabPage8.SuspendLayout();
			((ISupportInitialize)this.trackBar1).BeginInit();
			this.groupBox4.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox5.SuspendLayout();
			((ISupportInitialize)this.pictureBox1).BeginInit();
			this.tabPage7.SuspendLayout();
			((ISupportInitialize)this.rsswaitnum).BeginInit();
			this.tabPage2.SuspendLayout();
			this.tabControl2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			((ISupportInitialize)this.numericUpDown2).BeginInit();


			this.tabPage4.SuspendLayout();
			this.groupBox2.SuspendLayout();

		    this.tabPageQuan4.SuspendLayout();
		    this.groupBoxQuan2.SuspendLayout();


            this.tabPage1.SuspendLayout();
			((ISupportInitialize)this.numericUpDown6).BeginInit();
			((ISupportInitialize)this.numericUpDown3).BeginInit();
			((ISupportInitialize)this.itunesY).BeginInit();
			((ISupportInitialize)this.itunesX).BeginInit();
			((ISupportInitialize)this.safariY).BeginInit();
			((ISupportInitialize)this.safariX).BeginInit();
			this.Contact.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.BackColor = SystemColors.GradientInactiveCaption;
			this.groupBox1.Controls.Add(this.button20);
			this.groupBox1.Controls.Add(this.button23);
			this.groupBox1.Controls.Add(this.proxytool);
			this.groupBox1.Controls.Add(this.label21);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label20);
			this.groupBox1.Controls.Add(this.ipAddressControl1);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.numericUpDown1);
			this.groupBox1.Controls.Add(this.comboBox5);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Location = new Point(14, 442);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(626, 86);
			this.groupBox1.TabIndex = 17;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Proxy Setting";
			this.button20.Location = new Point(266, 49);
			this.button20.Name = "button20";
			this.button20.Size = new System.Drawing.Size(75, 23);
			this.button20.TabIndex = 17;
			this.button20.Text = "Change IP";
			this.button20.UseVisualStyleBackColor = true;
			this.button20.Click += new EventHandler(this.button20_Click_1);
			this.button23.BackColor = Color.Aqua;
			this.button23.Location = new Point(478, 19);
			this.button23.Name = "button23";
			this.button23.Size = new System.Drawing.Size(96, 33);
			this.button23.TabIndex = 16;
			this.button23.Text = "Apply";
			this.button23.UseVisualStyleBackColor = false;
			this.button23.Click += new EventHandler(this.button23_Click);
			this.proxytool.DisplayMember = "SSH";
			this.proxytool.DropDownStyle = ComboBoxStyle.DropDownList;
			this.proxytool.FormattingEnabled = true;
			this.proxytool.Items.AddRange(new object[] { "Vip72", "SSH", "Direct", "Lumi" });
			this.proxytool.Location = new Point(98, 23);
			this.proxytool.Name = "proxytool";
			this.proxytool.Size = new System.Drawing.Size(69, 21);
			this.proxytool.TabIndex = 11;
			this.proxytool.SelectedIndexChanged += new EventHandler(this.proxytool_SelectedIndexChanged);
			this.label21.AutoSize = true;
			this.label21.Location = new Point(225, 28);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(20, 13);
			this.label21.TabIndex = 9;
			this.label21.Text = "IP:";
			this.label5.AutoSize = true;
			this.label5.Location = new Point(21, 26);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(60, 13);
			this.label5.TabIndex = 10;
			this.label5.Text = "Proxy Tool:";
			this.label20.AutoSize = true;
			this.label20.Location = new Point(25, 52);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(46, 13);
			this.label20.TabIndex = 7;
			this.label20.Text = "Country:";
			this.ipAddressControl1.AllowInternalTab = false;
			this.ipAddressControl1.AutoHeight = true;
			this.ipAddressControl1.BackColor = SystemColors.Window;
			this.ipAddressControl1.BorderStyle = BorderStyle.Fixed3D;
			this.ipAddressControl1.Cursor = Cursors.IBeam;
			this.ipAddressControl1.Enabled = false;
			this.ipAddressControl1.Location = new Point(260, 25);
			this.ipAddressControl1.MinimumSize = new System.Drawing.Size(87, 20);
			this.ipAddressControl1.Name = "ipAddressControl1";
			this.ipAddressControl1.ReadOnly = false;
			this.ipAddressControl1.Size = new System.Drawing.Size(87, 20);
			this.ipAddressControl1.TabIndex = 8;
			this.ipAddressControl1.Text = "...";
			this.ipAddressControl1.TextChanged += new EventHandler(this.ipAddressControl1_TextChanged);
			this.ipAddressControl1.Click += new EventHandler(this.ipAddressControl1_Click);
			this.label3.AutoSize = true;
			this.label3.Location = new Point(58, 53);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(0, 13);
			this.label3.TabIndex = 7;
			this.numericUpDown1.Enabled = false;
			this.numericUpDown1.Location = new Point(411, 26);
			this.numericUpDown1.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
			this.numericUpDown1.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(50, 20);
			this.numericUpDown1.TabIndex = 4;
		    this.numericUpDown1.Value = new decimal(new int[] { 1080, 0, 0, 0 });
			this.numericUpDown1.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
			this.comboBox5.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBox5.FormattingEnabled = true;
			this.comboBox5.Location = new Point(98, 49);
			this.comboBox5.Name = "comboBox5";
			this.comboBox5.Size = new System.Drawing.Size(91, 21);
			this.comboBox5.TabIndex = 6;
			this.comboBox5.SelectedIndexChanged += new EventHandler(this.comboBox5_SelectedIndexChanged);
			this.label6.AutoSize = true;
			this.label6.Location = new Point(376, 28);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(29, 13);
			this.label6.TabIndex = 3;
			this.label6.Text = "Port:";
			this.button1.Location = new Point(541, 600);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "Close";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new EventHandler(this.button1_Click);
			this.label1.AutoSize = true;
			this.label1.FlatStyle = FlatStyle.System;
			this.label1.Location = new Point(39, 605);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(37, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Status";
			this.label2.AutoSize = true;
			this.label2.Location = new Point(39, 542);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(57, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Device IP:";
			this.button2.Location = new Point(280, 534);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 5;
			this.button2.Text = "Connect";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new EventHandler(this.button2_Click);
			this.DeviceIpControl.AllowInternalTab = false;
			this.DeviceIpControl.AutoHeight = true;
			this.DeviceIpControl.BackColor = SystemColors.Window;
			this.DeviceIpControl.BorderStyle = BorderStyle.Fixed3D;
			this.DeviceIpControl.Cursor = Cursors.IBeam;
			this.DeviceIpControl.Location = new Point(117, 536);
			this.DeviceIpControl.MinimumSize = new System.Drawing.Size(87, 20);
			this.DeviceIpControl.Name = "DeviceIpControl";
			this.DeviceIpControl.ReadOnly = false;
			this.DeviceIpControl.Size = new System.Drawing.Size(118, 20);
			this.DeviceIpControl.TabIndex = 6;
			this.DeviceIpControl.Text = "...";
			this.DeviceIpControl.TextChanged += new EventHandler(this.DeviceIpControl_TextChanged);
			this.DeviceIpControl.Click += new EventHandler(this.DeviceIpControl_Click);
			this.label4.AutoSize = true;
			this.label4.Location = new Point(52, 575);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(0, 13);
			this.label4.TabIndex = 8;
			this.labelSerial.AutoSize = true;
			this.labelSerial.Location = new Point(39, 575);
			this.labelSerial.Name = "labelSerial";
			this.labelSerial.Size = new System.Drawing.Size(76, 13);
			this.labelSerial.TabIndex = 12;
			this.labelSerial.Text = "Serial Number:";
			this.label12.AutoSize = true;
			this.label12.Location = new Point(184, 575);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(71, 13);
			this.label12.TabIndex = 18;
			this.label12.Text = "Date Expired:";
			this.imageList1.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageList1.ImageStream");
			this.imageList1.TransparentColor = Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "Folder.png");
			this.autoreconnect.AutoSize = true;
			this.autoreconnect.Location = new Point(412, 538);
			this.autoreconnect.Name = "autoreconnect";
			this.autoreconnect.Size = new System.Drawing.Size(104, 17);
			this.autoreconnect.TabIndex = 19;
			this.autoreconnect.Text = "Auto Reconnect";
			this.autoreconnect.UseVisualStyleBackColor = true;
			this.autoreconnect.CheckedChanged += new EventHandler(this.autoreconnect_CheckedChanged);
			this.contextMenuStrip1.Items.AddRange(new ToolStripItem[] { this.deleteToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(108, 26);
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
			this.deleteToolStripMenuItem.Text = "Delete";
			this.deleteToolStripMenuItem.Click += new EventHandler(this.deleteToolStripMenuItem_Click);
			this.contextMenuStrip2.Items.AddRange(new ToolStripItem[] { this.deleteToolStripMenuItem1, this.moveToSlotToolStripMenuItem });
			this.contextMenuStrip2.Name = "contextMenuStrip2";
			this.contextMenuStrip2.Size = new System.Drawing.Size(144, 48);
			this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
			this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(143, 22);
			this.deleteToolStripMenuItem1.Text = "Delete";
			this.deleteToolStripMenuItem1.Click += new EventHandler(this.deleteToolStripMenuItem1_Click);
			this.moveToSlotToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.toolStripMenuItem1 });
			this.moveToSlotToolStripMenuItem.Name = "moveToSlotToolStripMenuItem";
			this.moveToSlotToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
			this.moveToSlotToolStripMenuItem.Text = "Move To Slot";
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(103, 22);
			this.toolStripMenuItem1.Text = "None";
			this.toolStripMenuItem1.Click += new EventHandler(this.toolStripMenuItem1_Click);
			this.contextMenuStrip3.Items.AddRange(new ToolStripItem[] { this.bảoVệDữLiệuToolStripMenuItem });
			this.contextMenuStrip3.Name = "contextMenuStrip3";
			this.contextMenuStrip3.Size = new System.Drawing.Size(68, 26);
			this.bảoVệDữLiệuToolStripMenuItem.Name = "bảoVệDữLiệuToolStripMenuItem";
			this.bảoVệDữLiệuToolStripMenuItem.Size = new System.Drawing.Size(67, 22);
			this.contextMenuStrip4.Name = "contextMenuStrip4";
			this.contextMenuStrip4.Size = new System.Drawing.Size(61, 4);
			this.deleteToolStripMenuItem2.Name = "deleteToolStripMenuItem2";
			this.deleteToolStripMenuItem2.Size = new System.Drawing.Size(32, 19);
			this.tabPage5.Controls.Add(this.label25);
			this.tabPage5.Controls.Add(this.label24);
			this.tabPage5.Controls.Add(this.label23);
			this.tabPage5.Controls.Add(this.label13);
			this.tabPage5.Controls.Add(this.pictureBox5);
			this.tabPage5.Controls.Add(this.pictureBox4);
			this.tabPage5.Controls.Add(this.pictureBox3);
			this.tabPage5.Controls.Add(this.pictureBox2);
			this.tabPage5.Location = new Point(4, 22);
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage5.Size = new System.Drawing.Size(620, 398);
			this.tabPage5.TabIndex = 5;
			this.tabPage5.Text = "Contact";
			this.tabPage5.UseVisualStyleBackColor = true;
			this.label25.AutoSize = true;
			this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.label25.Location = new Point(133, 281);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(150, 16);
			this.label25.TabIndex = 8;
			this.label25.Text = "";
			this.label24.AutoSize = true;
			this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.label24.Location = new Point(133, 205);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(169, 16);
			this.label24.TabIndex = 7;
			this.label24.Text = "https://facebook.com/phatdatpq";
			this.label23.AutoSize = true;
			this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.label23.Location = new Point(133, 133);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(58, 16);
			this.label23.TabIndex = 6;
			this.label23.Text = "";
			this.label13.AutoSize = true;
			this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.label13.Location = new Point(133, 57);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(96, 16);
			this.label13.TabIndex = 5;
			this.label13.Text = "";
			this.pictureBox5.Image = (Image)componentResourceManager.GetObject("pictureBox5.Image");
			this.pictureBox5.Location = new Point(40, 261);
			this.pictureBox5.Name = "pictureBox5";
			this.pictureBox5.Size = new System.Drawing.Size(51, 60);
			this.pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
			this.pictureBox5.TabIndex = 4;
			this.pictureBox5.TabStop = false;
			this.pictureBox4.Image = Resources.social_facebook_box_blue_icon;
			this.pictureBox4.Location = new Point(43, 185);
			this.pictureBox4.Name = "pictureBox4";
			this.pictureBox4.Size = new System.Drawing.Size(48, 50);
			this.pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
			this.pictureBox4.TabIndex = 3;
			this.pictureBox4.TabStop = false;
			this.pictureBox3.Image = Resources.Skype_icon;
			this.pictureBox3.Location = new Point(43, 113);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new System.Drawing.Size(48, 50);
			this.pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
			this.pictureBox3.TabIndex = 2;
			this.pictureBox3.TabStop = false;
			this.pictureBox2.Image = Resources.Phone_icon;
			this.pictureBox2.Location = new Point(40, 31);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(51, 62);
			this.pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
			this.pictureBox2.TabIndex = 1;
			this.pictureBox2.TabStop = false;
			this.tabPage10.Controls.Add(this.button54);
			this.tabPage10.Controls.Add(this.checkBox16);
			this.tabPage10.Controls.Add(this.button53);
			this.tabPage10.Controls.Add(this.button52);
			this.tabPage10.Controls.Add(this.button51);
			this.tabPage10.Controls.Add(this.button50);
			this.tabPage10.Controls.Add(this.button49);
			this.tabPage10.Controls.Add(this.button48);
			this.tabPage10.Controls.Add(this.button47);
			this.tabPage10.Controls.Add(this.button46);
			this.tabPage10.Controls.Add(this.button45);
			this.tabPage10.Controls.Add(this.button44);
			this.tabPage10.Controls.Add(this.button43);
			this.tabPage10.Controls.Add(this.button42);
			this.tabPage10.Controls.Add(this.button41);
			this.tabPage10.Controls.Add(this.button40);
			this.tabPage10.Controls.Add(this.button39);
			this.tabPage10.Controls.Add(this.button38);
			this.tabPage10.Controls.Add(this.button37);
			this.tabPage10.Location = new Point(4, 22);
			this.tabPage10.Name = "tabPage10";
			this.tabPage10.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage10.Size = new System.Drawing.Size(620, 398);
			this.tabPage10.TabIndex = 9;
			this.tabPage10.Text = "ExSetting";
			this.tabPage10.UseVisualStyleBackColor = true;
			this.button54.Location = new Point(215, 343);
			this.button54.Name = "button54";
			this.button54.Size = new System.Drawing.Size(98, 34);
			this.button54.TabIndex = 18;
			this.button54.Text = "STOP ALL";
			this.button54.UseVisualStyleBackColor = true;
			this.button54.Click += new EventHandler(this.button54_Click);
			this.checkBox16.AutoSize = true;
			this.checkBox16.Checked = true;
			this.checkBox16.CheckState = CheckState.Checked;
			this.checkBox16.Location = new Point(478, 352);
			this.checkBox16.Name = "checkBox16";
			this.checkBox16.Size = new System.Drawing.Size(114, 17);
			this.checkBox16.TabIndex = 17;
			this.checkBox16.Text = "Phạm Vi Máy Tính";
			this.checkBox16.UseVisualStyleBackColor = true;
			this.checkBox16.Visible = false;
			this.button53.Location = new Point(358, 342);
			this.button53.Name = "button53";
			this.button53.Size = new System.Drawing.Size(101, 34);
			this.button53.TabIndex = 16;
			this.button53.Text = "RESET ALL";
			this.button53.UseVisualStyleBackColor = true;
			this.button53.Click += new EventHandler(this.button53_Click);
			this.button52.Location = new Point(59, 342);
			this.button52.Name = "button52";
			this.button52.Size = new System.Drawing.Size(95, 35);
			this.button52.TabIndex = 15;
			this.button52.Text = "START ALL";
			this.button52.UseVisualStyleBackColor = true;
			this.button52.Click += new EventHandler(this.button52_Click);
			this.button51.Location = new Point(39, 289);
			this.button51.Name = "button51";
			this.button51.Size = new System.Drawing.Size(117, 23);
			this.button51.TabIndex = 14;
			this.button51.Text = "Set Other Setting";
			this.button51.UseVisualStyleBackColor = true;
			this.button51.Click += new EventHandler(this.button51_Click);
			this.button50.Location = new Point(39, 229);
			this.button50.Name = "button50";
			this.button50.Size = new System.Drawing.Size(117, 23);
			this.button50.TabIndex = 13;
			this.button50.Text = "Set Vip72";
			this.button50.UseVisualStyleBackColor = true;
			this.button50.Click += new EventHandler(this.button50_Click);
			this.button49.Location = new Point(39, 170);
			this.button49.Name = "button49";
			this.button49.Size = new System.Drawing.Size(117, 23);
			this.button49.TabIndex = 12;
			this.button49.Text = "Set SSH";
			this.button49.UseVisualStyleBackColor = true;
			this.button49.Click += new EventHandler(this.button49_Click);
			this.button48.Location = new Point(39, 118);
			this.button48.Name = "button48";
			this.button48.Size = new System.Drawing.Size(117, 23);
			this.button48.TabIndex = 11;
			this.button48.Text = "Set Offer List";
			this.button48.UseVisualStyleBackColor = true;
			this.button48.Click += new EventHandler(this.button48_Click);
			this.button47.Location = new Point(39, 63);
			this.button47.Name = "button47";
			this.button47.Size = new System.Drawing.Size(117, 23);
			this.button47.TabIndex = 10;
			this.button47.Text = "Set All Setting";
			this.button47.UseVisualStyleBackColor = true;
			this.button47.Click += new EventHandler(this.button47_Click);
			this.button46.Location = new Point(431, 288);
			this.button46.Name = "button46";
			this.button46.Size = new System.Drawing.Size(116, 23);
			this.button46.TabIndex = 9;
			this.button46.Text = "Paste Other Setting";
			this.button46.UseVisualStyleBackColor = true;
			this.button46.Click += new EventHandler(this.button46_Click);
			this.button45.Location = new Point(431, 228);
			this.button45.Name = "button45";
			this.button45.Size = new System.Drawing.Size(116, 23);
			this.button45.TabIndex = 8;
			this.button45.Text = "Paste Vip72";
			this.button45.UseVisualStyleBackColor = true;
			this.button45.Click += new EventHandler(this.button45_Click);
			this.button44.Location = new Point(431, 169);
			this.button44.Name = "button44";
			this.button44.Size = new System.Drawing.Size(116, 23);
			this.button44.TabIndex = 7;
			this.button44.Text = "Paste SSH";
			this.button44.UseVisualStyleBackColor = true;
			this.button44.Click += new EventHandler(this.button44_Click);
			this.button43.Location = new Point(431, 118);
			this.button43.Name = "button43";
			this.button43.Size = new System.Drawing.Size(116, 23);
			this.button43.TabIndex = 6;
			this.button43.Text = "Paste Offer List";
			this.button43.UseVisualStyleBackColor = true;
			this.button43.Click += new EventHandler(this.button43_Click);
			this.button42.Location = new Point(431, 63);
			this.button42.Name = "button42";
			this.button42.Size = new System.Drawing.Size(116, 23);
			this.button42.TabIndex = 5;
			this.button42.Text = "Paste All Setting";
			this.button42.UseVisualStyleBackColor = true;
			this.button42.Click += new EventHandler(this.button42_Click);
			this.button41.Location = new Point(234, 289);
			this.button41.Name = "button41";
			this.button41.Size = new System.Drawing.Size(111, 23);
			this.button41.TabIndex = 4;
			this.button41.Text = "Copy Other Setting";
			this.button41.UseVisualStyleBackColor = true;
			this.button41.Click += new EventHandler(this.button41_Click);
			this.button40.Location = new Point(234, 229);
			this.button40.Name = "button40";
			this.button40.Size = new System.Drawing.Size(111, 23);
			this.button40.TabIndex = 3;
			this.button40.Text = "Copy Vip72";
			this.button40.UseVisualStyleBackColor = true;
			this.button40.Click += new EventHandler(this.button40_Click);
			this.button39.Location = new Point(234, 170);
			this.button39.Name = "button39";
			this.button39.Size = new System.Drawing.Size(111, 23);
			this.button39.TabIndex = 2;
			this.button39.Text = "Copy SSH";
			this.button39.UseVisualStyleBackColor = true;
			this.button39.Click += new EventHandler(this.button39_Click);
			this.button38.Location = new Point(234, 118);
			this.button38.Name = "button38";
			this.button38.Size = new System.Drawing.Size(111, 23);
			this.button38.TabIndex = 1;
			this.button38.Text = "Copy Offer List";
			this.button38.UseVisualStyleBackColor = true;
			this.button38.Click += new EventHandler(this.button38_Click);
			this.button37.Location = new Point(234, 63);
			this.button37.Name = "button37";
			this.button37.Size = new System.Drawing.Size(111, 23);
			this.button37.TabIndex = 0;
			this.button37.Text = "Copy All Setting";
			this.button37.UseVisualStyleBackColor = true;
			this.button37.Click += new EventHandler(this.button37_Click);
			this.tabPage9.Controls.Add(this.groupBox8);
			this.tabPage9.Location = new Point(4, 22);
			this.tabPage9.Name = "tabPage9";
			this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage9.Size = new System.Drawing.Size(620, 398);
			this.tabPage9.TabIndex = 8;
			this.tabPage9.Text = "Nạp Code Quan";
			this.tabPage9.UseVisualStyleBackColor = true;
			this.groupBox8.Controls.Add(this.napcodestt);
			this.groupBox8.Controls.Add(this.deviceseri);
			this.groupBox8.Controls.Add(this.button36);
			this.groupBox8.Controls.Add(this.label36);
			this.groupBox8.Controls.Add(this.code);
			this.groupBox8.Controls.Add(this.label37);
			this.groupBox8.Location = new Point(110, 80);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Size = new System.Drawing.Size(404, 207);
			this.groupBox8.TabIndex = 5;
			this.groupBox8.TabStop = false;
			this.napcodestt.AutoSize = true;
			this.napcodestt.Location = new Point(63, 179);
			this.napcodestt.Name = "napcodestt";
			this.napcodestt.Size = new System.Drawing.Size(0, 13);
			this.napcodestt.TabIndex = 5;
			this.deviceseri.Location = new Point(138, 68);
			this.deviceseri.Name = "deviceseri";
			this.deviceseri.Size = new System.Drawing.Size(192, 20);
			this.deviceseri.TabIndex = 3;
			this.button36.Location = new Point(154, 112);
			this.button36.Name = "button36";
			this.button36.Size = new System.Drawing.Size(96, 33);
			this.button36.TabIndex = 4;
			this.button36.Text = "Nạp Code";
			this.button36.UseVisualStyleBackColor = true;
			this.button36.Click += new EventHandler(this.button36_Click);
			this.label36.AutoSize = true;
			this.label36.Location = new Point(56, 33);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(35, 13);
			this.label36.TabIndex = 0;
			this.label36.Text = "Code:";
			this.code.Location = new Point(138, 30);
			this.code.Name = "code";
			this.code.Size = new System.Drawing.Size(192, 20);
			this.code.TabIndex = 1;
			this.label37.AutoSize = true;
			this.label37.Location = new Point(56, 71);
			this.label37.Name = "label37";
			this.label37.Size = new System.Drawing.Size(73, 13);
			this.label37.TabIndex = 2;
			this.label37.Text = "Device Serial:";
			this.Script.Controls.Add(this.textBox9);
			this.Script.Controls.Add(this.label33);
			this.Script.Controls.Add(this.button34);
			this.Script.Controls.Add(this.button33);
			this.Script.Controls.Add(this.button32);
			this.Script.Controls.Add(this.listView7);
			this.Script.Controls.Add(this.textBox6);
			this.Script.Controls.Add(this.listView6);
			this.Script.Location = new Point(4, 22);
			this.Script.Name = "Script";
			this.Script.Padding = new System.Windows.Forms.Padding(3);
			this.Script.Size = new System.Drawing.Size(620, 398);
			this.Script.TabIndex = 7;
			this.Script.Text = "Script";
			this.Script.UseVisualStyleBackColor = true;
			this.textBox9.Enabled = false;
			this.textBox9.Location = new Point(435, 22);
			this.textBox9.Name = "textBox9";
			this.textBox9.Size = new System.Drawing.Size(117, 20);
			this.textBox9.TabIndex = 9;
			this.textBox9.TextChanged += new EventHandler(this.textBox9_TextChanged);
			this.label33.AutoSize = true;
			this.label33.Location = new Point(359, 25);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(68, 13);
			this.label33.TabIndex = 8;
			this.label33.Text = "Script Name:";
			this.button34.Enabled = false;
			this.button34.Location = new Point(177, 325);
			this.button34.Name = "button34";
			this.button34.Size = new System.Drawing.Size(75, 23);
			this.button34.TabIndex = 7;
			this.button34.Text = "Add Script";
			this.button34.UseVisualStyleBackColor = true;
			this.button34.Click += new EventHandler(this.button34_Click);
			this.button33.Enabled = false;
			this.button33.Location = new Point(39, 325);
			this.button33.Name = "button33";
			this.button33.Size = new System.Drawing.Size(75, 23);
			this.button33.TabIndex = 6;
			this.button33.Text = "Add Slot";
			this.button33.UseVisualStyleBackColor = true;
			this.button33.Click += new EventHandler(this.button33_Click);
			this.button32.Enabled = false;
			this.button32.Location = new Point(425, 325);
			this.button32.Name = "button32";
			this.button32.Size = new System.Drawing.Size(75, 23);
			this.button32.TabIndex = 5;
			this.button32.Text = "Test Script";
			this.button32.UseVisualStyleBackColor = true;
			this.button32.Click += new EventHandler(this.button32_Click);
			this.listView7.Columns.AddRange(new ColumnHeader[] { this.columnHeader14 });
			this.listView7.Enabled = false;
			this.listView7.Items.AddRange(new ListViewItem[] { listViewItem });
			this.listView7.Location = new Point(22, 22);
			this.listView7.Name = "listView7";
			this.listView7.Size = new System.Drawing.Size(103, 297);
			this.listView7.TabIndex = 2;
			this.listView7.UseCompatibleStateImageBehavior = false;
			this.listView7.View = View.Details;
			this.listView7.SelectedIndexChanged += new EventHandler(this.listView7_SelectedIndexChanged);
			this.listView7.KeyDown += new KeyEventHandler(this.listView7_KeyDown);
			this.listView7.MouseClick += new MouseEventHandler(this.listView7_MouseClick);
			this.listView7.MouseDown += new MouseEventHandler(this.listView7_MouseDown);
			this.listView7.MouseUp += new MouseEventHandler(this.listView7_MouseUp);
			this.columnHeader14.Text = "Slot";
			this.columnHeader14.Width = 89;
			this.textBox6.AutoCompleteCustomSource.AddRange(new string[] { "wait", "randomtext", "randomnumber", "send", "randomemail", "randomfirstname", "randomlastname", "waitrandom", "touch", "swipe", "close", "open" });
			this.textBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			this.textBox6.AutoCompleteSource = AutoCompleteSource.CustomSource;
			this.textBox6.Enabled = false;
			this.textBox6.Location = new Point(360, 55);
			this.textBox6.Multiline = true;
			this.textBox6.Name = "textBox6";
			this.textBox6.ScrollBars = ScrollBars.Vertical;
			this.textBox6.Size = new System.Drawing.Size(204, 264);
			this.textBox6.TabIndex = 1;
			this.textBox6.TextChanged += new EventHandler(this.textBox6_TextChanged);
			this.listView6.Columns.AddRange(new ColumnHeader[] { this.columnHeader13 });
			this.listView6.Enabled = false;
			this.listView6.Location = new Point(150, 22);
			this.listView6.Name = "listView6";
			this.listView6.Size = new System.Drawing.Size(140, 297);
			this.listView6.TabIndex = 0;
			this.listView6.UseCompatibleStateImageBehavior = false;
			this.listView6.View = View.Details;
			this.listView6.SelectedIndexChanged += new EventHandler(this.listView6_SelectedIndexChanged);
			this.listView6.KeyDown += new KeyEventHandler(this.listView6_KeyDown);
			this.listView6.MouseClick += new MouseEventHandler(this.listView6_MouseClick);
			this.listView6.MouseDown += new MouseEventHandler(this.listView6_MouseDown);
			this.listView6.MouseUp += new MouseEventHandler(this.listView6_MouseUp);
			this.columnHeader13.Text = "Script";
			this.columnHeader13.Width = 132;
			this.tabPage6.Controls.Add(this.textBox5);
			this.tabPage6.Controls.Add(this.listView5);
			this.tabPage6.Controls.Add(this.groupBox6);
			this.tabPage6.Controls.Add(this.groupBox7);
			this.tabPage6.Controls.Add(this.groupBox9);
			this.tabPage6.Location = new Point(4, 22);
			this.tabPage6.Name = "tabPage6";
			this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage6.Size = new System.Drawing.Size(620, 398);
			this.tabPage6.TabIndex = 6;
			this.tabPage6.Text = "Setting";
			this.tabPage6.UseVisualStyleBackColor = true;
			this.tabPage6.MouseClick += new MouseEventHandler(this.tabPage6_MouseClick);
			this.textBox5.Location = new Point(403, 48);
			this.textBox5.Name = "textBox5";
			this.textBox5.Size = new System.Drawing.Size(193, 20);
			this.textBox5.TabIndex = 16;
			this.textBox5.Visible = false;
			this.textBox5.TextChanged += new EventHandler(this.textBox5_TextChanged);
			this.listView5.GridLines = true;
			this.listView5.Location = new Point(403, 68);
			this.listView5.Name = "listView5";
			this.listView5.Size = new System.Drawing.Size(193, 150);
			this.listView5.TabIndex = 15;
			this.listView5.UseCompatibleStateImageBehavior = false;
			this.listView5.View = View.Tile;
			this.listView5.Visible = false;
			this.listView5.MouseClick += new MouseEventHandler(this.listView5_MouseClick);
			this.listView5.MouseDoubleClick += new MouseEventHandler(this.listView5_MouseDoubleClick);
			this.groupBox6.Controls.Add(this.checkBox20);
			this.groupBox6.Controls.Add(this.longtitude);
			this.groupBox6.Controls.Add(this.ltimezone);
			this.groupBox6.Controls.Add(this.checkBox19);
			this.groupBox6.Controls.Add(this.checkBox13);
			this.groupBox6.Controls.Add(this.label41);
			this.groupBox6.Controls.Add(this.checkBox5);
			this.groupBox6.Controls.Add(this.label40);
			this.groupBox6.Controls.Add(this.checkBox9);
			this.groupBox6.Controls.Add(this.latitude);
			this.groupBox6.Controls.Add(this.carrierBox);
			this.groupBox6.Controls.Add(this.fakelang);
			this.groupBox6.Controls.Add(this.comboBox2);
			this.groupBox6.Controls.Add(this.fakeregion);
			this.groupBox6.Controls.Add(this.comboBox1);
			this.groupBox6.Location = new Point(22, 15);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(578, 141);
			this.groupBox6.TabIndex = 14;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Fake Location Setting";
			this.checkBox20.AutoSize = true;
			this.checkBox20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.checkBox20.ForeColor = SystemColors.Highlight;
			this.checkBox20.Location = new Point(9, 16);
			this.checkBox20.Name = "checkBox20";
			this.checkBox20.Size = new System.Drawing.Size(227, 19);
			this.checkBox20.TabIndex = 26;
			this.checkBox20.Text = "Tự Động Fake Location Theo IP";
			this.checkBox20.UseVisualStyleBackColor = true;
			this.checkBox20.CheckedChanged += new EventHandler(this.checkBox20_CheckedChanged);
			this.longtitude.DecimalPlaces = 6;
			this.longtitude.Location = new Point(338, 106);
			this.longtitude.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
			this.longtitude.Minimum = new decimal(new int[] { 180, 0, 0, -2147483648 });
			this.longtitude.Name = "longtitude";
			this.longtitude.Size = new System.Drawing.Size(67, 20);
			this.longtitude.TabIndex = 25;
			this.longtitude.ValueChanged += new EventHandler(this.longtitude_ValueChanged);
			this.ltimezone.AutoSize = true;
			this.ltimezone.Location = new Point(381, 16);
			this.ltimezone.Name = "ltimezone";
			this.ltimezone.Size = new System.Drawing.Size(96, 13);
			this.ltimezone.TabIndex = 14;
			this.ltimezone.Text = "Asia/Ho_Chi_Minh";
			this.ltimezone.TextChanged += new EventHandler(this.ltimezone_TextChanged);
			this.ltimezone.Click += new EventHandler(this.ltimezone_Click);
			this.checkBox19.AutoSize = true;
			this.checkBox19.Location = new Point(9, 106);
			this.checkBox19.Name = "checkBox19";
			this.checkBox19.Size = new System.Drawing.Size(75, 17);
			this.checkBox19.TabIndex = 21;
			this.checkBox19.Text = "Fake GPS";
			this.checkBox19.UseVisualStyleBackColor = true;
			this.checkBox19.CheckedChanged += new EventHandler(this.checkBox19_CheckedChanged);
			this.checkBox13.AutoSize = true;
			this.checkBox13.Checked = true;
			this.checkBox13.CheckState = CheckState.Checked;
			this.checkBox13.Location = new Point(9, 47);
			this.checkBox13.Name = "checkBox13";
			this.checkBox13.Size = new System.Drawing.Size(83, 17);
			this.checkBox13.TabIndex = 12;
			this.checkBox13.Text = "Fake Carrier";
			this.checkBox13.UseVisualStyleBackColor = true;
			this.checkBox13.CheckedChanged += new EventHandler(this.checkBox13_CheckedChanged);
			this.label41.AutoSize = true;
			this.label41.Location = new Point(270, 108);
			this.label41.Name = "label41";
			this.label41.Size = new System.Drawing.Size(57, 13);
			this.label41.TabIndex = 24;
			this.label41.Text = "Longitude:";
			this.checkBox5.AutoSize = true;
			this.checkBox5.Location = new Point(277, 15);
			this.checkBox5.Name = "checkBox5";
			this.checkBox5.Size = new System.Drawing.Size(101, 17);
			this.checkBox5.TabIndex = 13;
			this.checkBox5.Text = "Fake TimeZone";
			this.checkBox5.UseVisualStyleBackColor = true;
			this.checkBox5.CheckedChanged += new EventHandler(this.checkBox5_CheckedChanged);
			this.label40.AutoSize = true;
			this.label40.Location = new Point(121, 108);
			this.label40.Name = "label40";
			this.label40.Size = new System.Drawing.Size(48, 13);
			this.label40.TabIndex = 22;
			this.label40.Text = "Latitude:";
			this.checkBox9.AutoSize = true;
			this.checkBox9.Location = new Point(130, 47);
			this.checkBox9.Name = "checkBox9";
			this.checkBox9.Size = new System.Drawing.Size(100, 17);
			this.checkBox9.TabIndex = 17;
			this.checkBox9.Text = "Custom Country";
			this.checkBox9.UseVisualStyleBackColor = true;
			this.checkBox9.CheckedChanged += new EventHandler(this.checkBox9_CheckedChanged);
			this.latitude.DecimalPlaces = 6;
			this.latitude.Location = new Point(173, 106);
			this.latitude.Maximum = new decimal(new int[] { 90, 0, 0, 0 });
			this.latitude.Minimum = new decimal(new int[] { 90, 0, 0, -2147483648 });
			this.latitude.Name = "latitude";
			this.latitude.Size = new System.Drawing.Size(74, 20);
			this.latitude.TabIndex = 23;
			this.latitude.TextAlign = HorizontalAlignment.Right;
			this.latitude.ValueChanged += new EventHandler(this.latitude_ValueChanged);
			this.carrierBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this.carrierBox.Enabled = false;
			this.carrierBox.FormattingEnabled = true;
			this.carrierBox.Location = new Point(262, 45);
			this.carrierBox.Name = "carrierBox";
			this.carrierBox.Size = new System.Drawing.Size(194, 21);
			this.carrierBox.Sorted = true;
			this.carrierBox.TabIndex = 18;
			this.carrierBox.SelectedIndexChanged += new EventHandler(this.carrierBox_SelectedIndexChanged);
			this.fakelang.AutoSize = true;
			this.fakelang.Location = new Point(9, 77);
			this.fakelang.Name = "fakelang";
			this.fakelang.Size = new System.Drawing.Size(101, 17);
			this.fakelang.TabIndex = 5;
			this.fakelang.Text = "Fake Language";
			this.fakelang.UseVisualStyleBackColor = true;
			this.fakelang.CheckedChanged += new EventHandler(this.fakelang_CheckedChanged);
			this.comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBox2.Enabled = false;
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Location = new Point(380, 73);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(118, 21);
			this.comboBox2.Sorted = true;
			this.comboBox2.TabIndex = 8;
			this.comboBox2.SelectedIndexChanged += new EventHandler(this.comboBox2_SelectedIndexChanged);
			this.fakeregion.AutoSize = true;
			this.fakeregion.Location = new Point(287, 77);
			this.fakeregion.Name = "fakeregion";
			this.fakeregion.Size = new System.Drawing.Size(87, 17);
			this.fakeregion.TabIndex = 7;
			this.fakeregion.Text = "Fake Region";
			this.fakeregion.UseVisualStyleBackColor = true;
			this.fakeregion.CheckedChanged += new EventHandler(this.fakeregion_CheckedChanged);
			this.comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			this.comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBox1.Enabled = false;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new Point(143, 73);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(108, 21);
			this.comboBox1.Sorted = true;
			this.comboBox1.TabIndex = 6;
			this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
			this.comboBox1.TextChanged += new EventHandler(this.comboBox1_TextChanged);
			this.comboBox1.KeyPress += new KeyPressEventHandler(this.comboBox1_KeyPress);
			this.groupBox7.Controls.Add(this.label63);
			this.groupBox7.Controls.Add(this.label43);
			this.groupBox7.Controls.Add(this.numericUpDown10);
			this.groupBox7.Controls.Add(this.label31);
			this.groupBox7.Controls.Add(this.label32);
			this.groupBox7.Controls.Add(this.numericUpDown5);
			this.groupBox7.Controls.Add(this.checkBox4);
			this.groupBox7.Controls.Add(this.numericUpDown4);
			this.groupBox7.Controls.Add(this.label30);
			this.groupBox7.Location = new Point(22, 276);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(578, 116);
			this.groupBox7.TabIndex = 13;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Tool Setting";
			this.groupBox7.Enter += new EventHandler(this.groupBox7_Enter);
			this.label63.AutoSize = true;
			this.label63.Location = new Point(214, 48);
			this.label63.Name = "label63";
			this.label63.Size = new System.Drawing.Size(28, 13);
			this.label63.TabIndex = 9;
			this.label63.Text = "Giây";
			this.label43.AutoSize = true;
			this.label43.Location = new Point(6, 47);
			this.label43.Name = "label43";
			this.label43.Size = new System.Drawing.Size(128, 13);
			this.label43.TabIndex = 8;
			this.label43.Text = "Thời gian wipe App tối đa";
			this.numericUpDown10.Location = new Point(144, 45);
			this.numericUpDown10.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
			this.numericUpDown10.Name = "numericUpDown10";
			this.numericUpDown10.Size = new System.Drawing.Size(55, 20);
			this.numericUpDown10.TabIndex = 7;
			this.numericUpDown10.Value = new decimal(new int[] { 120, 0, 0, 0 });
			this.numericUpDown10.ValueChanged += new EventHandler(this.numericUpDown10_ValueChanged);
			this.label31.AutoSize = true;
			this.label31.Location = new Point(240, 21);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(49, 13);
			this.label31.TabIndex = 2;
			this.label31.Text = "Seconds";
			this.label32.AutoSize = true;
			this.label32.Location = new Point(384, 77);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(25, 13);
			this.label32.TabIndex = 5;
			this.label32.Text = "Lần";
			this.numericUpDown5.Location = new Point(305, 70);
			this.numericUpDown5.Name = "numericUpDown5";
			this.numericUpDown5.Size = new System.Drawing.Size(55, 20);
			this.numericUpDown5.TabIndex = 4;
			this.numericUpDown5.Value = new decimal(new int[] { 4, 0, 0, 0 });
			this.numericUpDown5.ValueChanged += new EventHandler(this.numericUpDown5_ValueChanged);
			this.checkBox4.AutoSize = true;
			this.checkBox4.Location = new Point(9, 73);
			this.checkBox4.Name = "checkBox4";
			this.checkBox4.Size = new System.Drawing.Size(275, 17);
			this.checkBox4.TabIndex = 3;
			this.checkBox4.Text = "Tự động bỏ tick offer nếu offer không vào AppStore ";
			this.checkBox4.UseVisualStyleBackColor = true;
			this.checkBox4.CheckedChanged += new EventHandler(this.checkBox4_CheckedChanged);
			this.numericUpDown4.Location = new Point(181, 19);
			this.numericUpDown4.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
			this.numericUpDown4.Name = "numericUpDown4";
			this.numericUpDown4.Size = new System.Drawing.Size(47, 20);
			this.numericUpDown4.TabIndex = 1;
			this.numericUpDown4.Value = new decimal(new int[] { 60, 0, 0, 0 });
			this.numericUpDown4.ValueChanged += new EventHandler(this.numericUpDown4_ValueChanged);
			this.label30.AutoSize = true;
			this.label30.Location = new Point(4, 21);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(155, 13);
			this.label30.TabIndex = 0;
			this.label30.Text = "Thời gian load Offer URL tối đa";
			this.groupBox9.Controls.Add(this.fakeversion);
			this.groupBox9.Controls.Add(this.iphone);
			this.groupBox9.Controls.Add(this.ipad);
			this.groupBox9.Controls.Add(this.ipod);
			this.groupBox9.Controls.Add(this.checkBox11);
			this.groupBox9.Controls.Add(this.fakemodel);
			this.groupBox9.Controls.Add(this.fileofname);
			this.groupBox9.Controls.Add(this.fakedevice);
			this.groupBox9.Controls.Add(this.checkBox15);
			this.groupBox9.Controls.Add(this.checkBox14);
			this.groupBox9.Location = new Point(22, 171);
			this.groupBox9.Name = "groupBox9";
			this.groupBox9.Size = new System.Drawing.Size(578, 99);
			this.groupBox9.TabIndex = 15;
			this.groupBox9.TabStop = false;
			this.groupBox9.Text = "Fake Device Setting";
			this.fakeversion.AutoSize = true;
			this.fakeversion.Checked = true;
			this.fakeversion.CheckState = CheckState.Checked;
			this.fakeversion.Location = new Point(9, 49);
			this.fakeversion.Name = "fakeversion";
			this.fakeversion.Size = new System.Drawing.Size(125, 17);
			this.fakeversion.TabIndex = 11;
			this.fakeversion.Text = "Fake Device Version";
			this.fakeversion.UseVisualStyleBackColor = true;
			this.fakeversion.CheckedChanged += new EventHandler(this.fakeversion_CheckedChanged);
			this.iphone.AutoSize = true;
			this.iphone.Checked = true;
			this.iphone.CheckState = CheckState.Checked;
			this.iphone.Location = new Point(276, 77);
			this.iphone.Name = "iphone";
			this.iphone.Size = new System.Drawing.Size(59, 17);
			this.iphone.TabIndex = 3;
			this.iphone.Text = "iPhone";
			this.iphone.UseVisualStyleBackColor = true;
			this.iphone.CheckedChanged += new EventHandler(this.iphone_CheckedChanged);
			this.ipad.AutoSize = true;
			this.ipad.Checked = true;
			this.ipad.CheckState = CheckState.Checked;
			this.ipad.Location = new Point(183, 77);
			this.ipad.Name = "ipad";
			this.ipad.Size = new System.Drawing.Size(47, 17);
			this.ipad.TabIndex = 2;
			this.ipad.Text = "iPad";
			this.ipad.UseVisualStyleBackColor = true;
			this.ipad.CheckedChanged += new EventHandler(this.ipad_CheckedChanged);
			this.ipod.AutoSize = true;
			this.ipod.Location = new Point(366, 77);
			this.ipod.Name = "ipod";
			this.ipod.Size = new System.Drawing.Size(77, 17);
			this.ipod.TabIndex = 4;
			this.ipod.Text = "iPod touch";
			this.ipod.UseVisualStyleBackColor = true;
			this.ipod.CheckedChanged += new EventHandler(this.ipod_CheckedChanged);
			this.checkBox11.AutoSize = true;
			this.checkBox11.Location = new Point(183, 18);
			this.checkBox11.Name = "checkBox11";
			this.checkBox11.Size = new System.Drawing.Size(68, 17);
			this.checkBox11.TabIndex = 9;
			this.checkBox11.Text = "From file:";
			this.checkBox11.UseVisualStyleBackColor = true;
			this.checkBox11.CheckedChanged += new EventHandler(this.checkBox11_CheckedChanged);
			this.fakemodel.AutoSize = true;
			this.fakemodel.Checked = true;
			this.fakemodel.CheckState = CheckState.Checked;
			this.fakemodel.Location = new Point(9, 77);
			this.fakemodel.Name = "fakemodel";
			this.fakemodel.Size = new System.Drawing.Size(119, 17);
			this.fakemodel.TabIndex = 1;
			this.fakemodel.Text = "Fake Device Model";
			this.fakemodel.UseVisualStyleBackColor = true;
			this.fakemodel.CheckedChanged += new EventHandler(this.fakemodel_CheckedChanged);
			this.fileofname.AutoSize = true;
			this.fileofname.Location = new Point(273, 17);
			this.fileofname.Name = "fileofname";
			this.fileofname.Size = new System.Drawing.Size(0, 13);
			this.fileofname.TabIndex = 10;
			this.fakedevice.AutoSize = true;
			this.fakedevice.Checked = true;
			this.fakedevice.CheckState = CheckState.Checked;
			this.fakedevice.Location = new Point(9, 19);
			this.fakedevice.Name = "fakedevice";
			this.fakedevice.Size = new System.Drawing.Size(118, 17);
			this.fakedevice.TabIndex = 0;
			this.fakedevice.Text = "Fake Device Name";
			this.fakedevice.UseVisualStyleBackColor = true;
			this.fakedevice.CheckedChanged += new EventHandler(this.fakedevice_CheckedChanged);
			this.checkBox15.AutoSize = true;
			this.checkBox15.Checked = true;
			this.checkBox15.CheckState = CheckState.Checked;
			this.checkBox15.Location = new Point(276, 49);
			this.checkBox15.Name = "checkBox15";
			this.checkBox15.Size = new System.Drawing.Size(52, 17);
			this.checkBox15.TabIndex = 20;
			this.checkBox15.Text = "iOS 9";
			this.checkBox15.UseVisualStyleBackColor = true;
			this.checkBox15.CheckedChanged += new EventHandler(this.checkBox15_CheckedChanged);
			this.checkBox14.AutoSize = true;
			this.checkBox14.Checked = true;
			this.checkBox14.CheckState = CheckState.Checked;
			this.checkBox14.Location = new Point(183, 49);
			this.checkBox14.Name = "checkBox14";
			this.checkBox14.Size = new System.Drawing.Size(52, 17);
			this.checkBox14.TabIndex = 19;
			this.checkBox14.Text = "iOS 10";
			this.checkBox14.UseVisualStyleBackColor = true;
			this.checkBox14.CheckedChanged += new EventHandler(this.checkBox14_CheckedChanged);
			this.tabPage8.Controls.Add(this.pausescript);
			this.tabPage8.Controls.Add(this.textBox2);
			this.tabPage8.Controls.Add(this.button64);
			this.tabPage8.Controls.Add(this.button35);
			this.tabPage8.Controls.Add(this.button30);
			this.tabPage8.Controls.Add(this.trackBar1);
			this.tabPage8.Controls.Add(this.groupBox4);
			this.tabPage8.Controls.Add(this.groupBox3);
			this.tabPage8.Controls.Add(this.button18);
			this.tabPage8.Controls.Add(this.groupBox5);
			this.tabPage8.Controls.Add(this.pictureBox1);
			this.tabPage8.Location = new Point(4, 22);
			this.tabPage8.Name = "tabPage8";
			this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage8.Size = new System.Drawing.Size(620, 398);
			this.tabPage8.TabIndex = 4;
			this.tabPage8.Text = "Support";
			this.tabPage8.UseVisualStyleBackColor = true;
			this.tabPage8.Click += new EventHandler(this.tabPage8_Click);
			this.pausescript.BackgroundImage = Resources.red_stop_playback;
			this.pausescript.BackgroundImageLayout = ImageLayout.Stretch;
			this.pausescript.Enabled = false;
			this.pausescript.Location = new Point(82, 307);
			this.pausescript.Name = "pausescript";
			this.pausescript.Size = new System.Drawing.Size(31, 29);
			this.pausescript.TabIndex = 26;
			this.pausescript.UseVisualStyleBackColor = true;
			this.pausescript.Click += new EventHandler(this.pausescript_Click);
			this.textBox2.Location = new Point(119, 271);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(220, 111);
			this.textBox2.TabIndex = 25;
			this.textBox2.Text = "";
			this.textBox2.TextChanged += new EventHandler(this.textBox2_TextChanged_1);
			this.button64.BackgroundImage = Resources.fast_delivery_3;
			this.button64.BackgroundImageLayout = ImageLayout.Stretch;
			this.button64.Location = new Point(341, 24);
			this.button64.Name = "button64";
			this.button64.Size = new System.Drawing.Size(40, 39);
			this.button64.TabIndex = 22;
			this.button64.UseVisualStyleBackColor = true;
			this.button64.Click += new EventHandler(this.button64_Click);
			this.button35.Location = new Point(29, 351);
			this.button35.Name = "button35";
			this.button35.Size = new System.Drawing.Size(75, 23);
			this.button35.TabIndex = 20;
			this.button35.Text = "Mở rộng";
			this.button35.UseVisualStyleBackColor = true;
			this.button35.Click += new EventHandler(this.button35_Click);
			this.button30.Location = new Point(377, 353);
			this.button30.Name = "button30";
			this.button30.Size = new System.Drawing.Size(82, 31);
			this.button30.TabIndex = 19;
			this.button30.Text = "Record";
			this.button30.UseVisualStyleBackColor = true;
			this.button30.Click += new EventHandler(this.button30_Click);
			this.trackBar1.Location = new Point(468, 347);
			this.trackBar1.Maximum = 6;
			this.trackBar1.Minimum = 1;
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Size = new System.Drawing.Size(117, 45);
			this.trackBar1.TabIndex = 18;
			this.trackBar1.Value = 2;
			this.groupBox4.Controls.Add(this.button11);
			this.groupBox4.Controls.Add(this.textBox1);
			this.groupBox4.Location = new Point(6, 101);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(340, 82);
			this.groupBox4.TabIndex = 16;
			this.groupBox4.TabStop = false;
			this.button11.Enabled = false;
			this.button11.Location = new Point(138, 12);
			this.button11.Name = "button11";
			this.button11.Size = new System.Drawing.Size(75, 23);
			this.button11.TabIndex = 9;
			this.button11.Text = "Open URL";
			this.button11.UseVisualStyleBackColor = true;
			this.button11.Click += new EventHandler(this.button11_Click);
			this.textBox1.Location = new Point(23, 45);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(293, 20);
			this.textBox1.TabIndex = 10;
			this.textBox1.TextChanged += new EventHandler(this.textBox1_TextChanged);
			this.groupBox3.Controls.Add(this.button26);
			this.groupBox3.Controls.Add(this.textBox7);
			this.groupBox3.Controls.Add(this.label18);
			this.groupBox3.Controls.Add(this.label19);
			this.groupBox3.Controls.Add(this.textBox8);
			this.groupBox3.Controls.Add(this.textBox3);
			this.groupBox3.Location = new Point(11, 8);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(286, 92);
			this.groupBox3.TabIndex = 15;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Touch Support";
			this.button26.Location = new Point(156, 19);
			this.button26.Name = "button26";
			this.button26.Size = new System.Drawing.Size(91, 23);
			this.button26.TabIndex = 7;
			this.button26.Text = "Enable Mouse";
			this.button26.UseVisualStyleBackColor = true;
			this.button26.Click += new EventHandler(this.button26_Click);
			this.textBox7.Location = new Point(69, 19);
			this.textBox7.Name = "textBox7";
			this.textBox7.ReadOnly = true;
			this.textBox7.Size = new System.Drawing.Size(58, 20);
			this.textBox7.TabIndex = 2;
			this.label18.AutoSize = true;
			this.label18.Location = new Point(6, 22);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(57, 13);
			this.label18.TabIndex = 1;
			this.label18.Text = "Position X:";
			this.label19.AutoSize = true;
			this.label19.Location = new Point(6, 59);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(57, 13);
			this.label19.TabIndex = 3;
			this.label19.Text = "Position Y:";
			this.textBox8.Location = new Point(69, 56);
			this.textBox8.Name = "textBox8";
			this.textBox8.ReadOnly = true;
			this.textBox8.Size = new System.Drawing.Size(58, 20);
			this.textBox8.TabIndex = 4;
			this.textBox3.Location = new Point(156, 59);
			this.textBox3.Name = "textBox3";
			this.textBox3.ReadOnly = true;
			this.textBox3.ScrollBars = ScrollBars.Vertical;
			this.textBox3.Size = new System.Drawing.Size(91, 20);
			this.textBox3.TabIndex = 6;
			this.button18.BackgroundImage = Resources.Play_icon__1_;
			this.button18.BackgroundImageLayout = ImageLayout.Stretch;
			this.button18.Enabled = false;
			this.button18.Location = new Point(20, 286);
			this.button18.Name = "button18";
			this.button18.Size = new System.Drawing.Size(54, 50);
			this.button18.TabIndex = 13;
			this.button18.UseVisualStyleBackColor = true;
			this.button18.Click += new EventHandler(this.button18_Click);
			this.groupBox5.Controls.Add(this.button28);
			this.groupBox5.Controls.Add(this.label11);
			this.groupBox5.Controls.Add(this.button13);
			this.groupBox5.Controls.Add(this.button12);
			this.groupBox5.Controls.Add(this.button10);
			this.groupBox5.Controls.Add(this.wipecombo);
			this.groupBox5.Location = new Point(6, 184);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(335, 74);
			this.groupBox5.TabIndex = 17;
			this.groupBox5.TabStop = false;
			this.button28.Location = new Point(234, 44);
			this.button28.Name = "button28";
			this.button28.Size = new System.Drawing.Size(75, 23);
			this.button28.TabIndex = 14;
			this.button28.Text = "Update List";
			this.button28.UseVisualStyleBackColor = true;
			this.button28.Click += new EventHandler(this.button28_Click);
			this.label11.AutoSize = true;
			this.label11.Location = new Point(36, 47);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(58, 13);
			this.label11.TabIndex = 13;
			this.label11.Text = "App name:";
			this.button13.Enabled = false;
			this.button13.Location = new Point(249, 15);
			this.button13.Name = "button13";
			this.button13.Size = new System.Drawing.Size(75, 23);
			this.button13.TabIndex = 12;
			this.button13.Text = "Backup";
			this.button13.UseVisualStyleBackColor = true;
			this.button13.Click += new EventHandler(this.button13_Click);
			this.button12.Enabled = false;
			this.button12.Location = new Point(33, 15);
			this.button12.Name = "button12";
			this.button12.Size = new System.Drawing.Size(75, 23);
			this.button12.TabIndex = 11;
			this.button12.Text = "Open App";
			this.button12.UseVisualStyleBackColor = true;
			this.button12.Click += new EventHandler(this.button12_Click);
			this.button10.Enabled = false;
			this.button10.Location = new Point(134, 15);
			this.button10.Name = "button10";
			this.button10.Size = new System.Drawing.Size(75, 23);
			this.button10.TabIndex = 7;
			this.button10.Text = "Wipe";
			this.button10.UseVisualStyleBackColor = true;
			this.button10.Click += new EventHandler(this.button10_Click_1);
			this.wipecombo.DropDownStyle = ComboBoxStyle.DropDownList;
			this.wipecombo.Enabled = false;
			this.wipecombo.FormattingEnabled = true;
			this.wipecombo.Location = new Point(107, 44);
			this.wipecombo.Name = "wipecombo";
			this.wipecombo.Size = new System.Drawing.Size(100, 21);
			this.wipecombo.Sorted = true;
			this.wipecombo.TabIndex = 8;
			this.wipecombo.SelectedIndexChanged += new EventHandler(this.wipecombo_SelectedIndexChanged);
			this.pictureBox1.Image = Resources._1234;
			this.pictureBox1.InitialImage = null;
			this.pictureBox1.Location = new Point(387, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(198, 335);
			this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Click += new EventHandler(this.pictureBox1_Click);
			this.pictureBox1.MouseDown += new MouseEventHandler(this.pictureBox1_MouseDown);
			this.pictureBox1.MouseMove += new MouseEventHandler(this.pictureBox1_MouseMove);
			this.tabPage7.Controls.Add(this.label35);
			this.tabPage7.Controls.Add(this.label34);
			this.tabPage7.Controls.Add(this.checkBox7);
			this.tabPage7.Controls.Add(this.comboBox3);
			this.tabPage7.Controls.Add(this.checkBox6);
			this.tabPage7.Controls.Add(this.button31);
			this.tabPage7.Controls.Add(this.label26);
			this.tabPage7.Controls.Add(this.textBox4);
			this.tabPage7.Controls.Add(this.button29);
			this.tabPage7.Controls.Add(this.button27);
			this.tabPage7.Controls.Add(this.label9);
			this.tabPage7.Controls.Add(this.rsswaitnum);
			this.tabPage7.Controls.Add(this.label8);
			this.tabPage7.Controls.Add(this.bkreset);
			this.tabPage7.Controls.Add(this.button19);
			this.tabPage7.Controls.Add(this.button17);
			this.tabPage7.Controls.Add(this.button16);
			this.tabPage7.Controls.Add(this.button15);
			this.tabPage7.Controls.Add(this.listView4);
			this.tabPage7.Location = new Point(4, 22);
			this.tabPage7.Name = "tabPage7";
			this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage7.Size = new System.Drawing.Size(620, 398);
			this.tabPage7.TabIndex = 3;
			this.tabPage7.Text = "RRS";
			this.tabPage7.UseVisualStyleBackColor = true;
			this.tabPage7.Click += new EventHandler(this.tabPage7_Click);
			this.label35.AutoSize = true;
			this.label35.ForeColor = Color.ForestGreen;
			this.label35.Location = new Point(128, 317);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(72, 13);
			this.label35.TabIndex = 19;
			this.label35.Text = "Seleted RRS:";
			this.label34.AutoSize = true;
			this.label34.ForeColor = SystemColors.Highlight;
			this.label34.Location = new Point(20, 317);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(66, 13);
			this.label34.TabIndex = 18;
			this.label34.Text = "Total RRS:0";
			this.checkBox7.AutoSize = true;
			this.checkBox7.Enabled = false;
			this.checkBox7.Location = new Point(360, 311);
			this.checkBox7.Name = "checkBox7";
			this.checkBox7.Size = new System.Drawing.Size(96, 17);
			this.checkBox7.TabIndex = 17;
			this.checkBox7.Text = "Random Script";
			this.checkBox7.UseVisualStyleBackColor = true;
			this.checkBox7.CheckedChanged += new EventHandler(this.checkBox7_CheckedChanged);
			this.comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBox3.Enabled = false;
			this.comboBox3.FormattingEnabled = true;
			this.comboBox3.Location = new Point(489, 309);
			this.comboBox3.Name = "comboBox3";
			this.comboBox3.Size = new System.Drawing.Size(110, 21);
			this.comboBox3.TabIndex = 16;
			this.comboBox3.SelectedIndexChanged += new EventHandler(this.comboBox3_SelectedIndexChanged);
			this.checkBox6.AutoSize = true;
			this.checkBox6.Enabled = false;
			this.checkBox6.Location = new Point(279, 311);
			this.checkBox6.Name = "checkBox6";
			this.checkBox6.Size = new System.Drawing.Size(75, 17);
			this.checkBox6.TabIndex = 15;
			this.checkBox6.Text = "Use Script";
			this.checkBox6.UseVisualStyleBackColor = true;
			this.checkBox6.CheckedChanged += new EventHandler(this.checkBox6_CheckedChanged);
			this.button31.Location = new Point(518, 281);
			this.button31.Name = "button31";
			this.button31.Size = new System.Drawing.Size(101, 23);
			this.button31.TabIndex = 14;
			this.button31.Text = "Save Comment";
			this.button31.UseVisualStyleBackColor = true;
			this.button31.Click += new EventHandler(this.button31_Click);
			this.label26.AutoSize = true;
			this.label26.Location = new Point(16, 286);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(54, 13);
			this.label26.TabIndex = 13;
			this.label26.Text = "Comment:";
			this.textBox4.Location = new Point(82, 283);
			this.textBox4.Multiline = true;
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new System.Drawing.Size(405, 20);
			this.textBox4.TabIndex = 12;
			this.button29.Enabled = false;
			this.button29.Location = new Point(245, 245);
			this.button29.Name = "button29";
			this.button29.Size = new System.Drawing.Size(89, 29);
			this.button29.TabIndex = 11;
			this.button29.Text = "Save";
			this.button29.UseVisualStyleBackColor = true;
			this.button29.Click += new EventHandler(this.button29_Click);
			this.button27.Enabled = false;
			this.button27.Location = new Point(131, 247);
			this.button27.Name = "button27";
			this.button27.Size = new System.Drawing.Size(91, 27);
			this.button27.TabIndex = 10;
			this.button27.Text = "Restore";
			this.button27.UseVisualStyleBackColor = true;
			this.button27.Click += new EventHandler(this.button27_Click);
			this.label9.AutoSize = true;
			this.label9.Location = new Point(175, 355);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(44, 13);
			this.label9.TabIndex = 9;
			this.label9.Text = "Second";
			this.rsswaitnum.Location = new Point(100, 353);
			this.rsswaitnum.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
			this.rsswaitnum.Name = "rsswaitnum";
			this.rsswaitnum.Size = new System.Drawing.Size(64, 20);
			this.rsswaitnum.TabIndex = 8;
			this.rsswaitnum.Value = new decimal(new int[] { 20, 0, 0, 0 });
			this.rsswaitnum.ValueChanged += new EventHandler(this.rsswaitnum_ValueChanged);
			this.label8.AutoSize = true;
			this.label8.Location = new Point(25, 355);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(72, 13);
			this.label8.TabIndex = 7;
			this.label8.Text = "Waiting Time:";
			this.bkreset.Enabled = false;
			this.bkreset.Location = new Point(410, 354);
			this.bkreset.Name = "bkreset";
			this.bkreset.Size = new System.Drawing.Size(75, 23);
			this.bkreset.TabIndex = 6;
			this.bkreset.Text = "Reset";
			this.bkreset.UseVisualStyleBackColor = true;
			this.bkreset.Click += new EventHandler(this.bkreset_Click);
			this.button19.Enabled = false;
			this.button19.Location = new Point(259, 341);
			this.button19.Name = "button19";
			this.button19.Size = new System.Drawing.Size(103, 38);
			this.button19.TabIndex = 5;
			this.button19.Text = "START";
			this.button19.UseVisualStyleBackColor = true;
			this.button19.Click += new EventHandler(this.button19_Click);
			this.button17.Enabled = false;
			this.button17.Location = new Point(491, 247);
			this.button17.Name = "button17";
			this.button17.Size = new System.Drawing.Size(91, 25);
			this.button17.TabIndex = 3;
			this.button17.Text = "Remove All";
			this.button17.UseVisualStyleBackColor = true;
			this.button17.Click += new EventHandler(this.button17_Click);
			this.button16.Enabled = false;
			this.button16.Location = new Point(365, 245);
			this.button16.Name = "button16";
			this.button16.Size = new System.Drawing.Size(91, 28);
			this.button16.TabIndex = 2;
			this.button16.Text = "Remove";
			this.button16.UseVisualStyleBackColor = true;
			this.button16.Click += new EventHandler(this.button16_Click);
			this.button15.Enabled = false;
			this.button15.Location = new Point(10, 246);
			this.button15.Name = "button15";
			this.button15.Size = new System.Drawing.Size(91, 28);
			this.button15.TabIndex = 1;
			this.button15.Text = "Get Backup list";
			this.button15.UseVisualStyleBackColor = true;
			this.button15.Click += new EventHandler(this.button15_Click);
			this.listView4.CheckBoxes = true;
			this.listView4.Columns.AddRange(new ColumnHeader[] { this.columnHeader10, this.columnHeader7, this.columnHeader11, this.columnHeader12, this.columnHeader8, this.columnHeader9, this.columnHeader15, this.columnHeader16 });
			this.listView4.FullRowSelect = true;
			this.listView4.GridLines = true;
			this.listView4.Location = new Point(6, 6);
			this.listView4.Name = "listView4";
			this.listView4.Size = new System.Drawing.Size(610, 233);
			this.listView4.TabIndex = 0;
			this.listView4.UseCompatibleStateImageBehavior = false;
			this.listView4.View = View.Details;
			this.listView4.ColumnClick += new ColumnClickEventHandler(this.listView4_ColumnClick);
			this.listView4.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(this.listView4_DrawColumnHeader);
			this.listView4.DrawItem += new DrawListViewItemEventHandler(this.listView4_DrawItem);
			this.listView4.DrawSubItem += new DrawListViewSubItemEventHandler(this.listView4_DrawSubItem);
			this.listView4.ItemCheck += new ItemCheckEventHandler(this.listView4_ItemCheck);
			this.listView4.ItemChecked += new ItemCheckedEventHandler(this.listView4_ItemChecked);
			this.listView4.SelectedIndexChanged += new EventHandler(this.listView4_SelectedIndexChanged);
			this.listView4.KeyDown += new KeyEventHandler(this.listView4_KeyDown);
			this.columnHeader10.Text = "";
			this.columnHeader10.Width = 23;
			this.columnHeader7.Text = "Ngày tạo";
			this.columnHeader7.Width = 82;
			this.columnHeader11.Text = "Ngày điều chỉnh";
			this.columnHeader11.Width = 95;
			this.columnHeader12.Text = "Số lần chạy";
			this.columnHeader12.Width = 51;
			this.columnHeader8.Text = "D.sách ứng dụng";
			this.columnHeader8.Width = 101;
			this.columnHeader9.Text = "Comment";
			this.columnHeader9.Width = 133;
			this.columnHeader15.Text = "Country";
			this.columnHeader15.Width = 90;
			this.columnHeader16.Text = "File Name";
			this.tabPage2.Controls.Add(this.tabControl2);
			this.tabPage2.Location = new Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(620, 398);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Proxy";
			this.tabPage2.UseVisualStyleBackColor = true;
			this.tabControl2.Controls.Add(this.tabPage3);

			this.tabControl2.Controls.Add(this.tabPage4);
			this.tabControl2.Controls.Add(this.tabPageQuan4);


			this.tabControl2.Location = new Point(-4, 0);
			this.tabControl2.Name = "tabControl2";
			this.tabControl2.SelectedIndex = 0;
			this.tabControl2.Size = new System.Drawing.Size(630, 480);
			this.tabControl2.TabIndex = 0;
			this.tabPage3.Controls.Add(this.checkBox17);
			this.tabPage3.Controls.Add(this.ss_dead);
			this.tabPage3.Controls.Add(this.ssh_used);
			this.tabPage3.Controls.Add(this.ssh_live);
			this.tabPage3.Controls.Add(this.ssh_uncheck);
			this.tabPage3.Controls.Add(this.numericUpDown2);
			this.tabPage3.Controls.Add(this.label14);
			this.tabPage3.Controls.Add(this.labeltotalssh);
			this.tabPage3.Controls.Add(this.button25);
			this.tabPage3.Controls.Add(this.button24);
			this.tabPage3.Controls.Add(this.button22);
			this.tabPage3.Controls.Add(this.button8);
			this.tabPage3.Controls.Add(this.button14);
			this.tabPage3.Controls.Add(this.button9);
			this.tabPage3.Controls.Add(this.importfromfile);
			this.tabPage3.Controls.Add(this.listView2);
			this.tabPage3.Location = new Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(622, 454);
			this.tabPage3.TabIndex = 0;
			this.tabPage3.Text = "SSH";
			this.tabPage3.UseVisualStyleBackColor = true;
			this.tabPage3.Click += new EventHandler(this.tabPage3_Click);
			this.checkBox17.AutoSize = true;
			this.checkBox17.Location = new Point(448, 276);
			this.checkBox17.Name = "checkBox17";
			this.checkBox17.Size = new System.Drawing.Size(152, 17);
			this.checkBox17.TabIndex = 29;
			this.checkBox17.Text = "Refresh SSH nếu hết SSH";
			this.checkBox17.UseVisualStyleBackColor = true;
			this.checkBox17.CheckedChanged += new EventHandler(this.checkBox17_CheckedChanged);
			this.ss_dead.AutoSize = true;
			this.ss_dead.ForeColor = Color.Red;
			this.ss_dead.Location = new Point(517, 359);
			this.ss_dead.Name = "ss_dead";
			this.ss_dead.Size = new System.Drawing.Size(36, 13);
			this.ss_dead.TabIndex = 21;
			this.ss_dead.Text = "Dead:";
			this.ssh_used.AutoSize = true;
			this.ssh_used.ForeColor = SystemColors.Highlight;
			this.ssh_used.Location = new Point(406, 359);
			this.ssh_used.Name = "ssh_used";
			this.ssh_used.Size = new System.Drawing.Size(35, 13);
			this.ssh_used.TabIndex = 20;
			this.ssh_used.Text = "Used:";
			this.ssh_live.AutoSize = true;
			this.ssh_live.ForeColor = Color.FromArgb(0, 192, 0);
			this.ssh_live.Location = new Point(303, 359);
			this.ssh_live.Name = "ssh_live";
			this.ssh_live.Size = new System.Drawing.Size(30, 13);
			this.ssh_live.TabIndex = 19;
			this.ssh_live.Text = "Live:";
			this.ssh_uncheck.AutoSize = true;
			this.ssh_uncheck.ForeColor = Color.Gray;
			this.ssh_uncheck.Location = new Point(178, 359);
			this.ssh_uncheck.Name = "ssh_uncheck";
			this.ssh_uncheck.Size = new System.Drawing.Size(54, 13);
			this.ssh_uncheck.TabIndex = 18;
			this.ssh_uncheck.Text = "Uncheck:";
			this.numericUpDown2.Location = new Point(208, 278);
			this.numericUpDown2.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			this.numericUpDown2.Name = "numericUpDown2";
			this.numericUpDown2.Size = new System.Drawing.Size(56, 20);
			this.numericUpDown2.TabIndex = 17;
			this.numericUpDown2.Value = new decimal(new int[] { 10, 0, 0, 0 });
			this.numericUpDown2.ValueChanged += new EventHandler(this.numericUpDown2_ValueChanged);
			this.label14.AutoSize = true;
			this.label14.Location = new Point(106, 280);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(96, 13);
			this.label14.TabIndex = 16;
			this.label14.Text = "Number of Thread:";
			this.labeltotalssh.AutoSize = true;
			this.labeltotalssh.Location = new Point(47, 359);
			this.labeltotalssh.Name = "labeltotalssh";
			this.labeltotalssh.Size = new System.Drawing.Size(59, 13);
			this.labeltotalssh.TabIndex = 15;
			this.labeltotalssh.Text = "Total SSH:";
			this.button25.Location = new Point(391, 309);
			this.button25.Name = "button25";
			this.button25.Size = new System.Drawing.Size(109, 23);
			this.button25.TabIndex = 14;
			this.button25.Text = "Export To File";
			this.button25.UseVisualStyleBackColor = true;
			this.button25.Click += new EventHandler(this.button25_Click);
			this.button24.Location = new Point(476, 167);
			this.button24.Name = "button24";
			this.button24.Size = new System.Drawing.Size(75, 25);
			this.button24.TabIndex = 13;
			this.button24.Text = "Refresh";
			this.button24.UseVisualStyleBackColor = true;
			this.button24.Click += new EventHandler(this.button24_Click);
			this.button22.Location = new Point(476, 108);
			this.button22.Name = "button22";
			this.button22.Size = new System.Drawing.Size(75, 23);
			this.button22.TabIndex = 11;
			this.button22.Text = "Delete All";
			this.button22.UseVisualStyleBackColor = true;
			this.button22.Click += new EventHandler(this.button22_Click);
			this.button8.Location = new Point(476, 56);
			this.button8.Name = "button8";
			this.button8.Size = new System.Drawing.Size(75, 23);
			this.button8.TabIndex = 10;
			this.button8.Text = "Delete";
			this.button8.UseVisualStyleBackColor = true;
			this.button8.Click += new EventHandler(this.button8_Click);
			this.button14.Location = new Point(217, 309);
			this.button14.Name = "button14";
			this.button14.Size = new System.Drawing.Size(115, 23);
			this.button14.TabIndex = 5;
			this.button14.Text = "Import from Clipboard";
			this.button14.UseVisualStyleBackColor = true;
			this.button14.Click += new EventHandler(this.button14_Click);
			this.button9.Enabled = false;
			this.button9.Location = new Point(13, 275);
			this.button9.Name = "button9";
			this.button9.Size = new System.Drawing.Size(75, 23);
			this.button9.TabIndex = 2;
			this.button9.Text = "Check Live";
			this.button9.UseVisualStyleBackColor = true;
			this.button9.Click += new EventHandler(this.button9_Click);
			this.importfromfile.Location = new Point(50, 309);
			this.importfromfile.Name = "importfromfile";
			this.importfromfile.Size = new System.Drawing.Size(99, 23);
			this.importfromfile.TabIndex = 1;
			this.importfromfile.Text = "Import from File";
			this.importfromfile.UseVisualStyleBackColor = true;
			this.importfromfile.Click += new EventHandler(this.importfromfile_Click);
			this.listView2.Columns.AddRange(new ColumnHeader[] { this.columnHeader1, this.columnHeader2, this.columnHeader3, this.columnHeader4 });
			this.listView2.ForeColor = SystemColors.WindowText;
			this.listView2.GridLines = true;
			this.listView2.Location = new Point(11, 6);
			this.listView2.Name = "listView2";
			this.listView2.Size = new System.Drawing.Size(428, 256);
			this.listView2.TabIndex = 0;
			this.listView2.UseCompatibleStateImageBehavior = false;
			this.listView2.View = View.Details;
			this.listView2.KeyDown += new KeyEventHandler(this.listView2_KeyDown);
			this.listView2.KeyPress += new KeyPressEventHandler(this.listView2_KeyPress);
			this.columnHeader1.Text = "IP";
			this.columnHeader1.Width = 121;
			this.columnHeader2.Text = "username";
			this.columnHeader2.Width = 80;
			this.columnHeader3.Text = "password";
			this.columnHeader3.Width = 97;
			this.columnHeader4.Text = "Country";
			this.columnHeader4.Width = 134;


			this.tabPage4.Controls.Add(this.sameVip);
			this.tabPage4.Controls.Add(this.button57);
			this.tabPage4.Controls.Add(this.groupBox2);
			this.tabPage4.Controls.Add(this.vipdelete);
			this.tabPage4.Controls.Add(this.listView3);
			this.tabPage4.Location = new Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage4.Size = new System.Drawing.Size(622, 454);
			this.tabPage4.TabIndex = 1;
			this.tabPage4.Text = "VIP72";
			this.tabPage4.UseVisualStyleBackColor = true;
			this.tabPage4.Click += new EventHandler(this.tabPage4_Click);
            

            this.sameVip.AutoSize = true;
			this.sameVip.Checked = true;
			this.sameVip.CheckState = CheckState.Checked;
			this.sameVip.Location = new Point(39, 353);
			this.sameVip.Name = "sameVip";
			this.sameVip.Size = new System.Drawing.Size(176, 17);
			this.sameVip.TabIndex = 13;
			this.sameVip.Text = "Sử dụng chung Vip72 với nhau.";
			this.sameVip.UseVisualStyleBackColor = true;
			this.sameVip.CheckedChanged += new EventHandler(this.sameVip_CheckedChanged);

		    


            this.button57.Location = new Point(180, 304);
			this.button57.Name = "button57";
			this.button57.Size = new System.Drawing.Size(101, 23);
			this.button57.TabIndex = 12;
			this.button57.Text = "Check Account";
			this.button57.UseVisualStyleBackColor = true;
			this.groupBox2.Controls.Add(this.vippassword);
			this.groupBox2.Controls.Add(this.vipid);
			this.groupBox2.Controls.Add(this.label10);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.vipadd);
			this.groupBox2.Location = new Point(343, 40);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(229, 252);
			this.groupBox2.TabIndex = 11;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Add account";
			this.vippassword.Location = new Point(100, 164);
			this.vippassword.Name = "vippassword";
			this.vippassword.Size = new System.Drawing.Size(100, 20);
			this.vippassword.TabIndex = 5;
			this.vipid.Location = new Point(100, 36);
			this.vipid.Multiline = true;
			this.vipid.Name = "vipid";
			this.vipid.Size = new System.Drawing.Size(100, 106);
			this.vipid.TabIndex = 4;
			this.label10.AutoSize = true;
			this.label10.Location = new Point(26, 167);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(53, 13);
			this.label10.TabIndex = 3;
			this.label10.Text = "Password";
			this.label7.AutoSize = true;
			this.label7.Location = new Point(42, 82);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(18, 13);
			this.label7.TabIndex = 2;
			this.label7.Text = "ID";
			this.vipadd.Location = new Point(86, 214);
			this.vipadd.Name = "vipadd";
			this.vipadd.Size = new System.Drawing.Size(91, 23);
			this.vipadd.TabIndex = 1;
			this.vipadd.Text = "Add Account";
			this.vipadd.UseVisualStyleBackColor = true;
			this.vipadd.Click += new EventHandler(this.button10_Click);
			this.vipdelete.Location = new Point(39, 304);
			this.vipdelete.Name = "vipdelete";
			this.vipdelete.Size = new System.Drawing.Size(89, 23);
			this.vipdelete.TabIndex = 2;
			this.vipdelete.Text = "Delete Account";
			this.vipdelete.UseVisualStyleBackColor = true;
			this.vipdelete.Click += new EventHandler(this.vipdelete_Click);
			this.listView3.Columns.AddRange(new ColumnHeader[] { this.columnHeader5, this.columnHeader6 });
			this.listView3.Location = new Point(34, 20);
			this.listView3.Name = "listView3";
			this.listView3.Size = new System.Drawing.Size(237, 257);
			this.listView3.TabIndex = 0;
			this.listView3.UseCompatibleStateImageBehavior = false;
			this.listView3.View = View.Details;
			this.listView3.KeyDown += new KeyEventHandler(this.listView3_KeyDown);
			this.columnHeader5.Text = "Username";
			this.columnHeader5.Width = 112;
			this.columnHeader6.Text = "Password";
			this.columnHeader6.Width = 107;


            this.tabPageQuan4.Controls.Add(this.groupBoxQuan2);
            this.tabPageQuan4.Controls.Add(this.lumidelete);
            
            this.tabPageQuan4.Controls.Add(this.listViewQuan3);
            this.tabPageQuan4.Location = new Point(4, 22);
            this.tabPageQuan4.Name = "tabPageQuan4";
            this.tabPageQuan4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageQuan4.Size = new System.Drawing.Size(622, 454);
            this.tabPageQuan4.TabIndex = 2;
            this.tabPageQuan4.Text = "Luminatio";
            this.tabPageQuan4.UseVisualStyleBackColor = true;
            this.tabPageQuan4.Click += new EventHandler(this.tabPageQuan4_Click);

            this.groupBoxQuan2.Controls.Add(this.lumipassword);
            this.groupBoxQuan2.Controls.Add(this.lumiid);
            this.groupBoxQuan2.Controls.Add(this.lumizone);
            this.groupBoxQuan2.Controls.Add(this.labelQuan10);
            this.groupBoxQuan2.Controls.Add(this.labelQuan7);
            this.groupBoxQuan2.Controls.Add(this.labelQuanZone);
            this.groupBoxQuan2.Controls.Add(this.lumiadd);
            this.groupBoxQuan2.Location = new Point(343, 40);
            this.groupBoxQuan2.Name = "groupBoxQuan2";
            this.groupBoxQuan2.Size = new System.Drawing.Size(229, 252);
            this.groupBoxQuan2.TabIndex = 11;
            this.groupBoxQuan2.TabStop = false;
            this.groupBoxQuan2.Text = "Add account";

            this.lumipassword.Location = new Point(100, 164);
            this.lumipassword.Name = "lumipassword";
            this.lumipassword.Size = new System.Drawing.Size(100, 20);
            this.lumipassword.TabIndex = 5;

            this.lumiid.Location = new Point(100, 36);           
            this.lumiid.Name = "lumiid";
            this.lumiid.Size = new System.Drawing.Size(100, 36);
            this.lumiid.TabIndex = 4;

            this.lumizone.Location = new Point(100, 100);            
            this.lumizone.Name = "lumizone";
            this.lumizone.Size = new System.Drawing.Size(100, 20);
            this.lumizone.TabIndex = 6;

            this.labelQuan10.AutoSize = true;
            this.labelQuan10.Location = new Point(26, 167);
            this.labelQuan10.Name = "labelQuan10";
            this.labelQuan10.Size = new System.Drawing.Size(53, 13);
            this.labelQuan10.TabIndex = 3;
            this.labelQuan10.Text = "Password";


            this.labelQuan7.AutoSize = true;
            this.labelQuan7.Location = new Point(42, 46);
            this.labelQuan7.Name = "labelQuan7";
            this.labelQuan7.Size = new System.Drawing.Size(18, 13);
            this.labelQuan7.TabIndex = 2;
            this.labelQuan7.Text = "ID";

            this.labelQuanZone.AutoSize = true;
            this.labelQuanZone.Location = new Point(42, 110);
            this.labelQuanZone.Name = "labelQuanZone";
            this.labelQuanZone.Size = new System.Drawing.Size(18, 13);
            this.labelQuanZone.TabIndex = 7;
            this.labelQuanZone.Text = "Zone";

            this.lumiadd.Location = new Point(86, 214);
            this.lumiadd.Name = "lumiadd";
            this.lumiadd.Size = new System.Drawing.Size(91, 23);
            this.lumiadd.TabIndex = 1;
            this.lumiadd.Text = "Add Account";
            this.lumiadd.UseVisualStyleBackColor = true;
            this.lumiadd.Click += new EventHandler(this.buttonLumiadd_Click);


            this.lumidelete.Location = new Point(39, 304);
            this.lumidelete.Name = "lumidelete";
            this.lumidelete.Size = new System.Drawing.Size(89, 23);
            this.lumidelete.TabIndex = 2;
            this.lumidelete.Text = "Delete Account";
            this.lumidelete.UseVisualStyleBackColor = true;
            this.lumidelete.Click += new EventHandler(this.lumidelete_Click);

            this.listViewQuan3.Columns.AddRange(new ColumnHeader[] { this.columnHeaderQuan5, this.columnHeaderQuan6, this.columnHeaderQuanZone });
            this.listViewQuan3.Location = new Point(34, 20);
            this.listViewQuan3.Name = "listViewQuan3";
            this.listViewQuan3.Size = new System.Drawing.Size(237, 257);
            this.listViewQuan3.TabIndex = 0;
            this.listViewQuan3.UseCompatibleStateImageBehavior = false;
            this.listViewQuan3.View = View.Details;
            this.listViewQuan3.KeyDown += new KeyEventHandler(this.listViewQuan3_KeyDown);

            this.columnHeaderQuan5.Text = "Username";
            this.columnHeaderQuan5.Width = 60;

            this.columnHeaderQuan6.Text = "Password";
            this.columnHeaderQuan6.Width = 80;

            this.columnHeaderQuanZone.Text = "Zone";
            this.columnHeaderQuanZone.Width = 60;


            this.tabPage1.Controls.Add(this.textBox11);
			this.tabPage1.Controls.Add(this.comment);
			this.tabPage1.Controls.Add(this.label44);
			this.tabPage1.Controls.Add(this.checkBox18);
			this.tabPage1.Controls.Add(this.checkBox12);
			this.tabPage1.Controls.Add(this.numericUpDown6);
			this.tabPage1.Controls.Add(this.checkBox10);
			this.tabPage1.Controls.Add(this.backuprate);
			this.tabPage1.Controls.Add(this.backupoftime);
			this.tabPage1.Controls.Add(this.runoftime);
			this.tabPage1.Controls.Add(this.label29);
			this.tabPage1.Controls.Add(this.label28);
			this.tabPage1.Controls.Add(this.numericUpDown3);
			this.tabPage1.Controls.Add(this.label27);
			this.tabPage1.Controls.Add(this.itunesY);
			this.tabPage1.Controls.Add(this.label22);
			this.tabPage1.Controls.Add(this.itunesX);
			this.tabPage1.Controls.Add(this.label17);
			this.tabPage1.Controls.Add(this.safariY);
			this.tabPage1.Controls.Add(this.label16);
			this.tabPage1.Controls.Add(this.safariX);
			this.tabPage1.Controls.Add(this.label15);
			this.tabPage1.Controls.Add(this.button21);
			this.tabPage1.Controls.Add(this.Reset);
			this.tabPage1.Controls.Add(this.checkBox3);
			this.tabPage1.Controls.Add(this.checkBox2);
			this.tabPage1.Controls.Add(this.button7);
			this.tabPage1.Controls.Add(this.checkBox1);
			this.tabPage1.Controls.Add(this.button6);
			this.tabPage1.Controls.Add(this.button5);
			this.tabPage1.Controls.Add(this.button4);
			this.tabPage1.Controls.Add(this.button3);
			this.tabPage1.Controls.Add(this.listView1);
			this.tabPage1.Location = new Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(620, 398);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "AutoLead";
			this.tabPage1.UseVisualStyleBackColor = true;
			this.textBox11.Location = new Point(488, 369);
			this.textBox11.Name = "textBox11";
			this.textBox11.Size = new System.Drawing.Size(100, 20);
			this.textBox11.TabIndex = 37;
			this.textBox11.Text = "00000";
			this.textBox11.Visible = false;
			this.textBox11.TextChanged += new EventHandler(this.textBox11_TextChanged);
			this.comment.Location = new Point(66, 207);
			this.comment.Name = "comment";
			this.comment.Size = new System.Drawing.Size(260, 20);
			this.comment.TabIndex = 25;
			this.comment.TextChanged += new EventHandler(this.comment_TextChanged);
			this.label44.AutoSize = true;
			this.label44.Location = new Point(435, 375);
			this.label44.Name = "label44";
			this.label44.Size = new System.Drawing.Size(41, 13);
			this.label44.TabIndex = 36;
			this.label44.Text = "OfferID";
			this.label44.Visible = false;
			this.checkBox18.AutoSize = true;
			this.checkBox18.Location = new Point(463, 338);
			this.checkBox18.Name = "checkBox18";
			this.checkBox18.Size = new System.Drawing.Size(101, 17);
			this.checkBox18.TabIndex = 35;
			this.checkBox18.Text = "Check Trùng IP";
			this.checkBox18.UseVisualStyleBackColor = true;
			this.checkBox18.Visible = false;
			this.checkBox18.CheckedChanged += new EventHandler(this.checkBox18_CheckedChanged);
			this.checkBox12.AutoSize = true;
			this.checkBox12.Location = new Point(332, 210);
			this.checkBox12.Name = "checkBox12";
			this.checkBox12.Size = new System.Drawing.Size(127, 17);
			this.checkBox12.TabIndex = 34;
			this.checkBox12.Text = "Lưu IP trên comment.";
			this.checkBox12.UseVisualStyleBackColor = true;
			this.checkBox12.CheckedChanged += new EventHandler(this.checkBox12_CheckedChanged);
			this.numericUpDown6.Location = new Point(343, 239);
			this.numericUpDown6.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
			this.numericUpDown6.Name = "numericUpDown6";
			this.numericUpDown6.Size = new System.Drawing.Size(75, 20);
			this.numericUpDown6.TabIndex = 33;
			this.numericUpDown6.Value = new decimal(new int[] { 200, 0, 0, 0 });
			this.numericUpDown6.ValueChanged += new EventHandler(this.numericUpDown6_ValueChanged);
			this.checkBox10.AutoSize = true;
			this.checkBox10.Location = new Point(207, 240);
			this.checkBox10.Name = "checkBox10";
			this.checkBox10.Size = new System.Drawing.Size(130, 17);
			this.checkBox10.TabIndex = 32;
			this.checkBox10.Text = "Hạn chế số lượt chạy:";
			this.checkBox10.UseVisualStyleBackColor = true;
			this.checkBox10.CheckedChanged += new EventHandler(this.checkBox10_CheckedChanged);
			this.backuprate.AutoSize = true;
			this.backuprate.ForeColor = Color.FromArgb(192, 64, 0);
			this.backuprate.Location = new Point(26, 375);
			this.backuprate.Name = "backuprate";
			this.backuprate.Size = new System.Drawing.Size(87, 13);
			this.backuprate.TabIndex = 31;
			this.backuprate.Text = "Backup Rate:0%";
			this.backupoftime.AutoSize = true;
			this.backupoftime.ForeColor = SystemColors.MenuHighlight;
			this.backupoftime.Location = new Point(26, 356);
			this.backupoftime.Name = "backupoftime";
			this.backupoftime.Size = new System.Drawing.Size(56, 13);
			this.backupoftime.TabIndex = 30;
			this.backupoftime.Text = "Backup: 0";
			this.runoftime.AutoSize = true;
			this.runoftime.ForeColor = Color.DarkCyan;
			this.runoftime.Location = new Point(26, 338);
			this.runoftime.Name = "runoftime";
			this.runoftime.Size = new System.Drawing.Size(39, 13);
			this.runoftime.TabIndex = 29;
			this.runoftime.Text = "Run: 0";
			this.label29.AutoSize = true;
			this.label29.Location = new Point(182, 243);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(15, 13);
			this.label29.TabIndex = 28;
			this.label29.Text = "%";
			this.label28.AutoSize = true;
			this.label28.Location = new Point(92, 240);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(33, 13);
			this.label28.TabIndex = 27;
			this.label28.Text = "Tỷ lệ:";
			this.numericUpDown3.Location = new Point(130, 239);
			this.numericUpDown3.Name = "numericUpDown3";
			this.numericUpDown3.Size = new System.Drawing.Size(46, 20);
			this.numericUpDown3.TabIndex = 26;
			this.numericUpDown3.Value = new decimal(new int[] { 50, 0, 0, 0 });
			this.numericUpDown3.ValueChanged += new EventHandler(this.numericUpDown3_ValueChanged);
			this.label27.AutoSize = true;
			this.label27.Location = new Point(6, 212);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(54, 13);
			this.label27.TabIndex = 24;
			this.label27.Text = "Comment:";
			this.itunesY.Location = new Point(502, 271);
			this.itunesY.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
			this.itunesY.Minimum = new decimal(new int[] { 1, 0, 0, -2147483648 });
			this.itunesY.Name = "itunesY";
			this.itunesY.Size = new System.Drawing.Size(60, 20);
			this.itunesY.TabIndex = 23;
			this.itunesY.Value = new decimal(new int[] { 1, 0, 0, -2147483648 });
			this.itunesY.ValueChanged += new EventHandler(this.itunesY_ValueChanged);
			this.label22.AutoSize = true;
			this.label22.Location = new Point(451, 273);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(50, 13);
			this.label22.TabIndex = 22;
			this.label22.Text = "ITunesY:";
			this.itunesX.Location = new Point(354, 271);
			this.itunesX.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
			this.itunesX.Minimum = new decimal(new int[] { 1, 0, 0, -2147483648 });
			this.itunesX.Name = "itunesX";
			this.itunesX.Size = new System.Drawing.Size(60, 20);
			this.itunesX.TabIndex = 21;
			this.itunesX.Value = new decimal(new int[] { 1, 0, 0, -2147483648 });
			this.itunesX.ValueChanged += new EventHandler(this.itunesX_ValueChanged);
			this.label17.AutoSize = true;
			this.label17.Location = new Point(296, 273);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(49, 13);
			this.label17.TabIndex = 20;
			this.label17.Text = "iTunesX:";
			this.safariY.Enabled = false;
			this.safariY.Location = new Point(211, 271);
			this.safariY.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
			this.safariY.Minimum = new decimal(new int[] { 1, 0, 0, -2147483648 });
			this.safariY.Name = "safariY";
			this.safariY.Size = new System.Drawing.Size(58, 20);
			this.safariY.TabIndex = 19;
			this.safariY.Value = new decimal(new int[] { 1, 0, 0, -2147483648 });
			this.safariY.ValueChanged += new EventHandler(this.safariY_ValueChanged);
			this.label16.AutoSize = true;
			this.label16.Location = new Point(161, 273);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(44, 13);
			this.label16.TabIndex = 18;
			this.label16.Text = "SafariY:";
			this.safariX.Enabled = false;
			this.safariX.Location = new Point(76, 271);
			this.safariX.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
			this.safariX.Minimum = new decimal(new int[] { 1, 0, 0, -2147483648 });
			this.safariX.Name = "safariX";
			this.safariX.Size = new System.Drawing.Size(60, 20);
			this.safariX.TabIndex = 17;
			this.safariX.Value = new decimal(new int[] { 1, 0, 0, -2147483648 });
			this.safariX.ValueChanged += new EventHandler(this.safariX_ValueChanged);
			this.label15.AutoSize = true;
			this.label15.Location = new Point(26, 273);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(44, 13);
			this.label15.TabIndex = 16;
			this.label15.Text = "SafariX:";
			this.button21.Location = new Point(519, 237);
			this.button21.Name = "button21";
			this.button21.Size = new System.Drawing.Size(97, 25);
			this.button21.TabIndex = 15;
			this.button21.Text = "Clear Install File";
			this.button21.UseVisualStyleBackColor = true;
			this.button21.Click += new EventHandler(this.button21_Click);
			this.Reset.Enabled = false;
			this.Reset.Location = new Point(343, 369);
			this.Reset.Name = "Reset";
			this.Reset.Size = new System.Drawing.Size(75, 23);
			this.Reset.TabIndex = 14;
			this.Reset.Text = "Reset";
			this.Reset.UseVisualStyleBackColor = true;
			this.Reset.Click += new EventHandler(this.Reset_Click);
			this.checkBox3.AutoSize = true;
			this.checkBox3.Checked = true;
			this.checkBox3.CheckState = CheckState.Checked;
			this.checkBox3.Location = new Point(115, 352);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new System.Drawing.Size(82, 17);
			this.checkBox3.TabIndex = 13;
			this.checkBox3.Text = "Full Backup";
			this.checkBox3.UseVisualStyleBackColor = true;
			this.checkBox3.Visible = false;
			this.checkBox3.CheckedChanged += new EventHandler(this.checkBox3_CheckedChanged);
			this.checkBox2.AutoSize = true;
			this.checkBox2.Location = new Point(438, 240);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(70, 17);
			this.checkBox2.TabIndex = 12;
			this.checkBox2.Text = "Full Wipe";
			this.checkBox2.UseVisualStyleBackColor = true;
			this.checkBox2.CheckedChanged += new EventHandler(this.checkBox2_CheckedChanged);
			this.button7.BackColor = Color.Transparent;
			this.button7.Enabled = false;
			this.button7.Font = new System.Drawing.Font("Segoe UI Black", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.button7.Location = new Point(245, 347);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(87, 45);
			this.button7.TabIndex = 9;
			this.button7.Text = "START";
			this.button7.UseVisualStyleBackColor = false;
			this.button7.Click += new EventHandler(this.button7_Click);
			this.checkBox1.AutoSize = true;
			this.checkBox1.Checked = true;
			this.checkBox1.CheckState = CheckState.Checked;
			this.checkBox1.Location = new Point(22, 240);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(63, 17);
			this.checkBox1.TabIndex = 8;
			this.checkBox1.Text = "Backup";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
			this.button6.Location = new Point(476, 300);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(75, 23);
			this.button6.TabIndex = 7;
			this.button6.Text = "Remove All";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new EventHandler(this.button6_Click);
			this.button5.Location = new Point(329, 300);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(75, 23);
			this.button5.TabIndex = 6;
			this.button5.Text = "Remove";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new EventHandler(this.button5_Click);
			this.button4.Location = new Point(164, 300);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(75, 23);
			this.button4.TabIndex = 5;
			this.button4.Text = "Edit";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new EventHandler(this.button4_Click);
			this.button3.Location = new Point(23, 300);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 23);
			this.button3.TabIndex = 4;
			this.button3.Text = "Add";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new EventHandler(this.button3_Click);
			this.listView1.BackColor = SystemColors.Window;
			this.listView1.BorderStyle = BorderStyle.FixedSingle;
			this.listView1.CheckBoxes = true;
			this.listView1.Columns.AddRange(new ColumnHeader[] { this.offername, this.offerurl, this.appname, this.timedelay, this.usescript });
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.Location = new Point(3, 3);
			this.listView1.MultiSelect = false;
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(610, 198);
			this.listView1.TabIndex = 3;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = View.Details;
			this.listView1.ItemCheck += new ItemCheckEventHandler(this.listView1_ItemCheck);
			this.listView1.SelectedIndexChanged += new EventHandler(this.listView1_SelectedIndexChanged);
			this.offername.Text = "Tên Offer";
			this.offername.Width = 110;
			this.offerurl.Text = "Link Offer";
			this.offerurl.Width = 190;
			this.appname.Text = "Tên ứng dụng";
			this.appname.Width = 112;
			this.timedelay.Text = "T.gian mở App";
			this.timedelay.Width = 90;
			this.usescript.Text = "Thao tác khác";
			this.usescript.Width = 88;
			this.Contact.Controls.Add(this.tabPage1);
			this.Contact.Controls.Add(this.tabPage2);
			this.Contact.Controls.Add(this.tabPage7);
			this.Contact.Controls.Add(this.tabPage8);
			this.Contact.Controls.Add(this.tabPage6);
			this.Contact.Controls.Add(this.Script);
			this.Contact.Controls.Add(this.tabPage9);
			this.Contact.Controls.Add(this.tabPage10);
			this.Contact.Controls.Add(this.tabPage5);
			this.Contact.Location = new Point(12, 12);
			this.Contact.Name = "Contact";
			this.Contact.SelectedIndex = 0;
			this.Contact.Size = new System.Drawing.Size(628, 424);
			this.Contact.TabIndex = 0;
			this.Contact.SelectedIndexChanged += new EventHandler(this.Contact_SelectedIndexChanged);
			this.l_autover.AutoSize = true;
			this.l_autover.Location = new Point(312, 575);
			this.l_autover.Name = "l_autover";
			this.l_autover.Size = new System.Drawing.Size(94, 13);
			this.l_autover.TabIndex = 20;
			this.l_autover.Text = "AutoLead Version:";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(654, 638);
			base.Controls.Add(this.l_autover);
			base.Controls.Add(this.autoreconnect);
			base.Controls.Add(this.label12);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.labelSerial);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.DeviceIpControl);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.Contact);
			base.Icon = (System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "Form1";
			this.Text = "Auto Lead for iOS";
			base.FormClosed += new FormClosedEventHandler(this.Form1_FormClosed);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((ISupportInitialize)this.numericUpDown1).EndInit();
			this.contextMenuStrip1.ResumeLayout(false);
			this.contextMenuStrip2.ResumeLayout(false);
			this.contextMenuStrip3.ResumeLayout(false);
			this.tabPage5.ResumeLayout(false);
			this.tabPage5.PerformLayout();
			((ISupportInitialize)this.pictureBox5).EndInit();
			((ISupportInitialize)this.pictureBox4).EndInit();
			((ISupportInitialize)this.pictureBox3).EndInit();
			((ISupportInitialize)this.pictureBox2).EndInit();
			this.tabPage10.ResumeLayout(false);
			this.tabPage10.PerformLayout();
			this.tabPage9.ResumeLayout(false);
			this.groupBox8.ResumeLayout(false);
			this.groupBox8.PerformLayout();
			this.Script.ResumeLayout(false);
			this.Script.PerformLayout();
			this.tabPage6.ResumeLayout(false);
			this.tabPage6.PerformLayout();
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			((ISupportInitialize)this.longtitude).EndInit();
			((ISupportInitialize)this.latitude).EndInit();
			this.groupBox7.ResumeLayout(false);
			this.groupBox7.PerformLayout();
			((ISupportInitialize)this.numericUpDown10).EndInit();
			((ISupportInitialize)this.numericUpDown5).EndInit();
			((ISupportInitialize)this.numericUpDown4).EndInit();
			this.groupBox9.ResumeLayout(false);
			this.groupBox9.PerformLayout();
			this.tabPage8.ResumeLayout(false);
			this.tabPage8.PerformLayout();
			((ISupportInitialize)this.trackBar1).EndInit();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			((ISupportInitialize)this.pictureBox1).EndInit();
			this.tabPage7.ResumeLayout(false);
			this.tabPage7.PerformLayout();
			((ISupportInitialize)this.rsswaitnum).EndInit();
			this.tabPage2.ResumeLayout(false);
			this.tabControl2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			((ISupportInitialize)this.numericUpDown2).EndInit();

			this.tabPage4.ResumeLayout(false);
			this.tabPage4.PerformLayout();

		    this.tabPageQuan4.ResumeLayout(false);
		    this.tabPageQuan4.PerformLayout();


            this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			((ISupportInitialize)this.numericUpDown6).EndInit();
			((ISupportInitialize)this.numericUpDown3).EndInit();
			((ISupportInitialize)this.itunesY).EndInit();
			((ISupportInitialize)this.itunesX).EndInit();
			((ISupportInitialize)this.safariY).EndInit();
			((ISupportInitialize)this.safariX).EndInit();
			this.Contact.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

	    public static string IPAddresses()
		{
			string empty = string.Empty;
			IPAddress[] hostAddresses = Dns.GetHostAddresses(Dns.GetHostName());
			int num = 0;
			while (num < (int)hostAddresses.Length)
			{
				IPAddress pAddress = hostAddresses[num];
				if (pAddress.AddressFamily != AddressFamily.InterNetwork)
				{
					num++;
				}
				else
				{
					empty = pAddress.ToString();
					break;
				}
			}
			return empty;
		}

	    private void loadcarrier()
		{
			string[] strArrays = Resources.carrierlist.Split(new string[] { "\r\n" }, StringSplitOptions.None);
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				string str = strArrays[i];
				string[] strArrays1 = str.Split(new string[] { "||" }, StringSplitOptions.None);
				Carrier carrier = new Carrier()
				{
					CarrierName = strArrays1[0],
					country = strArrays1[1],
					ISOCountryCode = strArrays1[2],
					mobileCarrierCode = strArrays1[3],
					mobileCountryCode = strArrays1[4]
				};
				this.carrierList.Add(carrier);
				this._listcountry.Add(strArrays1[1]);
			}
			this._listcountry = this._listcountry.Distinct<string>().ToList<string>();
			this.carrierBox.Items.AddRange(this._listcountry.ToArray());
			this.carrierBox.Text = "United States of America";
		}

		private void loadcountrycode()
		{
			string str = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "countrycode.dat");
			string str1 = File.ReadAllText(str);
			string[] strArrays = str1.Split(new string[] { "\r\n" }, StringSplitOptions.None);
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				string str2 = strArrays[i];
				string[] strArrays1 = str2.Split(new string[] { "|" }, StringSplitOptions.None);
				if (strArrays1.Count<string>() == 2)
				{
					countrycode _countrycode = new countrycode()
					{
						country = strArrays1[0],
						code = Convert.ToByte(strArrays1[1])
					};
					this.listcountrycode.Add(_countrycode);
				}
			}
		}

		private void loadcountrycodeiOS()
		{
			string str = Resources.countrycodeiOS;
			string[] strArrays = str.Split(new string[] { "\r\n" }, StringSplitOptions.None);
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				string str1 = strArrays[i];
				try
				{
					string[] strArrays1 = str1.Split(new string[] { "|" }, StringSplitOptions.None);
					RegionInfo regionInfo = new RegionInfo(strArrays1[0]);
					countrycodeiOS countrycodeiO = new countrycodeiOS()
					{
						countrycode = strArrays1[0],
						countryname = regionInfo.EnglishName
					};
					if (strArrays1.Count<string>() != 1)
					{
						countrycodeiO.languageCode = strArrays1[1];
					}
					else
					{
						countrycodeiO.languageCode = "en";
					}
					this.listcountrycodeiOS.Add(countrycodeiO);
					this.comboBox2.Items.Add(regionInfo.EnglishName);
				}
				catch (Exception exception)
				{
				}
			}
			this.comboBox2.Text = "United States";
		}

		private void loadgeo()
		{
		}

		private void loadlangcode()
		{
			string str = Resources.languagecode;
			string[] strArrays = str.Split(new string[] { "\r\n" }, StringSplitOptions.None);
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				string str1 = strArrays[i];
				string[] strArrays1 = str1.Split(new string[] { "|" }, StringSplitOptions.None);
				languagecode _languagecode = new languagecode()
				{
					langcode = strArrays1[1],
					langname = strArrays1[0]
				};
				this.listlanguagecode.Add(_languagecode);
				this.comboBox1.Items.Add(_languagecode.langname);
			}
			this.comboBox1.Text = "English";
		}

		private void loadofferlist()
		{
			if (this.DeviceInfo.SerialNumber != null)
			{
				this.offerListItem.Clear();
				this.listView1.Items.Clear();
				if (File.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber, "\\offerlist.dat")))
				{
					string[] strArrays = File.ReadAllText(string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber, "\\offerlist.dat")).Split(new string[] { "\r\n" }, StringSplitOptions.None);
					if (Convert.ToInt16(strArrays[0]) != strArrays.Count<string>() - 1)
					{
						return;
					}
					for (int i = 0; i < strArrays.Count<string>() - 1; i++)
					{
						string str = strArrays[i + 1];
						string[] strArrays1 = str.Split(new string[] { "||" }, StringSplitOptions.None);
						offerItem _offerItem = new offerItem();
						try
						{
							_offerItem.offerEnable = Convert.ToBoolean(strArrays1[0]);
							_offerItem.offerName = strArrays1[1];
							_offerItem.offerURL = strArrays1[2];
							_offerItem.appName = strArrays1[3];
							_offerItem.appID = strArrays1[4];
							_offerItem.timeSleepBefore = Convert.ToInt32(strArrays1[5]);
							_offerItem.timeSleepBeforeRandom = Convert.ToBoolean(strArrays1[6]);
							_offerItem.range1 = Convert.ToInt32(strArrays1[7]);
							_offerItem.range2 = Convert.ToInt32(strArrays1[8]);
							_offerItem.timeSleep = Convert.ToInt32(strArrays1[9]);
							_offerItem.useScript = Convert.ToBoolean(strArrays1[10]);
							_offerItem.script = strArrays1[11].Replace("__", "\r\n");
							_offerItem.referer = "";
							_offerItem.referer = strArrays1[12].Replace("__", "\r\n");
						}
						catch (Exception exception)
						{
						}
						this.offerListItem.Add(_offerItem);
						string[] str1 = new string[] { _offerItem.offerName, _offerItem.offerURL, _offerItem.appName, null, null };
						str1[3] = _offerItem.timeSleep.ToString();
						str1[4] = _offerItem.useScript.ToString();
						ListViewItem listViewItem = new ListViewItem(str1);
						this.listView1.Items.Add(listViewItem);
					}
					foreach (offerItem _offerItem1 in this.offerListItem)
					{
						if (_offerItem1.offerEnable)
						{
							this.listView1.Items[this.offerListItem.IndexOf(_offerItem1)].Checked = true;
						}
					}
				}
			}
		}

		private void loadothresetting()
		{
			if (this.DeviceInfo.SerialNumber != null)
			{
				if (File.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber, "\\setting.dat")))
				{
					string[] strArrays = File.ReadAllText(string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber, "\\setting.dat")).Split(new string[] { "||" }, StringSplitOptions.None);
					try
					{
						this.checkBox1.Checked = Convert.ToBoolean(strArrays[0]);
						this.checkBox3.Checked = Convert.ToBoolean(strArrays[1]);
						this.checkBox2.Checked = Convert.ToBoolean(strArrays[2]);
						this.safariX.Value = Convert.ToDecimal(strArrays[3]);
						this.safariY.Value = Convert.ToDecimal(strArrays[4]);
						this.itunesX.Value = Convert.ToDecimal(strArrays[5]);
						this.itunesY.Value = Convert.ToDecimal(strArrays[6]);
						this.numericUpDown2.Value = Convert.ToDecimal(strArrays[7]);
						this.rsswaitnum.Value = Convert.ToDecimal(strArrays[8]);
						this.textBox1.Text = strArrays[9];
						this.textBox2.Text = strArrays[10].Replace("__", "\r\n");
						this.proxytool.Text = strArrays[11];
						this.proxytool.Refresh();
						this.comboBox5.Text = strArrays[12];
						this.comment.Text = strArrays[14];
						this.autoreconnect.Checked = Convert.ToBoolean(strArrays[13]);
						this.numericUpDown3.Value = Convert.ToDecimal(strArrays[15]);
						this.checkBox13.Checked = Convert.ToBoolean(strArrays[16]);
						this.fakedevice.Checked = Convert.ToBoolean(strArrays[17]);
						this.checkBox11.Checked = Convert.ToBoolean(strArrays[18]);
						this.fileofname.Text = strArrays[19];
						this.fakeversion.Checked = Convert.ToBoolean(strArrays[20]);
						this.fakemodel.Checked = Convert.ToBoolean(strArrays[21]);
						this.ipad.Checked = Convert.ToBoolean(strArrays[22]);
						this.iphone.Checked = Convert.ToBoolean(strArrays[23]);
						this.ipod.Checked = Convert.ToBoolean(strArrays[24]);
						this.fakelang.Checked = Convert.ToBoolean(strArrays[25]);
						this.comboBox1.Text = strArrays[26];
						this.fakeregion.Checked = Convert.ToBoolean(strArrays[27]);
						this.comboBox2.Text = strArrays[28];
						this.numericUpDown4.Value = Convert.ToDecimal(strArrays[29]);
						this.checkBox4.Checked = Convert.ToBoolean(strArrays[30]);
						this.numericUpDown5.Value = Convert.ToDecimal(strArrays[31]);
						this.checkBox5.Checked = Convert.ToBoolean(strArrays[32]);
						this.ltimezone.Text = strArrays[33];
						this.checkBox6.Checked = Convert.ToBoolean(strArrays[34]);
						this.checkBox7.Checked = Convert.ToBoolean(strArrays[35]);
						this.checkBox6_CheckedChanged(null, null);
						this.comboBox3.Text = strArrays[36];
						this.checkBox9.Checked = Convert.ToBoolean(strArrays[38]);
						this.carrierBox.Text = strArrays[39];
						this.checkBox10.Checked = Convert.ToBoolean(strArrays[40]);
						this.numericUpDown6.Value = Convert.ToDecimal(strArrays[41]);
						this.checkBox12.Checked = Convert.ToBoolean(strArrays[42]);
						this.checkBox14.Checked = Convert.ToBoolean(strArrays[43]);
						this.checkBox15.Checked = Convert.ToBoolean(strArrays[44]);
						this.checkBox17.Checked = Convert.ToBoolean(strArrays[45]);
						this.checkBox18.Checked = Convert.ToBoolean(strArrays[46]);
						this.textBox11.Text = strArrays[47];
						this.numericUpDown10.Value = Convert.ToInt32(strArrays[48]);
						this.checkBox19.Checked = Convert.ToBoolean(strArrays[49]);
						this.latitude.Value = Convert.ToDecimal(strArrays[50]);
						this.longtitude.Value = Convert.ToDecimal(strArrays[51]);
						this.checkBox20.Checked = Convert.ToBoolean(strArrays[52]);
						this.sameVip.Checked = Convert.ToBoolean(strArrays[53]);
					}
					catch (Exception exception)
					{
					}
				}
			}
		}

		private void loadscripts()
		{
			if (this.DeviceInfo.SerialNumber != null)
			{
				this.listscript.Clear();
				if (File.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber, "\\scripts.dat")))
				{
					string[] strArrays = File.ReadAllText(string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber, "\\scripts.dat")).Split(new string[] { "||" }, StringSplitOptions.None);
					try
					{
						for (int i = 1; i < Convert.ToInt32(strArrays[0]); i++)
						{
							this.listView7.Items.Add(new ListViewItem(string.Concat("Slot ", i.ToString())));
						}
						string[] strArrays1 = strArrays[1].Split(new string[] { "@@" }, StringSplitOptions.None);
						for (int j = 0; j < (int)strArrays1.Length; j++)
						{
							string str = strArrays1[j];
							string[] strArrays2 = str.Split(new string[] { "##" }, StringSplitOptions.None);
							AutoLead.Script script = new AutoLead.Script()
							{
								scriptname = strArrays2[0],
								script = strArrays2[1],
								slot = Convert.ToInt32(strArrays2[2])
							};
							this.listscript.Add(script);
						}
					}
					catch (Exception exception)
					{
					}
				}
			}
		}

		private void loadsetting()
		{
			this.loadssh();
			this.loadvip72();
			this.loadlumi();
			this.loadscripts();
			this.loadofferlist();
			this.loadothresetting();
		}

	    private void loadtimezone()
		{
			string str = Resources.timezone;
			this.listtimezone = str.Split(new string[] { "\",\"" }, StringSplitOptions.None).ToList<string>();
		}

		private void loadvip72()
		{
			if (this.DeviceInfo.SerialNumber != null)
			{
				this.listvipacc.Clear();
				this.listView3.Items.Clear();
				if (File.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber, "\\vip72.dat")))
				{
					Decryptor decryptor = new Decryptor();
					string[] strArrays = decryptor.Decrypt(File.ReadAllText(string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber, "\\vip72.dat")), this.privatekey).Split(new string[] { "\r\n" }, StringSplitOptions.None);
					for (int i = 0; i < (int)strArrays.Length; i++)
					{
						string str = strArrays[i];
						string[] strArrays1 = str.Split(new string[] { "||" }, StringSplitOptions.None);
						if (strArrays1.Count<string>() == 2)
						{
							vipaccount _vipaccount = new vipaccount()
							{
								username = strArrays1[0],
								password = strArrays1[1],
								limited = false
							};
							this.listvipacc.Add(_vipaccount);
							ListViewItem listViewItem = new ListViewItem(new string[] { _vipaccount.username, _vipaccount.password });
							this.listView3.Items.Add(listViewItem);
						}
					}
				}
			}
		}

        private void loadlumi()
        {
            string dir = "default";

            if (this.DeviceInfo.SerialNumber != null) {
                dir = this.DeviceInfo.SerialNumber;
            }

            this.listlumiacc.Clear();
            this.listViewQuan3.Items.Clear();
            if (File.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, dir, "\\lumi.dat")))
            {
                Decryptor decryptor = new Decryptor();
                string[] strArrays = decryptor.Decrypt(File.ReadAllText(string.Concat(AppDomain.CurrentDomain.BaseDirectory, dir, "\\lumi.dat")), this.privatekey).Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < (int)strArrays.Length; i++)
                {
                    string str = strArrays[i];
                    string[] strArrays1 = str.Split(new string[] { "||" }, StringSplitOptions.None);
                    if (strArrays1.Count<string>() > 2)
                    {
                        luminatio_account _lumiaccount = new luminatio_account()
                        {
                            username = strArrays1[0],
                            password = strArrays1[1],
                            zone = strArrays1[2]
                        };
                        this.listlumiacc.Add(_lumiaccount);
                        ListViewItem listViewItem = new ListViewItem(new string[] { _lumiaccount.username, _lumiaccount.password, _lumiaccount.zone });
                        this.listViewQuan3.Items.Add(listViewItem);
                    }
                }
            }           
        }

        private void passlistview(bool add, object sender)
		{
			if (!(this.listView1.SelectedItems.Count <= 0 | add))
			{
				int item = this.listView1.Items.IndexOf(this.listView1.SelectedItems[0]);
				offerItem _offerItem = (offerItem)sender;
				this.offerListItem.ElementAt<offerItem>(item).appID = this.AppList[Convert.ToInt32(_offerItem.appID)].appID;
				this.offerListItem.ElementAt<offerItem>(item).appName = this.AppList[Convert.ToInt32(_offerItem.appID)].appName;
				this.offerListItem.ElementAt<offerItem>(item).offerEnable = _offerItem.offerEnable;
				this.offerListItem.ElementAt<offerItem>(item).offerName = _offerItem.offerName;
				this.offerListItem.ElementAt<offerItem>(item).offerURL = _offerItem.offerURL;
				this.offerListItem.ElementAt<offerItem>(item).script = _offerItem.script;
				this.offerListItem.ElementAt<offerItem>(item).timeSleep = _offerItem.timeSleep;
				this.offerListItem.ElementAt<offerItem>(item).useScript = _offerItem.useScript;
				this.offerListItem.ElementAt<offerItem>(item).timeSleepBefore = _offerItem.timeSleepBefore;
				this.offerListItem.ElementAt<offerItem>(item).timeSleepBeforeRandom = _offerItem.timeSleepBeforeRandom;
				this.offerListItem.ElementAt<offerItem>(item).range1 = _offerItem.range1;
				this.offerListItem.ElementAt<offerItem>(item).range2 = _offerItem.range2;
				this.offerListItem.ElementAt<offerItem>(item).referer = _offerItem.referer;
				ListViewItem str = this.listView1.SelectedItems[0];
				str.Text = _offerItem.offerName;
				str.Checked = _offerItem.offerEnable;
				str.SubItems[1].Text = _offerItem.offerURL;
				str.SubItems[2].Text = _offerItem.appName;
				str.SubItems[3].Text = _offerItem.timeSleep.ToString();
				str.SubItems[4].Text = _offerItem.useScript.ToString();
			}
			else
			{
				offerItem item1 = (offerItem)sender;
				item1.appID = this.AppList[Convert.ToInt32(item1.appID)].appID;
				this.offerListItem.Add((offerItem)sender);
				offerItem _offerItem1 = (offerItem)sender;
				_offerItem1.appName = (
					from z in this.AppList
					where z.appID == item1.appID
					select z).First<appDetail>().appName;
				string[] strArrays = new string[] { _offerItem1.offerName, _offerItem1.offerURL, _offerItem1.appName, null, null };
				strArrays[3] = _offerItem1.timeSleep.ToString();
				strArrays[4] = _offerItem1.useScript.ToString();
				ListViewItem listViewItem = new ListViewItem(strArrays)
				{
					Checked = _offerItem1.offerEnable
				};
				this.listView1.Items.Add(listViewItem);
			}
			this.saveofferlist();
		}


	    private void ReceiveCallBack(IAsyncResult AR)
		{
			try
			{
				int num = this._clientSocket.EndReceive(AR);
				Array.Resize<byte>(ref this._buffer, num);
				string str = Encoding.Unicode.GetString(this._buffer);
				if (str != "")
				{
					this.AnalyData(str);
					Array.Resize<byte>(ref this._buffer, this._clientSocket.ReceiveBufferSize);
					this._clientSocket.BeginReceive(this._buffer, 0, (int)this._buffer.Length, SocketFlags.None, new AsyncCallback(this.ReceiveCallBack), null);
				}
				else
				{
					this.listView1.Invoke(new MethodInvoker(() => this.disconnect()));
				}
			}
			catch (SocketException socketException)
			{
				this.listView1.Invoke(new MethodInvoker(() => this.disconnect()));
			}
			catch (Exception exception)
			{
			}
		}

		private void reconnect()
		{
			for (int i = 0; i < 20; i++)
			{
				this.label1.Invoke(new MethodInvoker(() => {
					this.label1.Text = string.Concat("Auto-reconnect in ", (20 - i - 1).ToString(), " seconds");
					this.label1.Refresh();
				}));
				Thread.Sleep(1000);
			}
			this.button2.Invoke(new MethodInvoker(() => {
				if (this.button2.Text == "Connect")
				{
					this.button2_Click(null, null);
				}
			}));
		}

		private void removeandreplace()
		{
			if (AppDomain.CurrentDomain.FriendlyName != "_AutoLead.exe")
			{
				for (Process[] i = Process.GetProcessesByName("_AutoLead"); i.Count<Process>() > 0; i = Process.GetProcessesByName("_AutoLead"))
				{
					Thread.Sleep(100);
					Process[] processArray = i;
					for (int j = 0; j < (int)processArray.Length; j++)
					{
						Process process = processArray[j];
						try
						{
							process.Kill();
						}
						catch (Exception exception)
						{
						}
					}
				}
				try
				{
					if (File.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "_AutoLead.exe")))
					{
						MessageBox.Show("Để bật chức năng check IP trước khi mở off và mở ứng dụng, vào tab Setting-> Tick vào ô \"Check IP trước khi mở Link Offer và trước khi mở Ứng dụng \"");
					}
					File.Delete(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "_AutoLead.exe"));
				}
				catch (Exception exception1)
				{
				}
			}
			else
			{
				Thread.Sleep(100);
				Process[] processesByName = Process.GetProcessesByName("AutoLead");
				while (processesByName.Count<Process>() > 0)
				{
					Process[] processArray1 = processesByName;
					for (int k = 0; k < (int)processArray1.Length; k++)
					{
						Process process1 = processArray1[k];
						try
						{
							process1.Kill();
						}
						catch (Exception exception2)
						{
						}
					}
					processesByName = Process.GetProcessesByName("AutoLead");
					Thread.Sleep(100);
				}
				File.Delete(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "AutoLead.exe"));
				File.Copy(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "_AutoLead.exe"), string.Concat(AppDomain.CurrentDomain.BaseDirectory, "AutoLead.exe"), true);
				ProcessStartInfo processStartInfo = new ProcessStartInfo()
				{
					FileName = "AutoLead.exe",
					WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory
				};
				Process.Start(processStartInfo);
				Application.Exit();
				Environment.Exit(0);
			}
		}

	    private void restorethread()
		{
			int lime = 0;
			backup _backup = new backup();
			this.listView4.Invoke(new MethodInvoker(() => {
				lime = this.listView4.Items.IndexOf(this.listView4.SelectedItems[0]);
				_backup = this.listbackup.FirstOrDefault<backup>((backup x) => x.filename == this.listView4.Items[lime].SubItems[7].Text);
				this.listView4.SelectedItems[0].BackColor = Color.Yellow;
			}));
			this.cmdResult.wipe = false;
			this.button2.Invoke(new MethodInvoker(() => this.maxwait = (int)this.numericUpDown10.Value));
			this.cmd.wipe(string.Join(";", _backup.appList.ToArray()));
			DateTime now = DateTime.Now;
			while (true)
			{
				if (!this.cmdResult.wipe)
				{
					Thread.Sleep(1000);
					if ((DateTime.Now - now).TotalSeconds <= (double)this.maxwait)
					{
						this.cmd.checkwipe();
					}
					else
					{
						this.button2.Invoke(new MethodInvoker(() => {
							if (this.button2.Text == "Disconnect")
							{
								this.button2_Click(null, null);
							}
						}));
						break;
					}
				}
				else
				{
					this.button2.Invoke(new MethodInvoker(() => {
						if (this.fakelang.Checked)
						{
							this.cmd.changelanguage(this.listlanguagecode.FirstOrDefault<languagecode>((languagecode x) => x.langname == this.comboBox1.Text).langcode);
						}
						if (!this.checkBox5.Checked)
						{
							this.cmd.changetimezone("orig");
						}
						else
						{
							this.cmd.changetimezone(this.ltimezone.Text);
						}
						if (this.fakeregion.Checked)
						{
							this.cmd.changeregion(this.listcountrycodeiOS.FirstOrDefault<countrycodeiOS>((countrycodeiOS x) => x.countryname == this.comboBox2.Text).countrycode);
						}
					}));
					this.cmdResult.restore = false;
					this.cmd.restore(_backup.filename);
					DateTime dateTime = DateTime.Now;
					this.button2.Invoke(new MethodInvoker(() => this.maxwait = (int)this.numericUpDown10.Value));
					while (!this.cmdResult.restore)
					{
						Thread.Sleep(500);
						if ((DateTime.Now - dateTime).TotalSeconds <= (double)this.maxwait)
						{
							this.cmd.checkrestore();
						}
						else
						{
							return;
						}
					}
					this.listView4.Invoke(new MethodInvoker(() => {
						this.listView4.Items[lime].BackColor = Color.Lime;
						this.label1.Text = "App restored";
						this.button27.Enabled = true;
					}));
					break;
				}
			}
		}

		private void rrsdisableall()
		{
			this.button15.Enabled = false;
			this.button16.Enabled = false;
			this.button17.Enabled = false;
			this.bkreset.Enabled = false;
			this.button27.Enabled = false;
			this.button29.Enabled = false;
		}

		private void rssenableall()
		{
			this.button29.Enabled = true;
			this.button27.Enabled = true;
			this.button15.Enabled = true;
			this.button16.Enabled = this.button19.Text == "START";
			this.button17.Enabled = this.button19.Text == "START";
			this.bkreset.Enabled = this.button19.Text == "RESUME";
		}

	    private void savecheckedssh()
		{
			string str = string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber, "\\checkssh.dat");
			string str1 = "";
			foreach (ListViewItem item in this.listView4.Items)
			{
				if ((item == null ? false : item.Checked))
				{
					str1 = string.Concat(str1, item.SubItems[7].Text);
					str1 = string.Concat(str1, "\r\n");
				}
			}
			if (!Directory.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber)))
			{
				Directory.CreateDirectory(string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber));
			}
			File.WriteAllText(str, str1);
		}

		private void savecommentthread()
		{
			this.cmdResult.savecomment = false;
			this.label1.Invoke(new MethodInvoker(() => {
				this.label1.Text = "Saving comment..";
				this.label1.Refresh();
			}));
			DateTime now = DateTime.Now;
			while (true)
			{
				if (!this.cmdResult.savecomment)
				{
					Thread.Sleep(100);
					if ((DateTime.Now - now).TotalSeconds > 20)
					{
						this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "Request timeout..."));
						break;
					}
				}
				else
				{
					this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "Comment saved..."));
					break;
				}
			}
		}

		private void savedata()
		{
		}

		private void saveofferlist()
		{
			if (this.DeviceInfo.SerialNumber != null)
			{
				string str = string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber, "\\offerlist.dat");
				string str1 = this.offerListItem.Count.ToString();
				foreach (offerItem _offerItem in this.offerListItem)
				{
					str1 = string.Concat(str1, "\r\n");
					bool flag = _offerItem.offerEnable;
					str1 = string.Concat(str1, flag.ToString());
					str1 = string.Concat(str1, "||");
					str1 = string.Concat(str1, _offerItem.offerName);
					str1 = string.Concat(str1, "||");
					str1 = string.Concat(str1, _offerItem.offerURL);
					str1 = string.Concat(str1, "||");
					str1 = string.Concat(str1, _offerItem.appName);
					str1 = string.Concat(str1, "||");
					str1 = string.Concat(str1, _offerItem.appID);
					str1 = string.Concat(str1, "||");
					int num = _offerItem.timeSleepBefore;
					str1 = string.Concat(str1, num.ToString());
					str1 = string.Concat(str1, "||");
					flag = _offerItem.timeSleepBeforeRandom;
					str1 = string.Concat(str1, flag.ToString());
					str1 = string.Concat(str1, "||");
					num = _offerItem.range1;
					str1 = string.Concat(str1, num.ToString());
					str1 = string.Concat(str1, "||");
					num = _offerItem.range2;
					str1 = string.Concat(str1, num.ToString());
					str1 = string.Concat(str1, "||");
					num = _offerItem.timeSleep;
					str1 = string.Concat(str1, num.ToString());
					str1 = string.Concat(str1, "||");
					flag = _offerItem.useScript;
					str1 = string.Concat(str1, flag.ToString());
					str1 = string.Concat(str1, "||");
					str1 = string.Concat(str1, _offerItem.script.Replace("\r\n", "__"));
					str1 = string.Concat(str1, "||");
					str1 = string.Concat(str1, _offerItem.referer.Replace("\r\n", "__"));
				}
				if (!Directory.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber)))
				{
					Directory.CreateDirectory(string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber));
				}
				File.WriteAllText(str, str1);
			}
		}

		private void saveothersetting()
		{
			if (this.DeviceInfo.SerialNumber != null)
			{
				string str = string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber, "\\setting.dat");
				if (!Directory.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber)))
				{
					Directory.CreateDirectory(string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber));
				}
				string[] strArrays = new string[8];
				bool @checked = this.checkBox1.Checked;
				strArrays[0] = @checked.ToString();
				strArrays[1] = "||";
				@checked = this.checkBox3.Checked;
				strArrays[2] = @checked.ToString();
				strArrays[3] = "||";
				@checked = this.checkBox2.Checked;
				strArrays[4] = @checked.ToString();
				strArrays[5] = "||";
				decimal value = this.safariX.Value;
				strArrays[6] = value.ToString();
				strArrays[7] = "||";
				string str1 = string.Concat(strArrays);
				string[] strArrays1 = new string[] { str1, null, null, null, null, null, null, null, null, null };
				value = this.safariY.Value;
				strArrays1[1] = value.ToString();
				strArrays1[2] = "||";
				value = this.itunesX.Value;
				strArrays1[3] = value.ToString();
				strArrays1[4] = "||";
				value = this.itunesY.Value;
				strArrays1[5] = value.ToString();
				strArrays1[6] = "||";
				value = this.numericUpDown2.Value;
				strArrays1[7] = value.ToString();
				strArrays1[8] = "||";
				value = this.rsswaitnum.Value;
				strArrays1[9] = value.ToString();
				str1 = string.Concat(strArrays1);
				str1 = string.Concat(new string[] { str1, "||", this.textBox1.Text, "||", this.textBox2.Text.Replace("\r\n", "__") });
				string[] text = new string[] { str1, "||", this.proxytool.Text, "||", this.comboBox5.Text, "||", null, null, null, null, null };
				@checked = this.autoreconnect.Checked;
				text[6] = @checked.ToString();
				text[7] = "||";
				text[8] = this.comment.Text;
				text[9] = "||";
				value = this.numericUpDown3.Value;
				text[10] = value.ToString();
				str1 = string.Concat(text);
				string[] text1 = new string[] { str1, "||", null, null, null, null, null, null, null, null, null };
				@checked = this.checkBox13.Checked;
				text1[2] = @checked.ToString();
				text1[3] = "||";
				@checked = this.fakedevice.Checked;
				text1[4] = @checked.ToString();
				text1[5] = "||";
				@checked = this.checkBox11.Checked;
				text1[6] = @checked.ToString();
				text1[7] = "||";
				text1[8] = this.fileofname.Text;
				text1[9] = "||";
				@checked = this.fakeversion.Checked;
				text1[10] = @checked.ToString();
				str1 = string.Concat(text1);
				string[] str2 = new string[] { str1, "||", null, null, null, null, null, null, null };
				@checked = this.fakemodel.Checked;
				str2[2] = @checked.ToString();
				str2[3] = "||";
				@checked = this.ipad.Checked;
				str2[4] = @checked.ToString();
				str2[5] = "||";
				@checked = this.iphone.Checked;
				str2[6] = @checked.ToString();
				str2[7] = "||";
				@checked = this.ipod.Checked;
				str2[8] = @checked.ToString();
				str1 = string.Concat(str2);
				string[] text2 = new string[15];
				text2[0] = str1;
				text2[1] = "||";
				@checked = this.fakelang.Checked;
				text2[2] = @checked.ToString();
				text2[3] = "||";
				text2[4] = this.comboBox1.Text;
				text2[5] = "||";
				@checked = this.fakeregion.Checked;
				text2[6] = @checked.ToString();
				text2[7] = "||";
				text2[8] = this.comboBox2.Text;
				text2[9] = "||";
				value = this.numericUpDown4.Value;
				text2[10] = value.ToString();
				text2[11] = "||";
				@checked = this.checkBox4.Checked;
				text2[12] = @checked.ToString();
				text2[13] = "||";
				value = this.numericUpDown5.Value;
				text2[14] = value.ToString();
				str1 = string.Concat(text2);
				string[] strArrays2 = new string[] { str1, "||", null, null, null };
				@checked = this.checkBox5.Checked;
				strArrays2[2] = @checked.ToString();
				strArrays2[3] = "||";
				strArrays2[4] = this.ltimezone.Text;
				str1 = string.Concat(strArrays2);
				string[] str3 = new string[] { str1, "||", null, null, null, null, null, null };
				@checked = this.checkBox6.Checked;
				str3[2] = @checked.ToString();
				str3[3] = "||";
				@checked = this.checkBox7.Checked;
				str3[4] = @checked.ToString();
				str3[5] = "||";
				str3[6] = this.comboBox3.Text;
				str3[7] = "||true";
				str1 = string.Concat(str3);
				string[] text3 = new string[] { str1, "||", null, null, null };
				@checked = this.checkBox9.Checked;
				text3[2] = @checked.ToString();
				text3[3] = "||";
				text3[4] = this.carrierBox.Text;
				str1 = string.Concat(text3);
				string[] strArrays3 = new string[] { str1, "||", null, null, null };
				@checked = this.checkBox10.Checked;
				strArrays3[2] = @checked.ToString();
				strArrays3[3] = "||";
				value = this.numericUpDown6.Value;
				strArrays3[4] = value.ToString();
				str1 = string.Concat(strArrays3);
				@checked = this.checkBox12.Checked;
				str1 = string.Concat(str1, "||", @checked.ToString());
				@checked = this.checkBox14.Checked;
				str1 = string.Concat(str1, "||", @checked.ToString());
				@checked = this.checkBox15.Checked;
				str1 = string.Concat(str1, "||", @checked.ToString());
				@checked = this.checkBox17.Checked;
				str1 = string.Concat(str1, "||", @checked.ToString());
				@checked = this.checkBox18.Checked;
				str1 = string.Concat(str1, "||", @checked.ToString());
				str1 = string.Concat(str1, "||", this.textBox11.Text);
				value = this.numericUpDown10.Value;
				str1 = string.Concat(str1, "||", value.ToString());
				@checked = this.checkBox19.Checked;
				str1 = string.Concat(str1, "||", @checked.ToString());
				string[] str4 = new string[] { str1, "||", null, null, null };
				value = this.latitude.Value;
				str4[2] = value.ToString();
				str4[3] = "||";
				value = this.longtitude.Value;
				str4[4] = value.ToString();
				str1 = string.Concat(str4);
				@checked = this.checkBox20.Checked;
				str1 = string.Concat(str1, "||", @checked.ToString());
				@checked = this.sameVip.Checked;
				str1 = string.Concat(str1, "||", @checked.ToString());
				File.WriteAllText(str, str1);
			}
		}

		private void saverrsthread(ListViewItem currentSeletectItem)
		{
			backup now = listbackup.FirstOrDefault<backup>((backup x) => x.filename == currentSeletectItem.SubItems[7].Text);
			this.listView4.Invoke(new MethodInvoker(() => currentSeletectItem.BackColor = Color.Yellow));
			string str = "";
			base.Invoke(new MethodInvoker(() => {
				this.label1.Text = "Saving RRS...";
				now.timemod = DateTime.Now;
				backup _backup = now;
				_backup.runtime = _backup.runtime + 1;
				str = now.filename.Replace(".zip", "");
				this.cmd.backupfull(string.Join(";", now.appList.ToArray()), now.filename.Replace(".zip", ""), string.Concat(this.textBox4.Text, "[]", now.country), now.timemod.ToString("MM/dd/yyyy HH:mm:ss"), now.runtime.ToString());
			}));
			this.cmdResult.backup = false;
			DateTime dateTime = DateTime.Now;
			this.button2.Invoke(new MethodInvoker(() => this.maxwait = (int)this.numericUpDown10.Value));
			while (true)
			{
				if (!this.cmdResult.backup)
				{
					Thread.Sleep(500);
					if ((DateTime.Now - dateTime).TotalSeconds <= (double)this.maxwait)
					{
						this.cmd.checkbackup(str);
					}
					else
					{
						this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "Request timeout..."));
						break;
					}
				}
				else
				{
					this.label1.Invoke(new MethodInvoker(() => {
						currentSeletectItem.SubItems[5].Text = this.textBox4.Text;
						currentSeletectItem.SubItems[2].Text = now.timemod.ToString("MM/dd/yyyy HH:mm:ss");
						currentSeletectItem.SubItems[3].Text = now.runtime.ToString();
						this.label1.Text = "Saved RRS";
						this.button29.Enabled = true;
						this.textBox4.Enabled = true;
						this.button29.Text = "Save";
						currentSeletectItem.BackColor = Color.Lime;
					}));
					break;
				}
			}
		}

		private void savescripts()
		{
			if (this.DeviceInfo.SerialNumber != null)
			{
				string str = string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber, "\\scripts.dat");
				if (!Directory.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber)))
				{
					Directory.CreateDirectory(string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber));
				}
				string str1 = string.Concat(this.listView7.Items.Count, "||");
				foreach (AutoLead.Script script in this.listscript)
				{
					string[] strArrays = new string[] { str1, script.scriptname, "##", script.script, "##", null, null };
					strArrays[5] = script.slot.ToString();
					strArrays[6] = "@@";
					str1 = string.Concat(strArrays);
				}
				File.WriteAllText(str, str1);
			}
		}

		private void savessh()
		{
			if (this.DeviceInfo.SerialNumber != null)
			{
				string str = string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber, "\\ssh.dat");
				if (!Directory.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber)))
				{
					Directory.CreateDirectory(string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber));
				}
				List<string> strs = new List<string>();
				foreach (ssh _ssh in this.listssh)
				{
					string[] p = new string[] { _ssh.IP, "||", _ssh.username, "||", _ssh.password, "||", _ssh.country, "||", _ssh.countrycode, "||", null, null, null };
					p[10] = _ssh.used.ToString();
					p[11] = "||";
					p[12] = _ssh.live;
					strs.Add(string.Concat(p));
				}
				File.WriteAllText(str, string.Join("\r\n", strs));
			}
		}

		private void savevip72()
		{
			if (this.DeviceInfo.SerialNumber != null)
			{
				string str = string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber, "\\vip72.dat");
				if (!Directory.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber)))
				{
					Directory.CreateDirectory(string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber));
				}
				string str1 = "";
				foreach (vipaccount _vipaccount in this.listvipacc)
				{
					str1 = string.Concat(new string[] { str1, _vipaccount.username, "||", _vipaccount.password, "\r\n" });
				}
				Encryptor encryptor = new Encryptor();
				File.WriteAllText(str, encryptor.Encrypt(str1, this.privatekey));
			}
		}

        private void savelumi()
        {

            string dir = "default";

            if (this.DeviceInfo.SerialNumber != null) {
                dir = this.DeviceInfo.SerialNumber;
            }

            string str = string.Concat(AppDomain.CurrentDomain.BaseDirectory, dir, "\\lumi.dat");
            if (!Directory.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, dir)))
            {
                Directory.CreateDirectory(string.Concat(AppDomain.CurrentDomain.BaseDirectory, dir));
            }
            string str1 = "";
            foreach (luminatio_account _lumiaccount in this.listlumiacc)
            {
                str1 = string.Concat(new string[] { str1, _lumiaccount.username, "||", _lumiaccount.password, "||", _lumiaccount.zone, "\r\n" });
            }
            Encryptor encryptor = new Encryptor();
            File.WriteAllText(str, encryptor.Encrypt(str1, this.privatekey));

            this.label1.Invoke(new MethodInvoker(() =>
                                this.label1.Text = "Save LUmi IP To" + string.Concat(AppDomain.CurrentDomain.BaseDirectory, dir)));
        }

        private void Send(byte[] buffer)
		{
			try
			{
				this._clientSocket.BeginSend(buffer, 0, (int)buffer.Length, SocketFlags.None, new AsyncCallback(this.SendCallBack), null);
			}
			catch (SocketException socketException)
			{
				this.listView1.Invoke(new MethodInvoker(() => this.disconnect()));
			}
			catch (Exception exception)
			{
			}
		}

		private void SendCallBack(IAsyncResult AR)
		{
			try
			{
				this._clientSocket.EndSend(AR);
			}
			catch (Exception exception)
			{
			}
		}

		private void setInfo(string text)
		{
			string[] strArrays2 = text.Split(new string[] { ";" }, StringSplitOptions.None);
			string[] strArrays3 = strArrays2;
			for (int i = 0; i < (int)strArrays3.Length; i++)
			{
				string str2 = strArrays3[i];
				string[] strArrays4 = str2.Split(new string[] { ":" }, StringSplitOptions.None);
				string str3 = strArrays4[0];
				if (str3 == "Model")
				{
					this.DeviceInfo.DeviceModel = strArrays4[1];
				}
				else if (str3 == "Name")
				{
					this.DeviceInfo.DeviceName = strArrays4[1];
				}
				else if (str3 == "Version")
				{
					this.iOSversion = Convert.ToInt16(strArrays4[1].Substring(0, 1));
					this.safariY.Enabled = true;
					this.safariX.Enabled = true;
					this.DeviceInfo.DeviceOSVersion = strArrays4[1];
				}
				else if (str3 == "AutoVersion")
				{
					this.l_autover.Text = string.Concat("AutoLead Version:", strArrays4[1]);
					if (this.clientver != strArrays4[1])
					{
						Label lAutover = this.l_autover;
						lAutover.Text = string.Concat(lAutover.Text, " (Vui lòng vào cydia update lên phiên bản ", this.clientver, ")");
					}
				}
				else if (str3 == "Serial")
				{
					this.Text = this.DeviceIpControl.Text.Split(new string[] { "." }, StringSplitOptions.None)[3];
					this.labelSerial.Text = string.Concat("Serial:", strArrays4[1]);
					this.deviceseri.Text = strArrays4[1];
					this.DeviceInfo.SerialNumber = strArrays4[1];
					Encryptor encryptor = new Encryptor();
					string str4 = string.Concat(this.serverurl, encryptor.Encrypt(strArrays4[1], this.privatekey));
					HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(str4);
					httpWebRequest.UserAgent = "autoleadios";
					try
					{
						Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
						(new StreamReader(responseStream)).ReadToEnd();
						Decryptor decryptor = new Decryptor();
						string str5 = "9999";
						if (str5 == "Unregistered")
						{
							this.label12.Text = "Date Expired:Unregistered";
						}
						else if (str5 != "Expired")
						{
							this.cmd.getAllProtectData();
							TimeSpan timeSpan = TimeSpan.Parse(str5);
							DateTime now = DateTime.Now + timeSpan;
							this.label12.Text = string.Concat("Date Expired:", now.ToShortDateString());
							this._enable();
							this.enableAll();
							this.rssenableall();
							this.loadsetting();
							if (this.autoreconnect.Checked)
							{
								if (this.runningstt == 1)
								{
									this.button7_Click(null, null);
								}
								if (this.runningstt == 2)
								{
									(new Thread(new ThreadStart(this.threadcontinuerrs))).Start();
								}
							}
							IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
							int num18 = 0;
							while (num18 < (int)addressList.Length)
							{
								IPAddress pAddress = addressList[num18];
								string[] strArrays5 = pAddress.ToString().Split(new string[] { "." }, StringSplitOptions.None);
								string[] strArrays6 = this.DeviceIpControl.Text.Split(new string[] { "." }, StringSplitOptions.None);
								if ((!(strArrays5[0] == strArrays6[0]) || !(strArrays5[1] == strArrays6[1]) ? true : strArrays5[2] != strArrays6[2]))
								{
									num18++;
								}
								else
								{
									this.ipAddressControl1.IPAddress = pAddress;
									break;
								}
							}
							if (!Directory.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber)))
							{
								Directory.CreateDirectory(string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber));
							}
							if (!Directory.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting")))
							{
								Directory.CreateDirectory(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting"));
							}
							if (!File.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting\\changes.dat")))
							{
								string str6 = string.Concat(new string[] { this.changes.ToString(), "|", this.c_listoff.ToString(), "|", this.c_othersetting.ToString(), "|", this.c_ssh.ToString(), "|", this.c_vip.ToString(), "|", this.c_startall.ToString(), "|", this.c_stopall.ToString(), "|", this.c_resetall.ToString() });
								File.WriteAllText(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting\\changes.dat"), str6);
							}
							else
							{
								string str7 = File.ReadAllText(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting\\changes.dat"));
								try
								{
									string[] strArrays7 = str7.Split(new string[] { "|" }, StringSplitOptions.None);
									this.changes = Convert.ToInt32(strArrays7[0]);
									this.c_listoff = Convert.ToInt32(strArrays7[1]);
									this.c_othersetting = Convert.ToInt32(strArrays7[2]);
									this.c_ssh = Convert.ToInt32(strArrays7[3]);
									this.c_vip = Convert.ToInt32(strArrays7[4]);
									this.c_startall = Convert.ToInt32(strArrays7[5]);
									this.c_stopall = Convert.ToInt32(strArrays7[6]);
									this.c_resetall = Convert.ToInt32(strArrays7[7]);
								}
								catch (Exception exception3)
								{
									MessageBox.Show(exception3.Message);
								}
							}
							if (!File.Exists(string.Concat(this.documentfolder, "changes.dat")))
							{
								string str8 = string.Concat(new string[] { this.changeslocal.ToString(), "|", this.c_listofflocal.ToString(), "|", this.c_othersettinglocal.ToString(), "|", this.c_sshlocal.ToString(), "|", this.c_viplocal.ToString(), "|", this.c_startalllocal.ToString(), "|", this.c_stopalllocal.ToString(), "|", this.c_resetalllocal.ToString() });
								File.WriteAllText(string.Concat(this.documentfolder, "changes.dat"), str8);
							}
							else
							{
								string str9 = File.ReadAllText(string.Concat(this.documentfolder, "changes.dat"));
								try
								{
									string[] strArrays8 = str9.Split(new string[] { "|" }, StringSplitOptions.None);
									this.changeslocal = Convert.ToInt32(strArrays8[0]);
									this.c_listofflocal = Convert.ToInt32(strArrays8[1]);
									this.c_othersettinglocal = Convert.ToInt32(strArrays8[2]);
									this.c_sshlocal = Convert.ToInt32(strArrays8[3]);
									this.c_viplocal = Convert.ToInt32(strArrays8[4]);
									this.c_startalllocal = Convert.ToInt32(strArrays8[5]);
									this.c_stopalllocal = Convert.ToInt32(strArrays8[6]);
									this.c_resetalllocal = Convert.ToInt32(strArrays8[7]);
								}
								catch (Exception exception4)
								{
									MessageBox.Show(exception4.Message);
								}
							}
							if (!Directory.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "SSHFile")))
							{
								Directory.CreateDirectory(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "SSHFile"));
							}
							if (!File.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "SSHFile\\changes.dat")))
							{
								string str10 = this.changesssh.ToString();
								File.WriteAllText(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "SSHFile\\changes.dat"), str10);
							}
							else
							{
								this.changesssh = Convert.ToInt32(File.ReadAllText(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "SSHFile\\changes.dat")));
							}
							this.settingtime.Interval = 1000;
							this.settingtime.Tick += new EventHandler((object o, EventArgs ex) => {
								string str = File.ReadAllText(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting\\changes.dat"));
								try
								{
									string[] strArrays = str.Split(new string[] { "|" }, StringSplitOptions.None);
									int num1 = Convert.ToInt32(strArrays[0]);
									int num2 = Convert.ToInt32(strArrays[1]);
									int num3 = Convert.ToInt32(strArrays[2]);
									int num4 = Convert.ToInt32(strArrays[3]);
									int num5 = Convert.ToInt32(strArrays[4]);
									int num6 = Convert.ToInt32(strArrays[5]);
									int num7 = Convert.ToInt32(strArrays[6]);
									int num8 = Convert.ToInt32(strArrays[7]);
									if (this.changes != num1)
									{
										if (this.c_listoff != num2)
										{
											this.button43_Click(null, null);
										}
										if (this.c_othersetting != num3)
										{
											this.button46_Click(null, null);
										}
										if (this.c_ssh != num4)
										{
											this.button44_Click(null, null);
										}
										if (this.c_vip != num5)
										{
											this.button45_Click(null, null);
										}
										if (this.c_startall != num6)
										{
											if (this.button7.Text != "STOP")
											{
												this.button7_Click(null, null);
											}
										}
										if (this.c_stopall != num7)
										{
											if (this.button7.Text == "STOP")
											{
												this.button7_Click(null, null);
											}
										}
										if (this.c_resetall != num8)
										{
											if (this.button7.Text == "STOP")
											{
												this.button7_Click(null, null);
												this.Reset_Click(null, null);
											}
											else if (this.button7.Text == "RESUME")
											{
												this.Reset_Click(null, null);
											}
										}
										this.changes = num1;
										this.c_listoff = num2;
										this.c_othersetting = num3;
										this.c_ssh = num4;
										this.c_vip = num5;
										this.c_startall = num6;
										this.c_stopall = num7;
										this.c_resetall = num8;
									}
								}
								catch (Exception exception)
								{
								}
								str = File.ReadAllText(string.Concat(this.documentfolder, "changes.dat"));
								try
								{
									string[] strArrays1 = str.Split(new string[] { "|" }, StringSplitOptions.None);
									int num9 = Convert.ToInt32(strArrays1[0]);
									int num10 = Convert.ToInt32(strArrays1[1]);
									int num11 = Convert.ToInt32(strArrays1[2]);
									int num12 = Convert.ToInt32(strArrays1[3]);
									int num13 = Convert.ToInt32(strArrays1[4]);
									int num14 = Convert.ToInt32(strArrays1[5]);
									int num15 = Convert.ToInt32(strArrays1[6]);
									int num16 = Convert.ToInt32(strArrays1[7]);
									if (this.changeslocal != num9)
									{
										if (this.c_listofflocal != num10)
										{
											this.button43_Click(null, null);
										}
										if (this.c_sshlocal != num12)
										{
											this.button44_Click(null, null);
										}
										if (this.c_viplocal != num13)
										{
											this.button45_Click(null, null);
										}
										if (this.c_othersettinglocal != num11)
										{
											this.button46_Click(null, null);
										}
										if (this.c_startalllocal != num14)
										{
											if (this.button7.Text != "STOP")
											{
												this.button7_Click(null, null);
											}
										}
										if (this.c_stopalllocal != num15)
										{
											if (this.button7.Text == "STOP")
											{
												this.button7_Click(null, null);
											}
										}
										if (num16 != this.c_resetalllocal)
										{
											if (this.button7.Text == "STOP")
											{
												this.button7_Click(null, null);
												this.Reset_Click(null, null);
											}
											else if (this.button7.Text == "RESUME")
											{
												this.Reset_Click(null, null);
											}
										}
										this.changeslocal = num9;
										this.c_listofflocal = num10;
										this.c_othersettinglocal = num11;
										this.c_sshlocal = num12;
										this.c_viplocal = num13;
										this.c_startalllocal = num14;
										this.c_stopalllocal = num15;
										this.c_resetalllocal = num16;
									}
								}
								catch (Exception exception1)
								{
								}
								int num17 = Convert.ToInt32(File.ReadAllText(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "SSHFile\\changes.dat")));
								if (num17 != this.changesssh)
								{
									while (true)
									{
										List<string> list = Directory.GetFiles(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "SSHFile")).ToList<string>();
										if (list.Count <= 1)
										{
											break;
										}
										string str1 = "";
										try
										{
											str1 = File.ReadAllText(list.FirstOrDefault<string>((string x) => !x.Contains("changes.dat")));
											File.Delete(list.FirstOrDefault<string>((string x) => !x.Contains("changes.dat")));
										}
										catch (Exception exception2)
										{
											continue;
										}
										this.importssh(str1);
										this.savessh();
										this.ssh_uncheck.Invoke(new MethodInvoker(() => {
											Label sshUncheck = this.ssh_uncheck;
											int num = this.listssh.Count<ssh>((ssh x) => x.live == "uncheck");
											sshUncheck.Text = string.Concat("Uncheck:", num.ToString());
											Label sshUsed = this.ssh_used;
											num = this.listssh.Count<ssh>((ssh x) => x.used);
											sshUsed.Text = string.Concat("Used:", num.ToString());
											Label sshLive = this.ssh_live;
											num = this.listssh.Count<ssh>((ssh x) => x.live == "alive");
											sshLive.Text = string.Concat("Live:", num.ToString());
											Label ssDead = this.ss_dead;
											num = this.listssh.Count<ssh>((ssh x) => x.live == "dead");
											ssDead.Text = string.Concat("Dead:", num.ToString());
										}));
										break;
									}
									this.changesssh = num17;
								}
							});
							this.settingtime.Start();
						}
						else
						{
							this.label12.Text = "Date Expired:Hết Hạn";
						}
					}
					catch (Exception exception5)
					{
						MessageBox.Show(exception5.Message);
						this.button2_Click(null, null);
					}
				}
			}
		}

	    private void threadchecklive(List<ListViewItem> _items)
		{
			foreach (ListViewItem _item in _items)
			{
				this.listView2.Invoke(new MethodInvoker(() => {
					_item.SubItems[0].BackColor = Color.Yellow;
					this.listView2.Refresh();
				}));
				try
				{
					using (SshClient sshClient = new SshClient(_item.SubItems[0].Text, _item.SubItems[1].Text, _item.SubItems[2].Text))
					{
						sshClient.KeepAliveInterval = new TimeSpan(0, 0, 30);
						sshClient.ConnectionInfo.Timeout = new TimeSpan(0, 0, 15);
						try
						{
							sshClient.Connect();
							this.listView2.Invoke(new MethodInvoker(() => {
								_item.SubItems[0].BackColor = Color.Lime;
								this.listssh.ElementAt<ssh>(_item.Index).live = "alive";
								this.listView2.Refresh();
								this.savessh();
								this.ssh_uncheck.Invoke(new MethodInvoker(() => {
									Label sshUncheck = this.ssh_uncheck;
									int num = this.listssh.Count<ssh>((ssh x) => x.live == "uncheck");
									sshUncheck.Text = string.Concat("Uncheck:", num.ToString());
									Label sshUsed = this.ssh_used;
									num = this.listssh.Count<ssh>((ssh x) => x.used);
									sshUsed.Text = string.Concat("Used:", num.ToString());
									Label sshLive = this.ssh_live;
									num = this.listssh.Count<ssh>((ssh x) => x.live == "alive");
									sshLive.Text = string.Concat("Live:", num.ToString());
									Label ssDead = this.ss_dead;
									num = this.listssh.Count<ssh>((ssh x) => x.live == "dead");
									ssDead.Text = string.Concat("Dead:", num.ToString());
								}));
							}));
						}
						catch (Exception exception)
						{
							this.listView2.Invoke(new MethodInvoker(() => {
								_item.SubItems[0].BackColor = Color.Red;
								this.listssh.ElementAt<ssh>(_item.Index).live = "dead";
								this.listView2.Refresh();
							}));
						}
					}
				}
				catch (Exception exception1)
				{
				}
				this.ssh_uncheck.Invoke(new MethodInvoker(() => {
					Label sshUncheck = this.ssh_uncheck;
					int num = this.listssh.Count<ssh>((ssh x) => x.live == "uncheck");
					sshUncheck.Text = string.Concat("Uncheck:", num.ToString());
					Label sshUsed = this.ssh_used;
					num = this.listssh.Count<ssh>((ssh x) => x.used);
					sshUsed.Text = string.Concat("Used:", num.ToString());
					Label sshLive = this.ssh_live;
					num = this.listssh.Count<ssh>((ssh x) => x.live == "alive");
					sshLive.Text = string.Concat("Live:", num.ToString());
					Label ssDead = this.ss_dead;
					num = this.listssh.Count<ssh>((ssh x) => x.live == "dead");
					ssDead.Text = string.Concat("Dead:", num.ToString());
				}));
			}
			this.savessh();
		}

		private void threadcontinuerrs()
		{
			string text = "";
			this.button15.Invoke(new MethodInvoker(() => text = this.button15.Text));
			while (text == "Getting...")
			{
				Thread.Sleep(500);
				this.button15.Invoke(new MethodInvoker(() => text = this.button15.Text));
			}
			Thread.Sleep(5000);
			this.button15.Invoke(new MethodInvoker(() => this.button19_Click(null, null)));
		}

		private void threadgetbk()
		{
			this.cmdResult.getbackup = false;
			this.cmd.getbackup();
			DateTime now = DateTime.Now;
			while (!this.cmdResult.getbackup)
			{
				if ((DateTime.Now - now).TotalSeconds <= 20)
				{
					Thread.Sleep(100);
				}
				else
				{
					break;
				}
			}
			this.button15.Invoke(new MethodInvoker(() => {
				this.button15.Text = "Get Backup list";
				this.button15.Enabled = true;
				this.button15.Refresh();
			}));
		}

		private void threadsetsock()
		{
			this.cmdResult.changeport = false;
			this.cmd.setProxy(this.ipAddressControl1.Text, (int)this.numericUpDown1.Value);
			DateTime now = DateTime.Now;
			while (true)
			{
				if (!this.cmdResult.changeport)
				{
					Thread.Sleep(100);
					if ((DateTime.Now - now).TotalSeconds > 10)
					{
						this.button2.Invoke(new MethodInvoker(() => {
							if (this.button2.Text == "Disconnect")
							{
								this.button2_Click(null, null);
							}
						}));
						break;
					}
				}
				else
				{
					MessageBox.Show("Set proxy done.");
					this.button23.Invoke(new MethodInvoker(() => {
						this.button23.Text = "Disable Proxy";
						this.button23.BackColor = Color.Red;
						this.oriadd = this.ipAddressControl1.Text;
						this.oriport = (int)this.numericUpDown1.Value;
					}));
					break;
				}
			}
		}

	    private void wipethread()
		{
			string item = "";
			this.cmd.close("all");
			this.cmdResult.wipe = false;
			this.label1.Invoke(new MethodInvoker(() => {
				item = this.AppList[this.wipecombo.SelectedIndex].appID;
				if (item == "")
				{
					this.cmdResult.wipe = true;
				}
				this.label1.Text = string.Concat("Wiping Application ", item);
			}));
			this.cmd.faketype(this.getrandomdevice());
			if (this.fakeversion.Checked)
			{
				if ((this.checkBox14.Checked ? true : this.checkBox15.Checked))
				{
					string str = "";
					if ((!this.checkBox14.Checked ? true : !this.checkBox15.Checked))
					{
						str = (!this.checkBox14.Checked ? "9" : "8");
					}
					else
					{
						int num = (new Random()).Next(8, 10);
						str = num.ToString();
					}
					this.cmd.fakeversion(str);
				}
			}
			bool flag = false;
			this.checkBox2.Invoke(new MethodInvoker(() => {
				if (this.checkBox2.Checked)
				{
					flag = true;
				}
			}));
			this.cmd.randominfo();
			if (!flag)
			{
				this.cmd.wipe(item);
			}
			else
			{
				this.cmd.wipefull(item);
			}
			DateTime now = DateTime.Now;
			this.button2.Invoke(new MethodInvoker(() => this.maxwait = (int)this.numericUpDown10.Value));
			while (true)
			{
				if (!this.cmdResult.wipe)
				{
					Thread.Sleep(500);
					if ((DateTime.Now - now).TotalSeconds <= (double)this.maxwait)
					{
						this.cmd.checkwipe();
					}
					else
					{
						this.button2.Invoke(new MethodInvoker(() => {
							if (this.button2.Text == "Disconnect")
							{
								this.button2_Click(null, null);
							}
						}));
						break;
					}
				}
				else
				{
					this.button2.Invoke(new MethodInvoker(() => {
						if (!this.fakedevice.Checked)
						{
							this.cmd.changename("orig");
						}
						if ((!this.fakedevice.Checked || !this.checkBox11.Checked ? true : !File.Exists(this.fileofname.Text)))
						{
							this.cmd.changetimezone("orig");
						}
						else
						{
							string[] strArrays = File.ReadAllLines(this.fileofname.Text);
							Random random = new Random();
							this.cmd.changename(strArrays[random.Next(0, strArrays.Count<string>())]);
						}
						if (this.checkBox20.Checked)
						{
							this.fakeLocationByIP(this.curip);
						}
						if (this.checkBox5.Checked)
						{
							this.cmd.changetimezone(this.ltimezone.Text);
						}
						if (this.fakelang.Checked)
						{
							this.cmd.changelanguage(this.listlanguagecode.FirstOrDefault<languagecode>((languagecode x) => x.langname == this.comboBox1.Text).langcode);
						}
						if (this.fakeregion.Checked)
						{
							this.cmd.changeregion(this.listcountrycodeiOS.FirstOrDefault<countrycodeiOS>((countrycodeiOS x) => x.countryname == this.comboBox2.Text).countrycode);
						}
						if (!this.checkBox13.Checked)
						{
							this.cmd.changecarrier("orig", "orig", "orig", "orig");
						}
						else if (!this.checkBox9.Checked)
						{
							Random random1 = new Random();
							Carrier carrier = this.carrierList.ElementAt<Carrier>(random1.Next(0, this.carrierList.Count));
							this.cmd.changecarrier(carrier.CarrierName, carrier.mobileCountryCode, carrier.mobileCarrierCode, carrier.ISOCountryCode.ToLower());
						}
						else
						{
							List<Carrier> list = (
								from x in this.carrierList
								where x.country == this.carrierBox.Text
								select x).ToList<Carrier>();
							Carrier carrier1 = list.ElementAt<Carrier>((new Random()).Next(0, list.Count));
							this.cmd.changecarrier(carrier1.CarrierName, carrier1.mobileCountryCode, carrier1.mobileCarrierCode, carrier1.ISOCountryCode.ToLower());
						}
						if (!this.checkBox19.Checked)
						{
							this.cmd.fakeGPS(false);
						}
						else
						{
							this.cmd.fakeGPS(true, (double)((double)this.latitude.Value), (double)((double)this.longtitude.Value));
						}
					}));
					this.label1.Invoke(new MethodInvoker(() => {
						item = this.wipecombo.SelectedText;
						this.label1.Text = "Wipe finished...";
					}));
					break;
				}
			}
		}
	}
}