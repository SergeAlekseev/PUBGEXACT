using System.Collections.Generic;
using System.Diagnostics;

namespace ClassLibrary.ProcessingsClient
{
	public class GetBushesInfo : ProcessingClient
	{
		public List<Bush> listBush;

		public override void Process(ModelClient model)
		{
			model.Map.ListBush = listBush;
		}
	}
}
