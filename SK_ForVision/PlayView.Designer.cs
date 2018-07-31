namespace SK_FVision
{
    partial class PlayView
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
            this.RealPlayWnd = new AForge.Controls.PictureBox();
            this.Exception_LabeX = new DevComponents.DotNetBar.LabelX();
            ((System.ComponentModel.ISupportInitialize)(this.RealPlayWnd)).BeginInit();
            this.SuspendLayout();
            // 
            // RealPlayWnd
            // 
            this.RealPlayWnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RealPlayWnd.Image = null;
            this.RealPlayWnd.Location = new System.Drawing.Point(0, 0);
            this.RealPlayWnd.Name = "RealPlayWnd";
            this.RealPlayWnd.Size = new System.Drawing.Size(410, 208);
            this.RealPlayWnd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.RealPlayWnd.TabIndex = 0;
            this.RealPlayWnd.TabStop = false;
            // 
            // Exception_LabeX
            // 
            // 
            // 
            // 
            this.Exception_LabeX.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.Exception_LabeX.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Exception_LabeX.Location = new System.Drawing.Point(0, 325);
            this.Exception_LabeX.Name = "Exception_LabeX";
            this.Exception_LabeX.Size = new System.Drawing.Size(394, 23);
            this.Exception_LabeX.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeMobile2014;
            this.Exception_LabeX.TabIndex = 1;
            // 
            // PlayView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Exception_LabeX);
            this.Controls.Add(this.RealPlayWnd);
            this.Name = "PlayView";
            this.Size = new System.Drawing.Size(410, 208);
            this.Load += new System.EventHandler(this.PlayView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.RealPlayWnd)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AForge.Controls.PictureBox RealPlayWnd;
        private DevComponents.DotNetBar.LabelX Exception_LabeX;
    }
}
