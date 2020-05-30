using System.Diagnostics;

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
