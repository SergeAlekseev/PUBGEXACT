using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{
	class CMyMenu
	{
		Model model = new Model();

		public void JoinUser(string Name, string Password)
		{
			model.ThisUser.Name = Name;
			model.ThisUser.Password = Password;
		}
		public Model Model
		{
			get { return model; }
			set { model = value; }
		}
	}
}
