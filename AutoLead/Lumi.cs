using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Resources;
using System.Globalization;
using Newtonsoft.Json.Linq;


namespace AutoLead
{
    internal class Lumi
    {
       
        public Lumi()
        {
        }

        public static void changeCCProxySetting(string cc_path, string design_path, string config_copy_path, string username, string ipforward, string portforward)
        {
            File.Copy(design_path, cc_path, true);

            string reg_code = null;
            string reg_user = null;

            string lumi_pass = null;

            var configCopyLines = File.ReadAllLines(config_copy_path);



            foreach (var configLine in configCopyLines)
            {
                if (configLine.StartsWith("RegCode="))
                {
                    reg_code = configLine;

                }

                if (configLine.StartsWith("UserName="))
                {
                    reg_user = configLine;

                }

                if (configLine.StartsWith("[Port]"))
                {
                    break;
                }

            }

            foreach (var configLine in configCopyLines)
            {
                if (configLine.StartsWith("Password="))
                {
                    lumi_pass = configLine;

                }

                if (configLine.StartsWith("[StringEx]"))
                {
                    break;
                }

            }




            var originalLines = File.ReadAllLines(cc_path);

            var updatedLines = new List<string>();
            foreach (var line in originalLines)
            {
                var copyLine = line;
                if (line.Contains("#REG_CODE#"))
                {
                    copyLine = reg_code;
                }

                if (line.Contains("#REG_USER#"))
                {
                    copyLine = reg_user;
                }

                if (line.Contains("#SOCK_FORWARD_IP#"))
                {
                    copyLine = "SOCKS=" + ipforward;
                }

                if (line.Contains("#SOCK_FORWARD_PORT#"))
                {
                    copyLine = "SOCKS=" + portforward;
                }

                if (line.Contains("#LUMI_USER#"))
                {
                    copyLine = "UserName=" + username;
                }

                if (line.Contains("#LUMI_PASS#"))
                {
                    copyLine = lumi_pass;
                }

                updatedLines.Add(copyLine);
            }

            File.WriteAllLines(cc_path, updatedLines);
        }

        public static void closeCCProxy()
        {
            Process[] processesByName = Process.GetProcessesByName("CCProxy");
            for (int i = 0; i < (int)processesByName.Length; i++)
            {
                Process process = processesByName[i];
                process.Kill();
            }
        }

        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }


