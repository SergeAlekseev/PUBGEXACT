using ClassLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace BotLibrary
{
	class LoginTransfer
	{
		public bool readingStream(NetworkStream nStream2, GeneralInfo GInfo)
		{
			bool flag = false;
			while (!flag)
			{
				byte[] typeCommand = new byte[1];
				try
				{
					nStream2.Read(typeCommand, 0, 1);
				}
				catch
				{
					break;
				}
				if (typeCommand[0] == 10 || typeCommand[0] == 12)
				{
					string tmpString = reading(nStream2);
					GInfo = JsonConvert.DeserializeObject<GeneralInfo>(tmpString);
					flag = true;
					return true;
				}
				else if (typeCommand[0] == 11)
				{
					flag = false;
					return false;
				}

			}
			return false;
		}

		private string reading(NetworkStream nStream)
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

			string tmpString = Encoding.UTF8.GetString(readBytes);

			return tmpString;
		}

		public void writing(NetworkStream nStream2, object obj, byte numComand)
		{
			string serialized = JsonConvert.SerializeObject(obj);
			byte[] massByts = Encoding.UTF8.GetBytes(serialized);
			byte[] countRead = BitConverter.GetBytes(massByts.Count());
			byte[] typeComand = new byte[1];
			typeComand[0] = numComand;

			nStream2.Write(typeComand, 0, 1);//Отпраляет тип команды
			nStream2.Write(countRead, 0, 4);//Отпраляет кол-во байт, которое сервер должен будет читать
			nStream2.Write(massByts, 0, massByts.Count());
		}
	}
}
