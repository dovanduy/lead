using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Resources;

namespace AutoLead
{
	public class vipaccount
	{
		public bool limited
		{
			get;
			set;
		}

		public string password
		{
			get;
			set;
		}

		public string username
		{
			get;
			set;
		}

		public vipaccount()
		{
		}
	}

    public class luminatio_account
    {
        public bool limited
        {
            get;
            set;
        }

        public string password
        {
            get;
            set;
        }

        public string username
        {
            get;
            set;
        }

        public luminatio_account()
        {
        }
    }


    public class invoke
    {
        public MethodInvoker methodInvoker;
        public MethodInvoker methodInvoker1 = null;
        public MethodInvoker methodInvoker2 = null;
        public MethodInvoker methodInvoker3 = null;
        public MethodInvoker methodInvoker4 = null;
        public MethodInvoker methodInvoker5 = null;
        public dynamic vip72Chung;
        public dynamic Lumi;
        public DateTime now;
        public Uri uri;
        public string value = null;
        public string str = null;
        public string text = null;
        public bool flag;
        public bool flag1;
        public bool checked1 = false;
        public bool checked2 = false;
        public bool @checked;

    }

    public class Lumi
    {
        public const int WM_LBUTTONDOWN = 513;

        public const int WM_LBUTTONUP = 514;

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

        public static bool lumi_login(string username, string password)
        {
            bool is_correct = false;
            try
            {

                var client = new WebClient();
                client.Proxy = new WebProxy("zproxy.lum-superproxy.io:22225");
                client.Proxy.Credentials = new NetworkCredential(username + "-zone-static", password);
                string reponse = client.DownloadString("http://lumtest.com/myip.json");
                is_correct = true;
            }
            catch (Exception e)
            {

            }

            return is_correct;
        }



        public static void ControlClick(IntPtr hwnd)
        {
            int num = 0;
            Lumi.SendMessageTimeout(hwnd, 513, IntPtr.Zero, IntPtr.Zero, 2, 5000, out num);
            Lumi.SendMessageTimeout(hwnd, 514, IntPtr.Zero, IntPtr.Zero, 2, 5000, out num);
        }

        public static void ControlDoubleClick(IntPtr hwnd)
        {
            int num = 0;
            Lumi.SendMessageTimeout(hwnd, 515, IntPtr.Zero, IntPtr.Zero, 2, 5000, out num);
        }

        private static bool ControlGetCheck(IntPtr controlhand)
        {
            int num;
            Lumi.SendMessageTimeout(controlhand, 240, IntPtr.Zero, IntPtr.Zero, 2, 5000, out num);
            return num == 1;
        }

        public static IntPtr ControlGetHandle(IntPtr parentHandle, string _controlClass, int ID)
        {
            IntPtr zero;
            if (parentHandle != IntPtr.Zero)
            {
                IntPtr intPtr = IntPtr.Zero;
                IntPtr zero1 = IntPtr.Zero;
                int windowLong = -1;
                while (windowLong != ID)
                {
                    zero1 = Lumi.FindWindowEx(parentHandle, intPtr, _controlClass, null);
                    windowLong = (int)Lumi.GetWindowLong(zero1, -12);
                    intPtr = zero1;
                    if (zero1 == IntPtr.Zero)
                    {
                        zero = IntPtr.Zero;
                        return zero;
                    }
                }
                zero = intPtr;
            }
            else
            {
                zero = IntPtr.Zero;
            }
            return zero;
        }

        public static IntPtr ControlGetHandle(IntPtr parentHandle, string _controlClass, int ID, bool instance)
        {
            IntPtr zero;
            if (parentHandle != IntPtr.Zero)
            {
                IntPtr intPtr = IntPtr.Zero;
                IntPtr zero1 = IntPtr.Zero;
                int windowLong = -1;
                while (windowLong != ID)
                {
                    zero1 = Lumi.FindWindowEx(parentHandle, intPtr, _controlClass, null);
                    windowLong = (int)Lumi.GetWindowLong(zero1, -16);
                    intPtr = zero1;
                    if (zero1 == IntPtr.Zero)
                    {
                        zero = IntPtr.Zero;
                        return zero;
                    }
                }
                zero = intPtr;
            }
            else
            {
                zero = IntPtr.Zero;
            }
            return zero;
        }

        public static IntPtr ControlGetHandle(string _text, string _class, string _controlClass, int ID)
        {
            IntPtr zero;
            IntPtr intPtr = Lumi.FindWindow(_class, _text);
            if (intPtr != IntPtr.Zero)
            {
                IntPtr zero1 = IntPtr.Zero;
                IntPtr intPtr1 = IntPtr.Zero;
                int windowLong = -1;
                while (windowLong != ID)
                {
                    intPtr1 = Lumi.FindWindowEx(intPtr, zero1, _controlClass, null);
                    windowLong = (int)Lumi.GetWindowLong(intPtr1, -12);
                    zero1 = intPtr1;
                    if (intPtr1 == IntPtr.Zero)
                    {
                        zero = IntPtr.Zero;
                        return zero;
                    }
                }
                zero = zero1;
            }
            else
            {
                zero = IntPtr.Zero;
            }
            return zero;
        }

        private static bool ControlGetState(IntPtr controlhandle, int state)
        {
            bool flag;
            flag = (((int)Lumi.GetWindowLong(controlhandle, -16) & state) == 0 ? false : true);
            return flag;
        }

        public static string ControlGetText(IntPtr hwnd)
        {
            StringBuilder stringBuilder = new StringBuilder(255);
            int num = 0;
            Lumi.SendMessageTimeout(hwnd, 13, stringBuilder.Capacity, stringBuilder, 2, 5000, out num);
            return stringBuilder.ToString();
        }

        public static int ControlSetText(IntPtr hwnd, string text)
        {
            return Lumi.SendMessage(hwnd, 12, text.Length, text);
        }

        [DllImport("user32", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
        private static extern bool EnumThreadWindows(int threadId, Lumi.EnumWindowsProc callback, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        private static extern IntPtr FindWindow(string sClass, string sWindow);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

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

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
        public static extern IntPtr GetDlgItem(int hwnd, int childID);


        public Process[] GetProcessByName(string processName)
        {
            Process[] processes = Process.GetProcesses();
            List<Process> processes1 = new List<Process>();
            Process[] processArray = processes;
            for (int i = 0; i < (int)processArray.Length; i++)
            {
                Process process = processArray[i];
                if (process.ProcessName.StartsWith(processName))
                {
                    processes1.Add(process);
                }
            }
            return processes1.ToArray();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
        public static extern IntPtr GetWindowLong(IntPtr hwnd, int nIndex);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
        public static extern int GetWindowText(IntPtr hwnd, IntPtr windowString, int maxcount);

        [DllImport("user32", CharSet = CharSet.Auto, ExactSpelling = false, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxCount);

        [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
        internal static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int dwSize, out int lpNumberOfBytesRead);

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


        [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
        private static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
        private static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, int[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);
    }
}