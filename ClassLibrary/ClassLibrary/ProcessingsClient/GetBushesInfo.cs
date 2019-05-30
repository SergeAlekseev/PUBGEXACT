using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
