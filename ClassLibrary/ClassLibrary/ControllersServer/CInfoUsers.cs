using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ControllersServer
{
	public class CInfoUsers
	{
		ModelServer model;
		public CInfoUsers(ModelServer model)
		{
			this.model = model;
		}

		public GeneralInfo PlayerCheck(List<GeneralInfo> listUser, GeneralInfo newUser)//данные игрока
		{
			try
			{
				if (listUser != null)
					foreach (GeneralInfo user in listUser)
					{

						if (user != null && user.Name == newUser.Name) return user;

					}
				return null;
			}
			catch { return null; }
		}

		public bool CheckData(List<GeneralInfo> listUser, GeneralInfo newUser)//данные игрока
		{
			try
			{
				if (listUser != null)
					foreach (GeneralInfo user in listUser)
					{
						if (user != null && user.Name == newUser.Name && user.Password == newUser.Password) return true;
					}
				return false;
			}
			catch { return false; }
		}

		public List<GeneralInfo> PlayerRead(GeneralInfo newUser)//данные игрока
		{
			BinaryFormatter formatter = new BinaryFormatter();
			try
			{
				using (FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "usersInfo.dat", FileMode.Open))
				{
					return (List<GeneralInfo>)formatter.Deserialize(fs);
				}
			}
			catch (Exception)
			{
				//ErrorEvent("Произошло создание нового списка для зарегестрированных пользователей");
				List<GeneralInfo> newList = new List<GeneralInfo>();
				if (newUser != null)
					newList.Add(newUser);
				else
				{
					GeneralInfo g = new GeneralInfo();
					g.Name = "admin";
					g.Password = "admin";
					newList.Add(g);
				}
				PlayerSave(newList);
				return newList;
			}
		}

		public bool PlayerSave(List<GeneralInfo> listUsers)//данные игрока
		{
			try
			{
				BinaryFormatter formatter = new BinaryFormatter();
				using (FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "usersInfo.dat", FileMode.OpenOrCreate))
				{
					formatter.Serialize(fs, listUsers);
				}
				return true;
			}
			catch { return false; }
		}
	}
}
