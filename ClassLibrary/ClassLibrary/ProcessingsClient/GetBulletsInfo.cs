using ClassLibrary;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ProcessingsClient
{
	public class GetBulletsInfo : ProcessingClient
	{
		public List<BulletInfo> listBulets;
		public override void Process(ControllerClient controller)
		{
			controller.model.ListBullet = listBulets;
		}
	}
}
