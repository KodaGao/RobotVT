using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SK_FVision
{
    public partial class HIK_CameraSet : Form
    {
        public HIK_CameraSet()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            this.Text = HIK_StaticInfo.SetFormTitle;
            this.StartPosition = FormStartPosition.CenterParent;
            this.WindowState = FormWindowState.Normal;
            this.ControlBox = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
        }

        private void HIK_CameraSet_Load(object sender, EventArgs e)
        {

        }

    }
}
