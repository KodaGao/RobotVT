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
    public partial class ZX_MatchInfo : UserControl
    {
        public ZX_MatchInfo()
        {
            InitializeComponent();
        }

        private void ZX_MatchInfo_Load(object sender, EventArgs e)
        {
            this.CompareBox.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.comparebg;
            this.CompareTextPanel.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.greenBg;

            this.pictureA.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureB.SizeMode = PictureBoxSizeMode.Zoom;
        }
    }
}
