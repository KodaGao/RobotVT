namespace SK_FVision
{
    partial class HIK_CameraSet
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
            this.components = new System.ComponentModel.Container();
            this.styleManager = new DevComponents.DotNetBar.StyleManager(this.components);
            this.ipAddressInput1 = new DevComponents.Editors.IpAddressInput();
            ((System.ComponentModel.ISupportInitialize)(this.ipAddressInput1)).BeginInit();
            this.SuspendLayout();
            // 
            // styleManager
            // 
            this.styleManager.ManagerColorTint = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(22)))), ((int)(((byte)(50)))));
            this.styleManager.ManagerStyle = DevComponents.DotNetBar.eStyle.Metro;
            this.styleManager.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(22)))), ((int)(((byte)(50))))), System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(22)))), ((int)(((byte)(50))))));
            // 
            // ipAddressInput1
            // 
            this.ipAddressInput1.AutoOverwrite = true;
            // 
            // 
            // 
            this.ipAddressInput1.BackgroundStyle.Class = "DateTimeInputBackground";
            this.ipAddressInput1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ipAddressInput1.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.ipAddressInput1.ButtonFreeText.Visible = true;
            this.ipAddressInput1.Location = new System.Drawing.Point(186, 91);
            this.ipAddressInput1.Name = "ipAddressInput1";
            this.ipAddressInput1.Size = new System.Drawing.Size(227, 21);
            this.ipAddressInput1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ipAddressInput1.TabIndex = 4;
            // 
            // HIK_CameraSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 423);
            this.Controls.Add(this.ipAddressInput1);
            this.Name = "HIK_CameraSet";
            this.Load += new System.EventHandler(this.HIK_CameraSet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ipAddressInput1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.StyleManager styleManager;
        private DevComponents.Editors.IpAddressInput ipAddressInput1;
    }
}