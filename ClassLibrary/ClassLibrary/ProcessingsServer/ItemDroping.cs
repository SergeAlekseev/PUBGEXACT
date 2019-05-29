using ClassLibrary.ProcessingsClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassLibrary.ProcessingsServer
{
	public class ItemDroping : ProcessingServer
	{
		public List<Item> items;
		public Point itemLocation;

		public override void Process(ModelServer Model)
		{
			foreach (Item item in items)
			{
				for (int i = 1; i <= Model.ListUsers[num].Items.Length; i++)
					if (Model.ListUsers[num].Items[i].IdItem == item.IdItem)
					{
						Item voidItem = new Item();
						Model.ListUsers[num].Items[i] = voidItem; //Удаляем вещь из инвентаря игрока
						voidItem.IdItem = -1;
						item.Location = itemLocation;
						lock (Model.Map.ListItems)
						{
							Model.Map.ListItems.Add(item); //Добавляем вещь на карту
						}

						foreach (NetworkStream nStream in Model.ListNs) //Даём актуальную инфу всем игрокам об удалённом с карты предмете 
						{
							GetInfoItems items = new GetInfoItems();
							items.listItems = Model.Map.ListItems;
							CTransfers.Writing(items, nStream);
						}
						break;
					}
				itemLocation.X += 25;
			}

		}
	}
}
