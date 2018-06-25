using System;
using System.Text;

namespace AutoLead
{
	internal class command
	{
		public command.SendControl sendControl;

		public command()
		{
		}

		public void addProtectData(string path)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("addProtectData=", path, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void backup(string text)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("backup=", text, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void backup(string text, string filename, string comment, string timemod, string runtime)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat(new string[] { "backup=", text, "|", filename, "|", comment, timemod, "|", runtime, "{|}" }));
				this.sendControl(bytes);
			}
		}

		public void backupfull(string text)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("backupfull=", text, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void backupfull(string text, string filename, string comment, string timemod, string runtime)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat(new string[] { "backupfull=", text, "|", filename, "|", comment, "|", timemod, "|", runtime, "{|}" }));
				this.sendControl(bytes);
			}
		}

		public void changecarrier(string text)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("changecarrier=", text, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void changecarrier(string carriername, string countrycode, string carriercode, string ioscountrycode)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat(new string[] { "changecarrier=", carriername, "||", countrycode, "||", carriercode, "||", ioscountrycode, "{|}" }));
				this.sendControl(bytes);
			}
		}

		public void changedevice(string text)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("changedevice=", text, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void changelanguage(string text)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("changelanguage=", text, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void changename(string text)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("changename=", text, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void changeregion(string text)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("changeregion=", text, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void changetimezone(string text)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("changetimezone=", text, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void changeversion(string text)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("changeversion=", text, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void checkbackup(string filename)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("checkbackup=", filename, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void checkbackup()
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes("checkbackup=1{|}");
				this.sendControl(bytes);
			}
		}

		public void checkip(string text)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("checkip=", text, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void checkrestore()
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes("checkrestore=1{|}");
				this.sendControl(bytes);
			}
		}

		public void checkwipe()
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes("checkwipe=1{|}");
				this.sendControl(bytes);
			}
		}

		public void clearipa()
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes("cleanipa=1{|}");
				this.sendControl(bytes);
			}
		}

		public void close(string AppID)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("close=", AppID, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void deletebackup(string text)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("deletebackup=", text, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void disablemouse()
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes("enablemouse=NO{|}");
				this.sendControl(bytes);
			}
		}

		public void disableProxy()
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes("disableProxy={|}");
				this.sendControl(bytes);
			}
		}

		public void enablemouse()
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes("enablemouse=YES{|}");
				this.sendControl(bytes);
			}
		}

		public void excuteScript(string text)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("script=", text, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void fakeGPS(bool enable)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("locationFaker=", (enable ? "1" : "0"), "{|}"));
				this.sendControl(bytes);
			}
		}

		public void fakeGPS(bool enable, double latitude, double longitude)
		{
			if (this.sendControl != null)
			{
				Encoding unicode = Encoding.Unicode;
				string[] str = new string[] { "locationFaker=", null, null, null, null, null, null };
				str[1] = (enable ? "1" : "0");
				str[2] = "|";
				str[3] = latitude.ToString();
				str[4] = "|";
				str[5] = longitude.ToString();
				str[6] = "{|}";
				byte[] bytes = unicode.GetBytes(string.Concat(str));
				this.sendControl(bytes);
			}
		}

		public void faketype(string text)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("faketype=", text, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void fakeversion(string text)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("fakeversion=", text, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void getAllProtectData()
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes("getAllProtectData={|}");
				this.sendControl(bytes);
			}
		}

		public void getAppList()
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes("getapp=install{|}");
				this.sendControl(bytes);
			}
		}

		public void getbackup()
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes("getbackup=1{|}");
				this.sendControl(bytes);
			}
		}

		public void getDeviceInfo()
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes("getinfo=1{|}");
				this.sendControl(bytes);
			}
		}

		public void getfront()
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes("getapp=front{|}");
				this.sendControl(bytes);
			}
		}

		public void getProtectData(string appID)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("getProtectData=", appID, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void getproxy()
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes("getproxy=1{|}");
				this.sendControl(bytes);
			}
		}

		public void getSubFolder(string path)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("getSubFolder=", path, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void getversion()
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes("getversion=1{|}");
				this.sendControl(bytes);
			}
		}

		public void installapp(string appId)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("install=", appId, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void mousedown(int x, int y)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat(new string[] { "mousedown=", x.ToString(), " ", y.ToString(), "{|}" }));
				this.sendControl(bytes);
			}
		}

		public void mouseup(int x, int y)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat(new string[] { "mouseup=", x.ToString(), " ", y.ToString(), "{|}" }));
				this.sendControl(bytes);
			}
		}

		public void movemouse(int x, int y)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat(new string[] { "movemouse=", x.ToString(), " ", y.ToString(), "{|}" }));
				this.sendControl(bytes);
			}
		}

		public void openApp(string AppID)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("open=", AppID, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void openURL(string URL)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("openurl=", URL, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void pauseScript(string text)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes("pausescript=1{|}");
				this.sendControl(bytes);
			}
		}

		public void randominfo()
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes("randominfo=1{|}");
				this.sendControl(bytes);
			}
		}

		public void randomtouchpause()
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes("rdtouchPause={|}");
				this.sendControl(bytes);
			}
		}

		public void randomtouchresume()
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes("rdtouchResume={|}");
				this.sendControl(bytes);
			}
		}

		public void randomtouchstop()
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes("rdtouchStop={|}");
				this.sendControl(bytes);
			}
		}

		public void removeProtectData(string path)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("removeProtectData=", path, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void resping()
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes("resping={|}");
				this.sendControl(bytes);
			}
		}

		public void restore(string text)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("restore=", text, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void savecomment(string filename, string comment)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat(new string[] { "savecomment=", filename, "=", comment, "{|}" }));
				this.sendControl(bytes);
			}
		}

		public void sendtext(string text)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("send=", text, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void setProxy(string socks, int port)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat(new string[] { "setProxy=", socks, ":", port.ToString(), "{|}" }));
				this.sendControl(bytes);
			}
		}

		public void setReferer(string URL)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("setreferer=", URL, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void setsocks(string text)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("setsocks=", text, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void swipe(double x1, double y1, double x2, double y2, double time)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat(new string[] { "swipe=", x1.ToString(), " ", y1.ToString(), " ", x2.ToString(), " ", y2.ToString(), " ", time.ToString(), "{|}" }));
				this.sendControl(bytes);
			}
		}

		public void touch(double x, double y)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat(new string[] { "touch=", x.ToString(), " ", y.ToString(), "{|}" }));
				this.sendControl(bytes);
			}
		}

		public void touchRandom(double x, double y, double x1, double y1, double time, double speed)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat(new string[] { "randomtouch=", x.ToString(), " ", y.ToString(), " ", x1.ToString(), " ", y1.ToString(), " ", time.ToString(), " ", speed.ToString(), "{|}" }));
				this.sendControl(bytes);
			}
		}

		public void uninstallapp(string appId)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("uninstall=", appId, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void wipe(string text)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("wipe=", text, "{|}"));
				this.sendControl(bytes);
			}
		}

		public void wipefull(string text)
		{
			if (this.sendControl != null)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(string.Concat("wipefull=", text, "{|}"));
				this.sendControl(bytes);
			}
		}

		public delegate void SendControl(byte[] buffer);
	}
}