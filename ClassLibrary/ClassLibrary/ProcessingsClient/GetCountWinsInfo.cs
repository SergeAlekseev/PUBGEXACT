using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ProcessingsClient
{
	public class GetCountWinsInfo : ProcessingClient
	{
		public override void Process(ControllerClient controller)
		{
			controller.threadStart = false;
			controller.manualResetEvent.Set();
			controller.model.Win = true;
			controller.model.NStream.Close();
			controller.serverStart = false;
			controller.client.Close();
			controller.timerPing.Stop();
			controller.threadReading.Abort();
			controller.threadConsumer.Abort();
		}
	}
}
