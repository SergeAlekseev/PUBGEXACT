using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapEdit
{
	class CEditor
	{
		public Model model = new Model();

		public void Motion(Point mouseLocation)
		{
			if (mouseLocation.X >= 580 && mouseLocation.Y < 600)
			{
				model.MouseCoordSecond = new Point(model.MouseCoordSecond.X+4, model.MouseCoordSecond.Y);
				model.ThisUserLocation = new Point(model.ThisUserLocation.X+4, model.ThisUserLocation.Y);
			}
			if(mouseLocation.X < 600 && mouseLocation.Y >= 580)
			{
				model.MouseCoordSecond = new Point(model.MouseCoordSecond.X, model.MouseCoordSecond.Y + 4);
				model.ThisUserLocation = new Point(model.ThisUserLocation.X, model.ThisUserLocation.Y+4);
			}
			if (mouseLocation.X <= 20 && mouseLocation.Y < 600)
			{
				model.MouseCoordSecond = new Point(model.MouseCoordSecond.X-4, model.MouseCoordSecond.Y);
				model.ThisUserLocation = new Point(model.ThisUserLocation.X-4, model.ThisUserLocation.Y);
			}
			if (mouseLocation.X < 600 && mouseLocation.Y <=20)
			{
				model.MouseCoordSecond = new Point(model.MouseCoordSecond.X, model.MouseCoordSecond.Y - 4);
				model.ThisUserLocation = new Point(model.ThisUserLocation.X, model.ThisUserLocation.Y - 4);
			}			
		}

		public Point Sum(Point p1, Point p2)
		{
			p1.X += p2.X;
			p1.Y += p2.Y;

			return p1;
		}

		public void SaveMap()
		{
			BinaryFormatter formatter = new BinaryFormatter();
			SaveFileDialog sn = new SaveFileDialog();
			sn.DefaultExt = ".dat";
			sn.Filter = "Text files(*.dat)|*.dat|All files(*.*)|*.*";

			sn.InitialDirectory = @"C:\Users\Василий\Desktop\Exaxt\PUBGEXACT\MapEdit\MapEdit\bin\Debug";
			DialogResult res = sn.ShowDialog();
			if (res == DialogResult.Cancel)
				return;
			if (res == DialogResult.OK)
			{
				string NameFile = sn.FileName;
				try
				{
					using (FileStream fs = new FileStream(NameFile, FileMode.Create))
					{

						formatter.Serialize(fs, model.Map);
						fs.Close();
					}

					MessageBox.Show("Файл создан");
				}
				catch (Exception err)
				{
					MessageBox.Show(err.Message);
					return;
				}
			}
		}

		public void LoadMap()
		{
			BinaryFormatter formatter = new BinaryFormatter();
			OpenFileDialog sn = new OpenFileDialog();
			sn.DefaultExt = ".dat";
			sn.Filter = "Text files(*.dat)|*.dat|All files(*.*)|*.*";
			sn.InitialDirectory = @"G:\2 курс\О.О.П\Графический редактор\Графический редактор\bin\Debug\Save";
			DialogResult res = sn.ShowDialog();
			if (res == DialogResult.Cancel)
				return;
			if (res == DialogResult.OK)
			{
				string NameFile = sn.FileName;
				try
				{
					using (FileStream fs = new FileStream(NameFile, FileMode.Open))
					{
						model.Map = (Map)formatter.Deserialize(fs);
					}
				}
				catch (Exception)
				{
					MessageBox.Show("Ошибка!");
				}
			}
		}
	}
}
