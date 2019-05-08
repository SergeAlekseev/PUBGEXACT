using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Processing
{
	public class GetBulletsInfo : Processing
	{
		public List<BulletInfo> listBulets;
		public override void Process(Controller controller)
		{
			controller.model.ListBullet = listBulets;
		}
	}
}
