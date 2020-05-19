using BotLibrary;

using System.Drawing;

using ClassLibrary;
using System.Linq;
using ClassLibrary.ProcessingsServer;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

namespace BotForm
{
	class BotImpl : Bot
	{
		bool isFullFlag = false;
		bool isReload = false;
		bool braveBot = true;

		public BotImpl()
		{
			Random r = new Random();
			braveBot = r.Next(0, 2) == 0;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		protected override void doBot(BotModel model)
		{
			if (model.ThreadStart)
			{
				Point targetLoc = new Point(-300, -300);
				Point botLocation = model.ThisUser.userLocation;
				if (model.ListUsers.Count > 1 && gunExists(model))
				{
					UserInfo user = immediateTarget(model.ThisUser, model.ListUsers);
					targetLoc = user.userLocation;

					runToPlayer(targetLoc, botLocation, braveBot);
				}
				else if (model.Map.ListItems.Count > 0 && !isFullFlag && !isFull(model.ThisUser.Items) && isGrenade(model))
				{
					//Ѕот бежит к первой ближайшей вещи в зоне видимости, думаю тут  и так мен€ть ничего не нужно
					Point itemLocation = model.Map.ListItems[0].Location;
					moveToPoint(botLocation, itemLocation, false);

					tryTakeItem(itemLocation);
				}
				else
				{
					Point center = model.Map.NextZone.ZoneCenterCoordinates;
					moveToPoint(botLocation, center, false);
				}

				if (model.ListUsers.Count > 1 && gunExists(model))
				{
					captureTarget(targetLoc);
					if (!isEmptyBulets(model.ThisUser.Items[model.ThisUser.thisItem]))
					{
						shotOn();
						isReload = false;
					}

					else if (!isReload)
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

		private bool inBox(Point botLocation, List<Box> boxes)
		{
			foreach(Box box in boxes)
			{
				if ((botLocation.X > box.Location.X - 30 && botLocation.X < box.Location.X + 30) && (botLocation.Y > box.Location.Y - 30 && botLocation.Y < box.Location.Y + 30))
					return true;
			}
			return false;
		}

		private static bool isGrenade(BotModel model)
		{
			return model.Map.ListItems[0].Name != "Grenade";
		}

		private static bool gunExists(BotModel model)
		{
			return model.ThisUser.Items[model.ThisUser.thisItem] != null && model.ThisUser.Items[model.ThisUser.thisItem].Name != null;
		}

		private Point runToPlayer(Point targetLoc, Point botLocation, bool runToPlayer)
		{
			Vector v = new Vector(botLocation, targetLoc);
			if(runToPlayer)
			{
				if (v.Length > 150)
				{
						moveToPoint(botLocation, targetLoc, false);	
				}
				else
				{
					Point offset = new Point(targetLoc.X - botLocation.X, targetLoc.Y - botLocation.Y);			
					moveToPoint(botLocation, new Point(targetLoc.X - offset.X, targetLoc.Y - offset.Y), false);			
				}
			}
			else
			{
				//if (v.Length < 150)
					moveToPoint(targetLoc, botLocation, false);
			}

			return targetLoc;
		}

		private UserInfo immediateTarget(UserInfo thisUser, List<UserInfo> users)
		{
			Point botLocation = thisUser.userLocation;
			double minValue = double.MaxValue;
			UserInfo minUser = new UserInfo(new Point(-200, -200));

			if (users == null) return null;

			for(int i = 0; i < users.Count; i++)
			{
				UserInfo user = users[i];
				if (user.userNumber != thisUser.userNumber)
				{
					Vector v = new Vector(botLocation, user.userLocation);

					if (v.Length < minValue)
					{
						minValue = v.Length;
						minUser = user;
					}
				}
			}

			return minUser;
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
