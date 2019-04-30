using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEdit
{
	[Serializable]
	abstract class Items
	{
		public abstract void Add(Point mouseLocation);
	}
}