        public static string GetCountryCodeFromName(string countryName)
        {

            string ucfirstName = Lumi.FirstCharToUpper(countryName);

            try
            {
                // API key is recommended, but not required
                const string strGoogleApiKey = "";
                var strUrl = "https://maps.googleapis.com/maps/api/geocode/json?address={0}";
                strUrl = string.Format(strUrl, ucfirstName);
                var wc = new WebClient();
                var strJson = wc.DownloadString(strUrl);
                var json = JObject.Parse(strJson);
                var jResult = json["results"][0];
                var jAddressComponents = jResult["address_components"];
                foreach (var jAddressComponent in jAddressComponents)
                {
                    var jTypes = jAddressComponent["types"];
                    if (jTypes.Any(t => t.ToString() == "country"))
                    {
                        return jAddressComponent["short_name"].ToString();
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public static bool fake_proxy(string countryName, string ipforward, string portforward, ref Process refproc)
        {
            bool isValid = false;

            //check if configuration file is existed or not

            string path_to_cc_ini =
                string.Concat(AppDomain.CurrentDomain.BaseDirectory, "CCProxy\\", "CCProxy", ".ini");

            string config_copy_file = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "CCProxyProfile\\", "CCProxy_copy", ".ini");
            string design_config_file = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "CCProxyProfile\\", "CCProxy_design", ".ini");


            if (!File.Exists(config_copy_file))
            {
                File.Copy(path_to_cc_ini, config_copy_file);
            }
            DateTime now = DateTime.Now;
            int maxwait = 120;
            while (true)
            {
                string country_code = null;
                string username = null;

                if ((DateTime.Now - now).TotalSeconds <= (double) maxwait)
                {
                    country_code = Lumi.GetCountryCodeFromName(countryName);

                    if (country_code != null)
                    {
                        try
                        {
                            username = "lum-customer-appsuper-zone-static-country-" + country_code.ToLower();

                            Lumi.changeCCProxySetting(path_to_cc_ini, design_config_file, config_copy_file, username, ipforward, portforward);

                            string str2 = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "CCProxy\\CCProxy.exe");
                            Process process = Process.Start(str2);
                            refproc = process;
                            while ((DateTime.Now - process.StartTime).Seconds < 1)
                            {
                                Thread.Sleep(100);
                            }
                            IntPtr intPtr = Lumi.FindWindowInProcess(process, (string s) => s.StartsWith("CCProxy"));
                            while (intPtr == IntPtr.Zero)
                            {
                                intPtr = Lumi.FindWindowInProcess(process, (string s) => s.StartsWith("CCProxy"));
                                Thread.Sleep(100);
                            }
                            if (intPtr != IntPtr.Zero)
                            {
                                refproc = process;
                                isValid = true;
                            }
                            else
                            {
                                process.Kill();
                                isValid = false;
                            }

                            
                        }
                        catch (Exception e)
                        {
                            
                        }
                        break;

                    }
                }
                else
                {

                    break;
                }
            }

            return isValid;
        }

        

        public static string lumi_login(string username, string password)
        {
            try
            {

                var client = new WebClient();
                client.Proxy = new WebProxy("zproxy.lum-superproxy.io:22225");
                client.Proxy.Credentials = new NetworkCredential(username, password);
                return client.DownloadString("http://lumtest.com/myip.json");

            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public static IntPtr FindWindowInProcess(Process process, Func<string, bool> compareTitle)
        {
            IntPtr zero = IntPtr.Zero;
            foreach (ProcessThread thread in process.Threads)
            {
                zero = Lumi.FindWindowInThread(thread.Id, compareTitle);
                if (zero != IntPtr.Zero)
                {
                    break;
                }
            }
            return zero;
        }

        private static IntPtr FindWindowInThread(int threadId, Func<string, bool> compareTitle)
        {
            IntPtr zero = IntPtr.Zero;
            Lumi.EnumThreadWindows(threadId, (IntPtr hWnd, IntPtr lParam) => {
                bool flag;
                StringBuilder stringBuilder = new StringBuilder(200);
                Lumi.GetWindowText(hWnd, stringBuilder, 200);
                if (!compareTitle(stringBuilder.ToString()))
                {
                    flag = true;
                }
                else
                {
                    zero = hWnd;
                    flag = false;
                }
                return flag;
            }, IntPtr.Zero);
            return zero;
        }

        [DllImport("user32", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
        private static extern bool EnumThreadWindows(int threadId, Lumi.EnumWindowsProc callback, IntPtr lParam);
        public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
        public static extern int GetWindowText(IntPtr hwnd, IntPtr windowString, int maxcount);

        [DllImport("user32", CharSet = CharSet.Auto, ExactSpelling = false, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxCount);

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int Param, StringBuilder text);

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int Param, string text);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
        public static extern int SendMessage(IntPtr hWnd, int msg, IntPtr Param, IntPtr text);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = false, SetLastError = true)]
        public static extern int SendMessageTimeout(IntPtr hWnd, int Msg, int wParam, StringBuilder lParam, int fuFlags, int uTimeout, out int lpdwResult);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = false, SetLastError = true)]
        public static extern int SendMessageTimeout(IntPtr hWnd, int Msg, int wParam, string lParam, int fuFlags, int uTimeout, out int lpdwResult);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = false, SetLastError = true)]
        public static extern int SendMessageTimeout(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam, int fuFlags, int uTimeout, out int lpdwResult);

    }

}