using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ControllersServer
{
	public class CStart
	{
		ModelServer model;
		public CStart(ModelServer model)
		{
			this.model = model;
		}
		public void MenuConnecting(object tc)//старт
		{
			while (model.workingServer)
			{
				try
				{
					(tc as TcpListener).AcceptTcpClient();
				}
				catch (Exception err)
				{
					//ErrorEvent(err.Message + " |Ошибка в ControllerServer, методе MenuConnecting");
					break;
				}
			}

		}

		public void MenuStarting(object tl)//старт
		{
			TcpClient tc = null;
			NetworkStream nStream = null;
			while (model.workingServer)
			{

				try
				{
					tc = (tl as TcpListener).AcceptTcpClient();
				}
				catch
				{
					break;
				}

				nStream = tc.GetStream();

				byte[] typeCommand = new byte[1];
				nStream.Read(typeCommand, 0, 1);

				switch (typeCommand[0])
				{
					case 10:
						{

							string tmpString = CTransfers.Reading(nStream);
							GeneralInfo newUser = JsonConvert.DeserializeObject<GeneralInfo>(tmpString);

							GeneralInfo tmpUser = ControllersS.cInfoUsers.PlayerCheck(ControllersS.cInfoUsers.PlayerRead(newUser), newUser);
							if (!model.workingGame)
							{
								if (tmpUser == null)
								{
									model.ListGInfo.Add(new GeneralInfo());
									model.ListGInfo[model.ListGInfo.Count - 1].Name = newUser.Name;
									model.ListGInfo[model.ListGInfo.Count - 1].Password = newUser.Password;

									ControllersS.cInfoUsers.PlayerSave(model.ListGInfo);


									CTransfers.WritingInMenu(model.ListGInfo[model.ListGInfo.Count - 1], 10, nStream);
								}
								else
								{
									if (ControllersS.cInfoUsers.CheckData(model.ListGInfo, newUser))
									{
										model.ListGInfo = ControllersS.cInfoUsers.PlayerRead(newUser);
										CTransfers.WritingInMenu(tmpUser, 10, nStream);
									}
									else
									{
										CTransfers.WritingInMenu("1", 11, nStream);
									}
									//Если такой игрок уже есть , то при правильном пароле выдать всю инфу об игроке
								}
							}
							else
							{
								if (tmpUser == null && model.ListGInfo.Count > 0)
								{
									model.ListGInfo.Add(new GeneralInfo());
									model.ListGInfo[model.ListGInfo.Count - 1].Name = newUser.Name;
									model.ListGInfo[model.ListGInfo.Count - 1].Password = newUser.Password;

									ControllersS.cInfoUsers.PlayerSave(model.ListGInfo);
									CTransfers.WritingInMenu(model.ListGInfo[model.ListGInfo.Count - 1], 12, nStream);
								}
								else
								{
									if (ControllersS.cInfoUsers.CheckData(model.ListGInfo, newUser))
									{
										model.ListGInfo = ControllersS.cInfoUsers.PlayerRead(newUser);
										CTransfers.WritingInMenu(tmpUser, 12, nStream);
									}
									else
									{
										CTransfers.WritingInMenu("1", 12, nStream);
									}
								}
							}
							break;
						}
				}
				tc.Close();
			}
		}
	}
}
