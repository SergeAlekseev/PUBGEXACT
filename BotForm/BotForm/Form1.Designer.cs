namespace BotForm
{
	partial class Form1
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
			this.button1 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.serverIp = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.name = new System.Windows.Forms.TextBox();
			this.password = new System.Windows.Forms.TextBox();
			this.start = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.Настройки = new System.Windows.Forms.TabPage();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.start.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.Настройки.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(18, 6);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(111, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Запустить";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label1
			// 
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label1.Location = new System.Drawing.Point(18, 41);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(111, 80);
			this.label1.TabIndex = 1;
			this.label1.UseMnemonic = false;
			// 
			// serverIp
			// 
			this.serverIp.Location = new System.Drawing.Point(75, 9);
			this.serverIp.Name = "serverIp";
			this.serverIp.Size = new System.Drawing.Size(99, 20);
			this.serverIp.TabIndex = 2;
			this.serverIp.Text = "127.0.0.1";
			this.serverIp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 12);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(61, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Ip сервера";
			// 
			// name
			// 
			this.name.Location = new System.Drawing.Point(75, 35);
			this.name.Name = "name";
			this.name.Size = new System.Drawing.Size(99, 20);
			this.name.TabIndex = 4;
			this.name.Text = "chippa1";
			// 
			// password
			// 
			this.password.Location = new System.Drawing.Point(75, 61);
			this.password.Name = "password";
			this.password.PasswordChar = '*';
			this.password.Size = new System.Drawing.Size(99, 20);
			this.password.TabIndex = 5;
			this.password.Text = "123";
			// 
			// start
			// 
			this.start.Controls.Add(this.tabPage1);
			this.start.Controls.Add(this.Настройки);
			this.start.Dock = System.Windows.Forms.DockStyle.Fill;
			this.start.Location = new System.Drawing.Point(0, 0);
			this.start.Name = "start";
			this.start.SelectedIndex = 0;
			this.start.Size = new System.Drawing.Size(228, 163);
			this.start.TabIndex = 6;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.button1);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(220, 137);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Запуск";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// Настройки
			// 
			this.Настройки.Controls.Add(this.label4);
			this.Настройки.Controls.Add(this.label3);
			this.Настройки.Controls.Add(this.label2);
			this.Настройки.Controls.Add(this.password);
			this.Настройки.Controls.Add(this.name);
			this.Настройки.Controls.Add(this.serverIp);
			this.Настройки.Location = new System.Drawing.Point(4, 22);
			this.Настройки.Name = "Настройки";
			this.Настройки.Padding = new System.Windows.Forms.Padding(3);
			this.Настройки.Size = new System.Drawing.Size(220, 137);
			this.Настройки.TabIndex = 1;
			this.Настройки.Text = "Настройки";
			this.Настройки.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(8, 64);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(45, 13);
			this.label4.TabIndex = 7;
			this.label4.Text = "Пароль";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(8, 38);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(29, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Имя";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(228, 163);
			this.Controls.Add(this.start);
			this.MaximumSize = new System.Drawing.Size(244, 201);
			this.MinimumSize = new System.Drawing.Size(244, 201);
			this.Name = "Form1";
			this.Text = "Бот";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.start.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.Настройки.ResumeLayout(false);
			this.Настройки.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox serverIp;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox name;
		private System.Windows.Forms.TextBox password;
		private System.Windows.Forms.TabControl start;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage Настройки;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
	}
}

