using System;
using System.Windows.Forms;

namespace server
{
	static class Program
	{
		/// <summary>
		/// Главная точка входа для приложения.
		/// </summary>
		[STAThread]
		static void Main(String[] args)
		{
			String autoSetupArg = "-auto";

			if (args.Length == 0)
			{
				setupApplication(false);
			}
			else
			{
				foreach (String arg in args)
				{
					if(arg.Equals(autoSetupArg))
					{
						setupApplication(true);
						break;
					}
				}
				throw new ArgumentException("No matching arguments found");
			}
		}

		private static void setupApplication(bool isAuto)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Server(isAuto));
		}
	}
}
