﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
namespace ClassLibrary.ProcessingsClient
{
	public class GetKillsInfo : ProcessingClient
	{
		public Kill kill=new Kill();
		public override void Process(ModelClient model)
		{
			Kill[] arrayKills = new Kill[3];
			arrayKills[2] =	model.ArrayKills[1];
			arrayKills[1] = model.ArrayKills[0];
			arrayKills[0] = kill;
			model.ArrayKills = arrayKills;
		}
	}
}
