using System;
using System.Runtime.CompilerServices;

namespace AutoLead
{
	internal class offerItem
	{
		public string appID
		{
			get;
			set;
		}

		public string appName
		{
			get;
			set;
		}

		public bool offerEnable
		{
			get;
			set;
		}

		public string offerName
		{
			get;
			set;
		}

		public string offerURL
		{
			get;
			set;
		}

		public int range1
		{
			get;
			set;
		}

		public int range2
		{
			get;
			set;
		}

		public string referer
		{
			get;
			set;
		}

		public string script
		{
			get;
			set;
		}

		public int timeSleep
		{
			get;
			set;
		}

		public int timeSleepBefore
		{
			get;
			set;
		}

		public bool timeSleepBeforeRandom
		{
			get;
			set;
		}

		public bool useScript
		{
			get;
			set;
		}

		public offerItem()
		{
		}
	}
}