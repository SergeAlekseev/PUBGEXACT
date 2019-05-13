
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using System.Threading;
using System.Collections.Concurrent;

namespace ClassLibrary
{
	public class ModelServer
	{
		BlockingCollection<BulletInfo> listBullet = new BlockingCollection<BulletInfo>();
		List<UserInfo> listUsers = new List<UserInfo>();
		List<NetworkStream> listNs = new List<NetworkStream>();
		Map map = new Map();
		List<GeneralInfo> listGInfo = new List<GeneralInfo>();
		List<Item> items = new List<Item>();
		int countGamers = 0;
		List<MMove> listMove = new List<MMove>();
		List<Thread> listShoting = new List<Thread>();

		public bool workingGame;

		public List<Thread> ListShoting
		{
			get { return listShoting; }
			set { listShoting = value; }
		}

		public List<MMove> ListMove
		{
			get { return listMove; }
			set { listMove = value; }
		}

		public int CountGamers
		{
			get { return countGamers; }
			set { countGamers = value; }
		}
		public List<GeneralInfo> ListGInfo
		{
			get { return listGInfo; }
			set { listGInfo = value; }
		}

		public List<Item> Items
		{
			get { return items; }
			set { items = value; }
		}

		public List<UserInfo> ListUsers
		{
			get { return listUsers; }
			set { listUsers = value; }
		}

		public BlockingCollection<BulletInfo> ListBullet
		{
			get { return listBullet; }
			set { listBullet = value; }
		}

		public List<NetworkStream> ListNs
		{
			get { return listNs; }
			set { listNs = value; }
		}

		public Map Map
		{
			get { return map; }
			set { map = value; }
		}

		public void Remove()
		{
			countGamers = 0;
			workingGame = false;
			//listMove = new List<MMove>();
			listShoting = new List<Thread>();
			listBullet = new BlockingCollection<BulletInfo>();
			listUsers = new List<UserInfo>();
			listNs = new List<NetworkStream>();
			listGInfo = new List<GeneralInfo>();
			items = new List<Item>();
			map = new Map();
		}
	}
}
