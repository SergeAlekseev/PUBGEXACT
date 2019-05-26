
namespace ClassLibrary.ProcessingsClient
{
	public class PlayerDeath : ProcessingClient
	{
		public string Killer;
		public override void Process(ControllerClient controller)
		{
			controller.threadStart = false;
			controller.manualResetEvent.Set();
			controller.model.Die = true;
			controller.model.Killer = Killer;
			controller.model.NStream.Close();
			controller.threadStart = false;
			controller.client.Close();
			controller.timerPing.Stop();
			controller.threadReading.Abort();
			controller.threadConsumer.Abort();
		}
	}
}
