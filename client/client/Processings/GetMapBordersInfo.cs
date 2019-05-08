using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Processing
{
	public class GetMapBordersInfo : Processing
	{
		public Rectangle rectangle;
		public override void Process(Controller controller)
		{
			controller.model.Map.MapBorders = rectangle;
		}
	}
}
