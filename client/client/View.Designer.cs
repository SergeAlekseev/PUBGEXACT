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
			((System.ComponentModel.ISupportInitialize)(this.PlayingField)).BeginInit();
			this.SuspendLayout();
			// 
			// PlayingField
			// 
			this.PlayingField.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.PlayingField.Location = new System.Drawing.Point(12, 10);
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
			// Client
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(624, 622);
			this.Controls.Add(this.PlayingField);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Client";
			this.Text = "Client";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Client_FormClosing);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Client_Paint);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
			((System.ComponentModel.ISupportInitialize)(this.PlayingField)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox PlayingField;
		private System.Windows.Forms.Timer timerPaint;
		private System.Windows.Forms.Timer timerMouseLocation;
	}
}

