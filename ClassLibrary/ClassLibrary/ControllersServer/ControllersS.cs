﻿using ClassLibrary.ControllersServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{

	public class ControllersS
	{
		public static CMap cMap;
		public static CInfoUsers cInfoUsers;
		public static CPlay cPlay;
		public static CStart cStart;
		public static ControllerServer cServer;

		public ControllersS(ModelServer model, ControllerServer controller)
		{
			cMap = new CMap(model);
			cInfoUsers = new CInfoUsers(model);
			cPlay = new CPlay(model);
			cStart = new CStart(model);
			cServer = controller;
		}
	}
}
