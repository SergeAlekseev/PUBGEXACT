using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassLibrary.ProcessingsServer
{
	public abstract class ProcessingServer
	{
		public int num;
		
		public virtual void Process(ModelServer Model)
		{
			MessageBox.Show("Как так");
		}
		
	}
}
