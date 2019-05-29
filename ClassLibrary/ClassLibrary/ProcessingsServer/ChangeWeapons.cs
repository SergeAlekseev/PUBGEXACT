using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary.ProcessingsServer
{
	public class ChangeWeapons : ProcessingServer
	{
		public byte numItems;
		ModelServer Model;
		public override void Process(ModelServer Model)
		{
			this.Model = Model;
			Model.ListUsers[num].flagRecharge = false;
			Model.ListUsers[num].flagWaitShoting = true;
			Model.ListShoting[num].Abort();
			if (Model.ListUsers[num].Items[Model.ListUsers[num].thisItem] is Grenade)
			{
				(Model.ListUsers[num].Items[Model.ListUsers[num].thisItem] as Grenade).Grena.flagFly = true;
			}
			Model.ListUsers[num].flagShoting = false;
			short Time = Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].Time;
			Thread t = new Thread(() =>
			{
				Thread.Sleep(Time);
				Model.ListUsers[num].flagWaitShoting = false;
			});
			t.Start();
			Model.ListUsers[num].thisItem = numItems;
		}
	}
}
