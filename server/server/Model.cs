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
		static public int number; //Model
		bool workingThread; //Model
		bool workingServer;//Model

		public List<UserInfo> ListUsers
		{
			get { return listUsers; }
			set { listUsers = value; }
		}

		public int Number
		{
			get { return number; }
			set { number = value; }
		}

		public bool WorkingThread
		{
			get { return workingThread; }
			set { workingThread = value; }
		}

		public bool WorkingServer
		{
			get { return workingServer; }
			set { workingServer = value; }
		}
	}
}
