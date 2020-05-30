
using System.Diagnostics;

namespace ClassLibrary.ProcessingsClient
{
	public class GetZoneStartInfo : ProcessingClient
	{
		public Zone nextZone;
		public override void Process(ModelClient model)
		{
			model.Map.PrevZone = model.Map.NextZone;
			model.Map.NextZone = nextZone;
		}
	}
}
