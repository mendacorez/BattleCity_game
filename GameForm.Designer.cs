namespace BattleCity
{
    partial class GameForm
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
            this.infoPanel = new System.Windows.Forms.Panel();
            this.closePicture = new System.Windows.Forms.PictureBox();
            this.globalTimer = new System.Windows.Forms.Timer(this.components);
            this.botTimer = new System.Windows.Forms.Timer(this.components);
            this.hpInfo = new System.Windows.Forms.Label();
            this.enemiesInfo = new System.Windows.Forms.Label();
            this.hpCounter = new System.Windows.Forms.Label();
            this.enemiesCounter = new System.Windows.Forms.Label();
            this.infoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.closePicture)).BeginInit();
            this.SuspendLayout();
            // 
            // infoPanel
            // 
            this.infoPanel.BackColor = System.Drawing.Color.Gray;
            this.infoPanel.Controls.Add(this.enemiesCounter);
            this.infoPanel.Controls.Add(this.hpCounter);
            this.infoPanel.Controls.Add(this.enemiesInfo);
            this.infoPanel.Controls.Add(this.hpInfo);
            this.infoPanel.Controls.Add(this.closePicture);
            this.infoPanel.Location = new System.Drawing.Point(500, 0);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Size = new System.Drawing.Size(100, 500);
            this.infoPanel.TabIndex = 0;
            // 
            // closePicture
            // 
            this.closePicture.BackgroundImage = global::BattleCity.Properties.Resources.closeWindow;
            this.closePicture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.closePicture.Location = new System.Drawing.Point(19, 12);
            this.closePicture.Name = "closePicture";
            this.closePicture.Size = new System.Drawing.Size(60, 60);
            this.closePicture.TabIndex = 0;
            this.closePicture.TabStop = false;
            this.closePicture.Click += new System.EventHandler(this.closePicture_Click);
            // 
            // hpInfo
            // 
            this.hpInfo.AutoSize = true;
            this.hpInfo.Font = new System.Drawing.Font("Impact", 18F);
            this.hpInfo.Location = new System.Drawing.Point(3, 98);
            this.hpInfo.Name = "hpInfo";
            this.hpInfo.Size = new System.Drawing.Size(43, 29);
            this.hpInfo.TabIndex = 1;
            this.hpInfo.Text = "HP:";
            // 
            // enemiesInfo
            // 
            this.enemiesInfo.AutoSize = true;
            this.enemiesInfo.Font = new System.Drawing.Font("Impact", 17.5F);
            this.enemiesInfo.Location = new System.Drawing.Point(3, 144);
            this.enemiesInfo.Name = "enemiesInfo";
            this.enemiesInfo.Size = new System.Drawing.Size(101, 29);
            this.enemiesInfo.TabIndex = 2;
            this.enemiesInfo.Text = "Enemies:";
            // 
            // hpCounter
            // 
            this.hpCounter.AutoSize = true;
            this.hpCounter.Font = new System.Drawing.Font("Impact", 18F);
            this.hpCounter.Location = new System.Drawing.Point(45, 98);
            this.hpCounter.Name = "hpCounter";
            this.hpCounter.Size = new System.Drawing.Size(26, 29);
            this.hpCounter.TabIndex = 3;
            this.hpCounter.Text = "0";
            // 
            // enemiesCounter
            // 
            this.enemiesCounter.AutoSize = true;
            this.enemiesCounter.Font = new System.Drawing.Font("Impact", 18F);
            this.enemiesCounter.Location = new System.Drawing.Point(36, 173);
            this.enemiesCounter.Name = "enemiesCounter";
            this.enemiesCounter.Size = new System.Drawing.Size(26, 29);
            this.enemiesCounter.TabIndex = 4;
            this.enemiesCounter.Text = "0";
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(600, 500);
            this.Controls.Add(this.infoPanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Menu";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            this.infoPanel.ResumeLayout(false);
            this.infoPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.closePicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel infoPanel;
        private System.Windows.Forms.PictureBox closePicture;
        private System.Windows.Forms.Timer globalTimer;
        private System.Windows.Forms.Timer botTimer;
        private System.Windows.Forms.Label enemiesInfo;
        private System.Windows.Forms.Label hpInfo;
        private System.Windows.Forms.Label hpCounter;
        private System.Windows.Forms.Label enemiesCounter;
    }
}

