namespace RobotVT.Controller
{
    partial class ZX_MatchInfo
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
            this.CompareBox = new DevComponents.DotNetBar.PanelEx();
            this.lbXDevInfo = new DevComponents.DotNetBar.LabelX();
            this.lbXAbstime = new DevComponents.DotNetBar.LabelX();
            this.CompareTextPanel = new DevComponents.DotNetBar.PanelEx();
            this.lbXCompare = new DevComponents.DotNetBar.LabelX();
            this.pictureB = new AForge.Controls.PictureBox();
            this.pictureA = new AForge.Controls.PictureBox();
            this.CompareBox.SuspendLayout();
            this.CompareTextPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureA)).BeginInit();
            this.SuspendLayout();
            // 
            // CompareBox
            // 
            this.CompareBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CompareBox.CanvasColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.CompareBox.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.CompareBox.Controls.Add(this.lbXDevInfo);
            this.CompareBox.Controls.Add(this.lbXAbstime);
            this.CompareBox.Controls.Add(this.CompareTextPanel);
            this.CompareBox.Controls.Add(this.pictureB);
            this.CompareBox.Controls.Add(this.pictureA);
            this.CompareBox.DisabledBackColor = System.Drawing.Color.Empty;
            this.CompareBox.Location = new System.Drawing.Point(0, 0);
            this.CompareBox.Name = "CompareBox";
            this.CompareBox.Size = new System.Drawing.Size(611, 290);
            this.CompareBox.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(50)))), ((int)(((byte)(115)))));
            this.CompareBox.TabIndex = 4;
            // 
            // lbXDevInfo
            // 
            this.lbXDevInfo.AutoSize = true;
            this.lbXDevInfo.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbXDevInfo.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbXDevInfo.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbXDevInfo.ForeColor = System.Drawing.Color.White;
            this.lbXDevInfo.Location = new System.Drawing.Point(420, 264);
            this.lbXDevInfo.Name = "lbXDevInfo";
            this.lbXDevInfo.Size = new System.Drawing.Size(100, 23);
            this.lbXDevInfo.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.lbXDevInfo.TabIndex = 7;
            // 
            // lbXAbstime
            // 
            this.lbXAbstime.AutoSize = true;
            this.lbXAbstime.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbXAbstime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbXAbstime.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbXAbstime.ForeColor = System.Drawing.Color.White;
            this.lbXAbstime.Location = new System.Drawing.Point(65, 264);
            this.lbXAbstime.Name = "lbXAbstime";
            this.lbXAbstime.Size = new System.Drawing.Size(157, 23);
            this.lbXAbstime.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.lbXAbstime.TabIndex = 6;
            // 
            // CompareTextPanel
            // 
            this.CompareTextPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CompareTextPanel.Controls.Add(this.lbXCompare);
            this.CompareTextPanel.DisabledBackColor = System.Drawing.Color.Empty;
            this.CompareTextPanel.Location = new System.Drawing.Point(228, 223);
            this.CompareTextPanel.Name = "CompareTextPanel";
            this.CompareTextPanel.Size = new System.Drawing.Size(147, 40);
            this.CompareTextPanel.TabIndex = 2;
            // 
            // lbXCompare
            // 
            this.lbXCompare.AutoSize = true;
            this.lbXCompare.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbXCompare.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbXCompare.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbXCompare.ForeColor = System.Drawing.Color.White;
            this.lbXCompare.Location = new System.Drawing.Point(26, 6);
            this.lbXCompare.Name = "lbXCompare";
            this.lbXCompare.Size = new System.Drawing.Size(110, 32);
            this.lbXCompare.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.lbXCompare.TabIndex = 0;
            // 
            // pictureB
            // 
            this.pictureB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.pictureB.ForeColor = System.Drawing.Color.White;
            this.pictureB.Image = null;
            this.pictureB.Location = new System.Drawing.Point(316, 15);
            this.pictureB.Name = "pictureB";
            this.pictureB.Size = new System.Drawing.Size(283, 248);
            this.pictureB.TabIndex = 1;
            this.pictureB.TabStop = false;
            // 
            // pictureA
            // 
            this.pictureA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureA.BackColor = System.Drawing.Color.Transparent;
            this.pictureA.ForeColor = System.Drawing.Color.White;
            this.pictureA.Image = null;
            this.pictureA.Location = new System.Drawing.Point(18, 15);
            this.pictureA.Name = "pictureA";
            this.pictureA.Size = new System.Drawing.Size(273, 248);
            this.pictureA.TabIndex = 0;
            this.pictureA.TabStop = false;
            // 
            // ZX_MatchInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(22)))), ((int)(((byte)(50)))));
            this.Controls.Add(this.CompareBox);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ZX_MatchInfo";
            this.Size = new System.Drawing.Size(611, 290);
            this.Load += new System.EventHandler(this.ZX_MatchInfo_Load);
            this.CompareBox.ResumeLayout(false);
            this.CompareBox.PerformLayout();
            this.CompareTextPanel.ResumeLayout(false);
            this.CompareTextPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureA)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx CompareBox;
        private DevComponents.DotNetBar.PanelEx CompareTextPanel;
        private DevComponents.DotNetBar.LabelX lbXCompare;
        private AForge.Controls.PictureBox pictureB;
        private AForge.Controls.PictureBox pictureA;
        private DevComponents.DotNetBar.LabelX lbXAbstime;
        private DevComponents.DotNetBar.LabelX lbXDevInfo;
    }
}
