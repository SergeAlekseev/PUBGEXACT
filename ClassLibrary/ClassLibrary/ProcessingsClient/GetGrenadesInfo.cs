using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ProcessingsClient
{
	public class GetGrenadesInfo : ProcessingClient
	{
		public List<GrenadeInfo> grenadesInfo;
		public override void Process(ModelClient model)
		{
			model.ListGrenade = grenadesInfo;
		}
	}
}
