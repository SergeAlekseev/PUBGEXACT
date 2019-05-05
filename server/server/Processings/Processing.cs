using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server.Processings
{
	public abstract class Processing
	{
		public int num;
		
		public virtual void Process()
		{
			MessageBox.Show("Как так");
		}
		
	}
}
