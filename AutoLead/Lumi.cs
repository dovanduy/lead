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
using RestSharp;


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

        public static bool sendCheck()
        {
            bool isValid = false;
            try
            {
                var client = new RestClient("http://127.0.0.1:22999");
                // client.Authenticator = new HttpBasicAuthenticator(username, password);

                var request = new RestRequest("api/version", Method.GET);

                // execute the request
                IRestResponse response = client.Execute(request);
                var content = response.Content;

                var json = JObject.Parse(content);
                var jResult = json["version"];
                isValid = true;
            }
            catch (Exception e)
            {

            }

            return isValid;
        }

        public static void closeLuminatio(decimal port)
        {
            if (!Lumi.sendCheck())
            {
                //MessageBox.Show("Luminatio Proxy Manager is not running!", Application.ProductName, MessageBoxButtons.OK,MessageBoxIcon.Hand);
                
            }
            else
            {
                try
                {
                    var client = new RestClient("http://127.0.0.1:22999");
                    // client.Authenticator = new HttpBasicAuthenticator(username, password);

                    var request = new RestRequest("api/proxies/" + port.ToString(), Method.DELETE);

                    // execute the request
                    client.Execute(request);
                    
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error while Close Luminatio Port" + port.ToString(), Application.ProductName, MessageBoxButtons.OK,
                        MessageBoxIcon.Hand);
                }
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

        public static bool fake_proxy_lumi(string countryName, string portforward, string zone, string password, string username)
        {
            bool isValid = false;

            //check if configuration file is existed or not


            DateTime now = DateTime.Now;
            int maxwait = 30;
            while (true)
            {

                if ((DateTime.Now - now).TotalSeconds <= (double)maxwait)
                {
                    var countryCode = Lumi.GetCountryCodeFromName(countryName);

                    if (countryCode != null)
                    {
                        //check if port is exited.
                        if (!Lumi.sendCheck())
                        {
                            MessageBox.Show("Luminatio Proxy Manager is not running!", Application.ProductName, MessageBoxButtons.OK,MessageBoxIcon.Hand);
                            break;

                        }
                        else
                        {
                            //delete port whatever this have or not.
                            try
                            {
                                var clientDelete = new RestClient("http://127.0.0.1:22999");
                                // client.Authenticator = new HttpBasicAuthenticator(username, password);

                                var requestDelete = new RestRequest("api/proxies/" + portforward, Method.DELETE);

                                // execute the request
                                clientDelete.Execute(requestDelete);

                            }
                            catch (Exception e)
                            {
                              
                            }
                            //create port with country.
                            try
                            {
                                var client = new RestClient("http://127.0.0.1:22999");
                                // client.Authenticator = new HttpBasicAuthenticator(username, password);

                                var request = new RestRequest("api/proxies", Method.POST);
                                request.RequestFormat = DataFormat.Json;
                                request.AddBody(new { proxy = new
                                {
                                    port = portforward,
                                    zone = zone,
                                    country = countryCode.ToLower(),
                                    password = password,
                                    customer = username
                                } });

                                // execute the request
                                IRestResponse response = client.Execute(request);

                                var jsonRes = JObject.Parse(response.Content);
                                var jResult = jsonRes["data"];

                                Thread.Sleep(5);

                                //MessageBox.Show("Luminatio Proxy set country port with res" + response.Content, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);

                               
                                var request2 = new RestRequest("api/proxy_status/" + portforward, Method.GET);
                                IRestResponse response2 = client.Execute(request2);

                                var jsonRes2 = JObject.Parse(response2.Content);
                                string jResult2 = (string)jsonRes2["status"];

                                if (jResult2 == "ok")
                                {
                                    isValid = true;
                                }
                                else {

                                    try
                                    {                                       
                                        var request3 = new RestRequest("api/proxies/" + portforward, Method.DELETE);

                                        // execute the request
                                        client.Execute(request3);

                                    }
                                    catch (Exception e)
                                    {

                                    }

                                    MessageBox.Show("Luminatio Proxy set country port with error" + response2.Content, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);

                                }

                                break;

                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("Luminatio Proxy set country port with error" + e.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                break;
                            }

                        }


                    }
                }
                else
                {
                    MessageBox.Show("Timeout when finding country Code For Country" + countryName, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);

                    break;
                }
            }

            return isValid;
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



        public static string getCurrentLumiIPVer2(string ipforward, decimal portforward)
        {
            string currentIP = null;
            try
            {
                Chilkat.Http http = new Chilkat.Http();
                
                bool success;
                success = http.UnlockComponent("Anything for 30-day trial");
                if (success != true)
                {
                    MessageBox.Show("Error when retrieve IP" + http.LastErrorText, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);

                }
                else
                {
                    http.SocksHostname = ipforward;
                    http.SocksVersion = 5;
                    http.SocksPort = (int)portforward;
                    Chilkat.HttpRequest req = new Chilkat.HttpRequest();
                    currentIP = http.QuickGetStr("http://lumtest.com/myip");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error when retrieve IP" + e.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);

            }

            return currentIP;
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