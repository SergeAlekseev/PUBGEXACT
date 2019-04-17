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
			this.InfoName = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.PlayingField)).BeginInit();
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
			this.PlayingField.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PlayingField_MouseDown);
			this.PlayingField.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PlayingField_MouseMove);
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
			// InfoName
			// 
			this.InfoName.AutoSize = true;
			this.InfoName.Location = new System.Drawing.Point(12, 9);
			this.InfoName.Name = "InfoName";
			this.InfoName.Size = new System.Drawing.Size(35, 13);
			this.InfoName.TabIndex = 2;
			this.InfoName.Text = "label1";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(252, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "label1";
			// 
			// Client
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(624, 639);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.InfoName);
			this.Controls.Add(this.PlayingField);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Client";
			this.Text = "Client";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Client_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Client_FormClosed);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Client_Paint);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
			((System.ComponentModel.ISupportInitialize)(this.PlayingField)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox PlayingField;
		private System.Windows.Forms.Timer timerPaint;
		private System.Windows.Forms.Timer timerMouseLocation;
		private System.Windows.Forms.Label InfoName;
		private System.Windows.Forms.Label label1;
	}
}

