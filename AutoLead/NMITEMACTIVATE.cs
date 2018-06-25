using System;

namespace AutoLead
{
	internal struct NMITEMACTIVATE
	{
		public IntPtr hdr;

		public int iItem;

		public int iSubItem;

		public uint uNewState;

		public uint uOldState;

		public uint uChanged;

		public IntPtr ptAction;

		public uint lParam;

		public uint uKeyFlags;
	}
}