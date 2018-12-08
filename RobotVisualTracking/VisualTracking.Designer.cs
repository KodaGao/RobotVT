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
            this.components = new System.ComponentModel.Container();
            this.styleManager = new DevComponents.DotNetBar.StyleManager(this.components);
            this.centerMain = new DevComponents.DotNetBar.PanelEx();
            this.mainWindow2 = new DevComponents.DotNetBar.PanelEx();
            this.label4 = new DevComponents.DotNetBar.LabelX();
            this.label3 = new DevComponents.DotNetBar.LabelX();
            this.mainWindow = new DevComponents.DotNetBar.PanelEx();
            this.hiK_CloudControl = new SK_FVision.HIK_CloudControl();
            this.backCamera = new DevComponents.DotNetBar.PanelEx();
            this.hiK_BackView = new RobotVT.Controller.HIK_PlayView_New();
            this.leftCamera = new DevComponents.DotNetBar.PanelEx();
            this.hiK_LeftView = new RobotVT.Controller.HIK_PlayView_New();
            this.rightCamera = new DevComponents.DotNetBar.PanelEx();
            this.hiK_RightView = new RobotVT.Controller.HIK_PlayView_New();
            this.frontCamera = new DevComponents.DotNetBar.PanelEx();
            this.hiK_FrontView = new RobotVT.Controller.HIK_PlayView_New();
            this.cloudCamera = new DevComponents.DotNetBar.PanelEx();
            this.hiK_CloudView = new RobotVT.Controller.HIK_PlayView_New();
            this.mainCamera = new DevComponents.DotNetBar.PanelEx();
            this.hiK_MainView = new RobotVT.Controller.HIK_PlayView_New();
            this.centerMain.SuspendLayout();
            this.mainWindow2.SuspendLayout();
            this.mainWindow.SuspendLayout();
            this.backCamera.SuspendLayout();
            this.leftCamera.SuspendLayout();
            this.rightCamera.SuspendLayout();
            this.frontCamera.SuspendLayout();
            this.cloudCamera.SuspendLayout();
            this.mainCamera.SuspendLayout();
            this.SuspendLayout();
            // 
            // styleManager
            // 
            this.styleManager.ManagerColorTint = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(22)))), ((int)(((byte)(50)))));
            this.styleManager.ManagerStyle = DevComponents.DotNetBar.eStyle.VisualStudio2012Dark;
            this.styleManager.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48))))), System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204))))));
            // 
            // centerMain
            // 
            this.centerMain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.centerMain.Controls.Add(this.mainWindow2);
            this.centerMain.Controls.Add(this.mainWindow);
            this.centerMain.DisabledBackColor = System.Drawing.Color.Empty;
            this.centerMain.Location = new System.Drawing.Point(0, 73);
            this.centerMain.Name = "centerMain";
            this.centerMain.Size = new System.Drawing.Size(1920, 973);
            this.centerMain.TabIndex = 41;
            // 
            // mainWindow2
            // 
            this.mainWindow2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.mainWindow2.CanvasColor = System.Drawing.Color.DimGray;
            this.mainWindow2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.mainWindow2.Controls.Add(this.label4);
            this.mainWindow2.Controls.Add(this.label3);
            this.mainWindow2.DisabledBackColor = System.Drawing.Color.Empty;
            this.mainWindow2.Location = new System.Drawing.Point(1927, 7);
            this.mainWindow2.Name = "mainWindow2";
            this.mainWindow2.Size = new System.Drawing.Size(1890, 951);
            this.mainWindow2.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            // 
            // 
            // 
            this.label4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.label4.Font = new System.Drawing.Font("宋体", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(840, 517);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(209, 42);
            this.label4.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.label4.TabIndex = 7;
            this.label4.Text = "待对比：443人";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            // 
            // 
            // 
            this.label3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.label3.Font = new System.Drawing.Font("宋体", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(840, 421);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(209, 42);
            this.label3.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.label3.TabIndex = 6;
            this.label3.Text = "已对比：443人";
            // 
            // mainWindow
            // 
            this.mainWindow.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.mainWindow.Controls.Add(this.hiK_CloudControl);
            this.mainWindow.Controls.Add(this.backCamera);
            this.mainWindow.Controls.Add(this.leftCamera);
            this.mainWindow.Controls.Add(this.rightCamera);
            this.mainWindow.Controls.Add(this.frontCamera);
            this.mainWindow.Controls.Add(this.cloudCamera);
            this.mainWindow.Controls.Add(this.mainCamera);
            this.mainWindow.DisabledBackColor = System.Drawing.Color.Empty;
            this.mainWindow.Location = new System.Drawing.Point(0, 7);
            this.mainWindow.Name = "mainWindow";
            this.mainWindow.Size = new System.Drawing.Size(1904, 964);
            this.mainWindow.TabIndex = 1;
            // 
            // hiK_CloudControl
            // 
            this.hiK_CloudControl.BackColor = System.Drawing.Color.Transparent;
            this.hiK_CloudControl.Location = new System.Drawing.Point(1168, 36);
            this.hiK_CloudControl.Name = "hiK_CloudControl";
            this.hiK_CloudControl.Size = new System.Drawing.Size(128, 128);
            this.hiK_CloudControl.TabIndex = 22;
            // 
            // backCamera
            // 
            this.backCamera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.backCamera.Controls.Add(this.hiK_BackView);
            this.backCamera.DisabledBackColor = System.Drawing.Color.Empty;
            this.backCamera.Location = new System.Drawing.Point(1280, 630);
            this.backCamera.Name = "backCamera";
            this.backCamera.Size = new System.Drawing.Size(530, 294);
            this.backCamera.TabIndex = 3;
            // 
            // hiK_BackView
            // 
            this.hiK_BackView.BackColor = System.Drawing.Color.Transparent;
            this.hiK_BackView.CanPlay = false;
            this.hiK_BackView.ForeColor = System.Drawing.Color.White;
            this.hiK_BackView.ImagePlaySpan = 25;
            this.hiK_BackView.Location = new System.Drawing.Point(20, 30);
            this.hiK_BackView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.hiK_BackView.Name = "hiK_BackView";
            this.hiK_BackView.PlayModel = null;
            this.hiK_BackView.ShowTarget = false;
            this.hiK_BackView.Size = new System.Drawing.Size(489, 246);
            this.hiK_BackView.TabIndex = 1;
            // 
            // leftCamera
            // 
            this.leftCamera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.leftCamera.Controls.Add(this.hiK_LeftView);
            this.leftCamera.DisabledBackColor = System.Drawing.Color.Empty;
            this.leftCamera.Location = new System.Drawing.Point(663, 630);
            this.leftCamera.Name = "leftCamera";
            this.leftCamera.Size = new System.Drawing.Size(530, 294);
            this.leftCamera.TabIndex = 4;
            // 
            // hiK_LeftView
            // 
            this.hiK_LeftView.BackColor = System.Drawing.Color.Transparent;
            this.hiK_LeftView.CanPlay = false;
            this.hiK_LeftView.ForeColor = System.Drawing.Color.White;
            this.hiK_LeftView.ImagePlaySpan = 25;
            this.hiK_LeftView.Location = new System.Drawing.Point(20, 30);
            this.hiK_LeftView.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.hiK_LeftView.Name = "hiK_LeftView";
            this.hiK_LeftView.PlayModel = null;
            this.hiK_LeftView.ShowTarget = false;
            this.hiK_LeftView.Size = new System.Drawing.Size(489, 246);
            this.hiK_LeftView.TabIndex = 2;
            // 
            // rightCamera
            // 
            this.rightCamera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.rightCamera.Controls.Add(this.hiK_RightView);
            this.rightCamera.DisabledBackColor = System.Drawing.Color.Empty;
            this.rightCamera.Location = new System.Drawing.Point(97, 630);
            this.rightCamera.Name = "rightCamera";
            this.rightCamera.Size = new System.Drawing.Size(530, 294);
            this.rightCamera.TabIndex = 3;
            // 
            // hiK_RightView
            // 
            this.hiK_RightView.BackColor = System.Drawing.Color.Transparent;
            this.hiK_RightView.CanPlay = false;
            this.hiK_RightView.ForeColor = System.Drawing.Color.White;
            this.hiK_RightView.ImagePlaySpan = 25;
            this.hiK_RightView.Location = new System.Drawing.Point(20, 30);
            this.hiK_RightView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.hiK_RightView.Name = "hiK_RightView";
            this.hiK_RightView.PlayModel = null;
            this.hiK_RightView.ShowTarget = false;
            this.hiK_RightView.Size = new System.Drawing.Size(489, 246);
            this.hiK_RightView.TabIndex = 3;
            // 
            // frontCamera
            // 
            this.frontCamera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.frontCamera.Controls.Add(this.hiK_FrontView);
            this.frontCamera.DisabledBackColor = System.Drawing.Color.Empty;
            this.frontCamera.Location = new System.Drawing.Point(1280, 319);
            this.frontCamera.Name = "frontCamera";
            this.frontCamera.Size = new System.Drawing.Size(530, 294);
            this.frontCamera.TabIndex = 2;
            // 
            // hiK_FrontView
            // 
            this.hiK_FrontView.BackColor = System.Drawing.Color.Transparent;
            this.hiK_FrontView.CanPlay = false;
            this.hiK_FrontView.ForeColor = System.Drawing.Color.White;
            this.hiK_FrontView.ImagePlaySpan = 25;
            this.hiK_FrontView.Location = new System.Drawing.Point(20, 30);
            this.hiK_FrontView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.hiK_FrontView.Name = "hiK_FrontView";
            this.hiK_FrontView.PlayModel = null;
            this.hiK_FrontView.ShowTarget = false;
            this.hiK_FrontView.Size = new System.Drawing.Size(489, 246);
            this.hiK_FrontView.TabIndex = 0;
            // 
            // cloudCamera
            // 
            this.cloudCamera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cloudCamera.Controls.Add(this.hiK_CloudView);
            this.cloudCamera.DisabledBackColor = System.Drawing.Color.Empty;
            this.cloudCamera.Location = new System.Drawing.Point(1280, 0);
            this.cloudCamera.Name = "cloudCamera";
            this.cloudCamera.Size = new System.Drawing.Size(530, 294);
            this.cloudCamera.TabIndex = 1;
            // 
            // hiK_CloudView
            // 
            this.hiK_CloudView.BackColor = System.Drawing.Color.Transparent;
            this.hiK_CloudView.CanPlay = false;
            this.hiK_CloudView.ForeColor = System.Drawing.Color.White;
            this.hiK_CloudView.ImagePlaySpan = 25;
            this.hiK_CloudView.Location = new System.Drawing.Point(20, 30);
            this.hiK_CloudView.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.hiK_CloudView.Name = "hiK_CloudView";
            this.hiK_CloudView.PlayModel = null;
            this.hiK_CloudView.ShowTarget = false;
            this.hiK_CloudView.Size = new System.Drawing.Size(489, 246);
            this.hiK_CloudView.TabIndex = 1;
            // 
            // mainCamera
            // 
            this.mainCamera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.mainCamera.Controls.Add(this.hiK_MainView);
            this.mainCamera.DisabledBackColor = System.Drawing.Color.Empty;
            this.mainCamera.Location = new System.Drawing.Point(97, 0);
            this.mainCamera.Name = "mainCamera";
            this.mainCamera.Size = new System.Drawing.Size(1096, 613);
            this.mainCamera.TabIndex = 0;
            // 
            // hiK_MainView
            // 
            this.hiK_MainView.BackColor = System.Drawing.Color.Transparent;
            this.hiK_MainView.CanPlay = false;
            this.hiK_MainView.ForeColor = System.Drawing.Color.White;
            this.hiK_MainView.ImagePlaySpan = 25;
            this.hiK_MainView.Location = new System.Drawing.Point(19, 36);
            this.hiK_MainView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.hiK_MainView.Name = "hiK_MainView";
            this.hiK_MainView.PlayModel = null;
            this.hiK_MainView.ShowTarget = false;
            this.hiK_MainView.Size = new System.Drawing.Size(1056, 565);
            this.hiK_MainView.TabIndex = 0;
            // 
            // VisualTracking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(22)))), ((int)(((byte)(50)))));
            this.ClientSize = new System.Drawing.Size(1904, 1053);
            this.Controls.Add(this.centerMain);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "VisualTracking";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.VisualTracking_Load);
            this.centerMain.ResumeLayout(false);
            this.mainWindow2.ResumeLayout(false);
            this.mainWindow2.PerformLayout();
            this.mainWindow.ResumeLayout(false);
            this.backCamera.ResumeLayout(false);
            this.leftCamera.ResumeLayout(false);
            this.rightCamera.ResumeLayout(false);
            this.frontCamera.ResumeLayout(false);
            this.cloudCamera.ResumeLayout(false);
            this.mainCamera.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevComponents.DotNetBar.StyleManager styleManager;
        private DevComponents.DotNetBar.PanelEx centerMain;
        private Controller.ZX_RobotInfo zX_RobotInfo;
        private DevComponents.DotNetBar.LabelX label4;
        private DevComponents.DotNetBar.LabelX label3;
        private DevComponents.DotNetBar.PanelEx mainWindow;
        private DevComponents.DotNetBar.PanelEx backCamera;
        private DevComponents.DotNetBar.PanelEx leftCamera;
        private DevComponents.DotNetBar.PanelEx rightCamera;
        private DevComponents.DotNetBar.PanelEx frontCamera;
        private DevComponents.DotNetBar.PanelEx cloudCamera;
        private DevComponents.DotNetBar.PanelEx mainCamera;
        private DevComponents.DotNetBar.PanelEx mainWindow2;
        private Controller.ZX_MatchInfo zX_MatchInfo1;
        private Controller.ZX_MatchInfo zX_MatchInfo8;
        private Controller.ZX_MatchInfo zX_MatchInfo7;
        private Controller.ZX_MatchInfo zX_MatchInfo6;
        private Controller.ZX_MatchInfo zX_MatchInfo5;
        private Controller.ZX_MatchInfo zX_MatchInfo4;
        private Controller.ZX_MatchInfo zX_MatchInfo3;
        private Controller.ZX_MatchInfo zX_MatchInfo2;
        private Controller.HIK_PlayView_New hiK_MainView;
        private Controller.HIK_PlayView_New hiK_FrontView;
        private Controller.HIK_PlayView_New hiK_BackView;
        private Controller.HIK_PlayView_New hiK_LeftView;
        private Controller.HIK_PlayView_New hiK_RightView;
        private Controller.HIK_PlayView_New hiK_CloudView;
        private SK_FVision.HIK_CloudControl hiK_CloudControl;
    }
}

