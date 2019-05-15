using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ProcessingsClient
{
	class GetInfoItems : ProcessingClient
	{
		public List<Item> listItems;
		public override void Process(ControllerClient controller)
		{
			controller.model.Map.ListItems = listItems;
		}
	}
}
