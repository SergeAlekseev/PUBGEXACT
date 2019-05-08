using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
namespace client.ProcessingsServer
{
	public class PlayerMovementsInfo:Processing
	{
		public ClassLibrary.Action action;
	
		public override void Process()
		{
		}
	}
}
