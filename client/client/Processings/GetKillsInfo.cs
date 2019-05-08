﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
namespace client.Processing
{
	public class GetKillsInfo : Processing
	{
		public Kill kill;
		public override void Process(Controller controller)
		{
			Kill[] arrayKills = new Kill[3];
			arrayKills[2] = controller.model.ArrayKills[1];
			arrayKills[1] = controller.model.ArrayKills[0];
			arrayKills[0] = kill;
			controller.model.ArrayKills = arrayKills;
		}
	}
}
