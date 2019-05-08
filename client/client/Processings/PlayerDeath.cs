using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Processing
{
	public class PlayerDeath : Processing
	{
		public string Killer;
		public override void Process(Controller controller)
		{
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
