using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotVT.Controller
{
    public partial class HIK_CameraSet : SK_FVision.CameraSet
    {
        public HIK_CameraSet()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            base.Init();
            this.Text = StaticInfo.CameraSetFormTitle;
            //this.StartPosition = FormStartPosition.CenterParent;
            //this.WindowState = FormWindowState.Normal;
            //this.ControlBox = false;
            //this.ShowIcon = false;
            //this.ShowInTaskbar = false;

            this.CameraIP_AddressInput.Value = "192.168.1.64";
            this.Port_textBoxX.Text = "8000";
            this.UserName_textBoxX.Text = "admin";
            this.Password_textBoxX.Text = "zx123456";

            LoginModel_comboBoxEx.Items.Add(SK_FCommon.Methods.GetEnumDescription(SK_FVision.HIK_StaticInfo.ProtoType.Private));
            LoginModel_comboBoxEx.Items.Add(SK_FCommon.Methods.GetEnumDescription(SK_FVision.HIK_StaticInfo.ProtoType.RTSP));
            LoginModel_comboBoxEx.SelectedIndex = 0;

            HTTP_comboBoxEx.Items.Add(SK_FCommon.Methods.GetEnumDescription(SK_FVision.HIK_StaticInfo.HTTPS.HTTPS));
            HTTP_comboBoxEx.Items.Add(SK_FCommon.Methods.GetEnumDescription(SK_FVision.HIK_StaticInfo.HTTPS.HTTP));
            HTTP_comboBoxEx.Items.Add(SK_FCommon.Methods.GetEnumDescription(SK_FVision.HIK_StaticInfo.HTTPS.Auto));
            HTTP_comboBoxEx.SelectedIndex = 0;
        }

    }
}
