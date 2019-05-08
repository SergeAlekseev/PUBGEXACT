using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Processing
{
	public class GetCountWinsInfo : Processing
	{
		public override void Process(Controller controller)
		{
			controller.model.Win = true;
			controller.nStream.Close();
			controller.serverStart = false;
			controller.client.Close();
			controller.timerPing.Stop();
			controller.threadReading.Abort();
		}
	}
}
