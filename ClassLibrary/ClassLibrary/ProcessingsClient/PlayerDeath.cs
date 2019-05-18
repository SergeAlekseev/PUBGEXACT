using ClassLibrary.ProcessingsServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ProcessingsClient
{
	public class PlayerDeath : ProcessingClient
	{
		public string Killer;
		//public int num;
		//public NetworkStream nSream;
		public override void Process(ControllerClient controller)
		{
			//ItemDroping itemDrop = new ItemDroping();
			//List<Item> listItems = new List<Item>();
			//foreach (Item item in controller.model.ListUsers[num].Items)
			//	listItems.Add(item);
			//itemDrop.items = listItems;
			//itemDrop.itemLocation = controller.model.ListUsers[num].userLocation;
			//CTransfers.Writing(itemDrop, nSream);

			controller.threadStart = false;
			controller.manualResetEvent.Set();
			controller.model.Die = true;
			controller.model.Killer = Killer;
			controller.nStream.Close();
			controller.threadStart = false;
			controller.client.Close();
			controller.timerPing.Stop();
			controller.threadReading.Abort();	
		}
	}
}
