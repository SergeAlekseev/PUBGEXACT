using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.ProcessingsClient
{
	public class Disconnect : Processing
	{
		public delegate void DissD();
		public event DissD DissEvent;

		public override void Process(Controller controller)
		{
		}
	}
}
