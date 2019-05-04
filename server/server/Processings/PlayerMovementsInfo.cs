using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
namespace server.Processings
{
	public class PlayerMovementsInfo:Processing
	{

		int num;
		ClassLibrary.Action action;
		public PlayerMovementsInfo(int num, ClassLibrary.Action action )
		{
			this.num = num;
			this.action = action;
		}
		public override void Process()
		{
			string tmpString = CTransfers.Reading(Model.ListNs[num]);
			ClassLibrary.Action act = JsonConvert.DeserializeObject<ClassLibrary.Action>(tmpString);
			switch (act.act)
			{
				case ClassLibrary.Action.action.moveUp: Model.ListMove[num].moveUp = true; break;
				case ClassLibrary.Action.action.moveDown: Model.ListMove[num].moveDown = true; break;
				case ClassLibrary.Action.action.noveLeft: Model.ListMove[num].moveLeft = true; break;
				case ClassLibrary.Action.action.moveRight: Model.ListMove[num].moveRight = true; break;
				case ClassLibrary.Action.action.shiftDown: Model.ListMove[num].shift = true; break;

				case ClassLibrary.Action.action.stopUp: Model.ListMove[num].moveUp = false; break;
				case ClassLibrary.Action.action.stopDown: Model.ListMove[num].moveDown = false; break;
				case ClassLibrary.Action.action.stopLeft: Model.ListMove[num].moveLeft = false; break;
				case ClassLibrary.Action.action.stopRight: Model.ListMove[num].moveRight = false; break;
				case ClassLibrary.Action.action.shiftUp: Model.ListMove[num].shift = false; break;
			}
		}
	}
}
