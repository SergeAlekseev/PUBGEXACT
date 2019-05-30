﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary.ProcessingsServer
{
	public class ShotUp : ProcessingServer
	{
		ModelServer Model;
		public override void Process(ModelServer Model)
		{
			this.Model = Model;
			if (Model.ListUsers[num].flagShoting && !Model.ListUsers[num].flagWaitShoting)
			{
				Model.ListUsers[num].flagWaitShoting = true;
				Model.ListShoting[num].Abort();
				if (Model.ListUsers[num].Items[Model.ListUsers[num].thisItem] is Grenade)
				{
					(Model.ListUsers[num].Items[Model.ListUsers[num].thisItem] as Grenade).Grena.flagFly=true;
				}
				Model.ListUsers[num].flagShoting = false;
				Thread t = new Thread(() =>
				{
					Thread.Sleep(Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].Time);
					try
					{
						Model.ListUsers[num].flagWaitShoting = false;
					}
					catch { }
				});
				t.Start();
			}
		}
	}
}
