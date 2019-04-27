using client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
	class Model
	{
		List<BulletInfo> listBullet = new List<BulletInfo>();
		List<UserInfo> listUsers = new List<UserInfo>();
		List<NetworkStream> listNs = new List<NetworkStream>();
		Map map = new Map();
		List<GeneralInfo> listGInfo = new List<GeneralInfo>();
		List<Item> items = new List<Item>();
		int countGamers = 0;

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

		public List<BulletInfo> ListBullet
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
			listBullet = new List<BulletInfo>();
			listUsers = new List<UserInfo>();
			listNs = new List<NetworkStream>();
			listGInfo = new List<GeneralInfo>();
			items = new List<Item>();
			map = new Map();
		}
	}
}
