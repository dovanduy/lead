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
        private void autoLeadThread()
        {
            dynamic vip72Chung;
            DateTime now;
            MethodInvoker methodInvoker;
            Uri uri;
            string value;
            string str;
            bool flag;
            bool flag1;
            bool @checked;
            MethodInvoker methodInvoker1 = null;
            MethodInvoker methodInvoker2 = null;
            MethodInvoker methodInvoker3 = null;
            bool checked1;
            MethodInvoker methodInvoker4 = null;
            MethodInvoker methodInvoker5 = null;
            bool checked2;
        Label0:
            this.curip = "";
            if (this.offerListItem.FirstOrDefault<offerItem>((offerItem x) => x.offerEnable) != null)
            {
                string text = "SSH";
                Label2:
                while (true)
                {
                    this.proxytool.Invoke(new MethodInvoker(() => text = this.proxytool.Text));
                    Thread.Sleep(10);
                    @checked = false;
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
                        vip72Chung.clearIpWithPort((int)this.numericUpDown1.Value);
                        sshcommand.closebitvise((int)this.numericUpDown1.Value);
                        Lumi.closeCCProxy();
                        while (true)
                        {
                            string text55 = "";
                            this.label1.Invoke(new MethodInvoker(() => text55 = this.comboBox5.Text));

                            this.label1.Invoke(new MethodInvoker(() =>
                                this.label1.Text = "Fake IP over CCProxy for country=" + text55));

                            if (!Lumi.fake_proxy(text55, this.ipAddressControl1.Text, this.numericUpDown1.Value.ToString(),
                                ref this.bitproc))
                            {
                                MessageBox.Show("Failed To change proxy with this Luminatio Account",
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                goto Label0;
                            }
                            else
                            {
                                break;
                            }

                        }
                    }
                    else if (text == "SSH")
                    {
                        vip72Chung.clearIpWithPort((int)this.numericUpDown1.Value);
                        sshcommand.closebitvise((int)this.numericUpDown1.Value);
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
                            this.label1.Invoke(new MethodInvoker(() => {
                                this.label1.Text = "Checking SSH....";
                                text1 = this.comboBox5.Text;
                            }));
                            this._getssh = this.listssh.FirstOrDefault<ssh>((ssh x) => (!(x.live != "dead") || x.used ? false : x.country == text1));
                            checked1 = false;
                            this.label1.Invoke(new MethodInvoker(() => checked1 = this.checkBox17.Checked));
                            if (this._getssh == null)
                            {
                                goto Label5;
                            }
                            Random random2 = new Random();
                            int num1 = random2.Next(0, this.listssh.Count);
                            while (true)
                            {
                                if ((this.listssh.ElementAt<ssh>(num1).live == "dead" || this.listssh.ElementAt<ssh>(num1).used ? false : this.listssh.ElementAt<ssh>(num1).country == text1))
                                {
                                    break;
                                }
                                num1 = random2.Next(0, this.listssh.Count);
                            }
                            this._getssh = this.listssh.ElementAt<ssh>(num1);
                            this._getssh.used = true;
                            this.listView2.Invoke(new MethodInvoker(() => {
                                this.listView2.Items[this.listssh.IndexOf(this._getssh)].BackColor = Color.Aqua;
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
                            this.curip = this._getssh.IP;
                            if (sshcommand.SetSSH(this._getssh.IP, this._getssh.username, this._getssh.password, this.ipAddressControl1.Text, this.numericUpDown1.Value.ToString(), ref this.bitproc))
                            {
                                break;
                            }
                            this._getssh.live = "dead";
                            this.listView2.Invoke(new MethodInvoker(() => {
                                try
                                {
                                    this.listView2.Items[this.listssh.IndexOf(this._getssh)].BackColor = Color.Red;
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
                                }
                                catch (Exception exception)
                                {
                                }
                            }));
                        }
                        goto Label1;
                    }
                    else if (text != "Vip72")
                    {
                        if (text != "SSHServer")
                        {
                            this.curip = "";
                        }
                        else
                        {
                            bool flag2 = false;
                            string str1 = "";
                            vip72Chung.clearIpWithPort((int)this.numericUpDown1.Value);
                            sshcommand.closebitvise((int)this.numericUpDown1.Value);
                            Lumi.closeCCProxy();
                            while (true)
                            {
                                while (true)
                                {
                                    Label label = this.label1;
                                    MethodInvoker methodInvoker6 = methodInvoker3;
                                    if (methodInvoker6 == null)
                                    {
                                        MethodInvoker text2 = () =>
                                        {
                                            this.label1.Text = "Getting SSH from the server...";
                                            //this.checktrung = this.checkBox18.Checked;
                                            //this.offer_id = this.textBox11.Text;
                                        };
                                        methodInvoker = text2;
                                        methodInvoker3 = text2;
                                        methodInvoker6 = methodInvoker;
                                    }
                                    label.Invoke(methodInvoker6);
                                    string str2 = "";
                                    this.label1.Invoke(new MethodInvoker(() => str2 = this.comboBox5.Text));
                                    string str3 = string.Concat(this.sshserverurl, "/Home/getssh?country=", str2);
                                    if (flag2)
                                    {
                                        str3 = string.Concat(str3, "&offerID=", str1);
                                    }
                                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(str3);
                                    httpWebRequest.UserAgent = "autoleadios";
                                    try
                                    {
                                        StreamReader streamReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream());
                                        string end = streamReader.ReadToEnd();
                                        if (end != "hetssh")
                                        {
                                            string[] strArrays1 = end.Split(new string[] { "|" }, StringSplitOptions.None);
                                            if (strArrays1.Count<string>() >= 4)
                                            {
                                                this.curip = strArrays1[1];
                                                if (!sshcommand.SetSSH(strArrays1[1], strArrays1[2], strArrays1[3], this.ipAddressControl1.Text, this.numericUpDown1.Value.ToString(), ref this.bitproc))
                                                {
                                                    string str4 = string.Concat(this.sshserverurl, "/Home/xoassh?ID=", strArrays1[0]);
                                                    HttpWebRequest httpWebRequest1 = (HttpWebRequest)WebRequest.Create(str4);
                                                    httpWebRequest1.UserAgent = "autoleadios";
                                                    try
                                                    {
                                                        StreamReader streamReader1 = new StreamReader(httpWebRequest1.GetResponse().GetResponseStream());
                                                        streamReader.ReadToEnd();
                                                    }
                                                    catch (Exception exception2)
                                                    {
                                                    }
                                                    break;
                                                }
                                                else
                                                {
                                                    goto Label3;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            base.Invoke(new MethodInvoker(() => this.label1.Text = "SSh trên server đã hết, đang đợi ssh mới ..."));
                                            for (int i = 0; i < 10; i++)
                                            {
                                                Thread.Sleep(1000);
                                                base.Invoke(new MethodInvoker(() => this.label1.Text = string.Concat("Đợi để lấy SSH trên server trong ", (10 - i).ToString(), " giây")));
                                            }
                                        }
                                    }
                                    catch (Exception exception3)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        Label3:
                        break;
                    }
                    else
                    {
                        try
                        {
                            sshcommand.closebitvise((int)this.numericUpDown1.Value);
                            Lumi.closeCCProxy();
                            if (!this.bitproc.HasExited)
                            {
                                this.bitproc.Kill();
                            }
                        }
                        catch (Exception exception4)
                        {
                        }
                        this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "Đang đợi Để sử dụng Vip72..."));
                        vip72Chung.waitiotherVIP72();
                        this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "Getting Vip72 IP..."));
                        this.vipacc = this.listvipacc.FirstOrDefault<vipaccount>((vipaccount x) => !x.limited);
                        if (this.vipacc != null)
                        {
                            int num2 = 0;
                            this.listView3.Invoke(new MethodInvoker(() => {
                                this.listView3.Items[this.listvipacc.IndexOf(this.vipacc)].BackColor = Color.Yellow;
                                this.listView3.Refresh();
                            }));
                            while (true)
                            {
                                if (!(dynamic)(!vip72Chung.vip72login(this.vipacc.username, this.vipacc.password, (int)this.numericUpDown1.Value)))
                                {
                                    break;
                                }
                                num2++;
                                if (num2 > 0)
                                {
                                    goto Label4;
                                }
                            }
                            this.listView3.Invoke(new MethodInvoker(() => {
                                this.listView3.Items[this.listvipacc.IndexOf(this.vipacc)].BackColor = Color.Lime;
                                this.listView3.Refresh();
                            }));
                            while (true)
                            {
                                string text3 = "";
                                this.label1.Invoke(new MethodInvoker(() => text3 = this.comboBox5.Text));
                                if (!vip72Chung.getip(this.listcountrycode.FirstOrDefault<countrycode>((countrycode x) => x.country == text3).code))
                                {
                                    break;
                                }
                                this.label1.Invoke(new MethodInvoker(() => {
                                }));
                                string value1 = (string)vip72Chung.clickip((int)this.numericUpDown1.Value);
                                string str5 = value1;
                                if (str5 == "not running")
                                {
                                    goto Label2;
                                }
                                else if (str5 != "no IP")
                                {
                                    if (str5 != "dead")
                                    {
                                        if (str5 == "limited")
                                        {
                                            this.listView3.Invoke(new MethodInvoker(() => {
                                                this.listView3.Items[this.listvipacc.IndexOf(this.vipacc)].BackColor = Color.Red;
                                                this.listView3.Refresh();
                                            }));
                                            this.vipacc.limited = true;
                                            goto Label2;
                                        }
                                        else if (str5 == "maximum")
                                        {
                                            vip72Chung.clearip();
                                        }
                                        else if (str5 == "timeout")
                                        {
                                            goto Label2;
                                        }
                                        else
                                        {
                                            this.currentvipip = value1;
                                            this.curip = this.currentvipip;
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
                        }
                        else
                        {
                            MessageBox.Show("There is no account, Please add other Vip72 account to use", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "Vip72 account is out"));
                            this.button7.Invoke(new MethodInvoker(() => {
                                if (this.button7.Text == "STOP")
                                {
                                    this.button7_Click(null, null);
                                }
                                if (this.button19.Text == "STOP")
                                {
                                    this.button19_Click(null, null);
                                }
                            }));
                            goto Label0;
                        }
                    }
                }
            Label1:
                flag = (text == "Direct" ? false : text != "SSHServer");
                if (flag)
                {
                }
                this.cmd.close("all");
                Thread.Sleep(1000);
                this.cmd.sendtext("{HOME}");
                string str6 = "";
                foreach (offerItem _offerItem in this.offerListItem)
                {
                    if (_offerItem.offerEnable)
                    {
                        str6 = string.Concat(str6, _offerItem.appID);
                        str6 = string.Concat(str6, ";");
                    }
                }
                this.cmdResult.wipe = false;
                bool flag3 = false;
                this.label1.Invoke(new MethodInvoker(() => {
                    this.label1.Text = "Wiping Application data...";
                    if (this.checkBox2.Checked)
                    {
                        flag3 = true;
                    }
                }));
                this.cmd.faketype(this.getrandomdevice());
                if (this.fakeversion.Checked)
                {
                    if ((this.checkBox14.Checked ? true : this.checkBox15.Checked))
                    {
                        string str7 = "";
                        if ((!this.checkBox14.Checked ? true : !this.checkBox15.Checked))
                        {
                            str7 = (!this.checkBox14.Checked ? "9" : "8");
                        }
                        else
                        {
                            int num3 = (new Random()).Next(8, 10);
                            str7 = num3.ToString();
                        }
                        this.cmd.fakeversion(str7);
                    }
                }
                this.cmd.randominfo();
                this.cmd.wipe(str6);
                if (flag3)
                {
                    foreach (offerItem _offerItem1 in this.offerListItem)
                    {
                        if (_offerItem1.offerEnable)
                        {
                            this.cmd.uninstallapp(_offerItem1.appID);
                        }
                    }
                }
                this.button2.Invoke(new MethodInvoker(() => this.maxwait = (int)this.numericUpDown10.Value));
                now = DateTime.Now;
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
                            if (!this.fakedevice.Checked)
                            {
                                this.cmd.changename("orig");
                            }
                            if ((!this.fakedevice.Checked || !this.checkBox11.Checked ? false : File.Exists(this.fileofname.Text)))
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
                                this.cmd.changelanguage(this.listlanguagecode.FirstOrDefault<languagecode>((languagecode x) => x.langname == this.comboBox1.Text).langcode);
                            }
                            if (this.fakeregion.Checked)
                            {
                                this.cmd.changeregion(this.listcountrycodeiOS.FirstOrDefault<countrycodeiOS>((countrycodeiOS x) => x.countryname == this.comboBox2.Text).countrycode);
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
                        List<offerItem>.Enumerator enumerator = this.offerListItem.GetEnumerator();
                        try
                        {
                        Label7:
                            
                            if (enumerator.MoveNext())
                            {
                                offerItem current = enumerator.Current;
                                int num4 = 0;
                                Label8:
                                num4++;
                                this.label1.Invoke(new MethodInvoker(() => {
                                    if (this.checkBox4.Checked)
                                    {
                                        if (num4 > (int)this.numericUpDown5.Value)
                                        {
                                            current.offerEnable = false;
                                            this.listView1.Items[this.offerListItem.IndexOf(current)].Checked = false;
                                            this.listView1.Refresh();
                                        }
                                    }
                                }));
                                if (current.offerEnable)
                                {
                                    int lime = this.offerListItem.IndexOf(current);
                                    this.label1.Invoke(new MethodInvoker(() => {
                                        this.label1.Text = string.Concat("Running :", this.listView1.Items[lime].Text);
                                        this.listView1.Items[lime].UseItemStyleForSubItems = false;
                                        this.listView1.Items[lime].SubItems[0].BackColor = Color.Lime;
                                        this.listView1.Items[lime].SubItems[1].BackColor = Color.Yellow;
                                        this.listView1.Refresh();
                                    }));
                                    string str8 = "";
                                    this.label1.Invoke(new MethodInvoker(() => {
                                        this.label1.Text = "Opening Offer URL....";
                                        try
                                        {
                                            if (text == "Vip72")
                                            {
                                                str8 = string.Concat(str8, "Vip72||");
                                                str8 = string.Concat(str8, this.comboBox5.Text, "||");
                                                str8 = string.Concat(str8, this.currentvipip, "||");
                                                str8 = string.Concat(str8, this.vipacc.username, "||");
                                                str8 = string.Concat(str8, this.vipacc.password, "||");
                                            }
                                            if (text == "SSH")
                                            {
                                                str8 = string.Concat(str8, "SSH||");
                                                str8 = string.Concat(str8, this.comboBox5.Text, "||");
                                                str8 = string.Concat(str8, this._getssh.IP, "||");
                                                str8 = string.Concat(str8, this._getssh.username, "||");
                                                str8 = string.Concat(str8, this._getssh.password, "||");
                                            }
                                            if (text == "Direct")
                                            {
                                                str8 = string.Concat(str8, "Direct||");
                                            }
                                            if (text == "Lumi")
                                            {
                                                str8 = string.Concat(str8, "Lumi||");
                                            }
                                        }
                                        catch (Exception exception)
                                        {
                                        }
                                    }));
                                    this.cmdResult.openURL = false;
                                    str8 = string.Concat(str8, this.anaURL(current.offerURL), "||");
                                    str8 = string.Concat(str8, current.appID);
                                    string[] strArrays2 = current.referer.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                                    if (strArrays2.Count<string>() <= 0)
                                    {
                                        this.cmd.setReferer("");
                                    }
                                    else
                                    {
                                        if (!Uri.TryCreate(strArrays2[(new Random()).Next(0, strArrays2.Count<string>())], UriKind.Absolute, out uri))
                                        {
                                            flag1 = false;
                                        }
                                        else
                                        {
                                            flag1 = (uri.Scheme == Uri.UriSchemeHttp ? true : uri.Scheme == Uri.UriSchemeHttps);
                                        }
                                        if (!flag1)
                                        {
                                            this.cmd.setReferer("");
                                        }
                                        else
                                        {
                                            this.cmd.setReferer(uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped));
                                        }
                                        this.cmd.openURL(this.anaURL(current.offerURL));
                                    }
                                    DateTime dateTime = DateTime.Now;
                                    DateTime now1 = DateTime.Now;
                                    while (!this.cmdResult.openURL)
                                    {
                                        if ((DateTime.Now - now1).Seconds > 5)
                                        {
                                            this.cmd.openURL(this.anaURL(current.offerURL));
                                            now1 = DateTime.Now;
                                        }
                                        Thread.Sleep(100);
                                        if ((DateTime.Now - dateTime).TotalSeconds > (double)this.maxwait1)
                                        {
                                            this.button2.Invoke(new MethodInvoker(() => {
                                                if (this.button2.Text == "Disconnect")
                                                {
                                                    this.button2_Click(null, null);
                                                }
                                            }));
                                            return;
                                        }
                                    }
                                    this.listView1.Invoke(new MethodInvoker(() => {
                                        this.listView1.Items[lime].SubItems[1].BackColor = Color.Lime;
                                        this.listView1.Items[lime].SubItems[2].BackColor = Color.Yellow;
                                        this.listView1.Refresh();
                                        this.maxwait1 = (int)this.numericUpDown4.Value;
                                    }));
                                    this.cmdResult.frontAppByID = "";
                                    this.cmd.getfront();
                                    dateTime = DateTime.Now;
                                    while (true)
                                    {
                                        if ((this.cmdResult.frontAppByID == "" ? false : this.cmdResult.frontAppByID != "com.apple.mobilesafari"))
                                        {
                                            break;
                                        }
                                        if (this.safariX.Value != decimal.MinusOne)
                                        {
                                            this.cmd.touch((double)((double)this.itunesX.Value), (double)((double)this.itunesY.Value));
                                            Thread.Sleep(500);
                                            this.cmd.touch((double)((double)this.safariX.Value), (double)((double)this.safariY.Value));
                                        }
                                        if (this.cmdResult.frontAppByID != current.appID)
                                        {
                                            Thread.Sleep(500);
                                            if ((DateTime.Now - dateTime).TotalSeconds > (double)this.maxwait1)
                                            {
                                                if (this.cmdResult.frontAppByID == "")
                                                {
                                                    this.button2.Invoke(new MethodInvoker(() => {
                                                        if (this.button2.Text == "Disconnect")
                                                        {
                                                            this.button2_Click(null, null);
                                                        }
                                                    }));
                                                    return;
                                                }
                                                else if (this.cmdResult.frontAppByID != "")
                                                {
                                                    while (true)
                                                    {
                                                    Label15:
                                                        ComboBox comboBox = this.proxytool;
                                                        MethodInvoker methodInvoker7 = methodInvoker1;
                                                        if (methodInvoker7 == null)
                                                        {
                                                            MethodInvoker text4 = () => this.proxytype = this.proxytool.Text;
                                                            methodInvoker = text4;
                                                            methodInvoker1 = text4;
                                                            methodInvoker7 = methodInvoker;
                                                        }
                                                        comboBox.Invoke(methodInvoker7);
                                                        Thread.Sleep(10);
                                                        MethodInvoker methodInvoker8 = methodInvoker2;
                                                        if (methodInvoker8 == null)
                                                        {
                                                            MethodInvoker checked3 = () => this.svip = this.sameVip.Checked;
                                                            methodInvoker = checked3;
                                                            methodInvoker2 = checked3;
                                                            methodInvoker8 = methodInvoker;
                                                        }
                                                        base.Invoke(methodInvoker8);
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
                                                            vip72Chung.clearIpWithPort((int)this.numericUpDown1.Value);
                                                            sshcommand.closebitvise((int)this.numericUpDown1.Value);
                                                            Lumi.closeCCProxy();
                                                            while (true)
                                                            {
                                                                string text55 = "";
                                                                this.label1.Invoke(new MethodInvoker(() => text55 = this.comboBox5.Text));

                                                                this.label1.Invoke(new MethodInvoker(() =>
                                                                    this.label1.Text = "Fake IP over CCProxy for country=" + text55));

                                                                if (!Lumi.fake_proxy(text55, this.ipAddressControl1.Text, this.numericUpDown1.Value.ToString(),
                                                                    ref this.bitproc))
                                                                {
                                                                    MessageBox.Show("Failed To change proxy with this Luminatio Account",
                                                                        Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                                                    goto Label0;
                                                                }
                                                                else
                                                                {
                                                                    break;
                                                                }

                                                            }
                                                            Thread.Sleep(1000);
                                                            this.cmd.sendtext("{HOME}");
                                                            this.cmd.wipe("com.apple.mobilesafari");
                                                            this.button2.Invoke(new MethodInvoker(() => this.maxwait = (int)this.numericUpDown10.Value));
                                                            now = DateTime.Now;
                                                            while (!this.cmdResult.wipe)
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
                                                                    return;
                                                                }
                                                            }
                                                            goto Label8;
                                                        }
                                                        else if (text == "SSH")
                                                        {
                                                            vip72Chung.clearIpWithPort((int)this.numericUpDown1.Value);
                                                            sshcommand.closebitvise((int)this.numericUpDown1.Value);
                                                            Lumi.closeCCProxy();
                                                            while (true)
                                                            {
                                                                string text5 = "";
                                                                this.label1.Invoke(new MethodInvoker(() => text5 = this.comboBox5.Text));
                                                                this._getssh = this.listssh.FirstOrDefault<ssh>((ssh x) => (!(x.live != "dead") || x.used ? false : x.country == text5));
                                                                checked2 = false;
                                                                this.label1.Invoke(new MethodInvoker(() => checked2 = this.checkBox17.Checked));
                                                                if (this._getssh == null)
                                                                {
                                                                    goto Label14;
                                                                }
                                                                Random random3 = new Random();
                                                                int num5 = random3.Next(0, this.listssh.Count);
                                                                while (true)
                                                                {
                                                                    if ((this.listssh.ElementAt<ssh>(num5).live == "dead" || this.listssh.ElementAt<ssh>(num5).used ? false : this.listssh.ElementAt<ssh>(num5).country == text5))
                                                                    {
                                                                        break;
                                                                    }
                                                                    num5 = random3.Next(0, this.listssh.Count);
                                                                }
                                                                this._getssh = this.listssh.ElementAt<ssh>(num5);
                                                                this._getssh.used = true;
                                                                this.listView2.Invoke(new MethodInvoker(() => {
                                                                    this.listView2.Items[this.listssh.IndexOf(this._getssh)].BackColor = Color.Aqua;
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
                                                                try
                                                                {
                                                                    sshcommand.closebitvise((int)this.numericUpDown1.Value);
                                                                    if (!this.bitproc.HasExited)
                                                                    {
                                                                        this.bitproc.Kill();
                                                                    }
                                                                }
                                                                catch (Exception exception5)
                                                                {
                                                                }
                                                                this.curip = this._getssh.IP;
                                                                if (sshcommand.SetSSH(this._getssh.IP, this._getssh.username, this._getssh.password, this.ipAddressControl1.Text, this.numericUpDown1.Value.ToString(), ref this.bitproc))
                                                                {
                                                                    break;
                                                                }
                                                                this._getssh.live = "dead";
                                                                this.listView2.Invoke(new MethodInvoker(() => {
                                                                    this.listView2.Items[this.listssh.IndexOf(this._getssh)].BackColor = Color.Red;
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
                                                            Thread.Sleep(1000);
                                                            this.cmd.sendtext("{HOME}");
                                                            this.cmd.wipe("com.apple.mobilesafari");
                                                            this.button2.Invoke(new MethodInvoker(() => this.maxwait = (int)this.numericUpDown10.Value));
                                                            now = DateTime.Now;
                                                            while (!this.cmdResult.wipe)
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
                                                                    return;
                                                                }
                                                            }
                                                            goto Label8;
                                                        }
                                                        else if (text == "Vip72")
                                                        {
                                                            try
                                                            {
                                                                sshcommand.closebitvise((int)this.numericUpDown1.Value);
                                                                Lumi.closeCCProxy();
                                                                if (!this.bitproc.HasExited)
                                                                {
                                                                    this.bitproc.Kill();
                                                                }
                                                            }
                                                            catch (Exception exception6)
                                                            {
                                                            }
                                                            this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "Đang đợi Để sử dụng Vip72..."));
                                                            vip72Chung.waitiotherVIP72();
                                                            this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "Getting Vip72 IP...."));
                                                            this.vipacc = this.listvipacc.FirstOrDefault<vipaccount>((vipaccount x) => !x.limited);
                                                            if (this.vipacc != null)
                                                            {
                                                                this.listView3.Invoke(new MethodInvoker(() => {
                                                                    this.listView3.Items[this.listvipacc.IndexOf(this.vipacc)].BackColor = Color.Yellow;
                                                                    this.listView3.Refresh();
                                                                }));
                                                                int num6 = 0;
                                                                while (true)
                                                                {
                                                                    if (!(dynamic)(!vip72Chung.vip72login(this.vipacc.username, this.vipacc.password, (int)this.numericUpDown1.Value)))
                                                                    {
                                                                        break;
                                                                    }
                                                                    num6++;
                                                                    if (num6 > 0)
                                                                    {
                                                                        goto Label10;
                                                                    }
                                                                }
                                                                this.listView3.Invoke(new MethodInvoker(() => {
                                                                    this.listView3.Items[this.listvipacc.IndexOf(this.vipacc)].BackColor = Color.Lime;
                                                                    this.listView3.Refresh();
                                                                }));
                                                                while (true)
                                                                {
                                                                    string text6 = "";
                                                                    this.label1.Invoke(new MethodInvoker(() => text6 = this.comboBox5.Text));
                                                                    if (!vip72Chung.getip(this.listcountrycode.FirstOrDefault<countrycode>((countrycode x) => x.country == text6).code))
                                                                    {
                                                                        break;
                                                                    }
                                                                    this.label1.Invoke(new MethodInvoker(() => {
                                                                    }));
                                                                    value = (string)vip72Chung.clickip((int)this.numericUpDown1.Value);
                                                                    str = value;
                                                                    if (str == "not running")
                                                                    {
                                                                        goto Label15;
                                                                    }
                                                                    if (str != "no IP")
                                                                    {
                                                                        if (str != "dead")
                                                                        {
                                                                            if (str == "limited")
                                                                            {
                                                                                goto Label12;
                                                                            }
                                                                            if (str != "maximum")
                                                                            {
                                                                                goto Label13;
                                                                            }
                                                                            vip72Chung.clearip();
                                                                        }
                                                                    }
                                                                }
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                if (this.listvipacc.Count != 0)
                                                                {
                                                                    foreach (vipaccount _vipaccount1 in this.listvipacc)
                                                                    {
                                                                        _vipaccount1.limited = false;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    MessageBox.Show("All vip72 are limited or there is no account, Please add other Vip72 account to use", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                                                    this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "Vip72 account is out"));
                                                                    this.button7.Invoke(new MethodInvoker(() => {
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
                                                                Thread.Sleep(1000);
                                                                this.cmd.sendtext("{HOME}");
                                                                this.cmd.wipe("com.apple.mobilesafari");
                                                                this.button2.Invoke(new MethodInvoker(() => this.maxwait = (int)this.numericUpDown10.Value));
                                                                now = DateTime.Now;
                                                                while (!this.cmdResult.wipe)
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
                                                                        return;
                                                                    }
                                                                }
                                                                goto Label8;
                                                            }
                                                        }
                                                        else if (text != "SSHServer")
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            bool flag4 = false;
                                                            string str9 = "";
                                                            vip72Chung.clearIpWithPort((int)this.numericUpDown1.Value);
                                                            sshcommand.closebitvise((int)this.numericUpDown1.Value);
                                                            Lumi.closeCCProxy();
                                                            while (true)
                                                            {
                                                                while (true)
                                                                {
                                                                    Label label1 = this.label1;
                                                                    MethodInvoker methodInvoker9 = methodInvoker5;
                                                                    if (methodInvoker9 == null)
                                                                    {
                                                                        MethodInvoker text7 = () =>
                                                                        {
                                                                            this.label1.Text = "Getting SSH from the server...";
                                                                            //this.checktrung = this.checkBox18.Checked;
                                                                            //this.offer_id = this.textBox11.Text;
                                                                        };
                                                                        methodInvoker = text7;
                                                                        methodInvoker5 = text7;
                                                                        methodInvoker9 = methodInvoker;
                                                                    }
                                                                    label1.Invoke(methodInvoker9);
                                                                    string text8 = "";
                                                                    this.label1.Invoke(new MethodInvoker(() => text8 = this.comboBox5.Text));
                                                                    string str10 = string.Concat(this.sshserverurl, "/Home/getssh?country=", text8);
                                                                    if (flag4)
                                                                    {
                                                                        str10 = string.Concat(str10, "&offerID=", str9);
                                                                    }
                                                                    HttpWebRequest httpWebRequest2 = (HttpWebRequest)WebRequest.Create(str10);
                                                                    httpWebRequest2.UserAgent = "autoleadios";
                                                                    try
                                                                    {
                                                                        StreamReader streamReader2 = new StreamReader(httpWebRequest2.GetResponse().GetResponseStream());
                                                                        string end1 = streamReader2.ReadToEnd();
                                                                        if (end1 != "hetssh")
                                                                        {
                                                                            string[] strArrays3 = end1.Split(new string[] { "|" }, StringSplitOptions.None);
                                                                            if (strArrays3.Count<string>() >= 4)
                                                                            {
                                                                                this.curip = strArrays3[1];
                                                                                if (!sshcommand.SetSSH(strArrays3[1], strArrays3[2], strArrays3[3], this.ipAddressControl1.Text, this.numericUpDown1.Value.ToString(), ref this.bitproc))
                                                                                {
                                                                                    string str11 = string.Concat(this.sshserverurl, "/Home/xoassh?ID=", strArrays3[0]);
                                                                                    HttpWebRequest httpWebRequest3 = (HttpWebRequest)WebRequest.Create(str11);
                                                                                    httpWebRequest3.UserAgent = "autoleadios";
                                                                                    try
                                                                                    {
                                                                                        StreamReader streamReader3 = new StreamReader(httpWebRequest3.GetResponse().GetResponseStream());
                                                                                        streamReader2.ReadToEnd();
                                                                                    }
                                                                                    catch (Exception exception7)
                                                                                    {
                                                                                    }
                                                                                    break;
                                                                                }
                                                                                else
                                                                                {
                                                                                    goto Label8;
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            base.Invoke(new MethodInvoker(() => this.label1.Text = "SSh trên server đã hết, đang đợi ssh mới ..."));
                                                                            for (int j = 0; j < 10; j++)
                                                                            {
                                                                                Thread.Sleep(1000);
                                                                                base.Invoke(new MethodInvoker(() => this.label1.Text = string.Concat("Đợi để lấy SSH trên server trong ", (10 - j).ToString(), " giây")));
                                                                            }
                                                                        }
                                                                    }
                                                                    catch (Exception exception8)
                                                                    {
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                            goto Label8;
                                                        }
                                                    }
                                                }
                                            }
                                            this.cmd.getfront();
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    Thread.Sleep(2000);
                                    this.listView1.Invoke(new MethodInvoker(() => {
                                        this.listView1.Items[lime].SubItems[2].BackColor = Color.Lime;
                                        this.listView1.Items[lime].SubItems[3].BackColor = Color.Yellow;
                                        this.listView1.Refresh();
                                    }));
                                    this.cmdResult.wipe = false;
                                    if (flag3)
                                    {
                                        this.cmd.installapp(current.appID);
                                    }
                                    int num7 = current.timeSleepBefore;
                                    if (current.timeSleepBeforeRandom)
                                    {
                                        Random random4 = new Random();
                                        num7 = random4.Next(current.range1, current.range2);
                                    }
                                    for (int k = 0; k < num7; k++)
                                    {
                                        this.label1.Invoke(new MethodInvoker(() => this.label1.Text = string.Concat("Sleeping for ", (num7 - k - 1).ToString(), " seconds")));
                                        Thread.Sleep(1000);
                                        if (this.itunesX.Value != decimal.MinusOne)
                                        {
                                            this.cmd.touch((double)((double)this.itunesX.Value), (double)((double)this.itunesY.Value));
                                        }
                                    }
                                    if (flag3)
                                    {
                                        this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "Đang cài đặt ứng dụng..."));
                                        now = DateTime.Now;
                                        while (!this.cmdResult.wipe)
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
                                                return;
                                            }
                                        }
                                    }
                                    this.cmdResult.openApp = 0;
                                    this.label1.Invoke(new MethodInvoker(() => this.label1.Text = "Opening Aplication..."));
                                    this.cmd.openApp(current.appID);
                                    dateTime = DateTime.Now;
                                    this.button2.Invoke(new MethodInvoker(() => this.maxwait = (int)this.numericUpDown10.Value));
                                    while (true)
                                    {
                                        if ((this.cmdResult.openApp == 0 ? false : this.cmdResult.openApp != 2))
                                        {
                                            break;
                                        }
                                        Thread.Sleep(100);
                                        if ((DateTime.Now - dateTime).TotalSeconds > (double)this.maxwait1)
                                        {
                                            this.button2.Invoke(new MethodInvoker(() => {
                                                if (this.button2.Text == "Disconnect")
                                                {
                                                    this.button2_Click(null, null);
                                                }
                                            }));
                                            return;
                                        }
                                        else if (this.cmdResult.openApp == 2)
                                        {
                                            break;
                                        }
                                    }
                                    if (this.cmdResult.openApp == 1)
                                    {
                                        for (int l = 0; l < Convert.ToInt32(current.timeSleep); l++)
                                        {
                                            Thread.Sleep(1000);
                                            if (this.itunesX.Value != decimal.MinusOne)
                                            {
                                                this.cmd.touch((double)((double)this.itunesX.Value), (double)((double)this.itunesY.Value));
                                            }
                                            ListView listView = this.listView1;
                                            MethodInvoker methodInvoker10 = methodInvoker4;
                                            if (methodInvoker10 == null)
                                            {
                                                MethodInvoker text9 = () => {
                                                    this.listView1.Items[lime].SubItems[3].Text = (Convert.ToInt32(this.listView1.Items[lime].SubItems[3].Text) - 1).ToString();
                                                    this.label1.Text = string.Concat("Application will be closed in ", this.listView1.Items[lime].SubItems[3].Text, " sec");
                                                };
                                                methodInvoker = text9;
                                                methodInvoker4 = text9;
                                                methodInvoker10 = methodInvoker;
                                            }
                                            listView.Invoke(methodInvoker10);
                                        }
                                        this.listView1.Invoke(new MethodInvoker(() => {
                                            this.listView1.Items[lime].SubItems[3].Text = current.timeSleep.ToString();
                                            this.listView1.Items[lime].SubItems[3].BackColor = Color.Lime;
                                            this.listView1.Items[lime].SubItems[4].BackColor = Color.Yellow;
                                            this.listView1.Refresh();
                                        }));
                                        if (current.useScript)
                                        {
                                            this.excuteScript(current.script);
                                        }
                                        this.listView1.Invoke(new MethodInvoker(() => {
                                            this.listView1.Items[lime].SubItems[4].BackColor = Color.Lime;
                                            this.listView1.Refresh();
                                        }));
                                        this.cmd.close(current.appID);
                                    }
                                }
                                goto Label7;
                            }
                        }
                        finally
                        {
                            ((IDisposable)enumerator).Dispose();
                        }
                        bool checked4 = false;
                        bool checked5 = false;
                        int value2 = 0;
                        this.checkBox1.Invoke(new MethodInvoker(() => {
                            checked4 = this.checkBox1.Checked;
                            checked5 = this.checkBox3.Checked;
                            value2 = (int)this.numericUpDown3.Value;
                        }));
                        if ((!checked4 ? false : (this.backuptime + 1) * 100 / (this.runtime + 1) <= value2))
                        {
                            string text10 = "";
                            this.label1.Invoke(new MethodInvoker(() => {
                                this.label1.Text = "Backing Up the application...";
                                text10 = this.comment.Text;
                                if (this.checkBox12.Checked)
                                {
                                    text10 = string.Concat(text10, " IP:", this.curip);
                                }
                                text10 = string.Concat(text10, "[]", this.comboBox5.Text);
                            }));
                            string str12 = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                            if (!checked5)
                            {
                                this.cmd.backup(str6, str12, text10, "", "");
                            }
                            else
                            {
                                this.cmd.backupfull(str6, str12, text10, "", "");
                            }
                            this.cmdResult.backup = false;
                            DateTime dateTime1 = DateTime.Now;
                            this.button2.Invoke(new MethodInvoker(() => this.maxwait = (int)this.numericUpDown10.Value));
                            while (!this.cmdResult.backup)
                            {
                                Thread.Sleep(500);
                                if ((DateTime.Now - dateTime1).TotalSeconds <= (double)this.maxwait)
                                {
                                    this.cmd.checkbackup(str12);
                                }
                                else
                                {
                                    this.button2.Invoke(new MethodInvoker(() => {
                                        if (this.button2.Text == "Disconnect")
                                        {
                                            this.button2_Click(null, null);
                                        }
                                    }));
                                    return;
                                }
                            }
                            this.backupoftime.Invoke(new MethodInvoker(() => {
                                this.backuptime++;
                                this.backupoftime.Text = string.Concat("Backup:", this.backuptime.ToString());
                            }));
                        }
                        this.backupoftime.Invoke(new MethodInvoker(() => {
                            this.runtime++;
                            this.runoftime.Text = string.Concat("Run:", this.runtime.ToString());
                            this.backuprate.Text = string.Concat("Backup Rate:", Math.Round((double)this.backuptime / (double)this.runtime * 100, 2).ToString(), "%");
                            if ((!this.checkBox10.Checked ? false : this.runtime >= this.numericUpDown6.Value))
                            {
                                if (this.button7.Text == "STOP")
                                {
                                    this.button7_Click(null, null);
                                }
                            }
                        }));
                        this.listView1.Invoke(new MethodInvoker(() => {
                            foreach (ListViewItem item in this.listView1.Items)
                            {
                                item.SubItems[0].ResetStyle();
                                item.SubItems[1].ResetStyle();
                                item.SubItems[2].ResetStyle();
                                item.SubItems[3].ResetStyle();
                                item.SubItems[4].ResetStyle();
                                this.listView1.Refresh();
                            }
                        }));
                        goto Label0;
                    }
                }
                return;
            }
            else
            {
                this.button7.Invoke(new MethodInvoker(() => {
                    if (this.button7.Text == "STOP")
                    {
                        this.button7_Click(null, null);
                    }
                    if (this.button19.Text == "STOP")
                    {
                        this.button19_Click(null, null);
                    }
                }));
                goto Label0;
            }
        Label4:
            this.vipacc.limited = true;
            this.listView3.Invoke(new MethodInvoker(() => {
                this.listView3.Items[this.listvipacc.IndexOf(this.vipacc)].BackColor = Color.Red;
                this.listView3.Refresh();
            }));
            goto Label0;
        Label5:
            if (checked1)
            {
                this.label1.Invoke(new MethodInvoker(() => this.button24_Click(null, null)));
            }
            else
            {
                MessageBox.Show("All SSH are used or dead, please update new SSH list!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.label1.Invoke(new MethodInvoker(() => this.button7_Click(null, null)));
            }
            goto Label0;
        Label10:
            this.vipacc.limited = true;
            this.listView3.Invoke(new MethodInvoker(() => {
                this.listView3.Items[this.listvipacc.IndexOf(this.vipacc)].BackColor = Color.Red;
                this.listView3.Refresh();
            }));
            this.cmd.wipe("com.apple.mobilesafari");
            now = DateTime.Now;
            this.button2.Invoke(new MethodInvoker(() => this.maxwait = (int)this.numericUpDown10.Value));
            while (!this.cmdResult.wipe)
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
                    return;
                }
            }
            goto Label0;
        Label12:
            this.vipacc.limited = true;
            this.listView3.Invoke(new MethodInvoker(() => {
                this.listView3.Items[this.listvipacc.IndexOf(this.vipacc)].BackColor = Color.Red;
                this.listView3.Refresh();
            }));
            goto Label0;
        Label13:
            if (str == "timeout")
            {
                goto Label0;
            }
            else
            {
                this.cmd.close("com.apple.mobilesafari");
                this.currentvipip = value;
                this.curip = this.currentvipip;
                goto Label0;
            }
        Label14:
            if (checked2)
            {
                this.label1.Invoke(new MethodInvoker(() => this.button24_Click(null, null)));
            }
            else
            {
                MessageBox.Show("All SSH are used or dead, please update new SSH list!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.label1.Invoke(new MethodInvoker(() => this.button7_Click(null, null)));
            }
            goto Label0;

        }
    }
}