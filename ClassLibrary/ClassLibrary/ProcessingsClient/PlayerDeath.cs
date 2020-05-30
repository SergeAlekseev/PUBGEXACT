
using System.Diagnostics;

namespace ClassLibrary.ProcessingsClient
{
	public class PlayerDeath : ProcessingClient
	{
		public string Killer;
		public override void Process(ModelClient model)
		{
			model.threadStart = false;
			model.Die = true;
			model.Killer = Killer;
			model.NStream.Close();
			model.threadStart = false;
			model.exit();
		}
	}
}
