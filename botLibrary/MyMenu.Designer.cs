namespace botLibrary
{
	partial class MyMenu
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// MyMenu
			// 
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Name = "MyMenu";
			this.Load += new System.EventHandler(this.MyMenu_Load_1);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button bPlay;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label InfoName;
		private System.Windows.Forms.Label InfoWins;
		private System.Windows.Forms.Label InfoKills;
		private System.Windows.Forms.Label InfoIP;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label dies;
	}
}