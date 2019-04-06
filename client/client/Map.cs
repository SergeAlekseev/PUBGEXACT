﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{
	class Map
	{
		List<Box> listBox = new List<Box>();
		List<Bush> listBush = new List<Bush>();
		Rectangle mapBorders = new Rectangle();
		Zone zone = new Zone();

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

		public Rectangle MapBorders
		{
			get { return mapBorders; }
			set { mapBorders = value; }
		}

		public Zone Zone
		{
			get { return zone; }
			set { zone = value; }
		}
	}
}
