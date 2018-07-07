using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace AutoLead
{
    public partial class Form1
    {
        private void autoRRS()
        {
            dynamic vip72Chung;
            //Form1.DisplayClass156_5 variable;
            MethodInvoker methodInvoker = null;
            vipaccount yellow;
            luminatio_account yellow_lumi;
            bool @checked = false;
            Random random2 = new Random();
            List<backup>.Enumerator enumerator = this.listbackup.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    backup current = enumerator.Current;
                    bool flag = false;
                    backup _backup = current;
                    ListViewItem listViewItem = null;
                    this.listView4.Invoke(new MethodInvoker(() =>
                    {
                        foreach (ListViewItem item in this.listView4.Items)
                        {
                            if (item.SubItems[7].Text == current.filename)
                            {
                                listViewItem = item;
                                break;
                            }
                        }

                        flag = listViewItem.Checked;
                    }));
                    if ((!flag ? false : listViewItem != null))
                    {
                        base.Invoke(new MethodInvoker(() =>
                        {
                            //this.currentlistview.BackColor = Color.Yellow;
                            this.listView4.Refresh();
                        }));
                        string str = "";
                        List<string>.Enumerator enumerator1 = current.appList.GetEnumerator();
                        try
                        {
                            while (enumerator1.MoveNext())
                            {
                                str = string.Concat(str, enumerator1.Current);
                                str = string.Concat(str, ";");
                            }
                        }
                        finally
                        {
                            ((IDisposable) enumerator1).Dispose();
                        }

                        this.cmdResult.wipe = false;
                        this.cmd.close("all");
                        bool flag1 = false;
                        this.checkBox2.Invoke(new MethodInvoker(() =>
                        {
                            this.label1.Text = "Wiping Application data...";
                            if (this.checkBox2.Checked)
                            {
                                flag1 = true;
                            }
                        }));
                        this.cmd.faketype(this.getrandomdevice());
                        if (this.fakeversion.Checked)
                        {
                            if ((this.checkBox14.Checked ? true : this.checkBox15.Checked))
                            {
                                string str1 = "";
                                if ((!this.checkBox14.Checked ? true : !this.checkBox15.Checked))
                                {
                                    str1 = (!this.checkBox14.Checked ? "9" : "8");
                                }
                                else
                                {
                                    int num1 = (new Random()).Next(8, 10);
                                    str1 = num1.ToString();
                                }

                                this.cmd.fakeversion(str1);
                            }
                        }

                        this.cmd.randominfo();
                        if (!flag1)
                        {
                            this.cmd.wipe(str);
                        }
                        else
                        {
                            this.cmd.wipefull(str);
                        }

                        DateTime now = DateTime.Now;
                        this.button2.Invoke(new MethodInvoker(() => this.maxwait = (int) this.numericUpDown10.Value));
                        while (!this.cmdResult.wipe)
                        {
                            Thread.Sleep(500);
                            if ((DateTime.Now - now).TotalSeconds <= (double) this.maxwait)
                            {
                                this.cmd.checkwipe();
                            }
                            else
                            {
                                this.button2.Invoke(new MethodInvoker(() =>
                                {
                                    if (this.button2.Text == "Disconnect")
                                    {
                                        this.button2_Click(null, null);
                                    }
                                }));
                                return;
                            }
                        }

                        while (true)
                        {
                            Label3:
                            string text = "SSH";
                            this.proxytool.Invoke(new MethodInvoker(() => text = this.proxytool.Text));
                            Thread.Sleep(10);
                            if (text != "Direct")
                            {
                            }

                            bool checked1 = false;
                            base.Invoke(new MethodInvoker(() => checked1 = this.sameVip.Checked));
                            if (checked1)
                            {
                                vip72Chung = new Vip72Chung();
                            }
                            else
                            {
                                vip72Chung = new Vip72();
                            }

                            vip72Chung.waitiotherVIP72();

                            if (text == "Lumi")
                            {
                                try
                                {
                                    vip72Chung.clearIpWithPort((int)this.numericUpDown1.Value);
                                    sshcommand.closebitvise((int)this.numericUpDown1.Value);
                                    //Lumi.closeLuminatio((int)this.numericUpDown1.Value);
                                    if (!this.bitproc.HasExited)
                                    {
                                        this.bitproc.Kill();
                                    }
                                }
                                catch (Exception exception)
                                {
                                }

                                while (true)
                                {
                                    this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "Checking Luminatio Account..."));
                                    string str11 = "";
                                    this.label1.Invoke(new MethodInvoker(() => str = this.comboBox5.Text));

                                    this.label1.Invoke(new MethodInvoker(() =>
                                        this.label1.Text = "Fake IP over CCProxy for country=" + str11));

                                    yellow_lumi = this.listlumiacc.FirstOrDefault<luminatio_account>((luminatio_account x) => !x.bad);


                                    if (yellow_lumi != null)
                                    {
                                        this.listViewQuan3.Invoke(new MethodInvoker(() =>
                                        {
                                            this.listViewQuan3.Items[this.listlumiacc.IndexOf(yellow_lumi)].BackColor = Color.Yellow;
                                            this.listViewQuan3.Refresh();
                                        }));

                                        if (!(Lumi.fake_proxy_lumi(str11, this.numericUpDown1.Value.ToString(), yellow_lumi.zone, yellow_lumi.password, yellow_lumi.username)))

                                        {
                                            yellow_lumi.bad = true;
                                            this.listViewQuan3.Invoke(new MethodInvoker(() =>
                                            {
                                                this.listViewQuan3.Items[this.listlumiacc.IndexOf(yellow_lumi)].BackColor = Color.Red;
                                                this.listViewQuan3.Refresh();
                                            }));
                                            this.savelumi();

                                        }
                                        else {

                                            this.listViewQuan3.Invoke(new MethodInvoker(() =>
                                            {
                                                this.listViewQuan3.Items[this.listlumiacc.IndexOf(yellow_lumi)].BackColor = Color.Lime;
                                                this.listViewQuan3.Refresh();
                                            }));

                                            this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "IP changed..." + Lumi.getCurrentLumiIPVer2(this.ipAddressControl1.Text, this.numericUpDown1.Value)));
                                            this.button20.Invoke(new MethodInvoker(() => this.button20.Enabled = true));
                                            this.curip = Lumi.getCurrentLumiIPVer2(this.ipAddressControl1.Text, this.numericUpDown1.Value);
                                            this.savelumi();
                                            break;
                                        }
                                    }
                                    else
                                    {                                       
                                        MessageBox.Show("There is no account, Please add other Lumi account to use",
                                            Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                        this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "Lumi account is out"));
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
                                       
                                    }
                                }

                                goto Label2;


                            } else if (text == "SSH")
                            {
                                vip72Chung.clearIpWithPort((int) this.numericUpDown1.Value);
                                sshcommand.closebitvise((int) this.numericUpDown1.Value);
                                Lumi.closeLuminatio((int)this.numericUpDown1.Value);
                                try
                                {
                                    if (!this.bitproc.HasExited)
                                    {
                                        this.bitproc.Kill();
                                    }
                                }
                                catch (Exception exception)
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
                                    ssh red = this.listssh.FirstOrDefault<ssh>((ssh x) =>
                                        (!(x.live != "dead") || x.used ? false : x.country == text1));
                                    @checked = false;
                                    this.label1.Invoke(new MethodInvoker(() => @checked = this.checkBox17.Checked));
                                    if (red == null)
                                    {
                                        goto Label6;
                                    }

                                    Random random3 = new Random();
                                    int aqua = random3.Next(0, this.listssh.Count);
                                    while (true)
                                    {
                                        if ((this.listssh.ElementAt<ssh>(aqua).live == "dead" ||
                                             this.listssh.ElementAt<ssh>(aqua).used
                                            ? false
                                            : this.listssh.ElementAt<ssh>(aqua).country == text1))
                                        {
                                            break;
                                        }

                                        aqua = random3.Next(0, this.listssh.Count);
                                    }

                                    red = this.listssh.ElementAt<ssh>(aqua);
                                    red.used = true;
                                    this.curip = red.IP;
                                    this.listView2.Invoke(new MethodInvoker(() =>
                                    {
                                        this.listView2.Items[aqua].BackColor = Color.Aqua;
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
                                    this.curip = red.IP;
                                    if (sshcommand.SetSSH(red.IP, red.username, red.password,
                                        this.ipAddressControl1.Text, this.numericUpDown1.Value.ToString(),
                                        ref this.bitproc))
                                    {
                                        break;
                                    }

                                    red.live = "dead";
                                    this.listView2.Invoke(new MethodInvoker(() =>
                                    {
                                        this.listView2.Items[this.listssh.IndexOf(red)].BackColor = Color.Red;
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

                                goto Label2;
                            }
                            else if (text != "Vip72")
                            {
                                if (text == "SSHServer")
                                {
                                    bool flag2 = false;
                                    string str2 = "";
                                    vip72Chung.clearIpWithPort((int) this.numericUpDown1.Value);
                                    sshcommand.closebitvise((int) this.numericUpDown1.Value);
                                    Lumi.closeLuminatio((int)this.numericUpDown1.Value);
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
                                            string str3 = string.Concat(this.sshserverurl, "/Home/getssh?country=",
                                                text3);
                                            if (flag2)
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
                                                    string[] strArrays1 = end.Split(new string[] {"|"},
                                                        StringSplitOptions.None);
                                                    if (strArrays1.Count<string>() >= 4)
                                                    {
                                                        this.curip = strArrays1[1];
                                                        if (!sshcommand.SetSSH(strArrays1[1], strArrays1[2],
                                                            strArrays1[3], this.ipAddressControl1.Text,
                                                            this.numericUpDown1.Value.ToString(), ref this.bitproc))
                                                        {
                                                            string str4 = string.Concat(this.sshserverurl,
                                                                "/Home/xoassh?ID=", strArrays1[0]);
                                                            HttpWebRequest httpWebRequest1 =
                                                                (HttpWebRequest) WebRequest.Create(str4);
                                                            httpWebRequest1.UserAgent = "autoleadios";
                                                            try
                                                            {
                                                                StreamReader streamReader1 =
                                                                    new StreamReader(httpWebRequest1.GetResponse()
                                                                        .GetResponseStream());
                                                                streamReader.ReadToEnd();
                                                            }
                                                            catch (Exception exception1)
                                                            {
                                                            }

                                                            break;
                                                        }
                                                        else
                                                        {
                                                            goto Label4;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    base.Invoke(new MethodInvoker(() =>
                                                        this.label1.Text =
                                                            "SSh trên server đã hết, đang đợi ssh mới ..."));
                                                    for (int i = 0; i < 10; i++)
                                                    {
                                                        Thread.Sleep(1000);
                                                        base.Invoke(new MethodInvoker(() =>
                                                            this.label1.Text =
                                                                string.Concat("Đợi để lấy SSH trên server trong ",
                                                                    (10 - i).ToString(), " giây")));
                                                    }
                                                }
                                            }
                                            catch (Exception exception2)
                                            {
                                                break;
                                            }
                                        }
                                    }

                                    Label4:
                                    this.label1.Text = "Nothing in label 4....";
                                }

                                break;
                            }
                            else
                            {
                                try
                                {
                                    sshcommand.closebitvise((int) this.numericUpDown1.Value);
                                    Lumi.closeLuminatio((int)this.numericUpDown1.Value);
                                    if (!this.bitproc.HasExited)
                                    {
                                        this.bitproc.Kill();
                                    }
                                }
                                catch (Exception exception3)
                                {
                                }

                                this.label1.Invoke(new MethodInvoker(() =>
                                    this.label1.Text = "Đang đợi Để sử dụng Vip72..."));
                                vip72Chung.waitiotherVIP72();
                                this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "Getting Vip72 IP..."));
                                yellow = this.listvipacc.FirstOrDefault<vipaccount>((vipaccount x) => !x.limited);
                                if (yellow != null)
                                {
                                    int num2 = 0;
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

                                        num2++;
                                        if (num2 > 0)
                                        {
                                            goto Label5;
                                        }
                                    }

                                    this.listView3.Invoke(new MethodInvoker(() =>
                                    {
                                        this.listView3.Items[this.listvipacc.IndexOf(yellow)].BackColor = Color.Lime;
                                        this.listView3.Refresh();
                                    }));
                                    while (true)
                                    {
                                        string text4 = "";
                                        this.label1.Invoke(new MethodInvoker(() => text4 = this.comboBox5.Text));
                                        if (!vip72Chung.getip(this.listcountrycode
                                            .FirstOrDefault<countrycode>((countrycode x) => x.country == text4).code))
                                        {
                                            break;
                                        }

                                        this.label1.Invoke(new MethodInvoker(() => { }));
                                        string value = (string) vip72Chung.clickip((int) this.numericUpDown1.Value);
                                        string str5 = value;
                                        if (str5 == "not running")
                                        {
                                            goto Label3;
                                        }
                                        else if (str5 != "no IP")
                                        {
                                            if (str5 != "dead")
                                            {
                                                if (str5 == "limited")
                                                {
                                                    yellow.limited = true;
                                                    this.listView3.Invoke(new MethodInvoker(() =>
                                                    {
                                                        this.listView3.Items[this.listvipacc.IndexOf(yellow)]
                                                            .BackColor = Color.Red;
                                                        this.listView3.Refresh();
                                                    }));
                                                    goto Label3;
                                                }
                                                else if (str5 == "maximum")
                                                {
                                                    vip72Chung.clearip();
                                                }
                                                else if (str5 == "timeout")
                                                {
                                                    goto Label3;
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
                                    List<vipaccount>.Enumerator enumerator2 = this.listvipacc.GetEnumerator();
                                    try
                                    {
                                        while (enumerator2.MoveNext())
                                        {
                                            enumerator2.Current.limited = false;
                                        }
                                    }
                                    finally
                                    {
                                        ((IDisposable) enumerator2).Dispose();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(
                                        "All vip72 are limited or there is no account, Please add other Vip72 account to use",
                                        Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                    this.label1.Invoke(
                                        new MethodInvoker(() => this.label1.Text = "Vip72 account is out"));
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
                                }
                            }
                        }

                        Label2:
                        this.button2.Invoke(new MethodInvoker(() =>
                        {
                            if (!this.fakedevice.Checked)
                            {
                                this.cmd.changename("orig");
                            }

                            if ((!this.fakedevice.Checked || !this.checkBox11.Checked
                                ? false
                                : File.Exists(this.fileofname.Text)))
                            {
                                string[] strArrays = File.ReadAllLines(this.fileofname.Text);
                                Random random = new Random();
                                this.cmd.changename(strArrays[random.Next(0, strArrays.Count<string>())]);
                            }

                            if (this.checkBox20.Checked)
                            {
                                this.fakeLocationByIP(this.curip);
                            }

                            if (this.fakelang.Checked)
                            {
                                this.cmd.changelanguage(this.listlanguagecode
                                    .FirstOrDefault<languagecode>((languagecode x) => x.langname == this.comboBox1.Text)
                                    .langcode);
                            }

                            if (this.fakeregion.Checked)
                            {
                                this.cmd.changeregion(this.listcountrycodeiOS
                                    .FirstOrDefault<countrycodeiOS>((countrycodeiOS x) =>
                                        x.countryname == this.comboBox2.Text).countrycode);
                            }

                            if (!this.checkBox5.Checked)
                            {
                                this.cmd.changetimezone("orig");
                            }
                            else
                            {
                                this.cmd.changetimezone(this.ltimezone.Text);
                            }

                            if (!this.checkBox13.Checked)
                            {
                                this.cmd.changecarrier("orig", "orig", "orig", "orig");
                            }
                            else if (!this.checkBox9.Checked)
                            {
                                Random random1 = new Random();
                                Carrier carrier =
                                    this.carrierList.ElementAt<Carrier>(random1.Next(0, this.carrierList.Count));
                                this.cmd.changecarrier(carrier.CarrierName, carrier.mobileCountryCode,
                                    carrier.mobileCarrierCode, carrier.ISOCountryCode.ToLower());
                            }
                            else
                            {
                                List<Carrier> list = (
                                    from x in this.carrierList
                                    where x.country == this.carrierBox.Text
                                    select x).ToList<Carrier>();
                                Carrier carrier1 = list.ElementAt<Carrier>((new Random()).Next(0, list.Count));
                                this.cmd.changecarrier(carrier1.CarrierName, carrier1.mobileCountryCode,
                                    carrier1.mobileCarrierCode, carrier1.ISOCountryCode.ToLower());
                            }

                            if (!this.checkBox19.Checked)
                            {
                                this.cmd.fakeGPS(false);
                            }
                            else
                            {
                                this.cmd.fakeGPS(true, (double) ((double) this.latitude.Value),
                                    (double) ((double) this.longtitude.Value));
                            }
                        }));
                        this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "Restoring data...."));
                        this.cmdResult.restore = false;
                        this.cmd.restore(_backup.filename);
                        DateTime dateTime = DateTime.Now;
                        this.button2.Invoke(new MethodInvoker(() => this.maxwait = (int) this.numericUpDown10.Value));
                        while (!this.cmdResult.restore)
                        {
                            Thread.Sleep(500);
                            if ((DateTime.Now - dateTime).TotalSeconds <= (double) this.maxwait)
                            {
                                this.cmd.checkrestore();
                            }
                            else
                            {
                                this.button2.Invoke(new MethodInvoker(() =>
                                {
                                    if (this.button2.Text == "Disconnect")
                                    {
                                        this.button2_Click(null, null);
                                    }
                                }));
                                return;
                            }
                        }

                        Thread.Sleep(1000);
                        this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "Opening application...."));
                        dateTime = DateTime.Now;
                        List<string>.Enumerator enumerator3 = _backup.appList.GetEnumerator();
                        try
                        {
                            while (enumerator3.MoveNext())
                            {
                                string current1 = enumerator3.Current;
                                this.cmdResult.openApp = 0;
                                this.cmd.openApp(current1);
                                while (this.cmdResult.openApp != 0)
                                {
                                    Thread.Sleep(1000);
                                    if ((DateTime.Now - dateTime).TotalSeconds > (double) this.maxwait1)
                                    {
                                        this.button2.Invoke(new MethodInvoker(() =>
                                        {
                                            if (this.button2.Text == "Disconnect")
                                            {
                                                this.button2_Click(null, null);
                                            }
                                        }));
                                        return;
                                    }
                                }

                                int value1 = 20;
                                this.rsswaitnum.Invoke(new MethodInvoker(() => value1 = (int) this.rsswaitnum.Value));
                                for (int j = 0; j < value1; j++)
                                {
                                    Thread.Sleep(1000);
                                    this.label1.Invoke(new MethodInvoker(() =>
                                        this.label1.Text = string.Concat("Application will be closed in ",
                                            (value1 - j - 1).ToString(), " seconds")));
                                }

                                AutoLead.Script script = new AutoLead.Script();
                                this.label1.Invoke(new MethodInvoker(() =>
                                {
                                    if ((!this.checkBox6.Checked ? false : this.comboBox3.SelectedIndex != -1))
                                    {
                                        if (!this.checkBox7.Checked)
                                        {
                                            script = this.listscript.ElementAt<AutoLead.Script>(this.comboBox3
                                                .SelectedIndex);
                                        }
                                        else
                                        {
                                            List<AutoLead.Script> list = (
                                                from x in this.listscript
                                                where (x.slot == this.comboBox3.SelectedIndex
                                                    ? true
                                                    : this.comboBox3.SelectedIndex == 0)
                                                select x).ToList<AutoLead.Script>();
                                            script = list.ElementAt<AutoLead.Script>(
                                                (new Random()).Next(0, list.Count));
                                        }

                                        this.label1.Text = string.Concat("Running script ", script.scriptname);
                                    }
                                }));
                                this.excuteScript(script.script);
                            }
                        }
                        finally
                        {
                            ((IDisposable) enumerator3).Dispose();
                        }

                        base.Invoke(new MethodInvoker(() =>
                        {
                            this.listView4.SelectedIndices.Clear();
                            //this.currentlistview.Selected = true;
                        }));
                        this.saverrsthread(listViewItem);
                        this.listView4.Invoke(new MethodInvoker(() =>
                        {
                            //this.currentlistview.BackColor = Color.Lime;
                            //this.currentlistview.Checked = false;
                            this.listView4.Refresh();
                        }));
                    }
                }

                this.label1.Invoke(new MethodInvoker(() =>
                {
                    this.label1.Text = "RRS done.";
                    this.button19.Text = "START";
                    this.button19.Refresh();
                    this.rssenableall();
                    this.enableAll();
                }));
                return;
            }
            finally
            {
                ((IDisposable) enumerator).Dispose();
            }

            return;
            Label5:
            yellow.limited = true;
            this.listView3.Invoke(new MethodInvoker(() =>
            {
                this.listView3.Items[this.listvipacc.IndexOf(this.vipacc)].BackColor = Color.Red;
                this.listView3.Refresh();
            }));
            //goto Label3;
            MessageBox.Show("No Lable3 to goto line 2974!", Application.ProductName, MessageBoxButtons.OK,
                MessageBoxIcon.Hand);

            Label6:
            if (@checked)
            {
                this.label1.Invoke(new MethodInvoker(() => this.button24_Click(null, null)));
            }
            else
            {
                MessageBox.Show("All SSH are used or dead, please update new SSH list!", Application.ProductName,
                    MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.label1.Invoke(new MethodInvoker(() => this.button7_Click(null, null)));
            }

            //goto Label3;
            MessageBox.Show("No Lable3 to goto line 2987!", Application.ProductName, MessageBoxButtons.OK,
                MessageBoxIcon.Hand);
        }
    }
}