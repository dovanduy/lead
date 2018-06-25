using System;

namespace AutoLead
{
	[Flags]
	public enum Win32AllocationTypes
	{
		MEM_COMMIT = 4096,
		MEM_RESERVE = 8192,
		MEM_DECOMMIT = 16384,
		MEM_RELEASE = 32768,
		MEM_RESET = 524288,
		MEM_TOP_DOWN = 1048576,
		WriteWatch = 2097152,
		MEM_PHYSICAL = 4194304,
		MEM_LARGE_PAGES = 536870912
	}
}