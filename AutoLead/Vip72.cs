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

namespace AutoLead
{
	public class Vip72
	{
		public static Process Vip72Process;

		public const int WM_LBUTTONDOWN = 513;

		public const int WM_LBUTTONUP = 514;

		static Vip72()
		{
			Vip72.Vip72Process = null;
		}

		public Vip72()
		{
		}

		public void clearip()
		{
			if (!Vip72.Vip72Process.HasExited)
			{
				Vip72.OpenProcess(2035711, false, Vip72.Vip72Process.Id);
				byte[] numArray = new byte[] { 144, 144 };
				IntPtr intPtr = Vip72.FindWindowInProcess(Vip72.Vip72Process, (string s) => s.StartsWith("VIP72 Socks Client"));
				IntPtr intPtr1 = Vip72.ControlGetHandle(intPtr, "SysListView32", 117);
				for (int i = 0; i < 30; i++)
				{
					int num = 0;
					Vip72.SendMessageTimeout(intPtr1, 256, (IntPtr)46, IntPtr.Zero, 2, 5000, out num);
				}
			}
		}

		public void clearIpWithPort(int port)
		{
			Process[] processByName = this.GetProcessByName("vip72socks");
			if (processByName.Count<Process>() > 0)
			{
				Process[] processArray = processByName;
				for (int i = 0; i < (int)processArray.Length; i++)
				{
					Process process = processArray[i];
					if (Vip72.ControlGetText(Vip72.ControlGetHandle(Vip72.FindWindowInProcess(Vip72.Vip72Process, (string s) => s.StartsWith("VIP72 Socks Client")), "Static", 7955)).EndsWith(string.Concat(":", port.ToString())))
					{
						process.Kill();
					}
				}
			}
		}

		public string clickip(int port)
		{
			string listViewItem;
			if (!Vip72.Vip72Process.HasExited)
			{
				IntPtr intPtr = Vip72.FindWindowInProcess(Vip72.Vip72Process, (string s) => s.StartsWith("VIP72 Socks Client"));
				int num = 4328465;
				int num1 = 4324304;
				IntPtr intPtr1 = Vip72.ControlGetHandle(intPtr, "Button", 7817);
				if (Vip72.ControlGetCheck(intPtr1))
				{
					Vip72.ControlClick(intPtr1);
				}
				IntPtr intPtr2 = Vip72.OpenProcess(2035711, false, Vip72.Vip72Process.Id);
				int num2 = 0;
				IntPtr zero = IntPtr.Zero;
				zero = Marshal.AllocHGlobal(4);
				Vip72.ReadProcessMemory(intPtr2, (IntPtr)num, zero, 4, out num2);
				Random random = new Random();
				uint id = (uint)Vip72.Vip72Process.Id;
				IntPtr intPtr3 = Vip72.ControlGetHandle(intPtr, "SysListView32", 117);
				int num3 = 0;
				while (ListViewItem1.GetListViewItem(intPtr3, id, num3, 4) != "")
				{
					string str = ListViewItem1.GetListViewItem(intPtr3, id, num3, 4);
					if ((str.Contains(port.ToString()) ? false : !str.Contains("main stream")))
					{
						num3++;
					}
					else
					{
						ListViewItem1.SelectListViewItem(intPtr3, id, num3);
						int num4 = 0;
						Vip72.SendMessageTimeout(intPtr3, 256, (IntPtr)46, IntPtr.Zero, 2, 5000, out num4);
					}
				}
				num3 = 0;
				int num5 = -1;
				IntPtr intPtr4 = Vip72.ControlGetHandle(intPtr, "SysListView32", 116);
				while (ListViewItem1.GetListViewItem(intPtr4, id, num3, 0) != "")
				{
					num3++;
				}
				int num6 = num3;
				if (num6 != 0)
				{
					num3 = 0;
					num5 = -1;
					while (ListViewItem1.GetListViewItem(intPtr4, id, num3, 0) != "")
					{
						if (!ListViewItem1.GetListViewItem(intPtr4, id, num3, 0).Contains(".**"))
						{
							num3++;
						}
						else
						{
							num5 = random.Next(0, num6);
							while (!ListViewItem1.GetListViewItem(intPtr4, id, num5, 0).Contains(".**"))
							{
								num5 = random.Next(0, num6);
							}
							break;
						}
					}
					if (num5 != -1)
					{
						int[] numArray = new int[] { num5 };
						int num7 = 0;
						Vip72.WriteProcessMemory((int)intPtr2, num1, numArray, 4, ref num7);
						ListViewItem1.SelectListViewItem(intPtr4, id, num5);
						Vip72.ControlDoubleClick(intPtr4);
						Thread.Sleep(500);
						IntPtr intPtr5 = Vip72.ControlGetHandle(intPtr, "Button", 7303);
						IntPtr intPtr6 = Vip72.ControlGetHandle(intPtr, "Edit", 131);
						DateTime now = DateTime.Now;
						while (!Vip72.ControlGetCheck(intPtr5))
						{
							if (Vip72.ControlGetText(intPtr6).Contains("ffline"))
							{
								listViewItem = "dead";
								return listViewItem;
							}
							else if (Vip72.ControlGetText(intPtr6).Contains("limit"))
							{
								try
								{
									if (!Vip72.Vip72Process.HasExited)
									{
										Vip72.Vip72Process.Kill();
									}
								}
								catch (Exception exception)
								{
								}
								listViewItem = "limited";
								return listViewItem;
							}
							else if (Vip72.ControlGetText(intPtr6).Contains("can't"))
							{
								listViewItem = "dead";
								return listViewItem;
							}
							else if (Vip72.ControlGetText(intPtr6).Contains("disconnect"))
							{
								listViewItem = "dead";
								return listViewItem;
							}
							else if (Vip72.ControlGetText(intPtr6).Contains("aximum"))
							{
								listViewItem = "maximum";
								return listViewItem;
							}
							else if ((DateTime.Now - now).TotalSeconds > 15)
							{
								listViewItem = "timeout";
								return listViewItem;
							}
						}
						Thread.Sleep(500);
						intPtr3 = Vip72.ControlGetHandle(intPtr, "SysListView32", 117);
						num3 = 0;
						while (ListViewItem1.GetListViewItem(intPtr3, id, num3, 4) != "")
						{
							if (!ListViewItem1.GetListViewItem(intPtr3, id, num3, 4).Contains("main stream"))
							{
								num3++;
							}
							else
							{
								listViewItem = ListViewItem1.GetListViewItem(intPtr3, id, num3, 0);
								return listViewItem;
							}
						}
						listViewItem = "limited";
					}
					else
					{
						listViewItem = "no IP";
					}
				}
				else
				{
					listViewItem = "no IP";
				}
			}
			else
			{
				listViewItem = "not running";
			}
			return listViewItem;
		}

