using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace client
{
	class Action
	{
		public enum action {moveUp,moveDown,moveRight,noveLeft };
		public action act;
		public Action(action act)
		{
			this.act = act;
		}
	}
}
