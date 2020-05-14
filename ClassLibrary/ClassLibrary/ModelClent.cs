
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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
		public delegate void CountGamersD(int countGamers);
		public event CountGamersD CountGamersEvent;

		public delegate void KillerD(string killer);
		public event KillerD KillerEvent;

		public delegate void LifesD(short lifes);
		public event LifesD LifesEvent;

		public delegate void ListBulletD(List<BulletInfo> listBullet);
		public event ListBulletD ListBulletEvent;

		public delegate void ListGrenadeD(List<GrenadeInfo> listGrenade);
		public event ListGrenadeD ListGrenadeEvent;

		public delegate void ThisUserD(UserInfo thisUser);
		public event ThisUserD ThisUserEvent;

		public delegate void ListUsersD(List<UserInfo> listUsers);
		public event ListUsersD ListUsersEvent;

		public delegate void ActionD(Action action);
		public event ActionD ActionEvent;

		public delegate void MapD(Map map);
		public event MapD MapEvent;

		public delegate void GInfoD(GeneralInfo gInfo);
		public event GInfoD GInfoEvent;

		public delegate void ArrayKillsD(Kill[] arrayKills);
		public event ArrayKillsD ArrayKillsEvent;

		public delegate void ThreadStartD(bool threadStart);
		public event ThreadStartD ThreadStartEvent;

		public delegate void exitD();
		public delegate bool setNameD(string s);

		public setNameD setName;
		public exitD exit;

		public Stopwatch PingWatch = new Stopwatch();

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

		bool ThreadStart = false;
		public bool serverStart;

		public int number;
		public int CountGamers
		{
			get { return countGamers; }
			set
			{ 
				countGamers = value;
				CountGamersEvent(value);
			}
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
		public string Killer { get { return killer; } set { killer = value; KillerEvent(value); } }
		public short Lifes { get { return lifes; } set { lifes = value; LifesEvent(value); } }
		public List<BulletInfo> ListBullet { get { return listBullet; } set { listBullet = value; ListBulletEvent(value); } }
		public List<GrenadeInfo> ListGrenade { get { return listGrenade; } set { listGrenade = value; ListGrenadeEvent(value); } }
		public UserInfo ThisUser { get { return thisUser; } set { thisUser = value; ThisUserEvent(value); } }
		public List<UserInfo> ListUsers { get { return listUsers; } set { listUsers = value; ListUsersEvent(value); } }
		public Action Action { get { return action; } set { action = value; ActionEvent(value); } }
		public int Ping { get { return ping; } set { ping = value;  } }
		public Map Map { get { MapEvent(map); return map; } set { map = value; MapEvent(value); } }
		public GeneralInfo GInfo { get { return gInfo; } set { gInfo = value; GInfoEvent(value); } }
		public List<GeneralInfo> ListGInfo { get { return listGInfo; } set { listGInfo = value; } }
		public Point MouseCoord { get { return mouseCoord; } set { mouseCoord = value; } }
		public Kill[] ArrayKills { get { return arrayKills; } set { arrayKills = value; ArrayKillsEvent(value); } }
		public bool threadStart { get => ThreadStart; set { ThreadStart = value; ThreadStartEvent(value); }  }  //FIXME
	}
}
