namespace MapEdit
{
	partial class View
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(View));
			this.map = new System.Windows.Forms.PictureBox();
			this.bushes = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.boxes = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.Start = new System.Windows.Forms.Button();
			this.timerPaint = new System.Windows.Forms.Timer(this.components);
			this.X = new System.Windows.Forms.Label();
			this.Y = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.save = new System.Windows.Forms.Button();
			this.load = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.map)).BeginInit();
			this.SuspendLayout();
			// 
			// map
			// 
			this.map.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.map.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.map.Cursor = System.Windows.Forms.Cursors.Hand;
			this.map.Location = new System.Drawing.Point(12, 12);
			this.map.Name = "map";
			this.map.Size = new System.Drawing.Size(600, 600);
			this.map.TabIndex = 0;
			this.map.TabStop = false;
			this.map.Click += new System.EventHandler(this.map_Click);
			this.map.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
			this.map.MouseLeave += new System.EventHandler(this.map_MouseLeave);
			this.map.MouseMove += new System.Windows.Forms.MouseEventHandler(this.map_MouseMove);
			// 
			// bushes
			// 
			this.bushes.Image = global::MapEdit.Properties.Resources.Bush;
			this.bushes.Location = new System.Drawing.Point(618, 57);
			this.bushes.Name = "bushes";
			this.bushes.Size = new System.Drawing.Size(46, 47);
			this.bushes.TabIndex = 1;
			this.bushes.UseVisualStyleBackColor = true;
			this.bushes.Click += new System.EventHandler(this.bushes_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(618, 31);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(100, 20);
			this.textBox1.TabIndex = 2;
			// 
			// boxes
			// 
			this.boxes.Image = global::MapEdit.Properties.Resources.box;
			this.boxes.Location = new System.Drawing.Point(670, 57);
			this.boxes.Name = "boxes";
			this.boxes.Size = new System.Drawing.Size(48, 47);
			this.boxes.TabIndex = 3;
			this.boxes.UseVisualStyleBackColor = true;
			this.boxes.Click += new System.EventHandler(this.boxes_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(618, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(97, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Размер карты(px)";
			// 
			// Start
			// 
			this.Start.Location = new System.Drawing.Point(621, 126);
			this.Start.Name = "Start";
			this.Start.Size = new System.Drawing.Size(94, 23);
			this.Start.TabIndex = 5;
			this.Start.Text = "Создать";
			this.Start.UseVisualStyleBackColor = true;
			this.Start.Click += new System.EventHandler(this.Start_Click);
			// 
			// timerPaint
			// 
			this.timerPaint.Interval = 40;
			this.timerPaint.Tick += new System.EventHandler(this.timerPaint_Tick);
			// 
			// X
			// 
			this.X.AutoSize = true;
			this.X.Location = new System.Drawing.Point(628, 190);
			this.X.Name = "X";
			this.X.Size = new System.Drawing.Size(35, 13);
			this.X.TabIndex = 6;
			this.X.Text = "label2";
			// 
			// Y
			// 
			this.Y.AutoSize = true;
			this.Y.Location = new System.Drawing.Point(629, 215);
			this.Y.Name = "Y";
			this.Y.Size = new System.Drawing.Size(35, 13);
			this.Y.TabIndex = 7;
			this.Y.Text = "label3";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(628, 258);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(35, 13);
			this.label2.TabIndex = 8;
			this.label2.Text = "label3";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(629, 290);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(35, 13);
			this.label3.TabIndex = 9;
			this.label3.Text = "label3";
			// 
			// save
			// 
			this.save.Location = new System.Drawing.Point(618, 332);
			this.save.Name = "save";
			this.save.Size = new System.Drawing.Size(94, 23);
			this.save.TabIndex = 10;
			this.save.Text = "Сохранить";
			this.save.UseVisualStyleBackColor = true;
			this.save.Click += new System.EventHandler(this.save_Click);
			// 
			// load
			// 
			this.load.Location = new System.Drawing.Point(618, 370);
			this.load.Name = "load";
			this.load.Size = new System.Drawing.Size(94, 23);
			this.load.TabIndex = 11;
			this.load.Text = "Загрузить";
			this.load.UseVisualStyleBackColor = true;
			this.load.Click += new System.EventHandler(this.load_Click);
			// 
			// View
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(726, 628);
			this.Controls.Add(this.load);
			this.Controls.Add(this.save);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.Y);
			this.Controls.Add(this.X);
			this.Controls.Add(this.Start);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.boxes);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.bushes);
			this.Controls.Add(this.map);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Name = "View";
			this.Text = "Редактор карт";
			((System.ComponentModel.ISupportInitialize)(this.map)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox map;
		private System.Windows.Forms.Button bushes;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button boxes;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button Start;
		private System.Windows.Forms.Timer timerPaint;
		private System.Windows.Forms.Label X;
		private System.Windows.Forms.Label Y;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button save;
		private System.Windows.Forms.Button load;
	}
}

