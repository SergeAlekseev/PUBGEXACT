﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{
	class Model
	{
		List<UserInfo> listUsers = new List<UserInfo>();
		Action action = new Action();
		int ping = 0;
		UserInfo thisUser = new UserInfo(new Point(300, 300));
		

		public Model()
		{
			listUsers = new List<UserInfo>();
			action = new Action();
			ping = 0;

		}

		public UserInfo ThisUser { get { return thisUser; } set { thisUser = value; } }
		public List<UserInfo> ListUsers { get { return listUsers; } set { listUsers = value; } }
		public Action Action { get { return action; } set { action = value; } }
		public int Ping { get { return ping; } set { ping = value; } }
	}
}
