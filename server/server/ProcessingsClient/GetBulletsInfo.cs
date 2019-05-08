using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.ProcessingsClient
{
	public class GetBulletsInfo : Processing
	{
		public List<BulletInfo> listBulets;
		public override void Process(Controller controller)
		{
		}
	}
}
