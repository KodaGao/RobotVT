namespace RobotVT.Controller
{
    partial class HIK_FaceSnap
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
            this.panelEx = new DevComponents.DotNetBar.PanelEx();
            this.pictureB1 = new AForge.Controls.PictureBox();
            this.compareText1 = new DevComponents.DotNetBar.LabelX();
            this.CompareTextPanel1 = new DevComponents.DotNetBar.PanelEx();
            this.pictureA1 = new AForge.Controls.PictureBox();
            this.panelEx.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureB1)).BeginInit();
            this.CompareTextPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureA1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEx
            // 
            this.panelEx.CanvasColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panelEx.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx.Controls.Add(this.CompareTextPanel1);
            this.panelEx.Controls.Add(this.pictureA1);
            this.panelEx.Controls.Add(this.pictureB1);
            this.panelEx.DisabledBackColor = System.Drawing.Color.Empty;
            this.panelEx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx.Location = new System.Drawing.Point(0, 0);
            this.panelEx.Name = "panelEx";
            this.panelEx.Size = new System.Drawing.Size(611, 290);
            this.panelEx.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(50)))), ((int)(((byte)(115)))));
            this.panelEx.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx.Style.GradientAngle = 90;
            this.panelEx.TabIndex = 0;
            this.panelEx.Text = "panelEx";
            // 
            // pictureB1
            // 
            this.pictureB1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pictureB1.ForeColor = System.Drawing.Color.Black;
            this.pictureB1.Image = null;
            this.pictureB1.Location = new System.Drawing.Point(314, 15);
            this.pictureB1.Name = "pictureB1";
            this.pictureB1.Size = new System.Drawing.Size(280, 261);
            this.pictureB1.TabIndex = 7;
            this.pictureB1.TabStop = false;
            // 
            // compareText1
            // 
            this.compareText1.AutoSize = true;
            this.compareText1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.compareText1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.compareText1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.compareText1.ForeColor = System.Drawing.Color.White;
            this.compareText1.Location = new System.Drawing.Point(18, 4);
            this.compareText1.Name = "compareText1";
            this.compareText1.Size = new System.Drawing.Size(110, 32);
            this.compareText1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.compareText1.TabIndex = 0;
            this.compareText1.Text = "相似度60%";
            // 
            // CompareTextPanel1
            // 
            this.CompareTextPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CompareTextPanel1.Controls.Add(this.compareText1);
            this.CompareTextPanel1.DisabledBackColor = System.Drawing.Color.Empty;
            this.CompareTextPanel1.Location = new System.Drawing.Point(226, 223);
            this.CompareTextPanel1.Name = "CompareTextPanel1";
            this.CompareTextPanel1.Size = new System.Drawing.Size(147, 40);
            this.CompareTextPanel1.TabIndex = 8;
            // 
            // pictureA1
            // 
            this.pictureA1.BackColor = System.Drawing.Color.Transparent;
            this.pictureA1.ForeColor = System.Drawing.Color.Black;
            this.pictureA1.Image = null;
            this.pictureA1.Location = new System.Drawing.Point(16, 15);
            this.pictureA1.Name = "pictureA1";
            this.pictureA1.Size = new System.Drawing.Size(280, 261);
            this.pictureA1.TabIndex = 6;
            this.pictureA1.TabStop = false;
            // 
            // HIK_FaceSnap
            // 
            this.Controls.Add(this.panelEx);
            this.Name = "HIK_FaceSnap";
            this.Size = new System.Drawing.Size(611, 290);
            this.panelEx.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureB1)).EndInit();
            this.CompareTextPanel1.ResumeLayout(false);
            this.CompareTextPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureA1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx;
        private DevComponents.DotNetBar.PanelEx CompareTextPanel1;
        private DevComponents.DotNetBar.LabelX compareText1;
        private AForge.Controls.PictureBox pictureA1;
        private AForge.Controls.PictureBox pictureB1;
    }
}
