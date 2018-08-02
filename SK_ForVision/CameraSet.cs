using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using SK_FVision.Properties;

namespace SK_FVision
{
    public partial class CameraSet : MetroForm
    {
        public CameraSet()
        {
            InitializeComponent();
            Init();
        }

        public virtual void Init()
        {
            this.Text = "";// HIK_StaticInfo.SetFormTitle;
            this.StartPosition = FormStartPosition.CenterParent;
            this.WindowState = FormWindowState.Normal;
            this.ControlBox = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
        }

        public virtual void HIK_CameraSet_Load(object sender, EventArgs e)
        {

        }

        public virtual void btX_Save_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public virtual void btX_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
