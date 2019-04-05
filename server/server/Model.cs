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
		static private List<BulletInfo> listBullet =new List<BulletInfo>();
		static private List<UserInfo> listUsers = new List<UserInfo>();
		static private List<NetworkStream> listNs = new List<NetworkStream>();
		static private List<Box> listBox = new List<Box>();

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

		public void Remove()
		{
			listBullet = new List<BulletInfo>();
			listUsers = new List<UserInfo>();
			listNs = new List<NetworkStream>();
		}

		public List<Box> ListBox
		{
			get { return listBox; }
			set { listBox = value; }
		}
	}
}
