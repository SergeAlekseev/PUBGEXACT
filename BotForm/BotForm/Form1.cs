

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace BotForm
{
	public partial class Form1 : Form
	{
		String str = "";
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				str += "Начали\n";
				this.label1.Text = str;
				BotImpl bot = new BotImpl();
		
				if(bot.join("127.0.0.1", "chippanah99ff", "12312312"))
				{
					str += "Подключився\n";
					this.label1.Text = str;
				}

				str += "Закончили\n";
				this.label1.Text = str;
		
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}
	}
}
