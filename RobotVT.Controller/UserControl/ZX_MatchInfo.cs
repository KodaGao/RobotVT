using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace RobotVT.Controller
{
    public partial class ZX_MatchInfo : UserControl
    {
        public int Number;

        public ZX_MatchInfo()
        {
            InitializeComponent();
            StaticInfo.HIKAnalysis.Event_FaceSnapAlarm += HIKAnalysis_Event_FaceSnapAlarm;
        }

        private void ZX_MatchInfo_Load(object sender, EventArgs e)
        {
            this.CompareBox.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.comparebg;
            this.CompareTextPanel.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.greenBg;

            this.pictureA.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureB.SizeMode = PictureBoxSizeMode.Zoom;
        }
        

        private void HIKAnalysis_Event_FaceSnapAlarm(HIK_AlarmInfo AlarmInfo)
        {
            if (AlarmInfo.QueueNubmer != Number) return;

            this.Invoke(new MethodInvoker(delegate
            {
                pictureA.Image = AlarmInfo.FacePic;
                pictureB.Image = AlarmInfo.BackgroudPic;
                if (AlarmInfo.FaceScore >0)
                { 
                    lbXCompare.Text = "相似度 " + AlarmInfo.FaceScore;
                    lbXCompare.ForeColor = Color.Green;
                }
                if (AlarmInfo.FaceScore == 0 && AlarmInfo.FaceType == 1)
                { 
                    lbXCompare.Text = "陌生人";
                    lbXCompare.ForeColor = Color.Red;
                    pictureB.Image = RobotVT.Resources.Properties.Resources.human;
                }
                if (AlarmInfo.FaceScore == 0 && AlarmInfo.FaceType == 2)
                {
                    lbXCompare.Text = "比对失败";
                    lbXCompare.ForeColor = Color.Red;
                    pictureB.Image = RobotVT.Resources.Properties.Resources.human;
                }
                lbXAbstime.Text = AlarmInfo.Abstime;
                lbXDevInfo.Text = AlarmInfo.DevIP;
            }));
        }
    }
}
