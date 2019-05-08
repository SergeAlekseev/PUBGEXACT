﻿using ClassLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace client
{
	class CMyMenu
	{
		TcpClient client;// 25.46.244.0 
		NetworkStream nStream;
		ModelClient model = new ModelClient();
		public bool flagGame = false;
		public void JoinUser(string Name, string Password)
		{
			model.GInfo.Name = Name;
			model.GInfo.Password = Password;
		}
		public CMyMenu(ModelClient model)
		{
			this.model = model;
		}

		public bool Connect(string ip)// Controller
		{

			client = new TcpClient(ip, 2337);
			nStream = client.GetStream();

			Writing(model.GInfo, 10);
			if (ReadingStream()) return true;
			else return false;

		}

		public bool ReadingStream()
		{
			bool flag = false;
			while (!flag)
			{
				byte[] typeCommand = new byte[1];
				try
				{
					nStream.Read(typeCommand, 0, 1);
				}
				catch
				{
					flagGame = false;
					break;
				}
				if (typeCommand[0] == 10)
				{
					string tmpString = Reading(nStream);
					model.GInfo = JsonConvert.DeserializeObject<GeneralInfo>(tmpString);
					flag = true;
					flagGame = false;
					return true;
				}
				else if (typeCommand[0] == 11)
				{
					flag = true;
					flagGame = false;
					return false;
				}
				else if (typeCommand[0] == 12)
				{
					string tmpString = Reading(nStream);
					model.GInfo = JsonConvert.DeserializeObject<GeneralInfo>(tmpString);
					flag = true;
					flagGame = true;
					return true;
				}
			}
			return false;
		}

		public string Reading(NetworkStream nStream)
		{
			byte[] countRead = new byte[4];
			int countReadingBytes = 0;
			while (countReadingBytes != 4)
				countReadingBytes += nStream.Read(countRead, countReadingBytes, countRead.Count() - countReadingBytes);

			countReadingBytes = 0;

			int lengthBytesRaed = BitConverter.ToInt32(countRead, 0);

			byte[] readBytes = new byte[lengthBytesRaed];


			while (countReadingBytes != lengthBytesRaed)
				countReadingBytes += nStream.Read(readBytes, countReadingBytes, readBytes.Count() - countReadingBytes);

			string tmpString = System.Text.Encoding.UTF8.GetString(readBytes);

			return tmpString;
		}

		private void Writing(object obj, byte numComand)
		{
			string serialized = JsonConvert.SerializeObject(obj);
			byte[] massByts = Encoding.UTF8.GetBytes(serialized);
			byte[] countRead = BitConverter.GetBytes(massByts.Count());
			byte[] typeComand = new byte[1];
			typeComand[0] = numComand;

			nStream.Write(typeComand, 0, 1);//Отпраляет тип команды
			nStream.Write(countRead, 0, 4);//Отпраляет кол-во байт, которое сервер должен будет читать
			nStream.Write(massByts, 0, massByts.Count());
		}
	}
}
