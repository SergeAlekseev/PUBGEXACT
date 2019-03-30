using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client;

namespace client
{
	public class Action
	{
		public enum action { moveUp, moveDown, moveRight, noveLeft, stopUp, stopDown, stopLeft, stopRight };
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
