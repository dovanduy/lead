using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoLead
{
	internal class backup
	{
		public List<string> appList
		{
			get;
			set;
		}

		public string comment
		{
			get;
			set;
		}

		public string country
		{
			get;
			set;
		}

		public string filename
		{
			get;
			set;
		}

		public int runtime
		{
			get;
			set;
		}

		public DateTime timecreate
		{
			get;
			set;
		}

		public DateTime timemod
		{
			get;
			set;
		}

		public backup()
		{
		}
	}
}