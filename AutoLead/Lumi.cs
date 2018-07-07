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
using System.Windows.Forms;
using Newtonsoft.Json.Linq;


namespace AutoLead
{
    

    internal class Lumi
    {
       
        public Lumi()
        {
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


            DateTime now = DateTime.Now;
            int maxwait = 120;
            string pathToCcProxyIni = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "CCProxy\\", "CCProxy", ".ini");
            string pathToCcProxyDesignIni = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "CCProxyProfile\\", "CCProxy_design", ".ini");
            while (true)
            {

                if ((DateTime.Now - now).TotalSeconds <= (double) maxwait)
                {
                    var countryCode = Lumi.GetCountryCodeFromName(countryName);

                    if (countryCode != null)
                    {
                        try
                        {
                            var configCopyLines = File.ReadAllLines(pathToCcProxyIni);

                            // check if password for luminatio is fill

                            string lumiPass = null;
                            string lumiUser = null;


                            foreach (var configLine in configCopyLines)
                            {

                                if (configLine.StartsWith("Password="))
                                {
                                    lumiPass = configLine;

                                }

                                if (configLine.StartsWith("UserName="))
                                {
                                    lumiUser = configLine;

                                }

                                if (configLine.StartsWith("[StringEx]"))
                                {
                                    break;
                                }

                            }

                            if ((lumiPass == "Password=") || (lumiUser == "UserName=") || (lumiUser == null) || (lumiPass == null))
                            {
                                MessageBox.Show("Please Open CCProxy and Fill Luminatio Username and Password!", Application.ProductName, MessageBoxButtons.OK,
                                    MessageBoxIcon.Hand);
                                break;
                            }

                         
                            //allready filled password.
                           

                            //change username to with CountryCode.

                            if (lumiUser.Contains("country-"))
                            {
                                lumiUser = lumiUser.Substring(0, lumiUser.Length - 2) + countryCode.ToLower();
                            }
                            else 
                            {
                                MessageBox.Show("Luminati Username Must Contain '-country-'!", Application.ProductName, MessageBoxButtons.OK,
                                    MessageBoxIcon.Hand);
                                break;
                            }


                            //replace 

                            File.Copy(pathToCcProxyDesignIni, pathToCcProxyIni, true);

                            // fill value to new ini file.

                            var originalLines = File.ReadAllLines(pathToCcProxyIni);

                            var updatedLines = new List<string>();
                            foreach (var line in originalLines)
                            {
                                var copyLine = line;

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
                                    copyLine = lumiUser;
                                }

                                if (line.Contains("#LUMI_PASS#"))
                                {
                                    copyLine = lumiPass;
                                }

                                updatedLines.Add(copyLine);
                            }

                            File.WriteAllLines(pathToCcProxyIni, updatedLines);


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

        

        public static string getCurrentLumiIP(string ipforward, decimal portforward)
        {

            DateTime now = DateTime.Now;
            int maxwait = 10;
            string response = null;
            try
            {
                var strUrl = "http://ip-api.com/line";

                BetterHttpClient.HttpClient wc = new BetterHttpClient.HttpClient(new BetterHttpClient.Proxy(ipforward, (int)portforward, BetterHttpClient.ProxyTypeEnum.Socks));
                ;
                return wc.DownloadString(strUrl);
                //var json = JObject.Parse(strJson);
                //response = json["ip"].ToString();
               

            }
            catch (Exception e)
            {
                response = e.ToString();
            }
            return response;
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