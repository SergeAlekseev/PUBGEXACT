using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ProcessingsClient
{
	public class PingInfoClient : ProcessingClient
	{
		

		public override void Process(ControllerClient controller)
		{
			controller.PingWatch.Stop();
			controller.model.Ping = (int)controller.PingWatch.ElapsedMilliseconds;
		}
	}
}
