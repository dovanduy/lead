using Routrek.SSHC;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace AutoLead
{
	public class SshChecker
	{
		private SSHConnection _conn;

		private Socket _socket;

		private Reader _reader;

		private SSHConnectionParameter _f;

		public static int _sshTimeOut;

		private string Host;

		private string User;

		private string Pass;

		public static bool checkDone;

		public static bool isFresh;

		private bool SSHConnectTimeout = false;

		static SshChecker()
		{
			SshChecker.checkDone = false;
			SshChecker.isFresh = false;
		}

		public SshChecker()
		{
		}

		public bool CheckFresh(string SSH, int timeout)
		{
			bool flag;
			try
			{
				string[] strArrays = SSH.Split(new char[] { '|' });
				this.Host = strArrays[0].Trim();
				this.User = strArrays[1].Trim();
				this.Pass = strArrays[2].Trim();
				this._f = new SSHConnectionParameter()
				{
					UserName = this.User,
					Password = this.Pass,
					Protocol = SSHProtocol.SSH2,
					AuthenticationType = AuthenticationType.Password,
					WindowSize = 4096
				};
				this._reader = new Reader();
				this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
				{
					SendTimeout = timeout,
					ReceiveTimeout = timeout
				};
				this._socket.Connect(new IPEndPoint(IPAddress.Parse(this.Host), 22));
				SshChecker._sshTimeOut = timeout;
				(new Thread(new ThreadStart(this.SetSSHConnectionTimeout))).Start();
				this._conn = SSHConnection.Connect(this._f, this._reader, this._socket);
				if (this.SSHConnectTimeout)
				{
					throw new Exception("SSH Connect Timeout");
				}
				this._reader._conn = this._conn;
				SSHChannel sSHChannel = this._conn.ForwardPort(this._reader, "ipinfo.io", 80, "localhost", 80);
				this._reader._pf = sSHChannel;
				int num = timeout;
				while (true)
				{
					if ((this._reader._ready ? true : num <= 0))
					{
						break;
					}
					num--;
					Thread.Sleep(1000);
				}
				if ((this._reader._ready ? false : num <= 0))
				{
					throw new Exception("Reader._ready timeout");
				}
				this._reader.Host = this.Host;
				this._reader.User = this.User;
				this._reader.Pass = this.Pass;
				this._reader.SetHTTPRequestTimeout();
				SshChecker.checkDone = false;
				this._reader._pf.Transmit(Encoding.ASCII.GetBytes("GET /json HTTP/1.1\r\nHost:ipinfo.io\r\n\r\n"));
				DateTime now = DateTime.Now;
				while (!SshChecker.checkDone)
				{
					Thread.Sleep(100);
					if ((DateTime.Now - now).TotalSeconds > (double)timeout)
					{
						throw new Exception("Request timeout");
					}
				}
				if (!SshChecker.isFresh)
				{
					flag = false;
					return flag;
				}
			}
			catch (Exception exception)
			{
				flag = false;
				return flag;
			}
			flag = true;
			return flag;
		}

		private void SetSSHConnectionTimeout()
		{
			int num = SshChecker._sshTimeOut;
			while (num > 0)
			{
				num--;
				Thread.Sleep(1000);
				if (this._conn != null)
				{
					break;
				}
			}
			if ((num > 0 ? false : this._conn == null))
			{
				this.SSHConnectTimeout = true;
				try
				{
					this._socket.Disconnect(false);
				}
				catch
				{
				}
				try
				{
					this._socket.Close();
				}
				catch
				{
				}
				try
				{
					this._f = new SSHConnectionParameter();
				}
				catch
				{
				}
				try
				{
					this._reader = new Reader();
				}
				catch
				{
				}
			}
		}
	}
}