using System.Diagnostics;

namespace ClassLibrary.ProcessingsClient
{
	public class GetCountGamesInfo : ProcessingClient
	{
		public int count;
		public override void Process(ModelClient model)
		{
			model.CountGamers = count;
		}
	}
}
