using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ProcessingsClient
{
	public class Disconnect : ProcessingClient
	{
		

		public override void Process(ControllerClient controller)
		{
			if (controller.threadStart)
			{
				controller.model.NStream.Close();
				controller.serverStart = false;
				controller.client.Close();
				controller.timerPing.Stop();
				controller.Disconnect();
				controller.threadReading.Abort();
				controller.threadConsumer.Abort();
				controller.manualResetEvent.Set();
			}
		}
	}
}
