using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotVT.Controller
{
    public partial class ZX_RobotInfo : UserControl
    {
        public ZX_RobotInfo()
        {
            InitializeComponent();
            topMain.MouseUp += new MouseEventHandler(topMainWnd_MouseUp);
            topMain.MouseDoubleClick += new MouseEventHandler(topMainWnd_MouseDoubleClick);
        }

        private void ZX_RobotInfo_Load(object sender, EventArgs e)
        {
            this.topMain.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.top_img;
            this.signal.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.signal_5;
            this.power1.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power1_3;
            this.power2.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power2_2;
            this.robotPower.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power3_3;
            this.lamp.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.lamp_0;
        }
        
        public void topMainWnd_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //if (!MouseUp && this.PlayModel == null) return;

                HIK_SerialPort hIK_SPort = new HIK_SerialPort();
                //hIK_CameraSet.Event_SetFinish += HIK_CameraSet_Event_SetFinish;
                //if (hIK_SPort != null)
                //{
                //    hIK_CameraSet._CameraSet = _CameraSet;
                //}
                //hIK_CameraSet.PlayModel = this.PlayModel;
                hIK_SPort.ShowDialog();
            }
        }

        public void topMainWnd_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }
    }
}
