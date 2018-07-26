namespace RobotVT
{
    partial class VisualTracking
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisualTracking));
            this.centerMain = new System.Windows.Forms.Panel();
            this.mainWindow = new System.Windows.Forms.Panel();
            this.backCamera = new System.Windows.Forms.Panel();
            this.backPictureBox = new System.Windows.Forms.PictureBox();
            this.leftCamera = new System.Windows.Forms.Panel();
            this.rightPictureBox = new System.Windows.Forms.PictureBox();
            this.rightCamera = new System.Windows.Forms.Panel();
            this.leftPictureBox = new System.Windows.Forms.PictureBox();
            this.frontCamera = new System.Windows.Forms.Panel();
            this.frontPictureBox = new System.Windows.Forms.PictureBox();
            this.cloudCamera = new System.Windows.Forms.Panel();
            this.cloudPictureBox = new System.Windows.Forms.PictureBox();
            this.mainCamera = new System.Windows.Forms.Panel();
            this.topMain = new System.Windows.Forms.Panel();
            this.speed = new System.Windows.Forms.Label();
            this.time = new System.Windows.Forms.Label();
            this.lamp = new System.Windows.Forms.Panel();
            this.robotPower = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.power2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.power1 = new System.Windows.Forms.Panel();
            this.power1_number = new System.Windows.Forms.Label();
            this.signal = new System.Windows.Forms.Panel();
            this.RealPlayWnd = new System.Windows.Forms.PictureBox();
            this.centerMain.SuspendLayout();
            this.mainWindow.SuspendLayout();
            this.backCamera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.backPictureBox)).BeginInit();
            this.leftCamera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rightPictureBox)).BeginInit();
            this.rightCamera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.leftPictureBox)).BeginInit();
            this.frontCamera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frontPictureBox)).BeginInit();
            this.cloudCamera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cloudPictureBox)).BeginInit();
            this.mainCamera.SuspendLayout();
            this.topMain.SuspendLayout();
            this.robotPower.SuspendLayout();
            this.power2.SuspendLayout();
            this.power1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RealPlayWnd)).BeginInit();
            this.SuspendLayout();
            // 
            // centerMain
            // 
            this.centerMain.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.centerMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            this.centerMain.Controls.Add(this.mainWindow);
            this.centerMain.ForeColor = System.Drawing.Color.Black;
            this.centerMain.Location = new System.Drawing.Point(2, 57);
            this.centerMain.Name = "centerMain";
            this.centerMain.Size = new System.Drawing.Size(1140, 807);
            this.centerMain.TabIndex = 5;
            // 
            // mainWindow
            // 
            this.mainWindow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            this.mainWindow.Controls.Add(this.backCamera);
            this.mainWindow.Controls.Add(this.leftCamera);
            this.mainWindow.Controls.Add(this.rightCamera);
            this.mainWindow.Controls.Add(this.frontCamera);
            this.mainWindow.Controls.Add(this.cloudCamera);
            this.mainWindow.Controls.Add(this.mainCamera);
            this.mainWindow.ForeColor = System.Drawing.Color.Black;
            this.mainWindow.Location = new System.Drawing.Point(12, 3);
            this.mainWindow.Name = "mainWindow";
            this.mainWindow.Size = new System.Drawing.Size(1116, 789);
            this.mainWindow.TabIndex = 1;
            // 
            // backCamera
            // 
            this.backCamera.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            this.backCamera.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("backCamera.BackgroundImage")));
            this.backCamera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.backCamera.Controls.Add(this.backPictureBox);
            this.backCamera.ForeColor = System.Drawing.Color.Black;
            this.backCamera.Location = new System.Drawing.Point(756, 532);
            this.backCamera.Name = "backCamera";
            this.backCamera.Size = new System.Drawing.Size(360, 254);
            this.backCamera.TabIndex = 3;
            // 
            // backPictureBox
            // 
            this.backPictureBox.BackColor = System.Drawing.Color.White;
            this.backPictureBox.ForeColor = System.Drawing.Color.Black;
            this.backPictureBox.Location = new System.Drawing.Point(19, 30);
            this.backPictureBox.Name = "backPictureBox";
            this.backPictureBox.Size = new System.Drawing.Size(323, 208);
            this.backPictureBox.TabIndex = 2;
            this.backPictureBox.TabStop = false;
            // 
            // leftCamera
            // 
            this.leftCamera.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            this.leftCamera.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("leftCamera.BackgroundImage")));
            this.leftCamera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.leftCamera.Controls.Add(this.rightPictureBox);
            this.leftCamera.ForeColor = System.Drawing.Color.Black;
            this.leftCamera.Location = new System.Drawing.Point(379, 532);
            this.leftCamera.Name = "leftCamera";
            this.leftCamera.Size = new System.Drawing.Size(360, 254);
            this.leftCamera.TabIndex = 4;
            // 
            // rightPictureBox
            // 
            this.rightPictureBox.BackColor = System.Drawing.Color.White;
            this.rightPictureBox.ForeColor = System.Drawing.Color.Black;
            this.rightPictureBox.Location = new System.Drawing.Point(20, 30);
            this.rightPictureBox.Name = "rightPictureBox";
            this.rightPictureBox.Size = new System.Drawing.Size(323, 208);
            this.rightPictureBox.TabIndex = 3;
            this.rightPictureBox.TabStop = false;
            // 
            // rightCamera
            // 
            this.rightCamera.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            this.rightCamera.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rightCamera.BackgroundImage")));
            this.rightCamera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.rightCamera.Controls.Add(this.leftPictureBox);
            this.rightCamera.ForeColor = System.Drawing.Color.Black;
            this.rightCamera.Location = new System.Drawing.Point(3, 532);
            this.rightCamera.Name = "rightCamera";
            this.rightCamera.Size = new System.Drawing.Size(360, 254);
            this.rightCamera.TabIndex = 3;
            // 
            // leftPictureBox
            // 
            this.leftPictureBox.BackColor = System.Drawing.Color.White;
            this.leftPictureBox.ForeColor = System.Drawing.Color.Black;
            this.leftPictureBox.Location = new System.Drawing.Point(16, 30);
            this.leftPictureBox.Name = "leftPictureBox";
            this.leftPictureBox.Size = new System.Drawing.Size(323, 208);
            this.leftPictureBox.TabIndex = 4;
            this.leftPictureBox.TabStop = false;
            // 
            // frontCamera
            // 
            this.frontCamera.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            this.frontCamera.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("frontCamera.BackgroundImage")));
            this.frontCamera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.frontCamera.Controls.Add(this.frontPictureBox);
            this.frontCamera.ForeColor = System.Drawing.Color.Black;
            this.frontCamera.Location = new System.Drawing.Point(756, 264);
            this.frontCamera.Name = "frontCamera";
            this.frontCamera.Size = new System.Drawing.Size(360, 254);
            this.frontCamera.TabIndex = 2;
            // 
            // frontPictureBox
            // 
            this.frontPictureBox.BackColor = System.Drawing.Color.White;
            this.frontPictureBox.ForeColor = System.Drawing.Color.Black;
            this.frontPictureBox.Location = new System.Drawing.Point(19, 32);
            this.frontPictureBox.Name = "frontPictureBox";
            this.frontPictureBox.Size = new System.Drawing.Size(323, 208);
            this.frontPictureBox.TabIndex = 1;
            this.frontPictureBox.TabStop = false;
            // 
            // cloudCamera
            // 
            this.cloudCamera.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            this.cloudCamera.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cloudCamera.BackgroundImage")));
            this.cloudCamera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cloudCamera.Controls.Add(this.cloudPictureBox);
            this.cloudCamera.ForeColor = System.Drawing.Color.Black;
            this.cloudCamera.Location = new System.Drawing.Point(756, 0);
            this.cloudCamera.Name = "cloudCamera";
            this.cloudCamera.Size = new System.Drawing.Size(360, 254);
            this.cloudCamera.TabIndex = 1;
            // 
            // cloudPictureBox
            // 
            this.cloudPictureBox.BackColor = System.Drawing.Color.White;
            this.cloudPictureBox.ForeColor = System.Drawing.Color.Black;
            this.cloudPictureBox.Location = new System.Drawing.Point(19, 31);
            this.cloudPictureBox.Name = "cloudPictureBox";
            this.cloudPictureBox.Size = new System.Drawing.Size(323, 208);
            this.cloudPictureBox.TabIndex = 0;
            this.cloudPictureBox.TabStop = false;
            // 
            // mainCamera
            // 
            this.mainCamera.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            this.mainCamera.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mainCamera.BackgroundImage")));
            this.mainCamera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.mainCamera.Controls.Add(this.RealPlayWnd);
            this.mainCamera.ForeColor = System.Drawing.Color.Black;
            this.mainCamera.Location = new System.Drawing.Point(3, 0);
            this.mainCamera.Name = "mainCamera";
            this.mainCamera.Size = new System.Drawing.Size(739, 518);
            this.mainCamera.TabIndex = 0;
            // 
            // topMain
            // 
            this.topMain.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.topMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            this.topMain.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("topMain.BackgroundImage")));
            this.topMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.topMain.Controls.Add(this.speed);
            this.topMain.Controls.Add(this.time);
            this.topMain.Controls.Add(this.lamp);
            this.topMain.Controls.Add(this.robotPower);
            this.topMain.Controls.Add(this.power2);
            this.topMain.Controls.Add(this.power1);
            this.topMain.Controls.Add(this.signal);
            this.topMain.ForeColor = System.Drawing.Color.Black;
            this.topMain.Location = new System.Drawing.Point(2, 1);
            this.topMain.Name = "topMain";
            this.topMain.Size = new System.Drawing.Size(1140, 53);
            this.topMain.TabIndex = 4;
            // 
            // speed
            // 
            this.speed.AutoSize = true;
            this.speed.BackColor = System.Drawing.Color.Transparent;
            this.speed.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.speed.ForeColor = System.Drawing.Color.Black;
            this.speed.Location = new System.Drawing.Point(950, 16);
            this.speed.Name = "speed";
            this.speed.Size = new System.Drawing.Size(69, 19);
            this.speed.TabIndex = 6;
            this.speed.Text = "9.8m/s";
            // 
            // time
            // 
            this.time.AutoSize = true;
            this.time.BackColor = System.Drawing.Color.Transparent;
            this.time.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.time.ForeColor = System.Drawing.Color.Black;
            this.time.Location = new System.Drawing.Point(271, 16);
            this.time.Name = "time";
            this.time.Size = new System.Drawing.Size(59, 19);
            this.time.TabIndex = 5;
            this.time.Text = "23:23";
            // 
            // lamp
            // 
            this.lamp.BackColor = System.Drawing.Color.Transparent;
            this.lamp.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("lamp.BackgroundImage")));
            this.lamp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.lamp.ForeColor = System.Drawing.Color.Black;
            this.lamp.Location = new System.Drawing.Point(1104, 9);
            this.lamp.Name = "lamp";
            this.lamp.Size = new System.Drawing.Size(36, 35);
            this.lamp.TabIndex = 4;
            // 
            // robotPower
            // 
            this.robotPower.BackColor = System.Drawing.Color.Transparent;
            this.robotPower.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("robotPower.BackgroundImage")));
            this.robotPower.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.robotPower.Controls.Add(this.label2);
            this.robotPower.ForeColor = System.Drawing.Color.Black;
            this.robotPower.Location = new System.Drawing.Point(787, 9);
            this.robotPower.Name = "robotPower";
            this.robotPower.Size = new System.Drawing.Size(63, 35);
            this.robotPower.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("宋体", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(25, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 11);
            this.label2.TabIndex = 9;
            this.label2.Text = "60%";
            // 
            // power2
            // 
            this.power2.BackColor = System.Drawing.Color.Transparent;
            this.power2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("power2.BackgroundImage")));
            this.power2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.power2.Controls.Add(this.label1);
            this.power2.ForeColor = System.Drawing.Color.Black;
            this.power2.Location = new System.Drawing.Point(613, 9);
            this.power2.Name = "power2";
            this.power2.Size = new System.Drawing.Size(63, 35);
            this.power2.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("宋体", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(24, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 11);
            this.label1.TabIndex = 8;
            this.label1.Text = "40%";
            // 
            // power1
            // 
            this.power1.BackColor = System.Drawing.Color.Transparent;
            this.power1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("power1.BackgroundImage")));
            this.power1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.power1.Controls.Add(this.power1_number);
            this.power1.ForeColor = System.Drawing.Color.Black;
            this.power1.Location = new System.Drawing.Point(443, 9);
            this.power1.Name = "power1";
            this.power1.Size = new System.Drawing.Size(63, 35);
            this.power1.TabIndex = 1;
            // 
            // power1_number
            // 
            this.power1_number.AutoSize = true;
            this.power1_number.BackColor = System.Drawing.Color.White;
            this.power1_number.Font = new System.Drawing.Font("宋体", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.power1_number.ForeColor = System.Drawing.Color.Black;
            this.power1_number.Location = new System.Drawing.Point(24, 11);
            this.power1_number.Name = "power1_number";
            this.power1_number.Size = new System.Drawing.Size(23, 11);
            this.power1_number.TabIndex = 7;
            this.power1_number.Text = "60%";
            // 
            // signal
            // 
            this.signal.BackColor = System.Drawing.Color.Transparent;
            this.signal.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("signal.BackgroundImage")));
            this.signal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.signal.ForeColor = System.Drawing.Color.Black;
            this.signal.Location = new System.Drawing.Point(95, 9);
            this.signal.Name = "signal";
            this.signal.Size = new System.Drawing.Size(63, 35);
            this.signal.TabIndex = 0;
            // 
            // RealPlayWnd
            // 
            this.RealPlayWnd.Location = new System.Drawing.Point(18, 28);
            this.RealPlayWnd.Name = "RealPlayWnd";
            this.RealPlayWnd.Size = new System.Drawing.Size(703, 463);
            this.RealPlayWnd.TabIndex = 1;
            this.RealPlayWnd.TabStop = false;
            // 
            // VisualTracking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(22)))), ((int)(((byte)(50)))));
            this.ClientSize = new System.Drawing.Size(1144, 864);
            this.ControlBox = false;
            this.Controls.Add(this.centerMain);
            this.Controls.Add(this.topMain);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "VisualTracking";
            this.Load += new System.EventHandler(this.VisualTracking_Load);
            this.centerMain.ResumeLayout(false);
            this.mainWindow.ResumeLayout(false);
            this.backCamera.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.backPictureBox)).EndInit();
            this.leftCamera.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rightPictureBox)).EndInit();
            this.rightCamera.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.leftPictureBox)).EndInit();
            this.frontCamera.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.frontPictureBox)).EndInit();
            this.cloudCamera.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cloudPictureBox)).EndInit();
            this.mainCamera.ResumeLayout(false);
            this.topMain.ResumeLayout(false);
            this.topMain.PerformLayout();
            this.robotPower.ResumeLayout(false);
            this.robotPower.PerformLayout();
            this.power2.ResumeLayout(false);
            this.power2.PerformLayout();
            this.power1.ResumeLayout(false);
            this.power1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RealPlayWnd)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel centerMain;
        private System.Windows.Forms.Panel mainWindow;
        private System.Windows.Forms.Panel backCamera;
        private System.Windows.Forms.PictureBox backPictureBox;
        private System.Windows.Forms.Panel leftCamera;
        private System.Windows.Forms.PictureBox rightPictureBox;
        private System.Windows.Forms.Panel rightCamera;
        private System.Windows.Forms.PictureBox leftPictureBox;
        private System.Windows.Forms.Panel frontCamera;
        private System.Windows.Forms.PictureBox frontPictureBox;
        private System.Windows.Forms.Panel cloudCamera;
        private System.Windows.Forms.PictureBox cloudPictureBox;
        private System.Windows.Forms.Panel mainCamera;
        private System.Windows.Forms.Panel topMain;
        private System.Windows.Forms.Label speed;
        private System.Windows.Forms.Label time;
        private System.Windows.Forms.Panel lamp;
        private System.Windows.Forms.Panel robotPower;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel power2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel power1;
        private System.Windows.Forms.Label power1_number;
        private System.Windows.Forms.Panel signal;
        private System.Windows.Forms.PictureBox RealPlayWnd;
    }
}

