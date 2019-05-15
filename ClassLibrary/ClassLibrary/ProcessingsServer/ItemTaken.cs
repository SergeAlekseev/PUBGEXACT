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
					Model.ListUsers[num].Items[i] = item; //Кладём вещь игроку

					UserItemInfo userItem = new UserItemInfo();
					userItem.Items = Model.ListUsers[num].Items; 
					CTransfers.Writing(userItem, Model.ListNs[num]); //Даём актуальную инфу игроку об его инвентаре

					lock (Model.Map.ListItems)
					{
						for (int j = 0; j < Model.Map.ListItems.Count; j++) if (Model.Map.ListItems[j].IdItem == item.IdItem) Model.Map.ListItems.RemoveAt(j); //Удаляем вещь с карты
					}

					foreach (NetworkStream nStream in Model.ListNs) //Даём актуальную инфу всем игрокам об удалённом с карты предмете 
					{
						GetInfoItems items = new GetInfoItems();
						items.listItems = Model.Map.ListItems;
						CTransfers.Writing(items, nStream);
					}
					break;
				}

		}
	}
}
