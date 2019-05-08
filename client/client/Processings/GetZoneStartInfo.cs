using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
namespace client.Processing
{
	public class GetZoneStartInfo : Processing
	{
		public Zone nextZone;
		public override void Process(Controller controller)
		{
			controller.model.Map.PrevZone = controller.model.Map.NextZone;
			controller.model.Map.NextZone = nextZone;
		}
	}
}
