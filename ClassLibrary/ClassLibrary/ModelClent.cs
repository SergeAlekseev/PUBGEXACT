
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using Action = ClassLibrary.Action;

namespace ClassLibrary
{
	public class ModelClient
	{

		List<UserInfo> listUsers = new List<UserInfo>();
		List<BulletInfo> listBullet = new List<BulletInfo>();
		List<GeneralInfo> listGInfo = new List<GeneralInfo>();
		List<GrenadeInfo> listGrenade = new List<GrenadeInfo>();
		Action action = new Action();
		int ping = 0;
		int countGamers = 0;
		UserInfo thisUser = new UserInfo(new Point(300, 300));	
		Map map = new Map();
		short lifes;
		bool die, win;
		public bool Start = false;
		Image[] images = new Image[20];
		GeneralInfo gInfo = new GeneralInfo();
		Point mouseCoord = new Point();
		double angelToZone;
		string killer;
		public Kill[] arrayKills = new Kill[3];
		 NetworkStream nStream;

		public int number;
		public int CountGamers
		{
			get { return countGamers; }
			set { countGamers = value; }
		}

		public ModelClient()
		{
			listUsers = new List<UserInfo>();
			action = new Action();
			ping = -1;
			thisUser = new UserInfo(new Point(300, 300));
			if (thisUser != null)
				Start = true;
		}

		public NetworkStream NStream { get { return nStream; } set { nStream = value; } }
		public double AngelToZone { get { return angelToZone; } set { angelToZone = value; } }
		public Image[] Images { get { return images; } set { images = value; } }
		public bool Die { get { return die; } set { die = value; } }
		public bool Win { get { return win; } set { win = value; } }
		public string Killer { get { return killer; } set { killer = value; } }
		public short Lifes { get { return lifes; } set { lifes = value; } }
		public List<BulletInfo> ListBullet { get { return listBullet; } set { listBullet = value; } }
		public List<GrenadeInfo> ListGrenade { get { return listGrenade; } set { listGrenade = value; } }
		public UserInfo ThisUser { get { return thisUser; } set { thisUser = value; } }
		public List<UserInfo> ListUsers { get { return listUsers; } set { listUsers = value; } }
		public Action Action { get { return action; } set { action = value; } }
		public int Ping { get { return ping; } set { ping = value; } }
		public Map Map { get { return map; } set { map = value; } }
		public GeneralInfo GInfo { get { return gInfo; } set { gInfo = value; } }
		public List<GeneralInfo> ListGInfo { get { return listGInfo; } set { listGInfo = value; } }
		public Point MouseCoord { get { return mouseCoord; } set { mouseCoord = value; } }

		public Kill[] ArrayKills { get { return arrayKills; } set { arrayKills = value; } }
		//	public double Rotate { get { return rotate; } set { rotate = value; } }
	}
}
