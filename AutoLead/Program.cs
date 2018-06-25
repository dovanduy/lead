using System;
using System.Linq;
using System.Windows.Forms;

namespace AutoLead
{
	internal static class Program
	{
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			if (args.Count<string>() <= 0)
			{
				Application.Run(new Form1("none"));
			}
			else
			{
				Application.Run(new Form1(args[0]));
			}
		}
	}
}