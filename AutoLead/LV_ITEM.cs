using System;

namespace AutoLead
{
	internal struct LV_ITEM
	{
		public int mask;

		public int iItem;

		public int iSubItem;

		public int state;

		public int stateMask;

		public unsafe char* pszText;

		public int cchTextMax;

		public int iImage;

		public int lParam;

		public int iIndent;
	}
}