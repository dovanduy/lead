using System;
using System.Runtime.CompilerServices;

namespace AutoLead
{
	internal class Script
	{
		public string script
		{
			get;
			set;
		}

		public string scriptname
		{
			get;
			set;
		}

		public int slot
		{
			get;
			set;
		}

		public Script()
		{
			this.scriptname = "";
			this.script = "";
			this.slot = 0;
		}
	}
}