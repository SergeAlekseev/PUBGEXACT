using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BotForm
{
	static class Program
	{
		/// <summary>
		/// Главная точка входа для приложения.
		/// </summary>
		[STAThread]
		static void Main(String[] args)
		{

			String autoArgName = "-auto";
			if (args.Length == 0)
			{
				setupApplication("");
				return;
			}
	
			if(args.Length == 2)
			{
				String reg =  @"^\d+\.\d+\.\d+\.\d+$";
				if (args[0].Equals(autoArgName) && Regex.IsMatch(args[1], reg))
				{
					setupApplication(args[1]);
					return;
				}
			}

				
				throw new ArgumentException("Mismatch argument found");			
		}

		private static void setupApplication(String ip)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			if (ip.Equals(""))
				Application.Run(new Form1());
			else
				Application.Run(new Form1(ip));
		}
	}
}
