using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using Action = ClassLibrary.Action;

namespace BotLibrary
{
	class BotModel
	{
		List<UserInfo> listUsers = new List<UserInfo>();
		List<BulletInfo> listBullet = new List<BulletInfo>();
		List<GrenadeInfo> listGrenade = new List<GrenadeInfo>();
		Action action = new Action();
		int countGamers = 0;
		UserInfo thisUser = new UserInfo(new Point(300, 300));
		Map map = new Map(); 
		short lifes;
		public bool Start = false;
		GeneralInfo gInfo = new GeneralInfo();
		Point mouseCoord = new Point();
		string killer;
		public Kill[] arrayKills = new Kill[3];
		public bool threadStart = false;
		public bool serverStart;
		public int number;
		public int CountGamers
		{
			get { return countGamers; }
			set { countGamers = value; }
		}

		/// <summary>
		/// Создаст ограниченную базу данныз для игрока из полной
		/// </summary>
		/// <param name="client">Полная база данных</param>
		public BotModel(ModelClient client) 
		{
		
		}

		public string Killer { get { return killer; } set { killer = value; } }
		public short Lifes { get { return lifes; } set { lifes = value; } }
		public List<BulletInfo> ListBullet { get { return listBullet; } set { listBullet = value; } }
		public List<GrenadeInfo> ListGrenade { get { return listGrenade; } set { listGrenade = value; } }
		public UserInfo ThisUser { get { return thisUser; } set { thisUser = value; } }
		public List<UserInfo> ListUsers { get { return listUsers; } set { listUsers = value; } }
		public Action Action { get { return action; } set { action = value; } }
		public Map Map { get { return map; } set { map = value; } }
		public GeneralInfo GInfo { get { return gInfo; } set { gInfo = value; } }
		public Point MouseCoord { get { return mouseCoord; } set { mouseCoord = value; } }
		public Kill[] ArrayKills { get { return arrayKills; } set { arrayKills = value; } }
	}
}
