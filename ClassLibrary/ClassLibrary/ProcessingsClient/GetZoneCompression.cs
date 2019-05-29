using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ProcessingsClient
{
	class GetZoneCompression:ProcessingClient
	{
		public double Count;
		public override void Process(ModelClient model)
		{
			model.Map.NextZone.TimeTocompression = Count;
		}
	}
}
