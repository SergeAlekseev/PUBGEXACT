namespace client
{
	partial class Client
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Client));
			this.PlayingField = new System.Windows.Forms.PictureBox();
			this.timerPaint = new System.Windows.Forms.Timer(this.components);
			this.timerMouseLocation = new System.Windows.Forms.Timer(this.components);
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.ip = new System.Windows.Forms.ToolStripTextBox();
			this.Connect = new System.Windows.Forms.ToolStripMenuItem();
			this.Name = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.PlayingField)).BeginInit();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// PlayingField
			// 
			this.PlayingField.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.PlayingField.Location = new System.Drawing.Point(12, 27);
			this.PlayingField.Name = "PlayingField";
			this.PlayingField.Size = new System.Drawing.Size(600, 600);
			this.PlayingField.TabIndex = 0;
			this.PlayingField.TabStop = false;
			this.PlayingField.Click += new System.EventHandler(this.PlayingField_Click);
			this.PlayingField.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PlayingField_MouseDown);
			this.PlayingField.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PlayingField_MouseUp);
			// 
			// timerPaint
			// 
			this.timerPaint.Tick += new System.EventHandler(this.timerPaint_Tick);
			// 
			// timerMouseLocation
			// 
			this.timerMouseLocation.Tick += new System.EventHandler(this.timerMouseLocation_Tick);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ip,
            this.Connect});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(624, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// ip
			// 
			this.ip.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.ip.Name = "ip";
			this.ip.Size = new System.Drawing.Size(150, 20);
			this.ip.Text = "25.46.244.0";
			this.ip.Leave += new System.EventHandler(this.SelectionOfLetters);
			this.ip.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
			// 
			// Connect
			// 
			this.Connect.Name = "Connect";
			this.Connect.Size = new System.Drawing.Size(64, 20);
			this.Connect.Text = "Connect";
			this.Connect.Click += new System.EventHandler(this.Connect_Click);
			// 
			// Name
			// 
			this.Name.AutoSize = true;
			this.Name.Location = new System.Drawing.Point(377, 9);
			this.Name.Name = "Name";
			this.Name.Size = new System.Drawing.Size(35, 13);
			this.Name.TabIndex = 2;
			this.Name.Text = "label1";
			// 
			// Client
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(624, 639);
			this.Controls.Add(this.Name);
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.PlayingField);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Text = "Client";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Client_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Client_FormClosed);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Client_Paint);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
			((System.ComponentModel.ISupportInitialize)(this.PlayingField)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox PlayingField;
		private System.Windows.Forms.Timer timerPaint;
		private System.Windows.Forms.Timer timerMouseLocation;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripTextBox ip;
		private System.Windows.Forms.ToolStripMenuItem Connect;
		private System.Windows.Forms.Label Name;
	}
}