		public static void ControlClick(IntPtr hwnd)
		{
			int num = 0;
			Vip72.SendMessageTimeout(hwnd, 513, IntPtr.Zero, IntPtr.Zero, 2, 5000, out num);
			Vip72.SendMessageTimeout(hwnd, 514, IntPtr.Zero, IntPtr.Zero, 2, 5000, out num);
		}

		public static void ControlDoubleClick(IntPtr hwnd)
		{
			int num = 0;
			Vip72.SendMessageTimeout(hwnd, 515, IntPtr.Zero, IntPtr.Zero, 2, 5000, out num);
		}

		private static bool ControlGetCheck(IntPtr controlhand)
		{
			int num;
			Vip72.SendMessageTimeout(controlhand, 240, IntPtr.Zero, IntPtr.Zero, 2, 5000, out num);
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
					zero1 = Vip72.FindWindowEx(parentHandle, intPtr, _controlClass, null);
					windowLong = (int)Vip72.GetWindowLong(zero1, -12);
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
					zero1 = Vip72.FindWindowEx(parentHandle, intPtr, _controlClass, null);
					windowLong = (int)Vip72.GetWindowLong(zero1, -16);
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
			IntPtr intPtr = Vip72.FindWindow(_class, _text);
			if (intPtr != IntPtr.Zero)
			{
				IntPtr zero1 = IntPtr.Zero;
				IntPtr intPtr1 = IntPtr.Zero;
				int windowLong = -1;
				while (windowLong != ID)
				{
					intPtr1 = Vip72.FindWindowEx(intPtr, zero1, _controlClass, null);
					windowLong = (int)Vip72.GetWindowLong(intPtr1, -12);
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
			flag = (((int)Vip72.GetWindowLong(controlhandle, -16) & state) == 0 ? false : true);
			return flag;
		}

		public static string ControlGetText(IntPtr hwnd)
		{
			StringBuilder stringBuilder = new StringBuilder(255);
			int num = 0;
			Vip72.SendMessageTimeout(hwnd, 13, stringBuilder.Capacity, stringBuilder, 2, 5000, out num);
			return stringBuilder.ToString();
		}

		public static int ControlSetText(IntPtr hwnd, string text)
		{
			return Vip72.SendMessage(hwnd, 12, text.Length, text);
		}

		[DllImport("user32", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		private static extern bool EnumThreadWindows(int threadId, Vip72.EnumWindowsProc callback, IntPtr lParam);

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern IntPtr FindWindow(string sClass, string sWindow);

		[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=false)]
		private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		public static IntPtr FindWindowInProcess(Process process, Func<string, bool> compareTitle)
		{
			IntPtr intPtr;
			IntPtr zero = IntPtr.Zero;
			if (process != null)
			{
				foreach (ProcessThread thread in process.Threads)
				{
					zero = Vip72.FindWindowInThread(thread.Id, compareTitle);
					if (zero != IntPtr.Zero)
					{
						break;
					}
				}
				intPtr = zero;
			}
			else
			{
				intPtr = zero;
			}
			return intPtr;
		}

		private static IntPtr FindWindowInThread(int threadId, Func<string, bool> compareTitle)
		{
			IntPtr zero = IntPtr.Zero;
			Vip72.EnumThreadWindows(threadId, (IntPtr hWnd, IntPtr lParam) => {
				bool flag;
				StringBuilder stringBuilder = new StringBuilder(200);
				Vip72.GetWindowText(hWnd, stringBuilder, 200);
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

		public bool getip(byte countryindex)
		{
			bool flag;
			byte[] numArray = new byte[1];
			int[] numArray1 = new int[1];
			numArray[0] = countryindex;
			int num = 4482683;
			if ((Vip72.Vip72Process == null ? false : !Vip72.Vip72Process.HasExited))
			{
				IntPtr intPtr = Vip72.FindWindowInProcess(Vip72.Vip72Process, (string s) => s.StartsWith("VIP72 Socks Client"));
				IntPtr intPtr1 = Vip72.OpenProcess(2035711, false, Vip72.Vip72Process.Id);
				int num1 = 0;
				Vip72.WriteProcessMemory((int)intPtr1, num, numArray, 1, ref num1);
				numArray1[0] = 0;
				if (File.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "citycode\\", countryindex.ToString(), ".dat")))
				{
					string[] strArrays = File.ReadAllLines(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "citycode\\", countryindex.ToString(), ".dat"));
					Random random = new Random();
					string str = strArrays[random.Next(0, strArrays.Count<string>())];
					int num2 = Convert.ToInt32(str.Split(new string[] { "|" }, StringSplitOptions.None)[1]);
					numArray1[0] = num2;
				}
				Vip72.WriteProcessMemory((int)intPtr1, num + 1, numArray1, 4, ref num1);
				IntPtr intPtr2 = Vip72.ControlGetHandle(intPtr, "Button", 127);
				Vip72.ControlClick(intPtr2);
				Vip72.ControlGetHandle(intPtr, "Edit", 131);
				DateTime now = DateTime.Now;
				while (Vip72.ControlGetState(intPtr2, 134217728))
				{
					Thread.Sleep(100);
					if (Vip72.Vip72Process.HasExited)
					{
						flag = false;
						return flag;
					}
					else if ((!Vip72.Vip72Process.Responding ? true : (DateTime.Now - now).TotalSeconds > 30))
					{
						try
						{
							if (!Vip72.Vip72Process.HasExited)
							{
								Vip72.Vip72Process.Kill();
							}
						}
						catch (Exception exception)
						{
						}
						flag = false;
						return flag;
					}
				}
			}
			flag = true;
			return flag;
		}

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

		[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=false)]
		public static extern IntPtr GetWindowLong(IntPtr hwnd, int nIndex);

		[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=false)]
		public static extern int GetWindowText(IntPtr hwnd, IntPtr windowString, int maxcount);

		[DllImport("user32", CharSet=CharSet.Auto, ExactSpelling=false, SetLastError=true)]
		private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxCount);

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		internal static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int dwSize, out int lpNumberOfBytesRead);

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern int SendMessage(IntPtr hWnd, int msg, int Param, StringBuilder text);

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern int SendMessage(IntPtr hWnd, int msg, int Param, string text);

