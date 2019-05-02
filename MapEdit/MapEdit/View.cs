using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary;
namespace MapEdit
{
	public partial class View : Form
	{
		Graphics graphics;
		BufferedGraphicsContext bufferedGraphicsContext;
		BufferedGraphics bufferedGraphics;
		Model model = Controllers.cEditor.model;

		public View()
		{
			InitializeComponent();

			model.Images[0] = Properties.Resources.Bush.GetThumbnailImage(23, 23, null, IntPtr.Zero);
			model.Images[1] = Properties.Resources.Grass.GetThumbnailImage(23, 23, null, IntPtr.Zero);
			model.Images[2] = Properties.Resources.box.GetThumbnailImage(25, 25, null, IntPtr.Zero);

			graphics = map.CreateGraphics();
			bufferedGraphicsContext = new BufferedGraphicsContext();
			bufferedGraphics = bufferedGraphicsContext.Allocate(graphics, new Rectangle(0, 0, map.Width, map.Height));
			bufferedGraphics.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			timerPaint.Start();
		}

		private void pictureBox1_Paint(object sender, PaintEventArgs e)
		{
			
			bufferedGraphics.Graphics.DrawString("Hallo", new Font("Times New Roman", 12, FontStyle.Bold), Brushes.Red, 100, 100);
			if (model.Images[1] != null)
			{
				TextureBrush grass = new TextureBrush(model.Images[1]);
				grass.TranslateTransform(-model.ThisUserLocation.X, -model.ThisUserLocation.Y);
				bufferedGraphics.Graphics.FillRectangle(grass, model.Map.MapBorders.X + map.Width / 2 - model.ThisUserLocation.X, model.Map.MapBorders.Y + map.Height / 2 - model.ThisUserLocation.Y, model.Map.MapBorders.Width + 3, model.Map.MapBorders.Height + 3);
			}
			foreach (Box box in model.Map.ListBox)
			{
				bufferedGraphics.Graphics.DrawImage(model.Images[2], box.Location.X + map.Width / 2 - 10 - model.ThisUserLocation.X, box.Location.Y + map.Height / 2 - 10 - model.ThisUserLocation.Y, 20, 20);
			}

			foreach (Bush bush in model.Map.ListBush)
			{
				bufferedGraphics.Graphics.DrawImage(model.Images[0], bush.Location.X + map.Width / 2 - 10 - model.ThisUserLocation.X, bush.Location.Y + map.Height / 2 - 10 - model.ThisUserLocation.Y, 20, 20);
			}
			bufferedGraphics.Graphics.DrawRectangle(Pens.Red, model.Map.MapBorders.X + map.Width / 2 - model.ThisUserLocation.X, model.Map.MapBorders.Y + map.Height / 2 - model.ThisUserLocation.Y, model.Map.MapBorders.Width + 3, model.Map.MapBorders.Height + 3);
			bufferedGraphics.Render(graphics);
			bufferedGraphics.Graphics.Clear(DefaultBackColor);
		}

		private void Start_Click(object sender, EventArgs e)
		{
			if (textBox1.Text == "") return;

			int Borders = Convert.ToInt32(textBox1.Text);
			model.Map.MapBorders = new Rectangle(0,0, Borders,Borders);
		}

		private void timerPaint_Tick(object sender, EventArgs e)
		{
			pictureBox1_Paint(null,null);

			X.Text = "" + model.MouseCoordFirst.X;
			Y.Text = "" + model.MouseCoordFirst.Y;

			label2.Text = "" + model.MouseCoordSecond.X;
			label3.Text = "" + model.MouseCoordSecond.Y;

			Controllers.cEditor.Motion(model.MouseCoordFirst);
		}

		private void bushes_Click(object sender, EventArgs e)
		{
			Items item = new CBush();
			model.Item = item.Add;
		}

		private void map_Click(object sender, EventArgs e)
		{
			if (model.Item != null) model.Item(Controllers.cEditor.Sum(model.MouseCoordFirst,model.MouseCoordSecond));// ща исправлю
		}

		private void boxes_Click(object sender, EventArgs e)
		{
			Items item = new CBox();
			model.Item = item.Add;
		}

		private void map_MouseMove(object sender, MouseEventArgs e)
		{
			if (model.MouseOutside == true) model.MouseOutside = false;
			model.MouseCoordFirst = e.Location;
		}

		private void save_Click(object sender, EventArgs e)
		{
			Controllers.cEditor.SaveMap();
		}

		private void load_Click(object sender, EventArgs e)
		{
			Controllers.cEditor.LoadMap();
		}

		private void map_MouseLeave(object sender, EventArgs e)
		{
			model.MouseOutside = true;
		}
	}
}
