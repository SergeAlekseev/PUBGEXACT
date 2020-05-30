using System.Diagnostics;
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
