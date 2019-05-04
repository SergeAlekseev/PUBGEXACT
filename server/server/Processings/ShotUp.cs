using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace server.Processings
{
	public class ShotUp : Processing
	{
		int num;

		public ShotUp(int num)
		{
			this.num = num;
		}
		public override void Process()
		{
			if (Model.ListUsers[num].flagShoting && !Model.ListUsers[num].flagWaitShoting)
			{
				Model.ListUsers[num].flagWaitShoting = true;
				Model.ListUsers[num].Shoting.Abort();
				Model.ListUsers[num].flagShoting = false;
				Thread t = new Thread(() =>
				{
					Thread.Sleep(Model.ListUsers[num].Items[Model.ListUsers[num].thisItem].Time);
					Model.ListUsers[num].flagWaitShoting = false;
				});
				t.Start();
			}
		}
	}
}
