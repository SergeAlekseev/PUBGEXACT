﻿namespace client
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
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.InfoName = new System.Windows.Forms.Label();
			this.InfoKills = new System.Windows.Forms.Label();
			this.InfoWins = new System.Windows.Forms.Label();
			this.InfoIP = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.button1.Location = new System.Drawing.Point(70, 136);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(173, 39);
			this.button1.TabIndex = 0;
			this.button1.Text = "Играть";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.button2.Location = new System.Drawing.Point(88, 223);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(135, 36);
			this.button2.TabIndex = 1;
			this.button2.Text = "Поддержка";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// button3
			// 
			this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.button3.Location = new System.Drawing.Point(88, 181);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(135, 36);
			this.button3.TabIndex = 2;
			this.button3.Text = "Настройки";
			this.button3.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.InfoIP);
			this.panel1.Controls.Add(this.InfoWins);
			this.panel1.Controls.Add(this.InfoKills);
			this.panel1.Controls.Add(this.InfoName);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(322, 130);
			this.panel1.TabIndex = 3;
			// 
			// InfoName
			// 
			this.InfoName.AutoSize = true;
			this.InfoName.Location = new System.Drawing.Point(95, 53);
			this.InfoName.Name = "InfoName";
			this.InfoName.Size = new System.Drawing.Size(174, 13);
			this.InfoName.TabIndex = 0;
			this.InfoName.Text = "Панель с информацией о игроке";
			// 
			// InfoKills
			// 
			this.InfoKills.AutoSize = true;
			this.InfoKills.Location = new System.Drawing.Point(95, 75);
			this.InfoKills.Name = "InfoKills";
			this.InfoKills.Size = new System.Drawing.Size(174, 13);
			this.InfoKills.TabIndex = 1;
			this.InfoKills.Text = "Панель с информацией о игроке";
			// 
			// InfoWins
			// 
			this.InfoWins.AutoSize = true;
			this.InfoWins.Location = new System.Drawing.Point(95, 100);
			this.InfoWins.Name = "InfoWins";
			this.InfoWins.Size = new System.Drawing.Size(174, 13);
			this.InfoWins.TabIndex = 2;
			this.InfoWins.Text = "Панель с информацией о игроке";
			// 
			// InfoIP
			// 
			this.InfoIP.AutoSize = true;
			this.InfoIP.Location = new System.Drawing.Point(101, 7);
			this.InfoIP.Name = "InfoIP";
			this.InfoIP.Size = new System.Drawing.Size(17, 13);
			this.InfoIP.TabIndex = 3;
			this.InfoIP.Text = "IP";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 53);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Никнейм:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 75);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(83, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Всего убийств:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 100);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(73, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Всего побед:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 7);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(92, 13);
			this.label4.TabIndex = 7;
			this.label4.Text = "Адресс сервера:";
			// 
			// MyMenu
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(322, 271);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Name = "MyMenu";
			this.Text = "Menu";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MyMenu_FormClosed);
			this.Load += new System.EventHandler(this.MyMenu_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button button1;
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
	}
}