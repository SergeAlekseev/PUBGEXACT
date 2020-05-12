using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
	public class Action
	{
		public enum action { moveUp, moveDown, moveRight, moveLeft, stopUp, stopDown, stopLeft, stopRight, shiftDown, shiftUp };
		public action act;
		public Action()
		{

		}
		public Action(action act)
		{
			this.act = act;
		}
		public action actionThishUser
		{
			get { return act; }
			set { act = value; }
		}
	}
}
