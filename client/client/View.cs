using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
namespace client
{
	public partial class Client : Form
	{
		public delegate void ConnectD();
		public event ConnectD ConnectEvent;

		public delegate void DisconnectD();
		public event DisconnectD DisconnectEvent;

		public delegate void ActionD ();
		public event ActionD ActionEvent;

		static Model model = new Model();
		Controller controller = new Controller(model);
		
		Graphics pictureBox;
		BufferedGraphicsContext bufferedGraphicsContext;
		BufferedGraphics bufferedGraphics;

		bool Start = false;


		public Client() 
		{
			InitializeComponent();
			timerPaint.Interval = 20;
			timerPaint.Start();
			
			pictureBox = PlayingField.CreateGraphics();
			bufferedGraphicsContext = new BufferedGraphicsContext();
			bufferedGraphics = bufferedGraphicsContext.Allocate(pictureBox, new Rectangle(0,0, PlayingField.Width, PlayingField.Height));
			bufferedGraphics.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			ActionEvent += controller.PressKey;
			ConnectEvent += controller.Connect;
			DisconnectEvent += controller.Disconnect;

			controller.CloseFormEvent += Client_FormClosing;
			controller.CloseEvent += ErrorOfConnect;
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e) //Обработчик нажатия на кнопку 
		{

		}

		private void Form1_KeyUp(object sender, KeyEventArgs e) //Обработчик  отпускания кнопки
		{

		}

		
		private void Form1_KeyPress(object sender, KeyPressEventArgs e)// ПРОДУМАТЬ,ШОБЕ МОЖНО БЫЛО ЗАЖИМАТЬ РАЗНЫЕ КЛАВИШИ 
		{
			
			switch (e.KeyChar)
			{
				case 'W': case 'w': case 'Ц': case 'ц': model.Action.actionThishUser = Action.action.moveUp; ActionEvent(); break;
				case 'S': case 's': case 'Ы': case 'ы': model.Action.actionThishUser = Action.action.moveDown; ActionEvent(); break;
				case 'D': case 'd': case 'В': case 'в': model.Action.actionThishUser = Action.action.moveRight; ActionEvent(); break;
				case 'A': case 'a': case 'Ф': case 'ф': model.Action.actionThishUser = Action.action.noveLeft; ActionEvent(); break;
				
			}
		}
		

		private void Client_Paint(object sender, PaintEventArgs e)

		{
			if (model.ListUsers != null)
			{
				foreach (UserInfo user in model.ListUsers)
				{
					if (user != null)
						bufferedGraphics.Graphics.FillEllipse(Brushes.Red, user.userLocation.X - 2, user.userLocation.Y - 2, 4, 4);
				}
				bufferedGraphics.Graphics.DrawString(model.Ping + "", new Font("Times New Roman", 10, FontStyle.Bold), Brushes.Green, 2, 2);

				bufferedGraphics.Render(pictureBox);
				bufferedGraphics.Graphics.Clear(DefaultBackColor);
			}
			
		}

		
		private void timerPaint_Tick(object sender, EventArgs e) 
		{
			Invalidate();
		}

		private void Client_FormClosing(object sender, FormClosingEventArgs e) //Вызвать событие в Controller
		{
			DisconnectEvent();
		}

		private void PlayingField_Click(object sender, EventArgs e) //Вызвать событие в Controller
		{
			if (!Start)
			{
				ConnectEvent();
				Start = true;
			}
		}

		public void ErrorOfConnect()
		{
			BeginInvoke(new MethodInvoker(delegate
			{
				Application.Exit();
				Close();
			}));
		}
	}
}
