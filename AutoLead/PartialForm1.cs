using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using AutoLead.Properties;

namespace AutoLead
{
    public partial class Form1
    {
       

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = this._listcountry.FirstOrDefault<string>((string x) => x == this.comboBox2.Text) ??
                         this._listcountry.FirstOrDefault<string>((string x) => x.Contains(this.comboBox2.Text));
            this.carrierBox.Text = str;
            this.comboBox1.Text = this.listlanguagecode.FirstOrDefault<languagecode>((languagecode x) =>
                x.langcode == this.listcountrycodeiOS
                    .FirstOrDefault<countrycodeiOS>((countrycodeiOS y) => y.countryname == this.comboBox2.Text)
                    .languageCode).langname;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void comment_TextChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void ConnectCallback(IAsyncResult AR)
        {
            try
            {
                this._clientSocket.EndConnect(AR);
                this._buffer = new byte[this._clientSocket.ReceiveBufferSize];
                this._clientSocket.BeginReceive(this._buffer, 0, (int) this._buffer.Length, SocketFlags.None,
                    new AsyncCallback(this.ReceiveCallBack), null);
                this.button2.Invoke(new MethodInvoker(() => this.button2.Text = "Disconnect"));
                this.DeviceIpControl.Invoke(new MethodInvoker(() => this.DeviceIpControl.Enabled = false));
                this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "Getting info.."));
                this.isconnected = true;
                Thread.Sleep(200);
                base.Invoke(new MethodInvoker(() =>
                {
                    this.button15_Click(null, null);
                    this.cmd.getproxy();
                    this.cmd.getDeviceInfo();
                    this.cmd.getAppList();
                }));
            }
            catch (SocketException socketException)
            {
                if (!this.autoreconnect.Checked)
                {
                    MessageBox.Show("Unable to connect to Idevice", Application.ProductName, MessageBoxButtons.OK,
                        MessageBoxIcon.Hand);
                    this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "Unable to connect to Idevice"));
                    this.button2.Invoke(new MethodInvoker(() => this.button2.Text = "Connect"));
                }
                else
                {
                    this.button2.Invoke(new MethodInvoker(() => this.button2.Text = "Connect"));
                    (new Thread(new ThreadStart(this.reconnect))).Start();
                }
            }
        }

        private void Applyvip_Click(object sender, EventArgs e)
        {
            string text = this.ipAddressControl1.Text;
            string[] strArrays = text.Split(new string[] { "." }, StringSplitOptions.None);
            bool flag = true;
            string[] strArrays1 = strArrays;
            int num = 0;
            while (num < (int)strArrays1.Length)
            {
                if (strArrays1[num] != "")
                {
                    num++;
                }
                else
                {
                    flag = false;
                    break;
                }
            }
            if (flag)
            {
                (new Thread(new ThreadStart(this.threadsetsock))).Start();
            }
            else
            {
                MessageBox.Show("IP Forwarding invalid!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void autoreconnect_CheckedChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void Bink(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            for (int i = 0; i < this.listView4.Items.Count; i++)
            {
                this.listView4.Items[i].Checked = checkBox.Checked;
            }
        }

        private void bkreset_Click(object sender, EventArgs e)
        {
            if (this.button19.Text == "RESUME")
            {
                foreach (ListViewItem item in this.listView4.Items)
                {
                    item.SubItems[0].ResetStyle();
                    this.listView4.Refresh();
                }
                this.button19.Text = "START";
                this.button19.Refresh();
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
                            try
                            {
                                this.bkThread.Abort();
                            }
                            catch (Exception exception)
                            {
                            }
                        }
                        catch (Exception exception1)
                        {
                        }
                    }
                }
                this.rssenableall();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.savedata();
            base.Close();
        }

        private void buttonLumiadd_Click(object sender, EventArgs e)
        {
            string id = this.lumiid.Text;
            string password = this.lumipassword.Text;
            string zone = this.lumizone.Text;

            if (id != "" && password != "" && zone != "")
            {
                luminatio_account _lumiaccount = this.listlumiacc.FirstOrDefault<luminatio_account>((luminatio_account x) => x.username == id && x.zone == zone);
                if (_lumiaccount != null)
                {
                    _lumiaccount.password = password;
                    try
                    {
                        this.listViewQuan3.Items[this.listlumiacc.IndexOf(_lumiaccount)].SubItems[1].Text = id;
                    }
                    catch (Exception exQuan)
                    {
                        MessageBox.Show("Please enter all information!" + exQuan.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);

                        _lumiaccount = new luminatio_account()
                        {
                            username = id,
                            password = password,
                            zone = zone,
                            bad = false
                        };
                        this.listlumiacc.Add(_lumiaccount);
                        ListViewItem listViewItem = new ListViewItem(new string[] { _lumiaccount.username, _lumiaccount.password, _lumiaccount.zone });
                        this.listViewQuan3.Items.Add(listViewItem);
                    }
                    
                }
                else
                {
                    _lumiaccount = new luminatio_account()
                    {
                        username = id,
                        password = password,
                        zone = zone,
                        bad = false
                    };
                    this.listlumiacc.Add(_lumiaccount);
                    ListViewItem listViewItem = new ListViewItem(new string[] { _lumiaccount.username, _lumiaccount.password, _lumiaccount.zone });
                    this.listViewQuan3.Items.Add(listViewItem);
                }
                this.savelumi();
            }
            else {
                MessageBox.Show("Please enter all information!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);

            }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            foreach (string list in this.vipid.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList<string>())
            {
                if ((list == "" ? true : this.vippassword.Text == ""))
                {
                    string[] strArrays = list.Split(new string[] { "|" }, StringSplitOptions.None);
                    if (strArrays.Count<string>() == 2)
                    {
                        string str = strArrays[0];
                        string str1 = strArrays[1];
                        vipaccount _vipaccount = this.listvipacc.FirstOrDefault<vipaccount>((vipaccount x) => x.username == str);
                        if (_vipaccount != null)
                        {
                            _vipaccount.password = str1;
                            this.listView3.Items[this.listvipacc.IndexOf(_vipaccount)].SubItems[1].Text = str1;
                        }
                        else
                        {
                            _vipaccount = new vipaccount()
                            {
                                username = str,
                                password = str1,
                                limited = false
                            };
                            this.listvipacc.Add(_vipaccount);
                            ListViewItem listViewItem = new ListViewItem(new string[] { _vipaccount.username, _vipaccount.password });
                            this.listView3.Items.Add(listViewItem);
                        }
                    }
                }
                else
                {
                    vipaccount text = this.listvipacc.FirstOrDefault<vipaccount>((vipaccount x) => x.username == list);
                    if (text != null)
                    {
                        text.password = this.vippassword.Text;
                        this.listView3.Items[this.listvipacc.IndexOf(text)].SubItems[1].Text = this.vippassword.Text;
                    }
                    else
                    {
                        text = new vipaccount()
                        {
                            username = list,
                            password = this.vippassword.Text,
                            limited = false
                        };
                        this.listvipacc.Add(text);
                        ListViewItem listViewItem1 = new ListViewItem(new string[] { text.username, text.password });
                        this.listView3.Items.Add(listViewItem1);
                    }
                }
                this.savevip72();
            }
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            (new Thread(new ThreadStart(this.wipethread))).Start();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Uri uri;
            bool flag;
            if (!Uri.TryCreate(this.textBox1.Text, UriKind.Absolute, out uri))
            {
                flag = false;
            }
            else
            {
                flag = (uri.Scheme == Uri.UriSchemeHttp ? true : uri.Scheme == Uri.UriSchemeHttps);
            }
            if (flag)
            {
                this.cmd.openURL(this.anaURL(this.textBox1.Text));
            }
            else
            {
                MessageBox.Show("Offer URL is invalid!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            this.cmd.openApp(this.AppList[this.wipecombo.SelectedIndex].appID);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            this.button13.Enabled = false;
            (new Thread(new ThreadStart(this.backupthread))).Start();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            this.importssh(Clipboard.GetText());
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
        }

        private void button15_Click(object sender, EventArgs e)
        {
            this.listbackup.Clear();
            this.listView4.Items.Clear();
            this.label34.Text = "Total RRS:0";
            this.label35.Text = "Selected RRS:0";
            if (this.button15.Text == "Get Backup list")
            {
                this.button15.Text = "Getting...";
                this.button15.Enabled = false;
                (new Thread(new ThreadStart(this.threadgetbk))).Start();
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            string str = "";
            for (int i = this.listView4.SelectedItems.Count - 1; i >= 0; i--)
            {
                str = string.Concat(str, this.listView4.SelectedItems[i].SubItems[7].Text);
                str = string.Concat(str, ";");
                this.listbackup.RemoveAll((backup x) => x.filename == this.listView4.SelectedItems[i].SubItems[7].Text);
                this.listView4.Items.Remove(this.listView4.SelectedItems[i]);
            }
            this.cmd.deletebackup(str);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa tất cả rrs không?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                string str = "";
                foreach (backup _backup in this.listbackup)
                {
                    str = string.Concat(str, _backup.filename);
                    str = string.Concat(str, ";");
                }
                this.cmd.deletebackup(str);
                this.listbackup.Clear();
                this.listView4.Items.Clear();
                this.label34.Text = "Total RRS:0";
                this.label35.Text = "Selected RRS:0";
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            this.pausescript.Enabled = true;
            string str = this.scriptstatus;
            if (str == "stop")
            {
                this.IPThread = new Thread(new ThreadStart(this.excutescriptthread));
                this.IPThread.Start();
                this.button18.BackgroundImage = Resources.Pause_icon;
                this.scriptstatus = "running";
            }
            else if (str == "running")
            {
                this.IPThread.Suspend();
                this.scriptstatus = "pause";
                this.button18.BackgroundImage = Resources.Resume_Button_48;
                this.cmd.randomtouchpause();
            }
            else if (str == "pause")
            {
                this.IPThread.Resume();
                this.scriptstatus = "running";
                this.cmd.randomtouchresume();
                this.button18.BackgroundImage = Resources.Pause_icon;
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if ((this.button19.Text == "START" ? false : this.button19.Text != "RESUME"))
            {
                this.runningstt = 0;
                this.cmd.randomtouchpause();
                this.button19.Text = "RESUME";
                this.button19.Refresh();
                this.rssenableall();
                this.button7.Enabled = true;
                try
                {
                    this.bkThread.Suspend();
                }
                catch (Exception exception)
                {
                }
            }
            else
            {
                if (this.button19.Text == "RESUME")
                {
                    this.cmd.randomtouchresume();
                }
                this.runningstt = 2;
                this.rrsdisableall();
                this.button19.Text = "STOP";
                if (this.button7.Text == "STOP")
                {
                    this.button7_Click(null, null);
                }
                this.button7.Enabled = false;
                if ((this.bkThread == null ? true : (this.bkThread.ThreadState & System.Threading.ThreadState.Stopped) == System.Threading.ThreadState.Stopped))
                {
                    this.bkThread = new Thread(new ThreadStart(this.autoRRS));
                }
                if ((this.bkThread.ThreadState & System.Threading.ThreadState.Suspended) == System.Threading.ThreadState.Suspended)
                {
                    this.bkThread.Resume();
                }
                else if (((this.bkThread.ThreadState & System.Threading.ThreadState.Unstarted) == System.Threading.ThreadState.Unstarted || (this.bkThread.ThreadState & System.Threading.ThreadState.AbortRequested) == System.Threading.ThreadState.AbortRequested || (this.bkThread.ThreadState & System.Threading.ThreadState.Aborted) == System.Threading.ThreadState.Aborted ? true : (this.bkThread.ThreadState & System.Threading.ThreadState.Stopped) == System.Threading.ThreadState.Stopped))
                {
                    this.bkThread = new Thread(new ThreadStart(this.autoRRS));
                    this.bkThread.Start();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.button2.Text == "Connect")
            {
                string text = this.DeviceIpControl.Text;
                string[] strArrays = text.Split(new string[] { "." }, StringSplitOptions.None);
                bool flag = true;
                string[] strArrays1 = strArrays;
                int num = 0;
                while (num < (int)strArrays1.Length)
                {
                    if (strArrays1[num] != "")
                    {
                        num++;
                    }
                    else
                    {
                        flag = false;
                        break;
                    }
                }
                if (!flag)
                {
                    MessageBox.Show("Ip invalid!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    this.button2.Text = "Connecting";
                    this.button2.Refresh();
                    this.label1.Text = "Connecting to iDevice, please wait....";
                    this.label1.Refresh();
                    this._clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPAddress pAddress = IPAddress.Parse(text);
                    this._clientSocket.BeginConnect(new IPEndPoint(pAddress, 2409), new AsyncCallback(this.ConnectCallback), null);
                    System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer()
                    {
                        Interval = 10000
                    };
                    timer.Tick += new EventHandler((object o, EventArgs ex) => {
                        timer.Stop();
                        this.label1.Invoke(new MethodInvoker(() => {
                            if ((this.label1.Text != "Getting info.." ? false : this.autoreconnect.Checked))
                            {
                                this.button2_Click(null, null);
                            }
                        }));
                    });
                    timer.Start();
                }
            }
            else if (this.button2.Text == "Disconnect")
            {
                this.disconnect();
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            this.textBox3.Text = "";
        }

        private void button20_Click_1(object sender, EventArgs e)
        {
            if ((this.ipAddressControl1.Text.Split(new string[] { "." }, StringSplitOptions.None).Count<string>() != 4 ? true : !this.numericUpDown1.Validate()))
            {
                MessageBox.Show("IP and Port invalid!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                this.button20.Enabled = false;
                (new Thread(new ThreadStart(this.threadchangeIP))).Start();
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            this.cmd.clearipa();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            this.listssh.Clear();
            this.listView2.Items.Clear();
            this.savessh();
            this.labeltotalssh.Text = "Total SSH:0";
        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (!this.button23.Text.Contains("Disable"))
            {
                string text = this.ipAddressControl1.Text;
                string[] strArrays = text.Split(new string[] { "." }, StringSplitOptions.None);
                bool flag = true;
                string[] strArrays1 = strArrays;
                int num = 0;
                while (num < (int)strArrays1.Length)
                {
                    if (strArrays1[num] != "")
                    {
                        num++;
                    }
                    else
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    (new Thread(new ThreadStart(this.threadsetsock))).Start();
                }
                else
                {
                    MessageBox.Show("IP Forwarding invalid!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            else
            {
                this.cmd.disableProxy();
                this.button23.Text = "Enable Proxy";
                this.button23.BackColor = Color.Aqua;
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            for (int i = this.listssh.Count - 1; i > 0; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (this.listssh.ElementAt<ssh>(i).IP == this.listssh.ElementAt<ssh>(j).IP)
                    {
                        this.listssh.RemoveAt(j);
                        i--;
                    }
                }
            }
            this.listssh.RemoveAll((ssh x) => (x.live == "dead" ? true : x.used));
            this.listssh = (
                from x in this.listssh
                orderby x.IP
                select x).ToList<ssh>();
            foreach (ssh _ssh in this.listssh)
            {
                _ssh.used = false;
            }
            this.listView2.Items.Clear();
            foreach (ssh _ssh1 in this.listssh)
            {
                ListViewItem listViewItem = new ListViewItem(new string[] { _ssh1.IP, _ssh1.username, _ssh1.password, _ssh1.country });
                if (_ssh1.live == "alive")
                {
                    listViewItem.BackColor = Color.Lime;
                }
                if (_ssh1.live == "dead")
                {
                    listViewItem.BackColor = Color.Red;
                }
                if (_ssh1.used)
                {
                    listViewItem.BackColor = Color.Aqua;
                }
                this.listView2.Items.Add(listViewItem);
            }
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
            Label label = this.labeltotalssh;
            int count = this.listView2.Items.Count;
            label.Text = string.Concat("Total SSH:", count.ToString());
        }

        private void button25_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string str = "";
                foreach (ssh _ssh in this.listssh)
                {
                    str = string.Concat(new string[] { str, _ssh.IP, "|", _ssh.username, "|", _ssh.password, "|", _ssh.country, "|", _ssh.countrycode });
                    str = string.Concat(str, "\r\n");
                }
                File.WriteAllText(saveFileDialog.FileName, str);
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            if (this.button26.Text != "Enable Mouse")
            {
                this.cmd.disablemouse();
                this.button26.Text = "Enable Mouse";
            }
            else
            {
                this.cmd.enablemouse();
                this.button26.Text = "Disable Mouse";
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            this.button27.Enabled = false;
            this.label1.Text = "Restoring App data...";
            if (this.listView4.SelectedItems.Count == 1)
            {
                (new Thread(new ThreadStart(this.restorethread))).Start();
            }
            else if (this.listView4.SelectedItems.Count > 1)
            {
                MessageBox.Show("Please select 1 item only", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            this.button28.Enabled = false;
            this.getApp();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            if (this.listView4.SelectedItems.Count == 1)
            {
                this.button29.Enabled = false;
                this.textBox4.Enabled = false;
                this.button29.Text = "Saving";
                (new Thread(() => this.saverrsthread(this.listView4.SelectedItems[0]))).Start();
            }
            else if (this.listView4.SelectedItems.Count > 1)
            {
                MessageBox.Show("Please select 1 item only", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.optionform.resetFormData();
            this.optionform.ShowDialog();
        }

        private void button30_Click(object sender, EventArgs e)
        {
            if (this.button30.Text != "Record")
            {
                this.button30.BackColor = Color.WhiteSmoke;
                this.button30.Text = "Record";
            }
            else
            {
                this.recordstep = 0;
                this.button30.Text = "Stop Record";
                this.button30.BackColor = Color.Red;
            }
        }

        private void button31_Click(object sender, EventArgs e)
        {
            int text = 0;
            text = this.listView4.Items.IndexOf(this.listView4.SelectedItems[0]);
            this.cmd.savecomment(this.listbackup.ElementAt<backup>(text).filename.Replace(".zip", ""), string.Concat(this.textBox4.Text, "[]", this.listbackup.ElementAt<backup>(text).country));
            this.listView4.Items[text].SubItems[5].Text = this.textBox4.Text;
        }

        private void button32_Click(object sender, EventArgs e)
        {
            this.button32.Enabled = false;
            (new Thread(new ThreadStart(this.excutescriptthread1))).Start();
        }

        private void button33_Click(object sender, EventArgs e)
        {
            int count = this.listView7.Items.Count;
            ListViewItem listViewItem = new ListViewItem(string.Concat("Slot ", count.ToString()));
            this.listView7.Items.Add(listViewItem);
            this.savescripts();
        }

        private void button34_Click(object sender, EventArgs e)
        {
            AutoLead.Script script = new AutoLead.Script();
            int count = this.listscript.Count + 1;
            script.scriptname = string.Concat("Script ", count.ToString());
            if (this.listView7.SelectedItems.Count <= 0)
            {
                this.listView7.Items[0].Selected = true;
            }
            else
            {
                script.slot = this.listView7.SelectedItems[0].Index;
            }
            this.listscript.Add(script);
            this.listView6.Items.Add(new ListViewItem(script.scriptname));
            this.savescripts();
        }

        private void button35_Click(object sender, EventArgs e)
        {
            if (this.button35.Text != "Mở rộng")
            {
                this.textBox2.Location = new Point(114, 281);
                this.textBox2.Size = new System.Drawing.Size(224, 98);
                this.button35.Text = "Mở rộng";
            }
            else
            {
                this.textBox2.Location = new Point(114, 31);
                this.textBox2.Size = new System.Drawing.Size(224, 348);
                this.button35.Text = "Thu nhỏ";
            }
        }

        private void button36_Click(object sender, EventArgs e)
        {
            if ((this.code.Text == "" ? false : this.deviceseri.Text != ""))
            {
                this.button36.Enabled = false;
                string str = string.Concat("http://5.249.146.35/random/napcode?code=", this.code.Text, "&serial=", this.deviceseri.Text);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(str);
                httpWebRequest.UserAgent = "autoleadios";
                try
                {
                    Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
                    string end = (new StreamReader(responseStream)).ReadToEnd();
                    if (end == "notexist")
                    {
                        this.napcodestt.Text = "Mã code không tồn tại";
                        this.napcodestt.ForeColor = Color.Red;
                        MessageBox.Show("Mã code không tồn tại", base.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        this.code.Text = "";
                        this.button36.Enabled = true;
                    }
                    else if (end != "redeemed")
                    {
                        this.napcodestt.Text = string.Concat("Bạn đã nạp thành công ", end, " ngày vào thiết bị");
                        this.napcodestt.ForeColor = Color.Green;
                        this.button36.Enabled = true;
                        this.code.Text = "";
                    }
                    else
                    {
                        this.napcodestt.Text = "Mã code đã được nạp";
                        this.napcodestt.ForeColor = Color.Red;
                        MessageBox.Show("Mã code đã được nạp", base.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        this.button36.Enabled = true;
                    }
                }
                catch (Exception exception)
                {
                    this.napcodestt.Text = "Không thể kết nối với máy chủ";
                    MessageBox.Show("Không thể kết nối với máy chủ", base.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.button36.Enabled = true;
                }
            }
            else
            {
                this.napcodestt.Text = "Vui lòng nhập đầy đủ thông tin";
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin", base.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void button37_Click(object sender, EventArgs e)
        {
            this.button38_Click(null, null);
            this.button39_Click(null, null);
            this.button40_Click(null, null);
            this.button41_Click(null, null);
        }

        private void button38_Click(object sender, EventArgs e)
        {
            string str = string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber, "\\offerlist.dat");
            string str1 = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting\\offerlist.dat");
            if (!Directory.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting")))
            {
                Directory.CreateDirectory(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting"));
            }
            try
            {
                File.Copy(str, str1, true);
            }
            catch (Exception exception)
            {
            }
        }

        private void button39_Click(object sender, EventArgs e)
        {
            string str = string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber, "\\ssh.dat");
            string str1 = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting\\ssh.dat");
            if (!Directory.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting")))
            {
                Directory.CreateDirectory(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting"));
            }
            try
            {
                File.Copy(str, str1, true);
            }
            catch (Exception exception)
            {
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                int num = this.listView1.Items.IndexOf(this.listView1.SelectedItems[0]);
                offerItem _offerItem = this.offerListItem.ElementAt<offerItem>(num);
                this.optionform.setFormData(_offerItem.offerEnable, _offerItem.offerName, _offerItem.offerURL, _offerItem.appName, _offerItem.timeSleepBefore, _offerItem.timeSleepBeforeRandom, _offerItem.range1, _offerItem.range2, _offerItem.timeSleep, _offerItem.useScript, _offerItem.script, _offerItem.referer);
                this.optionform.ShowDialog();
            }
        }

        private void button40_Click(object sender, EventArgs e)
        {
            string str = string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber, "\\vip72.dat");
            string str1 = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting\\vip72.dat");
            if (!Directory.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting")))
            {
                Directory.CreateDirectory(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting"));
            }
            try
            {
                File.Copy(str, str1, true);
            }
            catch (Exception exception)
            {
            }
        }

        private void button41_Click(object sender, EventArgs e)
        {
            string str = string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber, "\\setting.dat");
            string str1 = string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber, "\\scripts.dat");
            string str2 = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting\\setting.dat");
            string str3 = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting\\scripts.dat");
            if (!Directory.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting")))
            {
                Directory.CreateDirectory(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting"));
            }
            try
            {
                File.Copy(str, str2, true);
                File.Copy(str1, str3, true);
            }
            catch (Exception exception)
            {
            }
        }

        private void button42_Click(object sender, EventArgs e)
        {
            this.button43_Click(null, null);
            this.button44_Click(null, null);
            this.button45_Click(null, null);
            this.button46_Click(null, null);
        }

        private void button43_Click(object sender, EventArgs e)
        {
            string str = string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber, "\\offerlist.dat");
            string str1 = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting\\offerlist.dat");
            if (!Directory.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting")))
            {
                Directory.CreateDirectory(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting"));
            }
            try
            {
                File.Copy(str1, str, true);
            }
            catch (Exception exception)
            {
            }
            this.loadofferlist();
        }

        private void button44_Click(object sender, EventArgs e)
        {
            string str = string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber, "\\ssh.dat");
            string str1 = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting\\ssh.dat");
            if (!Directory.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting")))
            {
                Directory.CreateDirectory(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting"));
            }
            try
            {
                File.Copy(str1, str, true);
            }
            catch (Exception exception)
            {
            }
            this.loadssh();
        }

        private void button45_Click(object sender, EventArgs e)
        {
            string str = string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber, "\\vip72.dat");
            string str1 = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting\\vip72.dat");
            if (!Directory.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting")))
            {
                Directory.CreateDirectory(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting"));
            }
            try
            {
                File.Copy(str1, str, true);
            }
            catch (Exception exception)
            {
            }
            this.loadvip72();
        }

        private void button46_Click(object sender, EventArgs e)
        {
            string str = string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber, "\\setting.dat");
            string str1 = string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber, "\\scripts.dat");
            string str2 = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting\\setting.dat");
            string str3 = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting\\scripts.dat");
            if (!Directory.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting")))
            {
                Directory.CreateDirectory(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "GlobalSetting"));
            }
            try
            {
                File.Copy(str2, str, true);
                File.Copy(str3, str1, true);
            }
            catch (Exception exception)
            {
            }
            this.loadothresetting();
            this.loadscripts();
        }

        private void button47_Click(object sender, EventArgs e)
        {
            if (this.checkBox16.Checked)
            {
                this.changeslocal = 1 - this.changeslocal;
                this.c_listofflocal = 1 - this.c_listofflocal;
                this.c_othersettinglocal = 1 - this.c_othersettinglocal;
                this.c_sshlocal = 1 - this.c_sshlocal;
                this.c_viplocal = 1 - this.c_viplocal;
            }
            else
            {
                this.changes = 1 - this.changes;
                this.c_listoff = 1 - this.c_listoff;
                this.c_othersetting = 1 - this.c_othersetting;
                this.c_ssh = 1 - this.c_ssh;
                this.c_vip = 1 - this.c_vip;
            }
            this.button37_Click(null, null);
            this.exportchanges();
        }

        private void button48_Click(object sender, EventArgs e)
        {
            if (this.checkBox16.Checked)
            {
                this.changeslocal = 1 - this.changeslocal;
                this.c_listofflocal = 1 - this.c_listofflocal;
            }
            else
            {
                this.changes = 1 - this.changes;
                this.c_listoff = 1 - this.c_listoff;
            }
            this.button53_Click(null, null);
            this.button38_Click(null, null);
            this.exportchanges();
        }

        private void button49_Click(object sender, EventArgs e)
        {
            if (this.checkBox16.Checked)
            {
                this.changeslocal = 1 - this.changeslocal;
                this.c_sshlocal = 1 - this.c_sshlocal;
            }
            else
            {
                this.changes = 1 - this.changes;
                this.c_ssh = 1 - this.c_ssh;
            }
            this.button39_Click(null, null);
            this.exportchanges();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                int num = this.listView1.Items.IndexOf(this.listView1.SelectedItems[0]);
                this.offerListItem.RemoveAt(num);
                this.listView1.Items[num].Remove();
            }
        }

        private void button50_Click(object sender, EventArgs e)
        {
            if (this.checkBox16.Checked)
            {
                this.changeslocal = 1 - this.changeslocal;
                this.c_viplocal = 1 - this.c_viplocal;
            }
            else
            {
                this.changes = 1 - this.changes;
                this.c_vip = 1 - this.c_vip;
            }
            this.button40_Click(null, null);
            this.exportchanges();
        }

        private void button51_Click(object sender, EventArgs e)
        {
            if (this.checkBox16.Checked)
            {
                this.changeslocal = 1 - this.changeslocal;
                this.c_othersettinglocal = 1 - this.c_othersettinglocal;
            }
            else
            {
                this.changes = 1 - this.changes;
                this.c_othersetting = 1 - this.c_othersetting;
            }
            this.button41_Click(null, null);
            this.exportchanges();
        }

        private void button52_Click(object sender, EventArgs e)
        {
            if (this.checkBox16.Checked)
            {
                this.changeslocal = 1 - this.changeslocal;
                this.c_startalllocal = 1 - this.c_startalllocal;
            }
            else
            {
                this.changes = 1 - this.changes;
                this.c_startall = 1 - this.c_startall;
            }
            this.exportchanges();
        }

        private void button53_Click(object sender, EventArgs e)
        {
            if (this.checkBox16.Checked)
            {
                this.changeslocal = 1 - this.changeslocal;
                this.c_resetalllocal = 1 - this.c_resetalllocal;
            }
            else
            {
                this.changes = 1 - this.changes;
                this.c_resetall = 1 - this.c_resetall;
            }
            this.exportchanges();
        }

        private void button54_Click(object sender, EventArgs e)
        {
            if (this.checkBox16.Checked)
            {
                this.changeslocal = 1 - this.changeslocal;
                this.c_stopalllocal = 1 - this.c_stopalllocal;
            }
            else
            {
                this.changes = 1 - this.changes;
                this.c_stopall = 1 - this.c_stopall;
            }
            this.exportchanges();
        }

        private void button58_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string str = "";
                List<string> strs = new List<string>();
                foreach (ssh _ssh in this.listssh)
                {
                    string str1 = string.Concat(_ssh.username, "|", _ssh.password);
                    strs.Add(str1);
                }
                foreach (string str2 in strs.Distinct<string>())
                {
                    str = string.Concat(str, str2);
                    str = string.Concat(str, " ");
                    int num = strs.Count<string>((string x) => x == str2);
                    str = string.Concat(str, num.ToString());
                    str = string.Concat(str, "\r\n");
                }
                File.WriteAllText(saveFileDialog.FileName, str);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.offerListItem.Clear();
            this.listView1.Items.Clear();
        }

        private void button64_Click(object sender, EventArgs e)
        {
            this.cmd.resping();
        }

        private void button66_Click(object sender, EventArgs e)
        {
            this.cmd.randomtouchpause();
        }

        private void button66_Click_1(object sender, EventArgs e)
        {
        }

        private void button67_Click(object sender, EventArgs e)
        {
            this.cmd.randomtouchresume();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if ((this.button7.Text == "START" ? false : this.button7.Text != "RESUME"))
            {
                this.runningstt = 0;
                try
                {
                    this.oThread.Suspend();
                }
                catch (Exception exception)
                {
                }
                this.cmd.randomtouchpause();
                this.button7.Text = "RESUME";
                this.button7.Refresh();
                this.enableAll();
                this.button19.Enabled = true;
                this.button19.Refresh();
            }
            else
            {
                this.runningstt = 1;
                if (this.listView1.SelectedItems.Count > 0)
                {
                    this.listView1.SelectedItems[0].Selected = false;
                }
                this.button19.Enabled = false;
                this.button19.Refresh();
                this.disableAll();
                if (this.button7.Text != "START")
                {
                    this.cmd.randomtouchresume();
                    if ((this.oThread == null ? true : (this.oThread.ThreadState & System.Threading.ThreadState.Stopped) == System.Threading.ThreadState.Stopped))
                    {
                        this.oThread = new Thread(new ThreadStart(this.autoLeadThread));
                    }
                    if ((this.oThread.ThreadState & System.Threading.ThreadState.Suspended) == System.Threading.ThreadState.Suspended)
                    {
                        this.oThread.Resume();
                    }
                    else if (((this.oThread.ThreadState & System.Threading.ThreadState.Unstarted) == System.Threading.ThreadState.Unstarted || (this.oThread.ThreadState & System.Threading.ThreadState.AbortRequested) == System.Threading.ThreadState.AbortRequested || (this.oThread.ThreadState & System.Threading.ThreadState.Aborted) == System.Threading.ThreadState.Aborted ? true : (this.oThread.ThreadState & System.Threading.ThreadState.Stopped) == System.Threading.ThreadState.Stopped))
                    {
                        this.oThread = new Thread(new ThreadStart(this.autoLeadThread));
                        this.oThread.Start();
                    }
                }
                else
                {
                    this.oThread = new Thread(new ThreadStart(this.autoLeadThread));
                    this.oThread.Start();
                }
                this.button7.Text = "STOP";
                this.button7.Refresh();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            for (int i = this.listView2.SelectedItems.Count - 1; i >= 0; i--)
            {
                this.listssh.RemoveAt(this.listView2.SelectedItems[i].Index);
                this.listView2.Items.Remove(this.listView2.SelectedItems[i]);
            }
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
            Label label = this.labeltotalssh;
            int count = this.listView2.Items.Count;
            label.Text = string.Concat("Total SSH:", count.ToString());
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (this.button9.Text == "Check Live")
            {
                List<ListViewItem>[] listViewItems = new List<ListViewItem>[(int)this.numericUpDown2.Value];
                for (int i = 0; i < this.numericUpDown2.Value; i++)
                {
                    listViewItems[i] = new List<ListViewItem>();
                }
                int num = 0;
                int num1 = 0;
                while (num < this.listView2.Items.Count)
                {
                    ListViewItem item = this.listView2.Items[num];
                    listViewItems[num1].Add(item);
                    num++;
                    num1++;
                    if (num1 >= (int)this.numericUpDown2.Value)
                    {
                        num1 = 0;
                    }
                }
                for (int j = 0; j < (int)this.numericUpDown2.Value; j++)
                {
                    if (listViewItems[j].Count > 0)
                    {
                        (new Thread(() => this.threadchecklive(listViewItems[j]))).Start();
                        Thread.Sleep(100);
                    }
                }
            }
        }

        private void carrierBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.checkBox1.Checked)
            {
                this.comment.Enabled = false;
            }
            else
            {
                this.comment.Enabled = true;
            }
            this.checkBox3.Enabled = this.checkBox1.Checked;
            this.saveothersetting();
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox11.Checked)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.fileofname.Text = openFileDialog.FileName;
                }
                else if (this.fileofname.Text == "")
                {
                    this.checkBox11.Checked = false;
                }
            }
            this.saveothersetting();
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            this.carrierBox.Enabled = (!this.checkBox9.Checked ? false : !this.checkBox20.Checked);
            this.comboBox1.Enabled = (!this.fakelang.Checked ? false : !this.checkBox20.Checked);
            this.comboBox2.Enabled = (!this.fakeregion.Checked ? false : !this.checkBox20.Checked);
            Label label = this.ltimezone;
            NumericUpDown numericUpDown = this.longtitude;
            bool @checked = !this.checkBox20.Checked;
            bool flag = @checked;
            this.latitude.Enabled = @checked;
            bool flag1 = flag;
            bool flag2 = flag1;
            numericUpDown.Enabled = flag1;
            label.Enabled = flag2;
            if (this.checkBox20.Checked)
            {
                CheckBox checkBox = this.checkBox5;
                CheckBox checkBox1 = this.checkBox9;
                CheckBox checkBox2 = this.checkBox13;
                CheckBox checkBox3 = this.fakelang;
                CheckBox checkBox4 = this.fakeregion;
                int num = 1;
                bool flag3 = true;
                this.checkBox19.Checked = true;
                bool flag4 = flag3;
                bool flag5 = flag4;
                checkBox4.Checked = flag4;
                bool flag6 = flag5;
                bool flag7 = flag6;
                checkBox3.Checked = flag6;
                bool flag8 = flag7;
                flag = flag8;
                checkBox2.Checked = flag8;
                bool flag9 = flag;
                flag2 = flag9;
                checkBox1.Checked = flag9;
                checkBox.Checked = flag2;
            }
            this.saveothersetting();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.checkBox6.Checked)
            {
                this.checkBox7.Enabled = false;
                this.comboBox3.Enabled = false;
            }
            else
            {
                this.checkBox7.Enabled = true;
                this.comboBox3.Enabled = true;
            }
            this.checkBox7_CheckedChanged(null, null);
            this.saveothersetting();
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            this.comboBox3.Items.Clear();
            if (!this.checkBox7.Checked)
            {
                foreach (AutoLead.Script script in this.listscript)
                {
                    this.comboBox3.Items.Add(script.scriptname);
                }
            }
            else
            {
                foreach (ListViewItem item in this.listView7.Items)
                {
                    this.comboBox3.Items.Add(item.Text);
                }
            }
            this.saveothersetting();
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            this.carrierBox.Enabled = (!this.checkBox9.Checked ? false : !this.checkBox20.Checked);
            this.saveothersetting();
        }

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            long bytesReceived = e.BytesReceived;
            double num = double.Parse(bytesReceived.ToString());
            bytesReceived = e.TotalBytesToReceive;
            double num1 = double.Parse(bytesReceived.ToString());
            double num2 = num / num1 * 100;
            ProgressBar progressBar = this.downloadform.progressBar1;
            double num3 = Math.Truncate(num2);
            progressBar.Value = int.Parse(num3.ToString());
            this.downloadform.progressBar1.Refresh();
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void Contact_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Text = this.DeviceIpControl.Text.Split(new string[] { "." }, StringSplitOptions.None)[3];
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView7.SelectedItems.Count > 0)
            {
                this.listView6.Items.Clear();
                this.listscript.RemoveAll((AutoLead.Script x) => x.slot == this.listView7.SelectedItems[0].Index);
                IEnumerable<AutoLead.Script> index = 
                    from x in this.listscript
                    where x.slot > this.listView7.SelectedItems[0].Index
                    select x;
                foreach (AutoLead.Script script in index)
                {
                    AutoLead.Script script1 = script;
                    script1.slot = script1.slot - 1;
                }
                if (this.listView7.SelectedItems[0].Index <= 0)
                {
                    this.listscript.Clear();
                }
                else
                {
                    for (int i = this.listView7.SelectedItems[0].Index + 1; i < this.listView7.Items.Count; i++)
                    {
                        int num = i - 1;
                        this.listView7.Items[i].Text = string.Concat("Slot ", num.ToString());
                    }
                    this.listView7.SelectedItems[0].Remove();
                }
                this.checkBox6_CheckedChanged(null, null);
                this.savescripts();
            }
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.listView6.SelectedItems.Count > 0)
            {
                List<AutoLead.Script> list = (
                    from x in this.listscript
                    where (x.slot == this.listView7.SelectedItems[0].Index ? true : this.listView7.SelectedItems[0].Index == 0)
                    select x).ToList<AutoLead.Script>();
                this.listscript.Remove(list.ElementAt<AutoLead.Script>(this.listView6.SelectedItems[0].Index));
                this.listView6.Items.Remove(this.listView6.SelectedItems[0]);
                this.textBox6.Enabled = false;
                this.textBox6.Text = "";
                this.textBox9.Enabled = false;
                this.textBox9.Text = "";
                this.checkBox6_CheckedChanged(null, null);
                this.savescripts();
            }
        }

        private void DeviceIpControl_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.ipaddress = this.DeviceIpControl.Text;
            Settings.Default.Save();
        }

        private void downloadcompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Can't download file, please try again", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.downloadform.Hide();
            }
            else
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo()
                {
                    FileName = "_AutoLead.exe",
                    WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory
                };
                Process.Start(processStartInfo);
                Application.Exit();
                Environment.Exit(0);
            }
        }

        private void fakedevice_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.fakedevice.Checked)
            {
                this.checkBox11.Enabled = false;
            }
            else
            {
                this.checkBox11.Enabled = true;
            }
            this.saveothersetting();
        }

        private void fakemodel_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.fakemodel.Checked)
            {
                this.ipad.Enabled = false;
                this.iphone.Enabled = false;
                this.ipod.Enabled = false;
            }
            else
            {
                this.ipad.Enabled = true;
                this.iphone.Enabled = true;
                this.ipod.Enabled = true;
            }
            this.saveothersetting();
        }

        private void fakeregion_CheckedChanged(object sender, EventArgs e)
        {
            this.comboBox2.Enabled = (!this.fakeregion.Checked ? false : !this.checkBox20.Checked);
            this.saveothersetting();
        }

        private void fakeversion_CheckedChanged(object sender, EventArgs e)
        {
            this.checkBox14.Enabled = this.fakeversion.Checked;
            this.checkBox15.Enabled = this.fakeversion.Checked;
            this.saveothersetting();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this._clientSocket != null)
            {
                this._clientSocket.Close();
                this._clientSocket.Dispose();
            }
            try
            {
                sshcommand.closebitvise((int)this.numericUpDown1.Value);
                this.bitproc.Kill();
            }
            catch (Exception exception)
            {
            }
            Application.Exit();
            Environment.Exit(0);
        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {
        }

        private void importfromfile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.importssh(File.ReadAllText(openFileDialog.FileName));
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
            }
        }

        private void ipad_CheckedChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void ipAddressControl1_Click(object sender, EventArgs e)
        {
            this.button23.Text = "Apply";
        }

        private void ipAddressControl1_TextChanged(object sender, EventArgs e)
        {
        }

        private void iphone_CheckedChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void ipod_CheckedChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void itunesX_ValueChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void itunesY_ValueChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void latitude_ValueChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void listApp_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void listcmd_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void listView1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            ListViewItem item = this.listView1.Items[e.Index];
            if (e.NewValue == CheckState.Checked)
            {
                this.offerListItem.ElementAt<offerItem>(e.Index).offerEnable = true;
            }
            else if (e.NewValue == CheckState.Unchecked)
            {
                this.offerListItem.ElementAt<offerItem>(e.Index).offerEnable = false;
            }
            this.saveofferlist();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.button7.Text == "STOP")
            {
                if (this.listView1.SelectedItems.Count > 0)
                {
                    this.listView1.SelectedItems[0].Selected = false;
                }
            }
        }

        private void listView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                this.button8_Click(null, null);
            }
        }

        private void listView2_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void listView3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                this.vipdelete_Click(null, null);
            }
        }

        private void listViewQuan3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                this.lumidelete_Click(null, null);
            }
        }

        private void listView4_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column != 0)
            {
                if (e.Column != this.lvwColumnSorter.SortColumn)
                {
                    this.lvwColumnSorter.SortColumn = e.Column;
                    this.lvwColumnSorter.Order = SortOrder.Ascending;
                }
                else if (this.lvwColumnSorter.Order != SortOrder.Ascending)
                {
                    this.lvwColumnSorter.Order = SortOrder.Ascending;
                }
                else
                {
                    this.lvwColumnSorter.Order = SortOrder.Descending;
                }
                if (this.lvwColumnSorter.Order != SortOrder.Ascending)
                {
                    switch (e.Column)
                    {
                        case 1:
                        {
                            this.listbackup = (
                                from x in this.listbackup
                                orderby x.timecreate.ToString("MM/dd/yyyy HH:mm:ss") descending
                                select x).ToList<backup>();
                            break;
                        }
                        case 2:
                        {
                            this.listbackup = (
                                from x in this.listbackup
                                orderby x.timemod.ToString("MM/dd/yyyy HH:mm:ss") descending
                                select x).ToList<backup>();
                            break;
                        }
                        case 3:
                        {
                            this.listbackup = (
                                from x in this.listbackup
                                orderby x.runtime.ToString() descending
                                select x).ToList<backup>();
                            break;
                        }
                        case 4:
                        {
                            this.listbackup = (
                                from x in this.listbackup
                                orderby string.Join(";", x.appList.ToList<string>()).ToString() descending
                                select x).ToList<backup>();
                            break;
                        }
                        case 5:
                        {
                            this.listbackup = (
                                from x in this.listbackup
                                orderby x.comment.ToString() descending
                                select x).ToList<backup>();
                            break;
                        }
                        case 6:
                        {
                            this.listbackup = (
                                from x in this.listbackup
                                orderby x.country.ToString() descending
                                select x).ToList<backup>();
                            break;
                        }
                        case 7:
                        {
                            this.listbackup = (
                                from x in this.listbackup
                                orderby x.filename.ToString() descending
                                select x).ToList<backup>();
                            break;
                        }
                    }
                }
                else
                {
                    switch (e.Column)
                    {
                        case 1:
                        {
                            this.listbackup = (
                                from x in this.listbackup
                                orderby x.timecreate.ToString("MM/dd/yyyy HH:mm:ss")
                                select x).ToList<backup>();
                            break;
                        }
                        case 2:
                        {
                            this.listbackup = (
                                from x in this.listbackup
                                orderby x.timemod.ToString("MM/dd/yyyy HH:mm:ss")
                                select x).ToList<backup>();
                            break;
                        }
                        case 3:
                        {
                            this.listbackup = (
                                from x in this.listbackup
                                orderby x.runtime.ToString()
                                select x).ToList<backup>();
                            break;
                        }
                        case 4:
                        {
                            this.listbackup = (
                                from x in this.listbackup
                                orderby string.Join(";", x.appList.ToList<string>()).ToString()
                                select x).ToList<backup>();
                            break;
                        }
                        case 5:
                        {
                            this.listbackup = (
                                from x in this.listbackup
                                orderby x.comment.ToString()
                                select x).ToList<backup>();
                            break;
                        }
                        case 6:
                        {
                            this.listbackup = (
                                from x in this.listbackup
                                orderby x.country.ToString()
                                select x).ToList<backup>();
                            break;
                        }
                        case 7:
                        {
                            this.listbackup = (
                                from x in this.listbackup
                                orderby x.filename.ToString()
                                select x).ToList<backup>();
                            break;
                        }
                    }
                }
                this.listView4.Sort();
            }
        }

        private void listView4_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            if ((e.ColumnIndex != 0 ? true : this.first != 0))
            {
                e.DrawDefault = true;
            }
            else
            {
                this.first = 1;
                CheckBox checkBox = new CheckBox();
                base.Visible = true;
                this.listView4.SuspendLayout();
                e.DrawBackground();
                checkBox.BackColor = Color.Transparent;
                checkBox.UseVisualStyleBackColor = true;
                int x = e.Bounds.X;
                int y = e.Bounds.Y;
                int width = e.Bounds.Width;
                Rectangle bounds = e.Bounds;
                System.Drawing.Size preferredSize = checkBox.GetPreferredSize(new System.Drawing.Size(width, bounds.Height));
                int num = preferredSize.Width;
                int width1 = e.Bounds.Width;
                bounds = e.Bounds;
                preferredSize = checkBox.GetPreferredSize(new System.Drawing.Size(width1, bounds.Height));
                checkBox.SetBounds(x, y, num, preferredSize.Width);
                bounds = e.Bounds;
                int num1 = bounds.Width - 1;
                bounds = e.Bounds;
                preferredSize = checkBox.GetPreferredSize(new System.Drawing.Size(num1, bounds.Height));
                bounds = e.Bounds;
                checkBox.Size = new System.Drawing.Size(preferredSize.Width + 1, bounds.Height);
                checkBox.Location = new Point(3, 0);
                this.listView4.Controls.Add(checkBox);
                checkBox.Show();
                checkBox.BringToFront();
                e.DrawText(TextFormatFlags.VerticalCenter);
                checkBox.Click += new EventHandler(this.Bink);
                this.listView4.ResumeLayout(true);
            }
        }

        private void listView4_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView4_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView4_ItemCheck(object sender, ItemCheckEventArgs e)
        {
        }

        private void listView4_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (!this._sshssh)
            {
                this.savecheckedssh();
            }
        }

        private void listView4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                this.button16_Click(null, null);
            }
        }

        private void listView4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView4.SelectedItems.Count > 0)
            {
                this.textBox4.Text = this.listView4.SelectedItems[0].SubItems[5].Text;
            }
            Label label = this.label35;
            int count = this.listView4.SelectedItems.Count;
            label.Text = string.Concat("Selected RRS:", count.ToString());
        }

        private void listView5_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.listView5.SelectedItems.Count > 0)
            {
                this.ltimezone.Text = this.listView5.SelectedItems[0].Text;
                this.listView5.Hide();
                this.textBox5.Hide();
            }
        }

        private void listView5_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void listView6_KeyDown(object sender, KeyEventArgs e)
        {
            if ((this.listView6.SelectedItems.Count <= 0 ? false : e.KeyCode == Keys.Delete))
            {
                this.deleteToolStripMenuItem1_Click(null, null);
            }
        }

        private void listView6_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                this.moveToSlotToolStripMenuItem.DropDownItems.Clear();
                this.moveToSlotToolStripMenuItem.DropDownItems.Add("None", null, new EventHandler(this.toolStripMenuItem1_Click));
                foreach (ListViewItem item in this.listView7.Items)
                {
                    if (item.Text != "All Script")
                    {
                        this.moveToSlotToolStripMenuItem.DropDownItems.Add(item.Text, null, new EventHandler(this.toolStripMenuItem1_Click));
                    }
                }
                this.contextMenuStrip2.Show(this.listView6.PointToScreen(e.Location));
            }
        }

        private void listView6_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void listView6_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (this.listView6.SelectedItems.Count == 0)
                {
                    this.textBox6.Enabled = false;
                    this.textBox9.Enabled = false;
                    this.textBox6.Text = "";
                    this.textBox9.Text = "";
                }
            }
        }

        private void listView6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView6.SelectedItems.Count > 0)
            {
                this.textBox9.Enabled = true;
                this.textBox6.Enabled = true;
                List<AutoLead.Script> list = (
                    from x in this.listscript
                    where (x.slot == this.listView7.SelectedItems[0].Index ? true : this.listView7.SelectedItems[0].Index == 0)
                    select x).ToList<AutoLead.Script>();
                this.textBox9.Text = list.ElementAt<AutoLead.Script>(this.listView6.SelectedItems[0].Index).scriptname;
                this.textBox6.Text = list.ElementAt<AutoLead.Script>(this.listView6.SelectedItems[0].Index).script;
            }
        }

        private void listview7_DeleteSlot()
        {
            if (this.listView7.SelectedItems.Count > 0)
            {
            }
        }

        private void listView7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                this.deleteToolStripMenuItem_Click(null, null);
            }
        }

        private void listView7_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                this.contextMenuStrip1.Show(this.listView7.PointToScreen(e.Location));
            }
        }

        private void listView7_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void listView7_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.listView7.SelectedItems.Count == 0)
            {
                this.listView6.Items.Clear();
            }
        }

        private void listView7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView7.SelectedItems.Count > 0)
            {
                this.listView6.Items.Clear();
                List<AutoLead.Script> list = (
                    from x in this.listscript
                    where (x.slot == this.listView7.SelectedItems[0].Index ? true : this.listView7.SelectedItems[0].Index == 0)
                    select x).ToList<AutoLead.Script>();
                foreach (AutoLead.Script script in list)
                {
                    this.listView6.Items.Add(new ListViewItem(script.scriptname));
                }
            }
        }

        private void listView8_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void longtitude_ValueChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void ltimezone_Click(object sender, EventArgs e)
        {
            if (!this.listView5.Visible)
            {
                this.listView5.Show();
                this.textBox5.Show();
            }
            else
            {
                this.listView5.Hide();
                this.textBox5.Hide();
            }
        }

        private void ltimezone_TextChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if ((int)this.numericUpDown1.Value == 22999)
            {
                while (true)
                {
                    decimal design_value = new decimal(new int[] { 1080, 0, 0, 0 });

                    if ((int)design_value != 22999)
                    {
                        this.numericUpDown1.Value = design_value;
                        break;
                    }

                }

            }


            this.button23.Text = "Apply";
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            this.maxwait = (int)this.numericUpDown10.Value;
            this.saveothersetting();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void pausescript_Click(object sender, EventArgs e)
        {
            if (this.scriptstatus == "pause")
            {
                this.IPThread.Resume();
            }
            this.IPThread.Abort();
            this.button18.BackgroundImage = Resources.Play_icon__1_;
            this.scriptstatus = "stop";
            this.pausescript.Enabled = false;
            this.cmd.randomtouchstop();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            RichTextBox richTextBox;
            this.textBox3.Text = string.Concat(new string[] { "Touch(", this.textBox7.Text, ",", this.textBox8.Text, ")" });
            if (this.button30.Text == "Stop Record")
            {
                TimeSpan now = DateTime.Now - this.clicklast;
                double num = Math.Round(now.TotalMilliseconds / 1000, 2);
                if ((num >= 1 || ((MouseEventArgs)e).X * this.trackBar1.Value != this.prevx ? true : ((MouseEventArgs)e).Y * this.trackBar1.Value != this.prevy))
                {
                    richTextBox = this.textBox2;
                    richTextBox.Text = string.Concat(new string[] { richTextBox.Text, "swipe(", this.prevx.ToString(), ",", this.prevy.ToString(), ",", this.textBox7.Text, ",", this.textBox8.Text, ",", num.ToString(), ")" });
                }
                else
                {
                    richTextBox = this.textBox2;
                    richTextBox.Text = string.Concat(new string[] { richTextBox.Text, "Touch(", this.textBox7.Text, ",", this.textBox8.Text, ")" });
                }
                RichTextBox richTextBox1 = this.textBox2;
                richTextBox1.Text = string.Concat(richTextBox1.Text, "\r\n");
                this.textBox2.Focus();
                this.textBox2.SelectionStart = this.textBox2.Text.Length;
                this.textBox2.ScrollToCaret();
                this.cmd.mouseup(((MouseEventArgs)e).X * this.trackBar1.Value, ((MouseEventArgs)e).Y * this.trackBar1.Value);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.button30.Text == "Stop Record")
            {
                if (this.recordstep != 0)
                {
                    RichTextBox richTextBox = this.textBox2;
                    string text = richTextBox.Text;
                    TimeSpan now = DateTime.Now - this.recprev;
                    double num = Math.Round((double)(now.TotalMilliseconds / 1000), 2);
                    richTextBox.Text = string.Concat(text, "wait(", num.ToString(), ")");
                    RichTextBox richTextBox1 = this.textBox2;
                    richTextBox1.Text = string.Concat(richTextBox1.Text, "\r\n");
                    this.textBox2.Focus();
                    this.textBox2.SelectionStart = this.textBox2.Text.Length;
                    this.textBox2.ScrollToCaret();
                    this.recprev = DateTime.Now;
                }
                else
                {
                    this.recprev = DateTime.Now;
                }
                this.cmd.mousedown(e.X * this.trackBar1.Value, e.Y * this.trackBar1.Value);
                this.prevx = Convert.ToInt32(this.textBox7.Text);
                this.prevy = Convert.ToInt32(this.textBox8.Text);
                this.clicklast = DateTime.Now;
                this.recordstep++;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            TextBox str = this.textBox7;
            int x = e.X * this.trackBar1.Value;
            str.Text = x.ToString();
            TextBox textBox = this.textBox8;
            x = e.Y * this.trackBar1.Value;
            textBox.Text = x.ToString();
            if (this.button26.Text == "Disable Mouse")
            {
                if ((e.Button != System.Windows.Forms.MouseButtons.Left ? true : this.button30.Text != "Stop Record"))
                {
                    this.cmd.movemouse(e.X * this.trackBar1.Value, e.Y * this.trackBar1.Value);
                }
                else
                {
                    this.cmd.mousedown(e.X * this.trackBar1.Value, e.Y * this.trackBar1.Value);
                }
            }
        }

        private void proxytool_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.proxytool.Text == "SSH")
            {
                IEnumerable<string> strs = (
                    from x in this.listssh
                    select x.country).Distinct<string>();
                this.comboBox5.Items.Clear();
                foreach (string str in strs)
                {
                    this.comboBox5.Items.Add(str);
                }
                if (this.comboBox5.Items.Count > 0)
                {
                    this.comboBox5.Text = this.comboBox5.Items[0].ToString();
                }
            }
            else if (this.proxytool.Text == "Vip72")
            {
                this.comboBox5.Items.Clear();
                foreach (countrycode _countrycode in this.listcountrycode)
                {
                    this.comboBox5.Items.Add(_countrycode.country);
                }
                this.comboBox5.SelectedIndex = 0;
            }
            else if (this.proxytool.Text == "Lumi")
            {
                this.comboBox5.Items.Clear();
                foreach (countrycode _countrycode in this.listcountrycode)
                {
                    this.comboBox5.Items.Add(_countrycode.country);
                }
                this.comboBox5.SelectedIndex = 0;
            }
            else if (this.proxytool.Text != "SSHServer")
            {
                this.comboBox5.Items.Clear();
            }
            else
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Concat(this.sshserverurl, "/Home/countrylist"));
                httpWebRequest.UserAgent = "autoleadios";
                try
                {
                    Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
                    string end = (new StreamReader(responseStream)).ReadToEnd();
                    string[] strArrays = end.Split(new string[] { "|" }, StringSplitOptions.None);
                    this.comboBox5.Items.Clear();
                    this.comboBox5.Items.AddRange(strArrays);
                    this.comboBox5.SelectedIndex = 0;
                }
                catch (Exception exception)
                {
                }
            }
            this.saveothersetting();
        }

        private void randomSelectRRS_CheckedChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            this.runtime = 0;
            this.backuptime = 0;
            this.runoftime.Text = "Run:0";
            this.backupoftime.Text = "Backup:0";
            this.backuprate.Text = "Backup Rate:0%";
            if (this.button7.Text == "STOP")
            {
                this.button7_Click(null, null);
                this.button7.Refresh();
            }
            if (this.button7.Text == "RESUME")
            {
                foreach (ListViewItem item in this.listView1.Items)
                {
                    item.SubItems[0].ResetStyle();
                    item.SubItems[1].ResetStyle();
                    item.SubItems[2].ResetStyle();
                    item.SubItems[3].ResetStyle();
                    item.SubItems[4].ResetStyle();
                    this.listView1.Refresh();
                }
                this.cmd.randomtouchstop();
                this.button7.Text = "START";
                this.button7.Refresh();
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
                            try
                            {
                                this.oThread.Abort();
                            }
                            catch (Exception exception)
                            {
                            }
                        }
                        catch (Exception exception1)
                        {
                        }
                    }
                }
                this.enableAll();
            }
        }

        private void rsswaitnum_ValueChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void safariX_ValueChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void safariY_ValueChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void sameVip_CheckedChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {
        }

        private void tabPageQuan4_Click(object sender, EventArgs e)
        {
        }

        private void tabPage6_MouseClick(object sender, MouseEventArgs e)
        {
            this.listView5.Hide();
            this.textBox5.Hide();
        }

        private void tabPage7_Click(object sender, EventArgs e)
        {
        }

        private void tabPage8_Click(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            this.saveothersetting();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox5.Text != "")
            {
                this.listView5.Clear();
                List<string> list = (
                    from x in this.listtimezone
                    where x.ToLower().Contains(this.textBox5.Text.ToLower())
                    select x).ToList<string>();
                foreach (string str in list)
                {
                    this.listView5.Items.Add(new ListViewItem(str));
                }
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (this.listView6.SelectedItems.Count > 0)
            {
                List<AutoLead.Script> list = (
                    from x in this.listscript
                    where (x.slot == this.listView7.SelectedItems[0].Index ? true : this.listView7.SelectedItems[0].Index == 0)
                    select x).ToList<AutoLead.Script>();
                list.ElementAt<AutoLead.Script>(this.listView6.SelectedItems[0].Index).script = this.textBox6.Text;
                this.savescripts();
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            if (this.listView6.SelectedItems.Count > 0)
            {
                this.listView6.SelectedItems[0].Text = this.textBox9.Text;
                List<AutoLead.Script> list = (
                    from x in this.listscript
                    where (x.slot == this.listView7.SelectedItems[0].Index ? true : this.listView7.SelectedItems[0].Index == 0)
                    select x).ToList<AutoLead.Script>();
                list.ElementAt<AutoLead.Script>(this.listView6.SelectedItems[0].Index).scriptname = this.textBox9.Text;
                this.savescripts();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.listView6.SelectedItems.Count > 0)
            {
                List<AutoLead.Script> list = (
                    from x in this.listscript
                    where (x.slot == this.listView7.SelectedItems[0].Index ? true : this.listView7.SelectedItems[0].Index == 0)
                    select x).ToList<AutoLead.Script>();
                list.ElementAt<AutoLead.Script>(this.listView6.SelectedItems[0].Index).slot = 0;
                foreach (ListViewItem item in this.listView7.Items)
                {
                    if (item.Text == ((ToolStripMenuItem)sender).Text)
                    {
                        list.ElementAt<AutoLead.Script>(this.listView6.SelectedItems[0].Index).slot = item.Index;
                        break;
                    }
                }
                if (this.listView7.SelectedItems.Count > 0)
                {
                    if ((this.listView7.SelectedItems[0].Index <= 0 ? false : this.listView7.SelectedItems[0].Text != ((ToolStripMenuItem)sender).Text))
                    {
                        this.listView6.SelectedItems[0].Remove();
                    }
                }
                this.savescripts();
            }
        }

        private void vipdelete_Click(object sender, EventArgs e)
        {
            if (this.listView3.SelectedItems.Count > 0)
            {
                for (int i = this.listView3.SelectedItems.Count - 1; i >= 0; i--)
                {
                    this.listvipacc.RemoveAt(this.listView3.SelectedItems[i].Index);
                    this.listView3.Items.Remove(this.listView3.SelectedItems[i]);
                }
            }
            this.savevip72();
        }

        private void lumidelete_Click(object sender, EventArgs e)
        {
            if (this.listViewQuan3.SelectedItems.Count > 0)
            {
                for (int i = this.listViewQuan3.SelectedItems.Count - 1; i >= 0; i--)
                {
                    this.listlumiacc.RemoveAt(this.listViewQuan3.SelectedItems[i].Index);
                    this.listViewQuan3.Items.Remove(this.listViewQuan3.SelectedItems[i]);
                }
            }
            this.savelumi();
        }

        private void wipecombo_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}