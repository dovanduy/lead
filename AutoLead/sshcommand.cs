using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace AutoLead
{
	internal class sshcommand
	{
		private Process[] myProcess = Process.GetProcessesByName("program name here");

		public const int WM_LBUTTONDOWN = 513;

		public const int WM_LBUTTONUP = 514;

		public sshcommand()
		{
		}

		public static void closebitvise(int port)
		{
			Process[] processesByName = Process.GetProcessesByName("BvSsh");
			for (int i = 0; i < (int)processesByName.Length; i++)
			{
				Process process = processesByName[i];
				IntPtr intPtr = sshcommand.FindWindowInProcess(process, (string s) => s.StartsWith("Bitvise SSH Client"));
				StringBuilder stringBuilder = new StringBuilder(200);
				sshcommand.GetWindowText(intPtr, stringBuilder, 200);
				string str = stringBuilder.ToString();
				Match match = (new Regex("_(.*?).bscp")).Match(str);
				if (match.Success)
				{
					if (match.Groups[1].Value == port.ToString())
					{
						process.Kill();
					}
				}
			}
		}

		public static void ControlClick(IntPtr hwnd)
		{
			sshcommand.SendMessage1(hwnd, 513, IntPtr.Zero, IntPtr.Zero);
			sshcommand.SendMessage1(hwnd, 514, IntPtr.Zero, IntPtr.Zero);
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
					zero1 = sshcommand.FindWindowEx(parentHandle, intPtr, _controlClass, null);
					windowLong = (int)sshcommand.GetWindowLong(zero1, -12);
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
			IntPtr intPtr = sshcommand.FindWindow(_class, _text);
			if (intPtr != IntPtr.Zero)
			{
				IntPtr zero1 = IntPtr.Zero;
				IntPtr intPtr1 = IntPtr.Zero;
				int windowLong = -1;
				while (windowLong != ID)
				{
					intPtr1 = sshcommand.FindWindowEx(intPtr, zero1, _controlClass, null);
					windowLong = (int)sshcommand.GetWindowLong(intPtr1, -12);
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

		public static string ControlGetText(IntPtr hwnd)
		{
			StringBuilder stringBuilder = new StringBuilder(255);
			sshcommand.SendMessage(hwnd, 13, stringBuilder.Capacity, stringBuilder);
			return stringBuilder.ToString();
		}

		private static void createProfile(string IPforward, string Portforward, string fileLocation)
		{
			sshcommand.profile bytes = new sshcommand.profile();
			Stream stream = File.Open(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "1.bscp"), FileMode.Open);
			BinaryReader binaryReader = new BinaryReader(stream);
			long length = stream.Length;
			bytes.header = binaryReader.ReadBytes(21);
			bytes.length = binaryReader.ReadByte();
			bytes.body = binaryReader.ReadBytes((int)length - 189);
			bytes.iplen = binaryReader.ReadByte();
			bytes.ip = binaryReader.ReadBytes((int)bytes.iplen);
			bytes.s1 = binaryReader.ReadBytes(3);
			bytes.portlen = binaryReader.ReadByte();
			bytes.port = binaryReader.ReadBytes((int)bytes.portlen);
			bytes.end = binaryReader.ReadBytes(149);
			bytes.ip = Encoding.UTF8.GetBytes(IPforward);
			bytes.length = (byte)(bytes.length + (IPforward.Length + Portforward.Length - bytes.iplen - bytes.portlen));
			bytes.iplen = (byte)IPforward.Length;
			bytes.port = Encoding.UTF8.GetBytes(Portforward);
			bytes.portlen = (byte)Portforward.Length;
			stream.Close();
			using (Stream fileStream = new FileStream(fileLocation, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(fileStream, Encoding.Default))
				{
					binaryWriter.Write(bytes.header);
					binaryWriter.Write(bytes.length);
					binaryWriter.Write(bytes.body);
					binaryWriter.Write(bytes.iplen);
					binaryWriter.Write(bytes.ip);
					binaryWriter.Write(bytes.s1);
					binaryWriter.Write(bytes.portlen);
					binaryWriter.Write(bytes.port);
					binaryWriter.Write(bytes.end);
				}
				fileStream.Close();
			}
		}

		[DllImport("user32", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		private static extern bool EnumChildWindows(IntPtr hwndParent, sshcommand.EnumWindowsProc lpEnumFunc, IntPtr lParam);

		[DllImport("user32", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		private static extern bool EnumThreadWindows(int threadId, sshcommand.EnumWindowsProc callback, IntPtr lParam);

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern IntPtr FindWindow(string sClass, string sWindow);

		[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=false)]
		private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		public static IntPtr FindWindowInProcess(Process process, Func<string, bool> compareTitle)
		{
			IntPtr zero = IntPtr.Zero;
			foreach (ProcessThread thread in process.Threads)
			{
				zero = sshcommand.FindWindowInThread(thread.Id, compareTitle);
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
			sshcommand.EnumThreadWindows(threadId, (IntPtr hWnd, IntPtr lParam) => {
				bool flag;
				StringBuilder stringBuilder = new StringBuilder(200);
				sshcommand.GetWindowText(hWnd, stringBuilder, 200);
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

		[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=false)]
		public static extern IntPtr GetDlgItem(int hwnd, int childID);

		[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=false)]
		public static extern IntPtr GetWindowLong(IntPtr hwnd, int nIndex);

		[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=false)]
		public static extern int GetWindowText(IntPtr hwnd, IntPtr windowString, int maxcount);

		[DllImport("user32", CharSet=CharSet.Auto, ExactSpelling=false, SetLastError=true)]
		private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxCount);

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern int SendMessage(IntPtr hWnd, int msg, int Param, StringBuilder text);

		[DllImport("user32.dll", CharSet=CharSet.Auto, EntryPoint="SendMessage", ExactSpelling=false)]
		public static extern int SendMessage1(IntPtr hWnd, int msg, IntPtr Param, IntPtr text);

		[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=false)]
		private static extern int SendMessageA(IntPtr hwnd, int wMsg, int wParam, uint lParam);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		public static bool SetSSH(string host, string username, string password, string ipforward, string portfoward, ref Process refproc)
		{
			bool flag;
			SshChecker sshChecker = new SshChecker();
			string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
			if (File.Exists(string.Concat(directoryName, "\\Bitviseprofile")))
			{
				Directory.CreateDirectory(string.Concat(directoryName, "\\Bitviseprofile"));
			}
			directoryName = (new Uri(directoryName)).LocalPath;
			string str = Path.Combine(string.Concat(directoryName, "\\Bitviseprofile"), string.Concat("_", portfoward, ".bscp"));
			sshcommand.createProfile(ipforward, portfoward, str);
			string str1 = string.Concat(new string[] { " -profile=\"", str, "\" -host=", host, " -port=22 -user=", username, " -password=", password, " -openTerm=n -openSFTP=n -openRDP=n -loginOnStartup -menu=small" });
			string str2 = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Bitvise SSH Client\\BvSsh.exe");
			Process process = Process.Start(str2, str1);
			refproc = process;
			while ((DateTime.Now - process.StartTime).Seconds < 1)
			{
				Thread.Sleep(100);
			}
			IntPtr intPtr = sshcommand.FindWindowInProcess(process, (string s) => s.StartsWith("Bitvise SSH Client"));
			while (intPtr == IntPtr.Zero)
			{
				intPtr = sshcommand.FindWindowInProcess(process, (string s) => s.StartsWith("Bitvise SSH Client"));
				Thread.Sleep(100);
			}
			if (sshcommand.threadAccept(intPtr))
			{
				refproc = process;
				flag = true;
			}
			else
			{
				process.Kill();
				flag = false;
			}
			return flag;
		}

		public static bool threadAccept(IntPtr hwnd)
		{
			bool flag;
			IntPtr intPtr = sshcommand.ControlGetHandle(hwnd, "Button", 1);
			DateTime now = DateTime.Now;
			while (true)
			{
				IntPtr intPtr1 = sshcommand.ControlGetHandle("Host Key Verification", "#32770", "Button", 102);
				if (intPtr1 != IntPtr.Zero)
				{
					sshcommand.ControlClick(intPtr1);
				}
				Thread.Sleep(100);
				if (sshcommand.ControlGetText(intPtr) == "Logout")
				{
					flag = true;
					break;
				}
				else if ((DateTime.Now - now).Seconds > 15)
				{
					flag = false;
					break;
				}
			}
			return flag;
		}

		public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);

		private struct profile
		{
			public byte[] header;

			public byte length;

			public byte[] body;

			public byte iplen;

			public byte[] ip;

			public byte[] s1;

			public byte portlen;

			public byte[] port;

			public byte[] end;
		}
	}
}