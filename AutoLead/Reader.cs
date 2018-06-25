using Routrek.SSHC;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace AutoLead
{
	public class Reader : ISSHConnectionEventReceiver, ISSHChannelEventReceiver
	{
		public SSHConnection _conn;

		public SSHChannel _pf;

		public bool _ready;

		public int LineIndex;

		public string Host;

		public string User;

		public string Pass;

		private DateTime StartRequest;

		private DateTime EndRequest;

		public Reader()
		{
		}

		public PortForwardingCheckResult CheckPortForwardingRequest(string host, int port, string originator_host, int originator_port)
		{
			return new PortForwardingCheckResult()
			{
				allowed = true,
				channel = this
			};
		}

		public void EstablishPortforwarding(ISSHChannelEventReceiver rec, SSHChannel channel)
		{
			this._pf = channel;
		}

		public void OnAuthenticationPrompt(string[] msg)
		{
			Debug.WriteLine(string.Concat("Auth Prompt ", msg[0]));
		}

		public void OnChannelClosed()
		{
			Debug.WriteLine("Channel closed");
			this._conn.Disconnect("");
		}

		public void OnChannelEOF()
		{
			this._pf.Close();
			Debug.WriteLine("Channel EOF");
		}

		public void OnChannelError(Exception error, string msg)
		{
			Debug.WriteLine(string.Concat("Channel ERROR: ", msg));
		}

		public void OnChannelReady()
		{
			this._ready = true;
		}

		public void OnConnectionClosed()
		{
			Debug.WriteLine("Connection closed");
		}

		public void OnData(byte[] data, int offset, int length)
		{
			this.EndRequest = DateTime.Now;
			TimeSpan endRequest = this.EndRequest - this.StartRequest;
			string str = Encoding.ASCII.GetString(data, offset, length);
			bool flag = (str.Length > 0 ? true : false);
			try
			{
				this._conn.CancelForwardedPort("localhost", 80);
			}
			catch
			{
			}
			try
			{
				this._pf.Close();
			}
			catch
			{
			}
			try
			{
				this._conn.Disconnect("");
			}
			catch
			{
			}
			try
			{
				this._conn.Close();
			}
			catch
			{
			}
			string value = "";
			Match match = Regex.Match(str, "\"country\"\\:\\s?\"(?<country>[^\"]+)\"", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			if (match.Success)
			{
				value = match.Groups["country"].Value;
			}
			string value1 = "";
			match = Regex.Match(str, "\"region\"\\:\\s?\"(?<region>[^\"]+)\"", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			if (match.Success)
			{
				value1 = match.Groups["region"].Value;
			}
			string str1 = "";
			match = Regex.Match(str, "\"city\"\\:\\s?\"(?<city>[^\"]+)\"", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			if (match.Success)
			{
				str1 = match.Groups["city"].Value;
			}
			if (!flag)
			{
				throw new Exception("HTTP Response is empty");
			}
			SshChecker.checkDone = true;
			if (flag)
			{
				SshChecker.isFresh = true;
			}
		}

		public void OnDebugMessage(bool always_display, byte[] data)
		{
			Debug.WriteLine(string.Concat("DEBUG: ", Encoding.ASCII.GetString(data)));
		}

		public void OnError(Exception error, string msg)
		{
			Debug.WriteLine(string.Concat("ERROR: ", msg));
		}

		public void OnExtendedData(int type, byte[] data)
		{
			Debug.WriteLine("EXTENDED DATA");
		}

		public void OnIgnoreMessage(byte[] data)
		{
			Debug.WriteLine(string.Concat("Ignore: ", Encoding.ASCII.GetString(data)));
		}

		public void OnMiscPacket(byte type, byte[] data, int offset, int length)
		{
		}

		public void OnUnknownMessage(byte type, byte[] data)
		{
			Debug.WriteLine(string.Concat("Unknown Message ", type));
		}

		public void SetHTTPRequestTimeout()
		{
			bool flag;
			this.StartRequest = DateTime.Now;
			int num = SshChecker._sshTimeOut;
			while (num > 0)
			{
				num--;
				Thread.Sleep(1000);
				DateTime endRequest = this.EndRequest;
				if (true)
				{
					break;
				}
			}
			if (num > 0)
			{
				flag = false;
			}
			else
			{
				DateTime dateTime = this.EndRequest;
				flag = false;
			}
			if (flag)
			{
				try
				{
					this._conn.CancelForwardedPort("localhost", 80);
				}
				catch
				{
				}
				try
				{
					this._pf.Close();
				}
				catch
				{
				}
				try
				{
					this._conn.Disconnect("");
				}
				catch
				{
				}
				try
				{
					this._conn.Close();
				}
				catch
				{
				}
				throw new Exception("SSH Connect Timeout");
			}
		}
	}
}