using System;

namespace ClassLibrary
{
	[Serializable]
	public class GeneralInfo
	{
		public string Name;
		public string Password;
		public int Kills = 0;
		public int Wins = 0;
		public int Dies = 0;
	}
}
