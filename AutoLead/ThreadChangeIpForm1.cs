using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace AutoLead
{
    public partial class Form1
    {
        private void threadchangeIP()
        {
            dynamic vip72Chung;
            object obj;
            object obj1;
            string text;
            vipaccount yellow;
            MethodInvoker methodInvoker = null;
            while (true)
            {
                Label2:
                text = "SSH";
                this.proxytool.Invoke(new MethodInvoker(() => text = this.proxytool.Text));
                Thread.Sleep(10);
                bool @checked = false;
                base.Invoke(new MethodInvoker(() => @checked = this.sameVip.Checked));
                if (@checked)
                {
                    vip72Chung = new Vip72Chung();
                }
                else
                {
                    vip72Chung = new Vip72();
                }

                if (text == "Lumi")
                {
                    try
                    {
                        vip72Chung.clearIpWithPort((int) this.numericUpDown1.Value);
                        sshcommand.closebitvise((int) this.numericUpDown1.Value);
                        Lumi.closeCCProxy();
                        if (!this.bitproc.HasExited)
                        {
                            this.bitproc.Kill();
                        }
                    }
                    catch (Exception exception)
                    {
                    }

                    this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "Checking Luminatio Account..."));
                    string str = "";
                    this.label1.Invoke(new MethodInvoker(() => str = this.comboBox5.Text));

                    this.label1.Invoke(new MethodInvoker(() =>
                        this.label1.Text = "Fake IP over CCProxy for country=" + str));

                    if (!Lumi.fake_proxy(str, this.ipAddressControl1.Text, this.numericUpDown1.Value.ToString(),
                        ref this.bitproc))
                    {
                        MessageBox.Show("Failed To change proxy with this Luminatio Account",
                            Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "Failed To change Luminatio"));
                        this.button7.Invoke(new MethodInvoker(() =>
                        {
                            if (this.button7.Text == "STOP")
                            {
                                this.button7_Click(null, null);
                            }

                            if (this.button19.Text == "STOP")
                            {
                                this.button19_Click(null, null);
                            }
                        }));
                        this.button20.Invoke(new MethodInvoker(() => this.button20.Enabled = true));
                        return;
                    }
                    else
                    {
                        obj = this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "IP changed..."));
                        obj1 = this.button20.Invoke(new MethodInvoker(() => this.button20.Enabled = true));
                        return;
                    }
                }


                if (text != "SSH")
                {
                    if (text != "Vip72")
                    {
                        break;
                    }

                    try
                    {
                        sshcommand.closebitvise((int) this.numericUpDown1.Value);
                        Lumi.closeCCProxy();
                        if (!this.bitproc.HasExited)
                        {
                            this.bitproc.Kill();
                        }
                    }
                    catch (Exception exception)
                    {
                    }

                    this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "Đang đợi Để sử dụng Vip72..."));
                    vip72Chung.waitiotherVIP72();
                    this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "Getting Vip72 IP..."));
                    yellow = this.listvipacc.FirstOrDefault<vipaccount>((vipaccount x) => !x.limited);
                    if (yellow != null)
                    {
                        int num1 = 0;
                        this.listView3.Invoke(new MethodInvoker(() =>
                        {
                            this.listView3.Items[this.listvipacc.IndexOf(yellow)].BackColor = Color.Yellow;
                            this.listView3.Refresh();
                        }));
                        while (true)
                        {
                            if (!(dynamic) (!vip72Chung.vip72login(yellow.username, yellow.password,
                                (int) this.numericUpDown1.Value)))
                            {
                                break;
                            }

                            num1++;
                            if (num1 > 0)
                            {
                                goto Label3;
                            }
                        }

                        this.listView3.Invoke(new MethodInvoker(() =>
                        {
                            this.listView3.Items[this.listvipacc.IndexOf(yellow)].BackColor = Color.Lime;
                            this.listView3.Refresh();
                        }));
                        while (true)
                        {
                            string str = "";
                            this.label1.Invoke(new MethodInvoker(() => str = this.comboBox5.Text));
                            if (!vip72Chung.getip(this.listcountrycode
                                .FirstOrDefault<countrycode>((countrycode x) => x.country == str).code))
                            {
                                break;
                            }

                            this.label1.Invoke(new MethodInvoker(() => { }));
                            string value = (string) vip72Chung.clickip((int) this.numericUpDown1.Value);
                            string str1 = value;
                            if (str1 == "not running")
                            {
                                goto Label2;
                            }
                            else if (str1 != "no IP")
                            {
                                if (str1 != "dead")
                                {
                                    if (str1 == "limited")
                                    {
                                        this.listView3.Invoke(new MethodInvoker(() =>
                                        {
                                            this.listView3.Items[this.listvipacc.IndexOf(yellow)].BackColor = Color.Red;
                                            this.listView3.Refresh();
                                        }));
                                        yellow.limited = true;
                                        goto Label2;
                                    }
                                    else if (str1 == "maximum")
                                    {
                                        vip72Chung.clearip();
                                    }
                                    else if (str1 == "timeout")
                                    {
                                        goto Label2;
                                    }
                                    else
                                    {
                                        this.curip = value;
                                        break;
                                    }
                                }
                            }
                        }

                        break;
                    }
                    else if (this.listvipacc.Count != 0)
                    {
                        foreach (vipaccount _vipaccount in this.listvipacc)
                        {
                            _vipaccount.limited = false;
                        }

                        this.button10.Invoke(new MethodInvoker(() => this.button20.Enabled = true));
                        return;
                    }
                    else
                    {
                        MessageBox.Show("There is no account, Please add other Vip72 account to use",
                            Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "Vip72 account is out"));
                        this.button7.Invoke(new MethodInvoker(() =>
                        {
                            if (this.button7.Text == "STOP")
                            {
                                this.button7_Click(null, null);
                            }

                            if (this.button19.Text == "STOP")
                            {
                                this.button19_Click(null, null);
                            }
                        }));
                        this.button20.Invoke(new MethodInvoker(() => this.button20.Enabled = true));
                        return;
                    }
                }
                else
                {
                    vip72Chung.clearIpWithPort((int) this.numericUpDown1.Value);
                    sshcommand.closebitvise((int) this.numericUpDown1.Value);
                    Lumi.closeCCProxy();
                    try
                    {
                        if (!this.bitproc.HasExited)
                        {
                            this.bitproc.Kill();
                        }
                    }
                    catch (Exception exception1)
                    {
                    }

                    while (true)
                    {
                        string text1 = "";
                        this.label1.Invoke(new MethodInvoker(() =>
                        {
                            text1 = this.comboBox5.Text;
                            this.label1.Text = "Checking SSH....";
                        }));
                        ssh aqua = this.listssh.FirstOrDefault<ssh>((ssh x) =>
                            (!(x.live != "dead") || x.used ? false : x.country == text1));
                        if (aqua == null)
                        {
                            MessageBox.Show("All SSH are used or dead, please update new SSH list!",
                                Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "SSH is out!"));
                            this.button7.Invoke(new MethodInvoker(() =>
                            {
                                if (this.button7.Text == "STOP")
                                {
                                    this.button7_Click(null, null);
                                }

                                if (this.button19.Text == "STOP")
                                {
                                    this.button19_Click(null, null);
                                }
                            }));
                            this.button20.Invoke(new MethodInvoker(() => this.button20.Enabled = true));
                            return;
                        }

                        Random random = new Random();
                        int num2 = random.Next(0, this.listssh.Count);
                        while (true)
                        {
                            if ((this.listssh.ElementAt<ssh>(num2).live == "dead" ||
                                 this.listssh.ElementAt<ssh>(num2).used
                                ? false
                                : this.listssh.ElementAt<ssh>(num2).country == text1))
                            {
                                break;
                            }

                            num2 = random.Next(0, this.listssh.Count);
                        }

                        aqua = this.listssh.ElementAt<ssh>(num2);
                        aqua.used = true;
                        this.listView2.Invoke(new MethodInvoker(() =>
                        {
                            this.listView2.Items[this.listssh.IndexOf(aqua)].BackColor = Color.Aqua;
                            this.listView2.Refresh();
                            this.savessh();
                            this.ssh_uncheck.Invoke(new MethodInvoker(() =>
                            {
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
                        this.curip = aqua.IP;
                        if (sshcommand.SetSSH(aqua.IP, aqua.username, aqua.password, this.ipAddressControl1.Text,
                            this.numericUpDown1.Value.ToString(), ref this.bitproc))
                        {
                            break;
                        }

                        aqua.live = "dead";
                        this.listView2.Invoke(new MethodInvoker(() =>
                        {
                            this.listView2.Items[this.listssh.IndexOf(aqua)].BackColor = Color.Red;
                            this.listView2.Refresh();
                            this.savessh();
                            this.ssh_uncheck.Invoke(new MethodInvoker(() =>
                            {
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

                    obj = this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "IP changed..."));
                    obj1 = this.button20.Invoke(new MethodInvoker(() => this.button20.Enabled = true));
                    return;
                }
            }

            if (text == "SSHServer")
            {
                bool flag = false;
                string str2 = "";
                vip72Chung.clearIpWithPort((int) this.numericUpDown1.Value);
                sshcommand.closebitvise((int) this.numericUpDown1.Value);
                Lumi.closeCCProxy();
                while (true)
                {
                    while (true)
                    {
                        Label label = this.label1;
                        MethodInvoker methodInvoker1 = methodInvoker;
                        if (methodInvoker1 == null)
                        {
                            MethodInvoker text2 = () =>
                            {
                                this.label1.Text = "Getting SSH from the server...";
                                //this.checktrung = this.checkBox18.Checked;
                                //this.offer_id = this.textBox11.Text;
                            };
                            MethodInvoker methodInvoker2 = text2;
                            methodInvoker = text2;
                            methodInvoker1 = methodInvoker2;
                        }

                        label.Invoke(methodInvoker1);
                        string text3 = "";
                        this.label1.Invoke(new MethodInvoker(() => text3 = this.comboBox5.Text));
                        string str3 = string.Concat(this.sshserverurl, "/Home/getssh?country=", text3);
                        if (flag)
                        {
                            str3 = string.Concat(str3, "&offerID=", str2);
                        }

                        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(str3);
                        httpWebRequest.UserAgent = "autoleadios";
                        try
                        {
                            StreamReader streamReader =
                                new StreamReader(httpWebRequest.GetResponse().GetResponseStream());
                            string end = streamReader.ReadToEnd();
                            if (end != "hetssh")
                            {
                                string[] strArrays = end.Split(new string[] {"|"}, StringSplitOptions.None);
                                if (strArrays.Count<string>() >= 4)
                                {
                                    this.curip = strArrays[1];
                                    if (!sshcommand.SetSSH(strArrays[1], strArrays[2], strArrays[3],
                                        this.ipAddressControl1.Text, this.numericUpDown1.Value.ToString(),
                                        ref this.bitproc))
                                    {
                                        string str4 = string.Concat(this.sshserverurl, "/Home/xoassh?ID=",
                                            strArrays[0]);
                                        HttpWebRequest httpWebRequest1 = (HttpWebRequest) WebRequest.Create(str4);
                                        httpWebRequest1.UserAgent = "autoleadios";
                                        try
                                        {
                                            StreamReader streamReader1 =
                                                new StreamReader(httpWebRequest1.GetResponse().GetResponseStream());
                                            streamReader.ReadToEnd();
                                        }
                                        catch (Exception exception2)
                                        {
                                        }

                                        break;
                                    }
                                    else
                                    {
                                        goto Label5;
                                    }
                                }
                            }
                            else
                            {
                                base.Invoke(new MethodInvoker(() =>
                                    this.label1.Text = "SSh trên server đã hết, đang đợi ssh mới ..."));
                                for (int i = 0; i < 10; i++)
                                {
                                    Thread.Sleep(1000);
                                    base.Invoke(new MethodInvoker(() =>
                                        this.label1.Text = string.Concat("Đợi để lấy SSH trên server trong ",
                                            (10 - i).ToString(), " giây")));
                                }
                            }
                        }
                        catch (Exception exception3)
                        {
                            break;
                        }
                    }
                }

                Label5:
                this.label1.Text = "SSh trên server đã hết, đang đợi ssh mới ...";
            }

            obj = this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "IP changed..."));
            obj1 = this.button20.Invoke(new MethodInvoker(() => this.button20.Enabled = true));
            return;
            Label3:
            yellow.limited = true;
            this.listView3.Invoke(new MethodInvoker(() =>
            {
                this.listView3.Items[this.listvipacc.IndexOf(this.vipacc)].BackColor = Color.Red;
                this.listView3.Refresh();
            }));
            //goto Label2;
            MessageBox.Show("No Lable2 to goto line 10572!", Application.ProductName, MessageBoxButtons.OK,
                MessageBoxIcon.Hand);
        }
    }
}