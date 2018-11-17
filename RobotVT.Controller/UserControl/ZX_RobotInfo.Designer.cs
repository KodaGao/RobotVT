namespace RobotVT.Controller
{
    partial class ZX_RobotInfo
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.topMain = new DevComponents.DotNetBar.PanelEx();
            this.speed = new DevComponents.DotNetBar.LabelX();
            this.time = new DevComponents.DotNetBar.LabelX();
            this.lamp = new DevComponents.DotNetBar.PanelEx();
            this.robotPower = new DevComponents.DotNetBar.PanelEx();
            this.robotPower_number = new DevComponents.DotNetBar.LabelX();
            this.power2 = new DevComponents.DotNetBar.PanelEx();
            this.power2_number = new DevComponents.DotNetBar.LabelX();
            this.power1 = new DevComponents.DotNetBar.PanelEx();
            this.power1_number = new DevComponents.DotNetBar.LabelX();
            this.signal = new DevComponents.DotNetBar.PanelEx();
            this.topMain.SuspendLayout();
            this.robotPower.SuspendLayout();
            this.power2.SuspendLayout();
            this.power1.SuspendLayout();
            this.SuspendLayout();
            // 
            // topMain
            // 
            this.topMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.topMain.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.topMain.Controls.Add(this.speed);
            this.topMain.Controls.Add(this.time);
            this.topMain.Controls.Add(this.lamp);
            this.topMain.Controls.Add(this.robotPower);
            this.topMain.Controls.Add(this.power2);
            this.topMain.Controls.Add(this.power1);
            this.topMain.Controls.Add(this.signal);
            this.topMain.DisabledBackColor = System.Drawing.Color.Empty;
            this.topMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topMain.Location = new System.Drawing.Point(0, 0);
            this.topMain.Name = "topMain";
            this.topMain.Size = new System.Drawing.Size(1912, 72);
            this.topMain.TabIndex = 47;
            // 
            // speed
            // 
            this.speed.AutoSize = true;
            this.speed.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.speed.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.speed.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.speed.ForeColor = System.Drawing.Color.White;
            this.speed.Location = new System.Drawing.Point(1597, 26);
            this.speed.Name = "speed";
            this.speed.Size = new System.Drawing.Size(76, 28);
            this.speed.TabIndex = 6;
            this.speed.Text = "9.8m/s";
            // 
            // time
            // 
            this.time.AutoSize = true;
            this.time.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.time.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.time.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.time.ForeColor = System.Drawing.Color.White;
            this.time.Location = new System.Drawing.Point(470, 25);
            this.time.Name = "time";
            this.time.Size = new System.Drawing.Size(65, 28);
            this.time.TabIndex = 5;
            this.time.Text = "23:23";
            // 
            // lamp
            // 
            this.lamp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.lamp.DisabledBackColor = System.Drawing.Color.Empty;
            this.lamp.Location = new System.Drawing.Point(1850, 20);
            this.lamp.Name = "lamp";
            this.lamp.Size = new System.Drawing.Size(50, 34);
            this.lamp.TabIndex = 4;
            // 
            // robotPower
            // 
            this.robotPower.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.robotPower.Controls.Add(this.robotPower_number);
            this.robotPower.DisabledBackColor = System.Drawing.Color.Empty;
            this.robotPower.Location = new System.Drawing.Point(1317, 8);
            this.robotPower.Name = "robotPower";
            this.robotPower.Size = new System.Drawing.Size(92, 57);
            this.robotPower.TabIndex = 3;
            // 
            // robotPower_number
            // 
            this.robotPower_number.AutoSize = true;
            this.robotPower_number.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            // 
            // 
            // 
            this.robotPower_number.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.robotPower_number.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.robotPower_number.ForeColor = System.Drawing.Color.White;
            this.robotPower_number.Location = new System.Drawing.Point(30, 17);
            this.robotPower_number.Name = "robotPower_number";
            this.robotPower_number.Size = new System.Drawing.Size(38, 25);
            this.robotPower_number.TabIndex = 9;
            this.robotPower_number.Text = "60%";
            // 
            // power2
            // 
            this.power2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.power2.Controls.Add(this.power2_number);
            this.power2.DisabledBackColor = System.Drawing.Color.Empty;
            this.power2.Location = new System.Drawing.Point(1029, 8);
            this.power2.Name = "power2";
            this.power2.Size = new System.Drawing.Size(92, 57);
            this.power2.TabIndex = 2;
            // 
            // power2_number
            // 
            this.power2_number.AutoSize = true;
            this.power2_number.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            // 
            // 
            // 
            this.power2_number.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.power2_number.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.power2_number.ForeColor = System.Drawing.Color.White;
            this.power2_number.Location = new System.Drawing.Point(30, 18);
            this.power2_number.Name = "power2_number";
            this.power2_number.Size = new System.Drawing.Size(38, 25);
            this.power2_number.TabIndex = 8;
            this.power2_number.Text = "40%";
            // 
            // power1
            // 
            this.power1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.power1.Controls.Add(this.power1_number);
            this.power1.DisabledBackColor = System.Drawing.Color.Empty;
            this.power1.Location = new System.Drawing.Point(741, 8);
            this.power1.Name = "power1";
            this.power1.Size = new System.Drawing.Size(92, 57);
            this.power1.TabIndex = 1;
            // 
            // power1_number
            // 
            this.power1_number.AutoSize = true;
            this.power1_number.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            // 
            // 
            // 
            this.power1_number.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.power1_number.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.power1_number.ForeColor = System.Drawing.Color.White;
            this.power1_number.Location = new System.Drawing.Point(29, 19);
            this.power1_number.Name = "power1_number";
            this.power1_number.Size = new System.Drawing.Size(38, 25);
            this.power1_number.TabIndex = 7;
            this.power1_number.Text = "60%";
            // 
            // signal
            // 
            this.signal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.signal.DisabledBackColor = System.Drawing.Color.Empty;
            this.signal.Location = new System.Drawing.Point(166, 8);
            this.signal.Name = "signal";
            this.signal.Size = new System.Drawing.Size(92, 57);
            this.signal.TabIndex = 0;
            // 
            // ZX_RobotInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(22)))), ((int)(((byte)(50)))));
            this.Controls.Add(this.topMain);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ZX_RobotInfo";
            this.Size = new System.Drawing.Size(1912, 72);
            this.Load += new System.EventHandler(this.ZX_RobotInfo_Load);
            this.topMain.ResumeLayout(false);
            this.topMain.PerformLayout();
            this.robotPower.ResumeLayout(false);
            this.robotPower.PerformLayout();
            this.power2.ResumeLayout(false);
            this.power2.PerformLayout();
            this.power1.ResumeLayout(false);
            this.power1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx topMain;
        private DevComponents.DotNetBar.LabelX speed;
        private DevComponents.DotNetBar.LabelX time;
        private DevComponents.DotNetBar.PanelEx lamp;
        private DevComponents.DotNetBar.PanelEx robotPower;
        private DevComponents.DotNetBar.LabelX robotPower_number;
        private DevComponents.DotNetBar.PanelEx power2;
        private DevComponents.DotNetBar.LabelX power2_number;
        private DevComponents.DotNetBar.PanelEx power1;
        private DevComponents.DotNetBar.LabelX power1_number;
        private DevComponents.DotNetBar.PanelEx signal;
    }
}
