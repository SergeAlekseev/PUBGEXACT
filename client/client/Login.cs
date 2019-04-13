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
	public partial class Login : Form
	{
		public Login()
		{
			InitializeComponent();
		}

		private void Join_Click(object sender, EventArgs e)
		{
			MyMenu form = new MyMenu(tName.Text, Pass.Text,ip.Text);
			form.Show();
			this.Hide();
		}
	}
}
