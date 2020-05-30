using System.Collections.Generic;
using System.Diagnostics;

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
