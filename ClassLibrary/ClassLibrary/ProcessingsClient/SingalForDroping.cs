using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using ClassLibrary.ProcessingsServer;
using System.Windows.Forms;

namespace ClassLibrary.ProcessingsClient
{
	public class SingalForDroping : ProcessingClient
	{
		public override void Process(ControllerClient controller)
		{
			ItemDroping itemDrop = new ItemDroping();
			List<Item> listItems = new List<Item>();
			foreach (Item item in controller.model.ListUsers[controller.model.ThisUser.userNumber].Items)
			{
				if (item != null)
				{
					if(item.Name != null)
					listItems.Add(item);
				}
			}
			itemDrop.items = listItems;
			itemDrop.itemLocation = controller.model.ListUsers[controller.model.ThisUser.userNumber].userLocation;
			CTransfers.Writing(itemDrop, controller.model.NStream);
		}
	}
}
