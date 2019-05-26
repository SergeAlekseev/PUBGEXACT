﻿
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
		BlockingCollection<GrenadeInfo> listGrenade = new BlockingCollection<GrenadeInfo>();
		List<UserInfo> listUsers = new List<UserInfo>();
		List<NetworkStream> listNs = new List<NetworkStream>();
		Map map = new Map();
		List<GeneralInfo> listGInfo = new List<GeneralInfo>();
		int countGamers = 0;
		List<MMove> listMove = new List<MMove>();
		List<Thread> listShoting = new List<Thread>();
		List<System.Timers.Timer> listTimers = new List<System.Timers.Timer>();


		public bool workingGame;

		public List<System.Timers.Timer> ListTimers
		{
			get { return listTimers; }
			set { listTimers = value; }
		}
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

		public BlockingCollection<GrenadeInfo> ListGrenade
		{
			get { return listGrenade; }
			set { listGrenade = value; }
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
			listTimers = new List<System.Timers.Timer>();
			listShoting = new List<Thread>();
			listBullet = new BlockingCollection<BulletInfo>();
			listUsers = new List<UserInfo>();
			listNs = new List<NetworkStream>();
			listGInfo = new List<GeneralInfo>();
			map = new Map();
		}
	}
}
