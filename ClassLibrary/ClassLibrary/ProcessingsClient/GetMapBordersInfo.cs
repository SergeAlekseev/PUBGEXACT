using System.Diagnostics;
using System.Drawing;

namespace ClassLibrary.ProcessingsClient
{
	public class GetMapBordersInfo : ProcessingClient
	{
		public Rectangle rectangle;
		public override void Process(ModelClient model)
		{
			model.Map.MapBorders = rectangle;
		}
	}
}
