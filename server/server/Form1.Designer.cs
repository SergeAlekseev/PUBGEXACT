namespace server
{
	partial class Server
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server));
			this.start = new System.Windows.Forms.Button();
			this.stop = new System.Windows.Forms.Button();
			this.status = new System.Windows.Forms.Label();
			this.startGame = new System.Windows.Forms.Button();
			this.maps = new System.Windows.Forms.Button();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// start
			// 
			this.start.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.start.Location = new System.Drawing.Point(25, 191);
			this.start.Name = "start";
			this.start.Size = new System.Drawing.Size(101, 23);
			this.start.TabIndex = 0;
			this.start.Text = "Старт сервер";
			this.start.UseVisualStyleBackColor = true;
			this.start.Click += new System.EventHandler(this.start_Click);
			// 
			// stop
			// 
			this.stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.stop.Location = new System.Drawing.Point(224, 191);
			this.stop.Name = "stop";
			this.stop.Size = new System.Drawing.Size(75, 23);
			this.stop.TabIndex = 1;
			this.stop.Text = "Стоп";
			this.stop.UseVisualStyleBackColor = true;
			this.stop.Click += new System.EventHandler(this.stop_Click);
			// 
			// status
			// 
			this.status.AutoSize = true;
			this.status.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.status.Location = new System.Drawing.Point(12, 29);
			this.status.Name = "status";
			this.status.Size = new System.Drawing.Size(110, 13);
			this.status.TabIndex = 2;
			this.status.Text = "Сервер отключен";
			// 
			// startGame
			// 
			this.startGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.startGame.Location = new System.Drawing.Point(25, 110);
			this.startGame.Name = "startGame";
			this.startGame.Size = new System.Drawing.Size(274, 52);
			this.startGame.TabIndex = 3;
			this.startGame.Text = "Начало игры";
			this.startGame.UseVisualStyleBackColor = true;
			this.startGame.Click += new System.EventHandler(this.startGame_Click);
			// 
			// maps
			// 
			this.maps.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.maps.Location = new System.Drawing.Point(132, 191);
			this.maps.Name = "maps";
			this.maps.Size = new System.Drawing.Size(86, 23);
			this.maps.TabIndex = 4;
			this.maps.Text = "Карты";
			this.maps.UseVisualStyleBackColor = true;
			this.maps.Click += new System.EventHandler(this.maps_Click);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(445, 24);
			this.menuStrip1.TabIndex = 5;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// listBox1
			// 
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Location = new System.Drawing.Point(313, 12);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(120, 173);
			this.listBox1.TabIndex = 6;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(313, 191);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(120, 23);
			this.button1.TabIndex = 7;
			this.button1.Text = "Смотреть";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// Server
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(445, 238);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.maps);
			this.Controls.Add(this.startGame);
			this.Controls.Add(this.status);
			this.Controls.Add(this.stop);
			this.Controls.Add(this.start);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Server";
			this.Text = "Server";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Server_FormClosing);
			this.Load += new System.EventHandler(this.Server_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button start;
		private System.Windows.Forms.Button stop;
		private System.Windows.Forms.Label status;
		private System.Windows.Forms.Button startGame;
		private System.Windows.Forms.Button maps;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Button button1;
	}
}

