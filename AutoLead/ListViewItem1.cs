using System;
using System.Runtime.InteropServices;

namespace AutoLead
{
	public class ListViewItem1
	{
		private readonly static int GWL_ID;

		static ListViewItem1()
		{
			ListViewItem1.GWL_ID = -12;
		}

		public ListViewItem1()
		{
		}

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		internal static extern bool CloseHandle(IntPtr hObject);

		public static void DoubleClickListView(IntPtr hwnd, uint processId, int item, int subitem)
		{
			NMHDR nMHDR = new NMHDR();
			POINT pOINT = new POINT();
			IntPtr zero = IntPtr.Zero;
			IntPtr intPtr = IntPtr.Zero;
			nMHDR.hwndFrom = (int)hwnd;
			nMHDR.idFrom = 116;
			nMHDR.code = 515;
			IntPtr zero1 = IntPtr.Zero;
			zero1 = ListViewItem1.OpenProcess(Win32ProcessAccessType.AllAccess, false, processId);
			zero = ListViewItem1.VirtualAllocEx(zero1, IntPtr.Zero, 2048, Win32AllocationTypes.MEM_COMMIT, Win32MemoryProtection.PAGE_READWRITE);
			IntPtr intPtr1 = IntPtr.Zero;
			int num = 0;
			ListViewItem1.WriteProcessMemory(zero1, zero, ref nMHDR, (uint)Marshal.SizeOf(typeof(NMHDR)), out num);
			pOINT.x = 31;
			pOINT.y = 31;
			intPtr = ListViewItem1.VirtualAllocEx(zero1, IntPtr.Zero, 2048, Win32AllocationTypes.MEM_COMMIT, Win32MemoryProtection.PAGE_READWRITE);
			ListViewItem1.WriteProcessMemory(zero1, intPtr, ref pOINT, (uint)Marshal.SizeOf(typeof(POINT)), out num);
			NMITEMACTIVATE nMITEMACTIVATE = new NMITEMACTIVATE()
			{
				hdr = zero,
				iItem = item,
				iSubItem = subitem,
				uOldState = 2,
				uNewState = 0,
				ptAction = intPtr
			};
			IntPtr zero2 = IntPtr.Zero;
			zero2 = ListViewItem1.VirtualAllocEx(zero1, IntPtr.Zero, 2048, Win32AllocationTypes.MEM_COMMIT, Win32MemoryProtection.PAGE_READWRITE);
			ListViewItem1.WriteProcessMemory(zero1, zero2, ref nMITEMACTIVATE, (uint)Marshal.SizeOf(typeof(NMITEMACTIVATE)), out num);
			ListViewItem1.SendMessageTimeout(hwnd, 78, (IntPtr)116, zero2, 2, 5000, IntPtr.Zero);
		}

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern int GetDlgCtrlID(IntPtr hwndCtl);

		public static string GetListViewItem(IntPtr hwnd, uint processId, int item, int subItem = 0)
		{
			unsafe
			{
				string stringUni;
				int num = 0;
				IntPtr zero = IntPtr.Zero;
				IntPtr intPtr = IntPtr.Zero;
				IntPtr zero1 = IntPtr.Zero;
				try
				{
					LV_ITEM lVITEM = new LV_ITEM();
					zero1 = Marshal.AllocHGlobal(2048);
					zero = ListViewItem1.OpenProcess(Win32ProcessAccessType.AllAccess, false, processId);
					if (zero == IntPtr.Zero)
					{
						throw new ApplicationException("Failed to access process!");
					}
					intPtr = ListViewItem1.VirtualAllocEx(zero, IntPtr.Zero, 2048, Win32AllocationTypes.MEM_COMMIT, Win32MemoryProtection.PAGE_READWRITE);
					if (intPtr == IntPtr.Zero)
					{
						throw new SystemException("Failed to allocate memory in remote process");
					}
					lVITEM.mask = 1;
					lVITEM.iItem = item;
					lVITEM.iSubItem = subItem;
					lVITEM.pszText = (char*)(intPtr.ToInt32() + Marshal.SizeOf(typeof(LV_ITEM)));
					lVITEM.cchTextMax = 500;
					if (!ListViewItem1.WriteProcessMemory(zero, intPtr, ref lVITEM, (uint)Marshal.SizeOf(typeof(LV_ITEM)), out num))
					{
						throw new SystemException("Failed to write to process memory");
					}
					ListViewItem1.SendMessageTimeout(hwnd, 4171, IntPtr.Zero, intPtr, 2, 5000, IntPtr.Zero);
					if (!ListViewItem1.ReadProcessMemory(zero, intPtr, zero1, 2048, out num))
					{
						throw new SystemException("Failed to read from process memory");
					}
					stringUni = Marshal.PtrToStringUni((IntPtr)(zero1.ToInt32() + Marshal.SizeOf(typeof(LV_ITEM))));
				}
				finally
				{
					if (zero1 != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(zero1);
					}
					if (intPtr != IntPtr.Zero)
					{
						ListViewItem1.VirtualFreeEx(zero, intPtr, 0, Win32AllocationTypes.MEM_RELEASE);
					}
					if (zero != IntPtr.Zero)
					{
						ListViewItem1.CloseHandle(zero);
					}
				}
				return stringUni;
			}
		}

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern IntPtr OpenProcess(Win32ProcessAccessType dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		internal static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int dwSize, out int lpNumberOfBytesRead);

		public static void SelectListViewItem(IntPtr hwnd, uint processId, int item)
		{
			IntPtr zero = IntPtr.Zero;
			IntPtr intPtr = IntPtr.Zero;
			IntPtr zero1 = IntPtr.Zero;
			LV_ITEM lVITEM = new LV_ITEM();
			zero1 = Marshal.AllocHGlobal(2048);
			zero = ListViewItem1.OpenProcess(Win32ProcessAccessType.AllAccess, false, processId);
			intPtr = ListViewItem1.VirtualAllocEx(zero, IntPtr.Zero, 2048, Win32AllocationTypes.MEM_COMMIT, Win32MemoryProtection.PAGE_READWRITE);
			lVITEM.state = 3;
			lVITEM.stateMask = 3;
			IntPtr intPtr1 = IntPtr.Zero;
			int num = 0;
			ListViewItem1.WriteProcessMemory(zero, intPtr, ref lVITEM, (uint)Marshal.SizeOf(typeof(LV_ITEM)), out num);
			ListViewItem1.SendMessageTimeout(hwnd, 4139, (IntPtr)item, intPtr, 2, 5000, IntPtr.Zero);
			ListViewItem1.VirtualFreeEx(zero, intPtr, 0, Win32AllocationTypes.MEM_RELEASE);
			ListViewItem1.CloseHandle(zero);
		}

		[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=false, SetLastError=true)]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=false, SetLastError=true)]
		public static extern IntPtr SendMessageTimeout(IntPtr hWnd, int Msg, IntPtr wParam, string lParam, int fuFlags, int uTimeout, IntPtr lpdwResult);

		[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=false, SetLastError=true)]
		public static extern IntPtr SendMessageTimeout(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam, int fuFlags, int uTimeout, IntPtr lpdwResult);

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=true, SetLastError=true)]
		internal static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, Win32AllocationTypes flWin32AllocationType, Win32MemoryProtection flProtect);

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=true, SetLastError=true)]
		internal static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, Win32AllocationTypes dwFreeType);

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		internal static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, ref LV_ITEM lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		internal static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, ref NMITEMACTIVATE lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		internal static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, ref POINT lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		internal static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, ref NMHDR lpBuffer, uint nSize, out int lpNumberOfBytesWritten);
	}
}