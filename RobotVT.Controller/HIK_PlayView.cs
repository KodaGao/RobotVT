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
    public partial class HIK_PlayView : SK_FVision.PlayView
    {
        public string PlayModel { get; set; }

        public HIK_PlayView()
        {
            InitializeComponent();
        }

        public override void PlayView_Load(object sender, EventArgs e)
        {
            base.PlayView_Load(sender, e);
        }

        public override void RealPlayWnd_MouseUp(object sender, MouseEventArgs e)
        {
            base.RealPlayWnd_MouseUp(sender, e);

            if (PlayModel == null) return;

            if (PlayModel.ToUpper() != "MAINPLAY")
            {
                HIK_CameraSet _CameraSet = new HIK_CameraSet();
                _CameraSet.ShowDialog();
            }
        }

        public override void RealPlayWnd_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            base.RealPlayWnd_MouseDoubleClick(sender, e);
            MessageBox.Show("切换");
        }

    }
}
