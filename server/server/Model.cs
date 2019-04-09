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
		private List<BulletInfo> listBullet =new List<BulletInfo>();
		private List<UserInfo> listUsers = new List<UserInfo>();
		private List<NetworkStream> listNs = new List<NetworkStream>();
		private Map map = new Map();

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
		}
	}
}
