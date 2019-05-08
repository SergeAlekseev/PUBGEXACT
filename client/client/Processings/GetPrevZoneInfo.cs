using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
namespace client.Processing
{
	public class GetPrevZoneInfo : Processing
	{
		public Zone prevZone;
		public override void Process(Controller controller)
		{
			controller.model.Map.PrevZone = prevZone;
		}
	}
}
