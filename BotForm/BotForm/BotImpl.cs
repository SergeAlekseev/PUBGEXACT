using BotLibrary;

using System.Drawing;

using ClassLibrary;
using System.Linq;
using ClassLibrary.ProcessingsServer;


namespace BotForm
{
	class BotImpl : Bot
	{
		bool isFullFlag = false;
		bool isReload = false;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		protected override void doBot(BotModel model)
		{
			if (model.ThreadStart)
			{
				Point botLocation = model.ThisUser.userLocation;
				if (model.Map.ListItems.Count > 0 && !isFullFlag && !isFull(model.ThisUser.Items))
				{
					Point itemLocation = model.Map.ListItems[0].Location;
					moveToPoint(botLocation, itemLocation, false);

					tryTakeItem(itemLocation);
				}
				else
				{
					Point center = model.Map.NextZone.ZoneCenterCoordinates;
					moveToPoint(botLocation, center, false);
				}

				if (model.ListUsers.Count > 1 && model.ThisUser.Items[model.ThisUser.thisItem] != null && model.ThisUser.Items[model.ThisUser.thisItem].Name != null)
				{
					captureTarget(model.ListUsers[0].userLocation);
					if (!isEmptyBulets(model.ThisUser.Items[model.ThisUser.thisItem]))
					{
						shotOn();
						isReload = false;
					}
						
					else if(!isReload)
					{
						shotOff();
						rechange();
						isReload = true;
					}
				}
				else
				{
					shotOff();
				}
			}
			
		}

		private bool isFull(Item[] items)
		{
			for(int i = 1; i < items.Count(); i++)
			{
				if (items[i].Name == null)
				{
					return false;
				}
			}
			isFullFlag = true;
			return true;
		}

		private bool isEmptyBulets(Item gun)
		{
			return gun.Count == 0;
		}
	}
}
