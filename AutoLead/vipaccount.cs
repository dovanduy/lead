using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Resources;

namespace AutoLead
{
	public class vipaccount
	{
		public bool limited
		{
			get;
			set;
		}

		public string password
		{
			get;
			set;
		}

		public string username
		{
			get;
			set;
		}

		public vipaccount()
		{
		}
	}

    public class luminatio_account
    {
        public bool bad
        {
            get;
            set;
        }
        public string zone
        {
            get;
            set;
        }

        public string password
        {
            get;
            set;
        }

        public string username
        {
            get;
            set;
        }

        public luminatio_account()
        {
        }
    }


    public class invoke
    {
        public MethodInvoker methodInvoker;
        public MethodInvoker methodInvoker1 = null;
        public MethodInvoker methodInvoker2 = null;
        public MethodInvoker methodInvoker3 = null;
        public MethodInvoker methodInvoker4 = null;
        public MethodInvoker methodInvoker5 = null;
        public dynamic vip72Chung;
        public dynamic Lumi;
        public DateTime now;
        public Uri uri;
        public string value = null;
        public string str = null;
        public string text = null;
        public bool flag;
        public bool flag1;
        public bool checked1 = false;
        public bool checked2 = false;
        public bool @checked;

    }

  
}