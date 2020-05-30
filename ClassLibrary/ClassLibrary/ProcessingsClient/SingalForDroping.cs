using System.Collections.Generic;
using System.Diagnostics;
using ClassLibrary.ProcessingsServer;

namespace ClassLibrary.ProcessingsClient
{
	public class SingalForDroping : ProcessingClient
	{
		public override void Process(ModelClient model)
		{
			ItemDroping itemDrop = new ItemDroping();
			List<Item> listItems = new List<Item>();
			foreach (Item item in model.ListUsers[model.ThisUser.userNumber].Items)
			{
				if (item != null)
				{
					if(item.Name != null)
					listItems.Add(item);
				}
			}
			itemDrop.items = listItems;
			itemDrop.itemLocation = model.ListUsers[model.ThisUser.userNumber].userLocation;
			itemDrop.num = model.ThisUser.userNumber;
			CTransfers.Writing(itemDrop, model.NStream);
		}
	}
}
