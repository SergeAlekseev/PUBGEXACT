using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Processing
{
	public class GetNumber : Processing
	{
		public int num;
		public override void Process(Controller controller)
		{
			controller.model.number = num;
			controller.threadStart = true;
			controller.setName(controller.model.GInfo.Name);
		}
	}
}