		[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=false)]
		public static extern int SendMessage(IntPtr hWnd, int msg, IntPtr Param, IntPtr text);

		[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=false, SetLastError=true)]
		public static extern int SendMessageTimeout(IntPtr hWnd, int Msg, int wParam, StringBuilder lParam, int fuFlags, int uTimeout, out int lpdwResult);

		[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=false, SetLastError=true)]
		public static extern int SendMessageTimeout(IntPtr hWnd, int Msg, int wParam, string lParam, int fuFlags, int uTimeout, out int lpdwResult);

		[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=false, SetLastError=true)]
		public static extern int SendMessageTimeout(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam, int fuFlags, int uTimeout, out int lpdwResult);

		public bool vip72login(string username, string password, int mainPort)
		{
			bool flag;
			Thread.Sleep(1000);
			Process[] processByName = this.GetProcessByName("vip72socks");
			int num = 0;
			while (num < (int)processByName.Length)
			{
				Process process = processByName[num];
				if (!Vip72.ControlGetText(Vip72.ControlGetHandle(Vip72.FindWindowInProcess(process, (string s) => s.StartsWith("VIP72 Socks Client")), "Static", 7955)).EndsWith(string.Concat(":", mainPort.ToString())))
				{
					num++;
				}
				else
				{
					Vip72.Vip72Process = process;
					break;
				}
			}
			if ((Vip72.Vip72Process == null || Vip72.Vip72Process.HasExited ? false : Vip72.Vip72Process.Responding))
			{
				if (!Vip72.ControlGetText(Vip72.ControlGetHandle(Vip72.FindWindowInProcess(Vip72.Vip72Process, (string s) => s.StartsWith("VIP72 Socks Client")), "Static", 7955)).EndsWith(string.Concat(":", mainPort.ToString())))
				{
					if (!Vip72.Vip72Process.HasExited)
					{
						Vip72.Vip72Process.Kill();
					}
					Vip72.Vip72Process = null;
				}
			}
			else if ((Vip72.Vip72Process == null ? false : !Vip72.Vip72Process.Responding))
			{
				if (!Vip72.Vip72Process.HasExited)
				{
					Vip72.Vip72Process.Kill();
				}
				Vip72.Vip72Process = null;
			}
			if ((Vip72.Vip72Process == null ? true : Vip72.Vip72Process.HasExited))
			{
				ProcessStartInfo processStartInfo = new ProcessStartInfo()
				{
					FileName = "vip72socks.exe",
					WorkingDirectory = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "vip72"),
					Arguments = string.Concat("-mp:", mainPort.ToString())
				};
				Vip72.Vip72Process = Process.Start(processStartInfo);
				IntPtr intPtr = Vip72.OpenProcess(2035711, false, Vip72.Vip72Process.Id);
				byte[] numArray = new byte[] { 235 };
				int num1 = 0;
				int num2 = 4234074;
				Vip72.WriteProcessMemory((int)intPtr, num2, numArray, 1, ref num1);
			}
			Thread.Sleep(500);
			IntPtr intPtr1 = Vip72.FindWindowInProcess(Vip72.Vip72Process, (string s) => s.StartsWith("VIP72 Socks Client"));
			IntPtr intPtr2 = Vip72.ControlGetHandle(intPtr1, "Button", 119);
			IntPtr intPtr3 = Vip72.ControlGetHandle(intPtr1, "Static", 7955);
			if (intPtr2 == IntPtr.Zero)
			{
				flag = this.vip72login(username, password, mainPort);
			}
			else
			{
				Vip72.ControlSetText(intPtr3, string.Concat("Port mac dinh:", mainPort.ToString()));
				if (!Vip72.ControlGetState(intPtr2, 134217728))
				{
					IntPtr intPtr4 = Vip72.ControlGetHandle(intPtr1, "Edit", 303);
					Vip72.ControlSetText(intPtr4, username);
					IntPtr intPtr5 = Vip72.ControlGetHandle(intPtr1, "Edit", 301);
					Vip72.ControlSetText(intPtr5, password);
					Vip72.ControlClick(intPtr2);
					IntPtr intPtr6 = Vip72.ControlGetHandle(intPtr1, "Edit", 131);
					DateTime now = DateTime.Now;
					while (Vip72.ControlGetText(intPtr6) != "System ready")
					{
						if (Vip72.ControlGetText(intPtr6) == "ERROR!Login and Password is incorrect")
						{
							flag = false;
							return flag;
						}
						else if (Vip72.ControlGetState(intPtr2, 134217728))
						{
							Thread.Sleep(100);
							if ((DateTime.Now - now).Seconds > 20)
							{
								flag = false;
								return flag;
							}
						}
						else
						{
							flag = false;
							return flag;
						}
					}
					Thread.Sleep(3000);
				}
				flag = true;
			}
			return flag;
		}

		public void waitiotherVIP72()
		{
		}

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		private static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		private static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, int[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

		public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);
	}
}