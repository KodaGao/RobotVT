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
        new public bool MouseUp = true;

        public string PlayModel { get; set; }
        public RobotVT.Model.S_D_CameraSet _CameraSet { get; set; }

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
            if (!MouseUp && this.PlayModel == null) return;

            HIK_CameraSet hIK_CameraSet = new HIK_CameraSet();
            if (_CameraSet != null)
            {
                hIK_CameraSet._CameraSet = _CameraSet;
            }
            hIK_CameraSet.PlayModel = this.PlayModel;
            hIK_CameraSet.ShowDialog();

            base.RealPlayWnd_MouseUp(sender, e);
        }

        public override void RealPlayWnd_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show("切换");
            base.RealPlayWnd_MouseDoubleClick(sender, e);
        }

    }
}
