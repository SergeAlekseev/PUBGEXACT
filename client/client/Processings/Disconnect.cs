using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Processing
{
	public class Disconnect : Processing
	{
		public delegate void DissD();
		public event DissD DissEvent;

		public override void Process(Controller controller)
		{
			if (controller.threadStart)
			{
				controller.nStream.Close();
				controller.serverStart = false;
				controller.client.Close();
				controller.timerPing.Stop();
				DissEvent();
				controller.threadReading.Abort();
			}
		}
	}
}
