using client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using System.Threading;

namespace server
{
	static class Model
	{
		static List<BulletInfo> listBullet = new List<BulletInfo>();
		static List<UserInfo> listUsers = new List<UserInfo>();
		static List<NetworkStream> listNs = new List<NetworkStream>();
		static Map map = new Map();
		static List<GeneralInfo> listGInfo = new List<GeneralInfo>();
		static List<Item> items = new List<Item>();
		static int countGamers = 0;
		static List<MMove> listMove = new List<MMove>();
		static List<Thread> listShoting = new List<Thread>();
		
		static public bool workingGame;

		static public List<Thread> ListShoting
		{
			get { return listShoting; }
			set { listShoting = value; }
		}
		static public List<MMove> ListMove
		{
			get { return listMove; }
			set { listMove = value; }
		}

		static public int CountGamers
		{
			get { return countGamers; }
			set { countGamers = value; }
		}
		static public List<GeneralInfo> ListGInfo
		{
			get { return listGInfo; }
			set { listGInfo = value; }
		}

		static public List<Item> Items
		{
			get { return items; }
			set { items = value; }
		}

		static public List<UserInfo> ListUsers
		{
			get { return listUsers; }
			set { listUsers = value; }
		}

		static public List<BulletInfo> ListBullet
		{
			get { return listBullet; }
			set { listBullet = value; }
		}

		static public List<NetworkStream> ListNs
		{
			get { return listNs; }
			set { listNs = value; }
		}

		static public Map Map
		{
			get { return map; }
			set { map = value; }
		}

		static public void Remove()
		{
			listBullet = new List<BulletInfo>();
			listUsers = new List<UserInfo>();
			listNs = new List<NetworkStream>();
			listGInfo = new List<GeneralInfo>();
			items = new List<Item>();
			map = new Map();
		}
	}
}
