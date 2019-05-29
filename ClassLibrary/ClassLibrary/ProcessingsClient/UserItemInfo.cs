using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ProcessingsClient
{
	class UserItemInfo : ProcessingClient
	{
		public Item[] Items;

		public override void Process(ModelClient model)
		{
			model.ThisUser.Items = Items;
		}
	}
}
