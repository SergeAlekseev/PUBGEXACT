using System.Collections.Generic;
using System.Diagnostics;

namespace ClassLibrary.ProcessingsClient
{
	public class GetBulletsInfo : ProcessingClient
	{
		public List<BulletInfo> listBulets;
		public override void Process(ModelClient model)
		{
			model.ListBullet = listBulets;
		}
	}
}
