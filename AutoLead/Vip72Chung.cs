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
	public class Vip72Chung
	{
		public const int WM_LBUTTONDOWN = 513;

		public const int WM_LBUTTONUP = 514;

		public Vip72Chung()
		{
		}

		public void clearip()
		{
			Process[] processByName = this.GetProcessByName("vip72socks");
			if (processByName.Count<Process>() > 0)
			{
				IntPtr intPtr = Vip72Chung.FindWindowInProcess(processByName[0], (string s) => s.StartsWith("VIP72 Socks Client"));
				Vip72Chung.OpenProcess(2035711, false, processByName[0].Id);
				byte[] numArray = new byte[] { 144, 144 };
				IntPtr intPtr1 = Vip72Chung.ControlGetHandle(intPtr, "SysListView32", 117);
				for (int i = 0; i < 30; i++)
				{
					int num = 0;
					Vip72Chung.SendMessageTimeout(intPtr1, 256, (IntPtr)46, IntPtr.Zero, 2, 5000, out num);
				}
			}
		}

		public void clearIpWithPort(int port)
		{
			Process[] processByName = this.GetProcessByName("vip72socks");
			if (processByName.Count<Process>() != 0)
			{
				uint id = (uint)processByName[0].Id;
				IntPtr intPtr = Vip72Chung.FindWindowInProcess(processByName[0], (string s) => s.StartsWith("VIP72 Socks Client"));
				IntPtr intPtr1 = Vip72Chung.ControlGetHandle(intPtr, "SysListView32", 117);
				for (int i = 0; ListViewItem1.GetListViewItem(intPtr1, id, i, 4) != ""; i++)
				{
					string listViewItem = ListViewItem1.GetListViewItem(intPtr1, id, i, 4);
					if ((listViewItem.Contains(port.ToString()) ? true : listViewItem.Contains("main stream")))
					{
						ListViewItem1.SelectListViewItem(intPtr1, id, i);
						int num = 0;
						Vip72Chung.SendMessageTimeout(intPtr1, 256, (IntPtr)46, IntPtr.Zero, 2, 5000, out num);
						i--;
					}
				}
			}
		}

		public string clickip(int port)
		{
			string listViewItem;
			int num = 4328465;
			int num1 = 4324611;
			int num2 = 4324304;
			int num3 = 4253085;
			Process[] processByName = this.GetProcessByName("vip72socks");
			if (processByName.Count<Process>() != 0)
			{
				IntPtr intPtr = Vip72Chung.FindWindowInProcess(processByName[0], (string s) => s.StartsWith("VIP72 Socks Client"));
				IntPtr intPtr1 = Vip72Chung.ControlGetHandle(intPtr, "Button", 7817);
				if (Vip72Chung.ControlGetCheck(intPtr1))
				{
					Vip72Chung.ControlClick(intPtr1);
				}
				IntPtr intPtr2 = Vip72Chung.OpenProcess(2035711, false, processByName[0].Id);
				int num4 = 0;
				IntPtr zero = IntPtr.Zero;
				zero = Marshal.AllocHGlobal(4);
				Vip72Chung.ReadProcessMemory(intPtr2, (IntPtr)num, zero, 4, out num4);
				Random random = new Random();
				uint id = (uint)processByName[0].Id;
				IntPtr intPtr3 = Vip72Chung.ControlGetHandle(intPtr, "SysListView32", 117);
				int num5 = 0;
				while (ListViewItem1.GetListViewItem(intPtr3, id, num5, 4) != "")
				{
					string str = ListViewItem1.GetListViewItem(intPtr3, id, num5, 4);
					if ((str.Contains(port.ToString()) ? false : !str.Contains("main stream")))
					{
						num5++;
					}
					else
					{
						ListViewItem1.SelectListViewItem(intPtr3, id, num5);
						int num6 = 0;
						Vip72Chung.SendMessageTimeout(intPtr3, 256, (IntPtr)46, IntPtr.Zero, 2, 5000, out num6);
					}
				}
				int num7 = -1;
				IntPtr intPtr4 = Vip72Chung.ControlGetHandle(intPtr, "SysListView32", 116);
				while (ListViewItem1.GetListViewItem(intPtr4, id, num5, 0) != "")
				{
					num5++;
				}
				int num8 = num5;
				if (num8 != 0)
				{
					num5 = 0;
					num7 = -1;
					while (ListViewItem1.GetListViewItem(intPtr4, id, num5, 0) != "")
					{
						if (!ListViewItem1.GetListViewItem(intPtr4, id, num5, 0).Contains(".**"))
						{
							num5++;
						}
						else
						{
							num7 = random.Next(0, num8);
							while (!ListViewItem1.GetListViewItem(intPtr4, id, num7, 0).Contains(".**"))
							{
								num7 = random.Next(0, num8);
							}
							break;
						}
					}
					if (num7 != -1)
					{
						int[] numArray = new int[] { 472809 };
						int[] numArray1 = new int[] { port };
						int[] numArray2 = new int[] { num7 };
						int num9 = 0;
						Vip72Chung.WriteProcessMemory((int)intPtr2, num1, numArray1, 4, ref num9);
						Vip72Chung.WriteProcessMemory((int)intPtr2, num3, numArray, 4, ref num9);
						Vip72Chung.WriteProcessMemory((int)intPtr2, num2, numArray2, 4, ref num9);
						ListViewItem1.SelectListViewItem(intPtr4, id, num7);
						Vip72Chung.ControlDoubleClick(intPtr4);
						Thread.Sleep(500);
						numArray[0] = 21529871;
						Vip72Chung.WriteProcessMemory((int)intPtr2, num3, numArray, 4, ref num9);
						IntPtr intPtr5 = Vip72Chung.ControlGetHandle(intPtr, "Button", 7303);
						IntPtr intPtr6 = Vip72Chung.ControlGetHandle(intPtr, "Edit", 131);
						DateTime now = DateTime.Now;
						while (!Vip72Chung.ControlGetCheck(intPtr5))
						{
							if (Vip72Chung.ControlGetText(intPtr6).Contains("ffline"))
							{
								listViewItem = "dead";
								return listViewItem;
							}
							else if (Vip72Chung.ControlGetText(intPtr6).Contains("limit"))
							{
								try
								{
									if (!processByName[0].HasExited)
									{
										processByName[0].Kill();
									}
								}
								catch (Exception exception)
								{
								}
								listViewItem = "limited";
								return listViewItem;
							}
							else if (Vip72Chung.ControlGetText(intPtr6).Contains("can't"))
							{
								listViewItem = "dead";
								return listViewItem;
							}
							else if (Vip72Chung.ControlGetText(intPtr6).Contains("disconnect"))
							{
								listViewItem = "dead";
								return listViewItem;
							}
							else if (Vip72Chung.ControlGetText(intPtr6).Contains("aximum"))
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
						intPtr3 = Vip72Chung.ControlGetHandle(intPtr, "SysListView32", 117);
						num5 = 0;
						while (ListViewItem1.GetListViewItem(intPtr3, id, num5, 4) != "")
						{
							if (!ListViewItem1.GetListViewItem(intPtr3, id, num5, 4).Contains(port.ToString()))
							{
								num5++;
							}
							else
							{
								listViewItem = ListViewItem1.GetListViewItem(intPtr3, id, num5, 0);
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
			Vip72Chung.SendMessageTimeout(hwnd, 513, IntPtr.Zero, IntPtr.Zero, 2, 5000, out num);
			Vip72Chung.SendMessageTimeout(hwnd, 514, IntPtr.Zero, IntPtr.Zero, 2, 5000, out num);
		}

		public static void ControlDoubleClick(IntPtr hwnd)
		{
			int num = 0;
			Vip72Chung.SendMessageTimeout(hwnd, 515, IntPtr.Zero, IntPtr.Zero, 2, 5000, out num);
		}

		private static bool ControlGetCheck(IntPtr controlhand)
		{
			int num;
			Vip72Chung.SendMessageTimeout(controlhand, 240, IntPtr.Zero, IntPtr.Zero, 2, 5000, out num);
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
					zero1 = Vip72Chung.FindWindowEx(parentHandle, intPtr, _controlClass, null);
					windowLong = (int)Vip72Chung.GetWindowLong(zero1, -12);
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
					zero1 = Vip72Chung.FindWindowEx(parentHandle, intPtr, _controlClass, null);
					windowLong = (int)Vip72Chung.GetWindowLong(zero1, -16);
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
			IntPtr intPtr = Vip72Chung.FindWindow(_class, _text);
			if (intPtr != IntPtr.Zero)
			{
				IntPtr zero1 = IntPtr.Zero;
				IntPtr intPtr1 = IntPtr.Zero;
				int windowLong = -1;
				while (windowLong != ID)
				{
					intPtr1 = Vip72Chung.FindWindowEx(intPtr, zero1, _controlClass, null);
					windowLong = (int)Vip72Chung.GetWindowLong(intPtr1, -12);
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
			flag = (((int)Vip72Chung.GetWindowLong(controlhandle, -16) & state) == 0 ? false : true);
			return flag;
		}

		public static string ControlGetText(IntPtr hwnd)
		{
			StringBuilder stringBuilder = new StringBuilder(255);
			int num = 0;
			Vip72Chung.SendMessageTimeout(hwnd, 13, stringBuilder.Capacity, stringBuilder, 2, 5000, out num);
			return stringBuilder.ToString();
		}

		public static int ControlSetText(IntPtr hwnd, string text)
		{
			return Vip72Chung.SendMessage(hwnd, 12, text.Length, text);
		}

		[DllImport("user32", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		private static extern bool EnumThreadWindows(int threadId, Vip72Chung.EnumWindowsProc callback, IntPtr lParam);

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern IntPtr FindWindow(string sClass, string sWindow);

		[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=false)]
		private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		public static IntPtr FindWindowInProcess(Process process, Func<string, bool> compareTitle)
		{
			IntPtr zero = IntPtr.Zero;
			foreach (ProcessThread thread in process.Threads)
			{
				zero = Vip72Chung.FindWindowInThread(thread.Id, compareTitle);
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
			Vip72Chung.EnumThreadWindows(threadId, (IntPtr hWnd, IntPtr lParam) => {
				bool flag;
				StringBuilder stringBuilder = new StringBuilder(200);
				Vip72Chung.GetWindowText(hWnd, stringBuilder, 200);
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
			Process[] processByName = this.GetProcessByName("vip72socks");
			Process process = new Process();
			if (processByName.Count<Process>() > 0)
			{
				process = processByName[0];
				IntPtr intPtr = Vip72Chung.FindWindowInProcess(process, (string s) => s.StartsWith("VIP72 Socks Client"));
				IntPtr intPtr1 = Vip72Chung.OpenProcess(2035711, false, process.Id);
				int num1 = 0;
				Vip72Chung.WriteProcessMemory((int)intPtr1, num, numArray, 1, ref num1);
				numArray1[0] = 0;
				if (File.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "citycode\\", countryindex.ToString(), ".dat")))
				{
					string[] strArrays = File.ReadAllLines(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "citycode\\", countryindex.ToString(), ".dat"));
					Random random = new Random();
					string str = strArrays[random.Next(0, strArrays.Count<string>())];
					int num2 = Convert.ToInt32(str.Split(new string[] { "|" }, StringSplitOptions.None)[1]);
					numArray1[0] = num2;
				}
				Vip72Chung.WriteProcessMemory((int)intPtr1, num + 1, numArray1, 4, ref num1);
				IntPtr intPtr2 = Vip72Chung.ControlGetHandle(intPtr, "Button", 127);
				Vip72Chung.ControlClick(intPtr2);
				Vip72Chung.ControlGetHandle(intPtr, "Edit", 131);
				DateTime now = DateTime.Now;
				while (Vip72Chung.ControlGetState(intPtr2, 134217728))
				{
					Thread.Sleep(100);
					Process[] processArray = this.GetProcessByName("vip72socks");
					if (processArray.Count<Process>() == 0)
					{
						flag = false;
						return flag;
					}
					else if ((!processArray[0].Responding ? true : (DateTime.Now - now).TotalSeconds > 30))
					{
						try
						{
							if (!processArray[0].HasExited)
							{
								processArray[0].Kill();
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
			if (processByName.Count<Process>() > 0)
			{
				if (!processByName[0].Responding)
				{
					try
					{
						if (!processByName[0].HasExited)
						{
							processByName[0].Kill();
						}
					}
					catch (Exception exception)
					{
					}
					flag = this.vip72login(username, password, mainPort);
					return flag;
				}
			}
			IntPtr zero = IntPtr.Zero;
			if (processByName.Count<Process>() > 0)
			{
				zero = Vip72Chung.FindWindowInProcess(processByName[0], (string s) => s.StartsWith("VIP72 Socks Client"));
			}
			IntPtr intPtr = Vip72Chung.ControlGetHandle(zero, "Button", 119);
			if (intPtr == IntPtr.Zero)
			{
				processByName = this.GetProcessByName("vip72socks");
				if (processByName.Count<Process>() != 0)
				{
					try
					{
						if (!processByName[0].HasExited)
						{
							processByName[0].Kill();
						}
					}
					catch (Exception exception1)
					{
					}
				}
				ProcessStartInfo processStartInfo = new ProcessStartInfo()
				{
					FileName = "vip72socks.exe",
					WorkingDirectory = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "vip72")
				};
				Process.Start(processStartInfo);
				Thread.Sleep(500);
				flag = (!this.vip72login(username, password, mainPort) ? false : true);
			}
			else
			{
				if (!Vip72Chung.ControlGetState(intPtr, 134217728))
				{
					IntPtr intPtr1 = Vip72Chung.ControlGetHandle(zero, "Edit", 303);
					Vip72Chung.ControlSetText(intPtr1, username);
					IntPtr intPtr2 = Vip72Chung.ControlGetHandle(zero, "Edit", 301);
					Vip72Chung.ControlSetText(intPtr2, password);
					Vip72Chung.ControlClick(intPtr);
					IntPtr intPtr3 = Vip72Chung.ControlGetHandle(zero, "Edit", 131);
					DateTime now = DateTime.Now;
					while (Vip72Chung.ControlGetText(intPtr3) != "System ready")
					{
						if (processByName.Count<Process>() > 0)
						{
							processByName = this.GetProcessByName("vip72socks");
							if (!processByName[0].Responding)
							{
								try
								{
									if (!processByName[0].HasExited)
									{
										processByName[0].Kill();
									}
								}
								catch (Exception exception2)
								{
								}
								if (!this.vip72login(username, password, mainPort))
								{
									flag = false;
									return flag;
								}
								else
								{
									flag = true;
									return flag;
								}
							}
						}
						if (processByName.Count<Process>() == 0)
						{
							flag = false;
							return flag;
						}
						else if (Vip72Chung.ControlGetText(intPtr3) == "ERROR!Login and Password is incorrect")
						{
							flag = false;
							return flag;
						}
						else if (Vip72Chung.ControlGetState(intPtr, 134217728))
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
			bool flag = true;
			Process[] processesByName = Process.GetProcessesByName("AutoLead");
			while (true)
			{
				flag = true;
				Process[] processArray = processesByName;
				for (int i = 0; i < (int)processArray.Length; i++)
				{
					Process process = processArray[i];
					if (Process.GetCurrentProcess().Id != process.Id)
					{
						if (Vip72Chung.ControlGetText(Vip72Chung.ControlGetHandle(Vip72Chung.FindWindowInProcess(process, (string x) => x.StartsWith("")), "WindowsForms10.STATIC.app.0.141b42a_r12_ad1", 1442840576, true)) == "Getting Vip72 IP...")
						{
							flag = false;
						}
					}
				}
				if (flag)
				{
					break;
				}
				Thread.Sleep(500);
			}
		}

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		private static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		private static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, int[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

		public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);
	}
}