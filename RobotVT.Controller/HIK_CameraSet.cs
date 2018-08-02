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
        public RobotVT.Model.S_D_CameraSet _CameraSet { get; set; }


        public HIK_CameraSet()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            this.Text = StaticInfo.CameraSetFormTitle;

            if (_CameraSet == null)
            {
                this.CameraIP_AddressInput.Value = "192.168.1.64";
                this.Port_textBoxX.Text = "8000";
                this.UserName_textBoxX.Text = "admin";
                this.Password_textBoxX.Text = "zx123456";
            }
            else
            {
                this.CameraIP_AddressInput.Value = _CameraSet.VT_IP;
                this.Port_textBoxX.Text = _CameraSet.VT_PORT;
                this.UserName_textBoxX.Text = _CameraSet.VT_NAME;
                this.Password_textBoxX.Text = _CameraSet.VT_PASSWORD;
            }

            LoginModel_comboBoxEx.Items.Add(SK_FCommon.Methods.GetEnumDescription(SK_FVision.HIK_StaticInfo.ProtoType.Private));
            LoginModel_comboBoxEx.Items.Add(SK_FCommon.Methods.GetEnumDescription(SK_FVision.HIK_StaticInfo.ProtoType.RTSP));
            LoginModel_comboBoxEx.SelectedIndex = 0;

            HTTP_comboBoxEx.Items.Add(SK_FCommon.Methods.GetEnumDescription(SK_FVision.HIK_StaticInfo.HTTPS.HTTPS));
            HTTP_comboBoxEx.Items.Add(SK_FCommon.Methods.GetEnumDescription(SK_FVision.HIK_StaticInfo.HTTPS.HTTP));
            HTTP_comboBoxEx.Items.Add(SK_FCommon.Methods.GetEnumDescription(SK_FVision.HIK_StaticInfo.HTTPS.Auto));
            HTTP_comboBoxEx.SelectedIndex = 0;

            base.Init();
        }

        public override void HIK_CameraSet_Load(object sender, EventArgs e)
        {
            base.HIK_CameraSet_Load(sender, e);
        }

        public override void btX_Save_Click(object sender, EventArgs e)
        {
            base.btX_Save_Click(sender, e);
        }

        public override void btX_Cancel_Click(object sender, EventArgs e)
        {
            base.btX_Cancel_Click(sender, e);
        }
    }
}
