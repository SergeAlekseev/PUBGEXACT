using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace server.Processings
{
	public class ChangeWeapons : Processing
	{
		public byte numItems;
		

		public override void Process()
		{
			Model.ListUsers[num].flagRecharge = false;
			Model.ListUsers[num].flagWaitShoting = true;
			Model.ListShoting[num].Abort();
			Model.ListUsers[num].flagShoting = false;
			Thread t = new Thread(() =>
			{
				Thread.Sleep(Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].Time);
				Model.ListUsers[num].flagWaitShoting = false;
			});
			t.Start();
			Model.ListUsers[num].thisItem = numItems;
		}
	}
}
