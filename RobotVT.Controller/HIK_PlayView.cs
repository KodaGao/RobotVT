using System;
using System.Windows.Forms;

namespace RobotVT.Controller
{
    public partial class HIK_PlayView : SK_FVision.PlayView
    {

        public string PlayModel
        {
            get;
            set;
        }

        public HIK_PlayView()
        {
            InitializeComponent();
        }

        public override void RealPlayWnd_MouseUp(object sender, MouseEventArgs e)
        {
            if (PlayModel == null) return;

            if (PlayModel.ToUpper() != "MAINPLAY")
            {
                HIK_CameraSet _CameraSet = new HIK_CameraSet();
                _CameraSet.ShowDialog();
            }
        }

        private void PlayView_Load(object sender, EventArgs e)
        {
        }


    }
}