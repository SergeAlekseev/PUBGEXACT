using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

			timerPaint.Start();

			model.Images[0] = Properties.Resources.Bush.GetThumbnailImage(23, 23, null, IntPtr.Zero);
			model.Images[1] = Properties.Resources.Grass.GetThumbnailImage(23, 23, null, IntPtr.Zero);
			model.Images[2] = Properties.Resources.Box.GetThumbnailImage(25, 25, null, IntPtr.Zero);

			graphics = map.CreateGraphics();
			bufferedGraphicsContext = new BufferedGraphicsContext();
			bufferedGraphics = bufferedGraphicsContext.Allocate(graphics, new Rectangle(0, 0, map.Width, map.Height));
			bufferedGraphics.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
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

			bufferedGraphics.Render(graphics);
			bufferedGraphics.Graphics.Clear(DefaultBackColor);
		}

		private void View_MouseMove(object sender, MouseEventArgs e)
		{
	
		}

		private void View_KeyDown(object sender, KeyEventArgs e)
		{
			model.MoovUser = true;
			//while (model.MoovUser)
			//{
			//	if (e.KeyCode == Keys.W)
			//	{
			//		model.MouseCoord = new Point(model.MouseCoord.X, model.MouseCoord.Y + 1);
			//		model.ThisUserLocation = new Point(model.ThisUserLocation.X, model.ThisUserLocation.Y + 1);
			//	}
			//	else if (e.KeyCode == Keys.S)
			//	{
			//		model.MouseCoord = new Point(model.MouseCoord.X, model.MouseCoord.Y - 1);
			//		model.ThisUserLocation = new Point(model.ThisUserLocation.X, model.ThisUserLocation.Y - 1);
			//	}
			//	else if (e.KeyCode == Keys.D)
			//	{
			//		model.MouseCoord = new Point(model.MouseCoord.X+1, model.MouseCoord.Y);
			//		model.ThisUserLocation = new Point(model.ThisUserLocation.X+1, model.ThisUserLocation.Y);
			//	}
			//	else if (e.KeyCode == Keys.A)
			//	{
			//		model.MouseCoord = new Point(model.MouseCoord.X -1, model.MouseCoord.Y);
			//		model.ThisUserLocation = new Point(model.ThisUserLocation.X -1, model.ThisUserLocation.Y);
			//	}				
			//}
		}

		private void View_KeyUp(object sender, KeyEventArgs e)
		{
			model.MoovUser = false;
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

			Controllers.cEditor.Motion(model.MouseCoordFirst);
		}

		private void bushes_Click(object sender, EventArgs e)
		{
			Items item = new Bush();
			model.Item = item.Add;
		}

		private void map_Click(object sender, EventArgs e)
		{

		}

		private void boxes_Click(object sender, EventArgs e)
		{
			Items item = new Box();
			model.Item = item.Add;
		}

		private void map_MouseMove(object sender, MouseEventArgs e)
		{
			model.MouseCoordFirst = e.Location;
			X.Text = "" + e.X;
			Y.Text = "" + e.Y;
		}
	}
}
