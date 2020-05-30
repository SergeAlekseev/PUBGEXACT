using System.Collections.Generic;
using System.Diagnostics;

namespace ClassLibrary.ProcessingsClient
{
	class GetInfoItems : ProcessingClient
	{
		public List<Item> listItems;
		public override void Process(ModelClient model)
		{
			model.Map.ListItems = listItems;
		}
	}
}
