namespace server
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Client));
			this.PlayingField = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.PlayingField)).BeginInit();
			this.SuspendLayout();
			// 
			// PlayingField
			// 
			this.PlayingField.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.PlayingField.Location = new System.Drawing.Point(12, 12);
			this.PlayingField.Name = "PlayingField";
			this.PlayingField.Size = new System.Drawing.Size(600, 600);
			this.PlayingField.TabIndex = 0;
			this.PlayingField.TabStop = false;
			this.PlayingField.Click += new System.EventHandler(this.PlayingField_Click);
			// 
			// Client
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(624, 629);
			this.Controls.Add(this.PlayingField);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Client";
			this.Text = "Client";
			((System.ComponentModel.ISupportInitialize)(this.PlayingField)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox PlayingField;
	}
}

