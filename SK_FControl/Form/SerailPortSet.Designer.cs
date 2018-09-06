namespace SK_FControl
{
    partial class SerailPortSet
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lbXTimeout = new DevComponents.DotNetBar.LabelX();
            this.lbXStopBits = new DevComponents.DotNetBar.LabelX();
            this.lbXPraity = new DevComponents.DotNetBar.LabelX();
            this.lbXDataBits = new DevComponents.DotNetBar.LabelX();
            this.lbXBaudRate = new DevComponents.DotNetBar.LabelX();
            this.lbXPortName = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cBEPortName = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cBEBaudRate = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cBEDataBits = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cBEParity = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cBEStopBits = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.tBXTimeOut = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.tBXSample = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lbXSample = new DevComponents.DotNetBar.LabelX();
            this.btX_Cancel = new DevComponents.DotNetBar.ButtonX();
            this.btX_Save = new DevComponents.DotNetBar.ButtonX();
            this.reflectionImage = new DevComponents.DotNetBar.Controls.ReflectionImage();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel.Controls.Add(this.lbXTimeout, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.lbXStopBits, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.lbXPraity, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.lbXDataBits, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.lbXBaudRate, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.lbXPortName, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelX1, 2, 6);
            this.tableLayoutPanel.Controls.Add(this.labelX2, 2, 5);
            this.tableLayoutPanel.Controls.Add(this.cBEPortName, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.cBEBaudRate, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.cBEDataBits, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.cBEParity, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.cBEStopBits, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.tBXTimeOut, 1, 5);
            this.tableLayoutPanel.Controls.Add(this.tBXSample, 1, 6);
            this.tableLayoutPanel.Controls.Add(this.lbXSample, 0, 6);
            this.tableLayoutPanel.Location = new System.Drawing.Point(165, 29);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 7;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(286, 255);
            this.tableLayoutPanel.TabIndex = 9;
            // 
            // lbXTimeout
            // 
            // 
            // 
            // 
            this.lbXTimeout.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbXTimeout.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lbXTimeout.Location = new System.Drawing.Point(3, 182);
            this.lbXTimeout.Name = "lbXTimeout";
            this.lbXTimeout.Size = new System.Drawing.Size(94, 23);
            this.lbXTimeout.TabIndex = 12;
            this.lbXTimeout.Text = "超时时间：";
            this.lbXTimeout.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // lbXStopBits
            // 
            // 
            // 
            // 
            this.lbXStopBits.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbXStopBits.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lbXStopBits.Location = new System.Drawing.Point(3, 147);
            this.lbXStopBits.Name = "lbXStopBits";
            this.lbXStopBits.Size = new System.Drawing.Size(94, 23);
            this.lbXStopBits.TabIndex = 11;
            this.lbXStopBits.Text = "停止位：";
            this.lbXStopBits.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // lbXPraity
            // 
            // 
            // 
            // 
            this.lbXPraity.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbXPraity.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lbXPraity.Location = new System.Drawing.Point(3, 111);
            this.lbXPraity.Name = "lbXPraity";
            this.lbXPraity.Size = new System.Drawing.Size(94, 23);
            this.lbXPraity.TabIndex = 10;
            this.lbXPraity.Text = "校验位：";
            this.lbXPraity.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // lbXDataBits
            // 
            // 
            // 
            // 
            this.lbXDataBits.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbXDataBits.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lbXDataBits.Location = new System.Drawing.Point(3, 75);
            this.lbXDataBits.Name = "lbXDataBits";
            this.lbXDataBits.Size = new System.Drawing.Size(94, 23);
            this.lbXDataBits.TabIndex = 9;
            this.lbXDataBits.Text = "数据位：";
            this.lbXDataBits.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // lbXBaudRate
            // 
            // 
            // 
            // 
            this.lbXBaudRate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbXBaudRate.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lbXBaudRate.Location = new System.Drawing.Point(3, 39);
            this.lbXBaudRate.Name = "lbXBaudRate";
            this.lbXBaudRate.Size = new System.Drawing.Size(94, 23);
            this.lbXBaudRate.TabIndex = 8;
            this.lbXBaudRate.Text = "波特率：";
            this.lbXBaudRate.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // lbXPortName
            // 
            // 
            // 
            // 
            this.lbXPortName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbXPortName.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lbXPortName.Location = new System.Drawing.Point(3, 3);
            this.lbXPortName.Name = "lbXPortName";
            this.lbXPortName.Size = new System.Drawing.Size(96, 23);
            this.lbXPortName.TabIndex = 7;
            this.lbXPortName.Text = "端口名称：";
            this.lbXPortName.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelX1.Location = new System.Drawing.Point(247, 217);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(36, 23);
            this.labelX1.TabIndex = 14;
            this.labelX1.Text = "ms";
            this.labelX1.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelX2.Location = new System.Drawing.Point(247, 182);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(36, 23);
            this.labelX2.TabIndex = 15;
            this.labelX2.Text = "ms";
            this.labelX2.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // cBEPortName
            // 
            this.cBEPortName.DisplayMember = "Text";
            this.cBEPortName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cBEPortName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBEPortName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cBEPortName.ForeColor = System.Drawing.Color.Black;
            this.cBEPortName.FormattingEnabled = true;
            this.cBEPortName.ItemHeight = 24;
            this.cBEPortName.Location = new System.Drawing.Point(105, 3);
            this.cBEPortName.Name = "cBEPortName";
            this.cBEPortName.Size = new System.Drawing.Size(136, 30);
            this.cBEPortName.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cBEPortName.TabIndex = 20;
            // 
            // cBEBaudRate
            // 
            this.cBEBaudRate.DisplayMember = "Text";
            this.cBEBaudRate.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cBEBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBEBaudRate.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cBEBaudRate.ForeColor = System.Drawing.Color.Black;
            this.cBEBaudRate.FormattingEnabled = true;
            this.cBEBaudRate.ItemHeight = 24;
            this.cBEBaudRate.Location = new System.Drawing.Point(105, 39);
            this.cBEBaudRate.Name = "cBEBaudRate";
            this.cBEBaudRate.Size = new System.Drawing.Size(136, 30);
            this.cBEBaudRate.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cBEBaudRate.TabIndex = 21;
            // 
            // cBEDataBits
            // 
            this.cBEDataBits.DisplayMember = "Text";
            this.cBEDataBits.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cBEDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBEDataBits.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cBEDataBits.ForeColor = System.Drawing.Color.Black;
            this.cBEDataBits.FormattingEnabled = true;
            this.cBEDataBits.ItemHeight = 24;
            this.cBEDataBits.Location = new System.Drawing.Point(105, 75);
            this.cBEDataBits.Name = "cBEDataBits";
            this.cBEDataBits.Size = new System.Drawing.Size(136, 30);
            this.cBEDataBits.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cBEDataBits.TabIndex = 22;
            // 
            // cBEParity
            // 
            this.cBEParity.DisplayMember = "Text";
            this.cBEParity.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cBEParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBEParity.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cBEParity.ForeColor = System.Drawing.Color.Black;
            this.cBEParity.FormattingEnabled = true;
            this.cBEParity.ItemHeight = 24;
            this.cBEParity.Location = new System.Drawing.Point(105, 111);
            this.cBEParity.Name = "cBEParity";
            this.cBEParity.Size = new System.Drawing.Size(136, 30);
            this.cBEParity.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cBEParity.TabIndex = 23;
            // 
            // cBEStopBits
            // 
            this.cBEStopBits.DisplayMember = "Text";
            this.cBEStopBits.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cBEStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBEStopBits.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cBEStopBits.ForeColor = System.Drawing.Color.Black;
            this.cBEStopBits.FormattingEnabled = true;
            this.cBEStopBits.ItemHeight = 24;
            this.cBEStopBits.Location = new System.Drawing.Point(105, 147);
            this.cBEStopBits.Name = "cBEStopBits";
            this.cBEStopBits.Size = new System.Drawing.Size(136, 30);
            this.cBEStopBits.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cBEStopBits.TabIndex = 24;
            // 
            // tBXTimeOut
            // 
            this.tBXTimeOut.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.tBXTimeOut.Border.Class = "TextBoxBorder";
            this.tBXTimeOut.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tBXTimeOut.DisabledBackColor = System.Drawing.Color.White;
            this.tBXTimeOut.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tBXTimeOut.ForeColor = System.Drawing.Color.Black;
            this.tBXTimeOut.Location = new System.Drawing.Point(105, 182);
            this.tBXTimeOut.MaxLength = 20;
            this.tBXTimeOut.Name = "tBXTimeOut";
            this.tBXTimeOut.PreventEnterBeep = true;
            this.tBXTimeOut.Size = new System.Drawing.Size(136, 29);
            this.tBXTimeOut.TabIndex = 25;
            // 
            // tBXSample
            // 
            this.tBXSample.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.tBXSample.Border.Class = "TextBoxBorder";
            this.tBXSample.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tBXSample.DisabledBackColor = System.Drawing.Color.White;
            this.tBXSample.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tBXSample.ForeColor = System.Drawing.Color.Black;
            this.tBXSample.Location = new System.Drawing.Point(105, 217);
            this.tBXSample.MaxLength = 20;
            this.tBXSample.Name = "tBXSample";
            this.tBXSample.PreventEnterBeep = true;
            this.tBXSample.Size = new System.Drawing.Size(136, 29);
            this.tBXSample.TabIndex = 26;
            // 
            // lbXSample
            // 
            // 
            // 
            // 
            this.lbXSample.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbXSample.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lbXSample.Location = new System.Drawing.Point(3, 217);
            this.lbXSample.Name = "lbXSample";
            this.lbXSample.Size = new System.Drawing.Size(94, 23);
            this.lbXSample.TabIndex = 13;
            this.lbXSample.Text = "采样时间：";
            this.lbXSample.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // btX_Cancel
            // 
            this.btX_Cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btX_Cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btX_Cancel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btX_Cancel.Location = new System.Drawing.Point(349, 310);
            this.btX_Cancel.Name = "btX_Cancel";
            this.btX_Cancel.Size = new System.Drawing.Size(85, 37);
            this.btX_Cancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btX_Cancel.TabIndex = 11;
            this.btX_Cancel.Text = "取消";
            this.btX_Cancel.Click += new System.EventHandler(this.btX_Cancel_Click);
            // 
            // btX_Save
            // 
            this.btX_Save.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btX_Save.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btX_Save.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btX_Save.Location = new System.Drawing.Point(237, 310);
            this.btX_Save.Name = "btX_Save";
            this.btX_Save.Size = new System.Drawing.Size(85, 37);
            this.btX_Save.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btX_Save.TabIndex = 10;
            this.btX_Save.Text = "保存";
            this.btX_Save.Click += new System.EventHandler(this.btX_Save_Click);
            // 
            // reflectionImage
            // 
            // 
            // 
            // 
            this.reflectionImage.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.reflectionImage.BackgroundStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.reflectionImage.Image = global::SK_FControl.Properties.Resources.serialport02;
            this.reflectionImage.Location = new System.Drawing.Point(21, 32);
            this.reflectionImage.Name = "reflectionImage";
            this.reflectionImage.Size = new System.Drawing.Size(128, 202);
            this.reflectionImage.TabIndex = 12;
            // 
            // SerailPortSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 373);
            this.Controls.Add(this.reflectionImage);
            this.Controls.Add(this.btX_Cancel);
            this.Controls.Add(this.btX_Save);
            this.Controls.Add(this.tableLayoutPanel);
            this.DoubleBuffered = true;
            this.Name = "SerailPortSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SerailPortSet";
            this.Load += new System.EventHandler(this.SerailPortSet_Load);
            this.tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private DevComponents.DotNetBar.LabelX lbXTimeout;
        private DevComponents.DotNetBar.LabelX lbXStopBits;
        private DevComponents.DotNetBar.LabelX lbXPraity;
        private DevComponents.DotNetBar.LabelX lbXDataBits;
        private DevComponents.DotNetBar.LabelX lbXBaudRate;
        private DevComponents.DotNetBar.LabelX lbXPortName;
        private DevComponents.DotNetBar.LabelX lbXSample;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.ButtonX btX_Cancel;
        private DevComponents.DotNetBar.ButtonX btX_Save;
        private DevComponents.DotNetBar.Controls.ReflectionImage reflectionImage;
        public DevComponents.DotNetBar.Controls.ComboBoxEx cBEPortName;
        public DevComponents.DotNetBar.Controls.ComboBoxEx cBEBaudRate;
        public DevComponents.DotNetBar.Controls.ComboBoxEx cBEDataBits;
        public DevComponents.DotNetBar.Controls.ComboBoxEx cBEParity;
        public DevComponents.DotNetBar.Controls.ComboBoxEx cBEStopBits;
        public DevComponents.DotNetBar.Controls.TextBoxX tBXTimeOut;
        public DevComponents.DotNetBar.Controls.TextBoxX tBXSample;
    }
}