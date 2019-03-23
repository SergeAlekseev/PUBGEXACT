namespace server
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
			this.start = new System.Windows.Forms.Button();
			this.stop = new System.Windows.Forms.Button();
			this.status = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// start
			// 
			this.start.Location = new System.Drawing.Point(116, 190);
			this.start.Name = "start";
			this.start.Size = new System.Drawing.Size(75, 23);
			this.start.TabIndex = 0;
			this.start.Text = "Старт";
			this.start.UseVisualStyleBackColor = true;
			this.start.Click += new System.EventHandler(this.start_Click);
			// 
			// stop
			// 
			this.stop.Location = new System.Drawing.Point(260, 190);
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
			this.status.Location = new System.Drawing.Point(12, 9);
			this.status.Name = "status";
			this.status.Size = new System.Drawing.Size(95, 13);
			this.status.TabIndex = 2;
			this.status.Text = "Сервер отключен";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(445, 238);
			this.Controls.Add(this.status);
			this.Controls.Add(this.stop);
			this.Controls.Add(this.start);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button start;
		private System.Windows.Forms.Button stop;
		private System.Windows.Forms.Label status;
	}
}

