using System.Diagnostics;

namespace ClassLibrary.ProcessingsClient
{
	public class Disconnect : ProcessingClient
	{
		public override void Process(ModelClient model)
		{
			if (model.threadStart)
			{
				model.NStream.Close();
				model.serverStart = false;
				model.exit();
			}
		}
	}
}
