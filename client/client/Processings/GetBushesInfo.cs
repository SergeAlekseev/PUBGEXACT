using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Processing
{
	public class GetBushesInfo : Processing
	{
		public List<Bush> listBush;

		public override void Process(Controller controller)
		{
			controller.model.Map.ListBush = listBush;
		}
	}
}
