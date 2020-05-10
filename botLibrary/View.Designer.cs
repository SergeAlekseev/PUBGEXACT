namespace botLibrary
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
			this.SuspendLayout();
			// 
			// Client
			// 
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Name = "Client";
			this.Load += new System.EventHandler(this.Client_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox PlayingField;
		private System.Windows.Forms.Timer timerPaint;
		private System.Windows.Forms.Timer timerMouseLocation;
		private System.Windows.Forms.Label InfoName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
	}
}

