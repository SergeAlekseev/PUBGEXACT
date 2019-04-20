
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{
	class Model
	{
		List<UserInfo> listUsers = new List<UserInfo>();
		Action action = new Action();
		int ping = 0;
		UserInfo thisUser = new UserInfo(new Point(300, 300));
		List<BulletInfo> listBullet = new List<BulletInfo>();
		Map map = new Map();
		short lifes;
		bool die;
		Image[] images = new Image[9];
		GeneralInfo gInfo = new GeneralInfo();
		List<GeneralInfo> listGInfo = new List<GeneralInfo>();
		public bool Start = false;
		Point mouseCoord = new Point();
	//	double rotate;
		public Model()
		{
			listUsers = new List<UserInfo>();
			action = new Action();
			ping = -1;
			thisUser = new UserInfo(new Point(300, 300));
			if (thisUser!=null)
			Start = true;
		}

		public Image[] Images { get { return images; } set { images = value; } }
		public bool Die { get { return die; } set { die = value; } }
		public short Lifes { get { return lifes; } set { lifes = value; } }
		public List<BulletInfo> ListBullet {get { return listBullet; }set { listBullet = value; }}
		public UserInfo ThisUser { get { return thisUser; } set { thisUser = value; } }
		public List<UserInfo> ListUsers { get { return listUsers; } set { listUsers = value; } }
		public Action Action { get { return action; } set { action = value; } }
		public int Ping { get { return ping; } set { ping = value; } }
		public Map Map { get { return map; } set { map = value; } }
		public GeneralInfo GInfo { get { return gInfo; } set { gInfo = value; } }
		public List<GeneralInfo> ListGInfo { get { return listGInfo; } set { listGInfo = value; } }
		public Point MouseCoord { get { return mouseCoord; } set { mouseCoord = value; } }
	//	public double Rotate { get { return rotate; } set { rotate = value; } }
	}
}
