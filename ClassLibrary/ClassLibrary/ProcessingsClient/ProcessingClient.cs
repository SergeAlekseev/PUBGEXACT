using System.Windows.Forms;

namespace ClassLibrary.ProcessingsClient
{
	public abstract class ProcessingClient
	{
		public virtual void Process(ModelClient model)
		{
			MessageBox.Show("Как так");
		}
	}
}
