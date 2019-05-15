using ClassLibrary.ProcessingsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
namespace ClassLibrary.ProcessingsServer
{
	class ItemTaken : ProcessingServer
	{
		public Item item;
		public override void Process(ModelServer Model)
		{
			for (int i = 1; i < Model.ListUsers[num].Items.Length; i++)
				if (Model.ListUsers[num].Items[i].Name == null )
				{
					Model.ListUsers[num].Items[i] = item;

					UserItemInfo userItem = new UserItemInfo();
					userItem.Items = Model.ListUsers[num].Items;
					CTransfers.Writing(userItem,Model.ListNs[num]);
					break;
				}

			lock (Model.Map.ListItems)
			{			
				for (int i = 0; i < Model.Map.ListItems.Count; i++) if (Model.Map.ListItems[i].IdItem == item.IdItem) Model.Map.ListItems.RemoveAt(i);
			}


			foreach (NetworkStream nStream in Model.ListNs)
			{
				GetInfoItems items = new GetInfoItems();
				items.listItems = Model.Map.ListItems;
				CTransfers.Writing(items, nStream);
			}
		}
	}
}
