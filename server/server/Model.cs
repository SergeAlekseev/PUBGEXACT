using client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
	class Model
	{
		static private List<UserInfo> listUsers; //Model
		public List<UserInfo> ListUsers
		{
			get { return listUsers; }
			set { listUsers = value; }
		}		
	}
}
