using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
namespace ClassLibrary.ProcessingsClient
{
	public class GetPrevZoneInfo : ProcessingClient
	{
		public Zone prevZone;
		public override void Process(ModelClient model)
		{
			model.Map.PrevZone = prevZone;
		}
	}
}
