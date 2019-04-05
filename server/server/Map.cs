using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
	class Map
	{
		//Пока не знаю,что здесь будет лежать	
		List<Box> listBox = new List<Box>();
		List<Bush> listBush = new List<Bush>();

		public List<Box> ListBox
		{
			get { return listBox; }
			set { listBox = value; }
		}

		public List<Bush> ListBush
		{
			get { return listBush; }
			set { listBush = value; }
		}
	}
}
