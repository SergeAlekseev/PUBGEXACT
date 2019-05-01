using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibary;
namespace MapEdit
{
	[Serializable]
	class Model
	{
		Image[] images = new Image[30];
		Map map = new Map();
		Point mouseCoordFirst = new Point();
		Point mouseCoordSecond = new Point(0,0);
		Point thisUserLocation = new Point(300,300);
		bool moovUser = false;
		public delegate void ItemD(Point mouseLocation);
		public ItemD Item;
		bool mouseOutside = false;

		public bool MouseOutside
		{
			get { return mouseOutside; }
			set { mouseOutside = value; }
		}

		public Image [] Images
		{
			get { return images; }
			set { images = value; }
		}

		public Map Map
		{
			get { return map; }
			set { map = value; }
		}

		public Point MouseCoordFirst
		{
			get { return mouseCoordFirst; }
			set { mouseCoordFirst = value; }
		}

		public Point MouseCoordSecond
		{
			get { return mouseCoordSecond; }
			set { mouseCoordSecond = value; }
		}

		public Point ThisUserLocation
		{
			get { return thisUserLocation; }
			set { thisUserLocation = value; }
		}

		public bool MoovUser
		{
			get { return moovUser; }
			set { moovUser = value; }
		}
	}
}
