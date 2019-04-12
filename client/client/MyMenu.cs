using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace client
{
	public partial class MyMenu : Form
	{
		CMyMenu Controller = new CMyMenu();

		public MyMenu(string Name, string Pass)
		{
			InitializeComponent();
			Controller.JoinUser(Name,Pass);
		}

		private void button1_Click(object sender, EventArgs e)
		{

			Client form = new Client(Controller.Model.ThisUser.Name, Controller.Model.ThisUser.Password);
			form.Show();
			this.Hide();
		}

		private void MyMenu_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}
	}
}
