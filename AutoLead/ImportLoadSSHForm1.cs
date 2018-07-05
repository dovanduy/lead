using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AutoLead
{
    public partial class Form1
    {
        private void importssh(string text)
        {
            string[] strArrays = text.Split(new string[] {"\r\n"}, StringSplitOptions.None);
            for (int i = 0; i < (int) strArrays.Length; i++)
            {
                string str = strArrays[i];
                string[] strArrays1 = str.Split(new string[] {"|"}, StringSplitOptions.None);
                if (strArrays1.Count<string>() == 1)
                {
                    strArrays1 = new string[] {strArrays1[0], "root", "admin", "USA"};
                }

                if (strArrays1.Count<string>() >= 3)
                {
                    string str1 = strArrays1[0];
                    string[] strArrays2 = str1.Split(new char[] {'.'});
                    bool flag = true;
                    if ((int) strArrays2.Length != 4)
                    {
                        flag = false;
                    }
                    else
                    {
                        string[] strArrays3 = strArrays2;
                        for (int j = 0; j < (int) strArrays3.Length; j++)
                        {
                            string str2 = strArrays3[j];
                            byte num = 0;
                            if (!byte.TryParse(str2, out num))
                            {
                                flag = false;
                            }
                        }
                    }

                    if (flag)
                    {
                        ssh _ssh = new ssh()
                        {
                            IP = strArrays1[0],
                            username = strArrays1[1],
                            password = strArrays1[2]
                        };
                        if (strArrays1.Count<string>() <= 3)
                        {
                            _ssh.country = "unknown";
                        }
                        else
                        {
                            string[] strArrays4 = strArrays1[3].Split(new string[] {"("}, StringSplitOptions.None);
                            _ssh.country = strArrays1[3];
                            if (strArrays4.Count<string>() > 1)
                            {
                                _ssh.country = Regex.Replace(strArrays4[0], "\\s+", "");
                            }
                        }

                        Regex regex = new Regex("\\((.*?)\\)");
                        if (strArrays1.Count<string>() == 3)
                        {
                            Array.Resize<string>(ref strArrays1, 4);
                            strArrays1[3] = "Unknown";
                        }

                        Match match = regex.Match(strArrays1[3]);
                        if (!match.Success)
                        {
                            _ssh.countrycode = "Unknow";
                        }
                        else
                        {
                            _ssh.countrycode = match.Groups[1].Value.ToString();
                        }

                        _ssh.live = "uncheck";
                        _ssh.used = false;
                        this.listssh.Add(_ssh);
                        ListViewItem listViewItem = new ListViewItem(new string[]
                            {_ssh.IP, _ssh.username, _ssh.password, _ssh.country});
                        this.listView2.Items.Add(listViewItem);
                    }
                }
            }

            if (this.proxytool.Text == "SSH")
            {
                IEnumerable<string> strs = (
                    from x in this.listssh
                    select x.country).Distinct<string>();
                this.comboBox5.Items.Clear();
                foreach (string str3 in strs)
                {
                    this.comboBox5.Items.Add(str3);
                }

                if (this.comboBox5.Items.Count > 0)
                {
                    this.comboBox5.Text = this.comboBox5.Items[0].ToString();
                }
            }

            Label label = this.labeltotalssh;
            int count = this.listView2.Items.Count;
            label.Text = string.Concat("Total SSH:", count.ToString());
        }

        private void loadssh()
        {
            if (this.DeviceInfo.SerialNumber != null)
            {
                this.listssh.Clear();
                this.listView2.Items.Clear();
                if (File.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber,
                    "\\ssh.dat")))
                {
                    string[] strArrays = File
                        .ReadAllText(string.Concat(AppDomain.CurrentDomain.BaseDirectory, this.DeviceInfo.SerialNumber,
                            "\\ssh.dat")).Split(new string[] {"\r\n"}, StringSplitOptions.None);
                    for (int i = 0; i < (int) strArrays.Length; i++)
                    {
                        string str = strArrays[i];
                        string[] strArrays1 = str.Split(new string[] {"||"}, StringSplitOptions.None);
                        if (strArrays1.Count<string>() == 7)
                        {
                            ssh _ssh = new ssh()
                            {
                                IP = strArrays1[0],
                                username = strArrays1[1],
                                password = strArrays1[2],
                                country = strArrays1[3],
                                countrycode = strArrays1[4],
                                used = Convert.ToBoolean(strArrays1[5]),
                                live = strArrays1[6]
                            };
                            this.listssh.Add(_ssh);
                            ListViewItem listViewItem = new ListViewItem(new string[]
                                {_ssh.IP, _ssh.username, _ssh.password, _ssh.country});
                            if (_ssh.live == "alive")
                            {
                                listViewItem.BackColor = Color.Lime;
                            }

                            if (_ssh.live == "dead")
                            {
                                listViewItem.BackColor = Color.Red;
                            }

                            if (_ssh.used)
                            {
                                listViewItem.BackColor = Color.Aqua;
                            }

                            this.listView2.Items.Add(listViewItem);
                        }
                    }
                }

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
            }

            if (this.proxytool.Text == "SSH")
            {
                IEnumerable<string> strs = (
                    from x in this.listssh
                    select x.country).Distinct<string>();
                this.comboBox5.Items.Clear();
                foreach (string str1 in strs)
                {
                    this.comboBox5.Items.Add(str1);
                }
            }
        }
    }
}