namespace SK_FVision
{
    partial class CameraSet
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
            this.btX_Save = new DevComponents.DotNetBar.ButtonX();
            this.btX_Cancel = new DevComponents.DotNetBar.ButtonX();
            this.reflectionImage = new DevComponents.DotNetBar.Controls.ReflectionImage();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.HTTP_comboBoxEx = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.Password_textBoxX = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.UserName_textBoxX = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.HTTP_labelX = new DevComponents.DotNetBar.LabelX();
            this.LoginModel_labelX = new DevComponents.DotNetBar.LabelX();
            this.Password_labelX = new DevComponents.DotNetBar.LabelX();
            this.UserName_labelX = new DevComponents.DotNetBar.LabelX();
            this.Port_labelX = new DevComponents.DotNetBar.LabelX();
            this.CameraIP_labelX = new DevComponents.DotNetBar.LabelX();
            this.CameraIP_AddressInput = new DevComponents.Editors.IpAddressInput();
            this.Port_textBoxX = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.LoginModel_comboBoxEx = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CameraIP_AddressInput)).BeginInit();
            this.SuspendLayout();
            // 
            // btX_Save
            // 
            this.btX_Save.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btX_Save.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btX_Save.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btX_Save.Location = new System.Drawing.Point(247, 270);
            this.btX_Save.Name = "btX_Save";
            this.btX_Save.Size = new System.Drawing.Size(85, 37);
            this.btX_Save.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btX_Save.TabIndex = 4;
            this.btX_Save.Text = "添加";
            this.btX_Save.Click += new System.EventHandler(this.btX_Save_Click);
            // 
            // btX_Cancel
            // 
            this.btX_Cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btX_Cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btX_Cancel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btX_Cancel.Location = new System.Drawing.Point(359, 270);
            this.btX_Cancel.Name = "btX_Cancel";
            this.btX_Cancel.Size = new System.Drawing.Size(85, 37);
            this.btX_Cancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btX_Cancel.TabIndex = 5;
            this.btX_Cancel.Text = "取消";
            this.btX_Cancel.Click += new System.EventHandler(this.btX_Cancel_Click);
            // 
            // reflectionImage
            // 
            // 
            // 
            // 
            this.reflectionImage.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.reflectionImage.BackgroundStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.reflectionImage.Image = global::SK_FVision.Properties.Resources.webcam_128px;
            this.reflectionImage.Location = new System.Drawing.Point(1, 29);
            this.reflectionImage.Name = "reflectionImage";
            this.reflectionImage.Size = new System.Drawing.Size(128, 202);
            this.reflectionImage.TabIndex = 7;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.HTTP_comboBoxEx, 1, 5);
            this.tableLayoutPanel.Controls.Add(this.Password_textBoxX, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.UserName_textBoxX, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.HTTP_labelX, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.LoginModel_labelX, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.Password_labelX, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.UserName_labelX, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.Port_labelX, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.CameraIP_labelX, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.CameraIP_AddressInput, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.Port_textBoxX, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.LoginModel_comboBoxEx, 1, 4);
            this.tableLayoutPanel.Location = new System.Drawing.Point(165, 29);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 6;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(282, 219);
            this.tableLayoutPanel.TabIndex = 8;
            // 
            // HTTP_comboBoxEx
            // 
            this.HTTP_comboBoxEx.DisplayMember = "Text";
            this.HTTP_comboBoxEx.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.HTTP_comboBoxEx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.HTTP_comboBoxEx.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HTTP_comboBoxEx.ForeColor = System.Drawing.Color.Black;
            this.HTTP_comboBoxEx.FormattingEnabled = true;
            this.HTTP_comboBoxEx.ItemHeight = 24;
            this.HTTP_comboBoxEx.Location = new System.Drawing.Point(105, 178);
            this.HTTP_comboBoxEx.Name = "HTTP_comboBoxEx";
            this.HTTP_comboBoxEx.Size = new System.Drawing.Size(174, 30);
            this.HTTP_comboBoxEx.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.HTTP_comboBoxEx.TabIndex = 18;
            // 
            // Password_textBoxX
            // 
            this.Password_textBoxX.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.Password_textBoxX.Border.Class = "TextBoxBorder";
            this.Password_textBoxX.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.Password_textBoxX.DisabledBackColor = System.Drawing.Color.White;
            this.Password_textBoxX.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Password_textBoxX.ForeColor = System.Drawing.Color.Black;
            this.Password_textBoxX.Location = new System.Drawing.Point(105, 108);
            this.Password_textBoxX.MaxLength = 20;
            this.Password_textBoxX.Name = "Password_textBoxX";
            this.Password_textBoxX.PasswordChar = '*';
            this.Password_textBoxX.PreventEnterBeep = true;
            this.Password_textBoxX.Size = new System.Drawing.Size(174, 29);
            this.Password_textBoxX.TabIndex = 16;
            // 
            // UserName_textBoxX
            // 
            this.UserName_textBoxX.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.UserName_textBoxX.Border.Class = "TextBoxBorder";
            this.UserName_textBoxX.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.UserName_textBoxX.DisabledBackColor = System.Drawing.Color.White;
            this.UserName_textBoxX.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UserName_textBoxX.ForeColor = System.Drawing.Color.Black;
            this.UserName_textBoxX.Location = new System.Drawing.Point(105, 73);
            this.UserName_textBoxX.MaxLength = 20;
            this.UserName_textBoxX.Name = "UserName_textBoxX";
            this.UserName_textBoxX.PreventEnterBeep = true;
            this.UserName_textBoxX.Size = new System.Drawing.Size(174, 29);
            this.UserName_textBoxX.TabIndex = 15;
            // 
            // HTTP_labelX
            // 
            // 
            // 
            // 
            this.HTTP_labelX.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.HTTP_labelX.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.HTTP_labelX.Location = new System.Drawing.Point(3, 178);
            this.HTTP_labelX.Name = "HTTP_labelX";
            this.HTTP_labelX.Size = new System.Drawing.Size(94, 23);
            this.HTTP_labelX.TabIndex = 12;
            this.HTTP_labelX.Text = "HTTP(S)：";
            // 
            // LoginModel_labelX
            // 
            // 
            // 
            // 
            this.LoginModel_labelX.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.LoginModel_labelX.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.LoginModel_labelX.Location = new System.Drawing.Point(3, 143);
            this.LoginModel_labelX.Name = "LoginModel_labelX";
            this.LoginModel_labelX.Size = new System.Drawing.Size(94, 23);
            this.LoginModel_labelX.TabIndex = 11;
            this.LoginModel_labelX.Text = "登陆模式：";
            // 
            // Password_labelX
            // 
            // 
            // 
            // 
            this.Password_labelX.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.Password_labelX.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.Password_labelX.Location = new System.Drawing.Point(3, 108);
            this.Password_labelX.Name = "Password_labelX";
            this.Password_labelX.Size = new System.Drawing.Size(94, 23);
            this.Password_labelX.TabIndex = 10;
            this.Password_labelX.Text = "密码：";
            // 
            // UserName_labelX
            // 
            // 
            // 
            // 
            this.UserName_labelX.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.UserName_labelX.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.UserName_labelX.Location = new System.Drawing.Point(3, 73);
            this.UserName_labelX.Name = "UserName_labelX";
            this.UserName_labelX.Size = new System.Drawing.Size(94, 23);
            this.UserName_labelX.TabIndex = 9;
            this.UserName_labelX.Text = "用户名：";
            // 
            // Port_labelX
            // 
            // 
            // 
            // 
            this.Port_labelX.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.Port_labelX.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.Port_labelX.Location = new System.Drawing.Point(3, 38);
            this.Port_labelX.Name = "Port_labelX";
            this.Port_labelX.Size = new System.Drawing.Size(94, 23);
            this.Port_labelX.TabIndex = 8;
            this.Port_labelX.Text = "端口号：";
            // 
            // CameraIP_labelX
            // 
            // 
            // 
            // 
            this.CameraIP_labelX.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.CameraIP_labelX.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.CameraIP_labelX.Location = new System.Drawing.Point(3, 3);
            this.CameraIP_labelX.Name = "CameraIP_labelX";
            this.CameraIP_labelX.Size = new System.Drawing.Size(96, 23);
            this.CameraIP_labelX.TabIndex = 7;
            this.CameraIP_labelX.Text = "设备地址：";
            // 
            // CameraIP_AddressInput
            // 
            this.CameraIP_AddressInput.AutoOverwrite = true;
            // 
            // 
            // 
            this.CameraIP_AddressInput.BackgroundStyle.Class = "DateTimeInputBackground";
            this.CameraIP_AddressInput.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.CameraIP_AddressInput.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.CameraIP_AddressInput.ButtonFreeText.Visible = true;
            this.CameraIP_AddressInput.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CameraIP_AddressInput.Location = new System.Drawing.Point(105, 3);
            this.CameraIP_AddressInput.Name = "CameraIP_AddressInput";
            this.CameraIP_AddressInput.Size = new System.Drawing.Size(174, 29);
            this.CameraIP_AddressInput.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.CameraIP_AddressInput.TabIndex = 13;
            // 
            // Port_textBoxX
            // 
            this.Port_textBoxX.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.Port_textBoxX.Border.Class = "TextBoxBorder";
            this.Port_textBoxX.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.Port_textBoxX.DisabledBackColor = System.Drawing.Color.White;
            this.Port_textBoxX.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Port_textBoxX.ForeColor = System.Drawing.Color.Black;
            this.Port_textBoxX.Location = new System.Drawing.Point(105, 38);
            this.Port_textBoxX.MaxLength = 20;
            this.Port_textBoxX.Name = "Port_textBoxX";
            this.Port_textBoxX.PreventEnterBeep = true;
            this.Port_textBoxX.Size = new System.Drawing.Size(174, 29);
            this.Port_textBoxX.TabIndex = 14;
            // 
            // LoginModel_comboBoxEx
            // 
            this.LoginModel_comboBoxEx.DisplayMember = "Text";
            this.LoginModel_comboBoxEx.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.LoginModel_comboBoxEx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LoginModel_comboBoxEx.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LoginModel_comboBoxEx.ForeColor = System.Drawing.Color.Black;
            this.LoginModel_comboBoxEx.FormattingEnabled = true;
            this.LoginModel_comboBoxEx.ItemHeight = 24;
            this.LoginModel_comboBoxEx.Location = new System.Drawing.Point(105, 143);
            this.LoginModel_comboBoxEx.Name = "LoginModel_comboBoxEx";
            this.LoginModel_comboBoxEx.Size = new System.Drawing.Size(174, 30);
            this.LoginModel_comboBoxEx.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LoginModel_comboBoxEx.TabIndex = 17;
            // 
            // HIK_CameraSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 319);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.reflectionImage);
            this.Controls.Add(this.btX_Cancel);
            this.Controls.Add(this.btX_Save);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "HIK_CameraSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.HIK_CameraSet_Load);
            this.tableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CameraIP_AddressInput)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevComponents.DotNetBar.ButtonX btX_Save;
        private DevComponents.DotNetBar.ButtonX btX_Cancel;
        private DevComponents.DotNetBar.Controls.ReflectionImage reflectionImage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private DevComponents.DotNetBar.LabelX HTTP_labelX;
        private DevComponents.DotNetBar.LabelX LoginModel_labelX;
        private DevComponents.DotNetBar.LabelX Password_labelX;
        private DevComponents.DotNetBar.LabelX UserName_labelX;
        private DevComponents.DotNetBar.LabelX Port_labelX;
        private DevComponents.DotNetBar.LabelX CameraIP_labelX;
        private DevComponents.Editors.IpAddressInput CameraIP_AddressInput;
        private DevComponents.DotNetBar.Controls.TextBoxX Port_textBoxX;
        private DevComponents.DotNetBar.Controls.TextBoxX UserName_textBoxX;
        private DevComponents.DotNetBar.Controls.TextBoxX Password_textBoxX;
        private DevComponents.DotNetBar.Controls.ComboBoxEx LoginModel_comboBoxEx;
        private DevComponents.DotNetBar.Controls.ComboBoxEx HTTP_comboBoxEx;
    }
}