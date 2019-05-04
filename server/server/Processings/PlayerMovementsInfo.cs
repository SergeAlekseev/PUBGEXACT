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
	class PlayerMovementsInfo:Processing
	{

		public override void Process(int numberUser)
		{
			string tmpString = CTransfers.Reading(Model.ListNs[numberUser]);
			ClassLibrary.Action act = JsonConvert.DeserializeObject<ClassLibrary.Action>(tmpString);
			switch (act.act)
			{
				case ClassLibrary.Action.action.moveUp: Model.ListMove[numberUser].moveUp = true; break;
				case ClassLibrary.Action.action.moveDown: Model.ListMove[numberUser].moveDown = true; break;
				case ClassLibrary.Action.action.noveLeft: Model.ListMove[numberUser].moveLeft = true; break;
				case ClassLibrary.Action.action.moveRight: Model.ListMove[numberUser].moveRight = true; break;
				case ClassLibrary.Action.action.shiftDown: Model.ListMove[numberUser].shift = true; break;

				case ClassLibrary.Action.action.stopUp: Model.ListMove[numberUser].moveUp = false; break;
				case ClassLibrary.Action.action.stopDown: Model.ListMove[numberUser].moveDown = false; break;
				case ClassLibrary.Action.action.stopLeft: Model.ListMove[numberUser].moveLeft = false; break;
				case ClassLibrary.Action.action.stopRight: Model.ListMove[numberUser].moveRight = false; break;
				case ClassLibrary.Action.action.shiftUp: Model.ListMove[numberUser].shift = false; break;
			}
		}
	}
}
