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

namespace SK_FControl.Form
{
    public partial class SplashScreen : System.Windows.Forms.Form
    {
        private static DevComponents.DotNetBar.LabelX LRunMsg;
        private static SplashScreen instance;

        public static SplashScreen Instance
        {
            get
            {
                return instance;
            }
            set
            {
                instance = value;
            }
        }

        public SplashScreen()
        {
            InitializeComponent();
            this.Text = "正在启动...";
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackgroundImage = Properties.Resources.SplashScreen;
            this.Size = this.BackgroundImage.Size;
            ShowRuningMessage(string.Empty);
        }

        public static void ShowSplashScreen()
        {
            LRunMsg = new DevComponents.DotNetBar.LabelX();
            LRunMsg.AutoSize = true;
            LRunMsg.Font = new Font("宋体", 9, FontStyle.Regular);
            LRunMsg.BackColor = Color.Transparent;
            LRunMsg.ForeColor = Color.White;
            LRunMsg.Dock = DockStyle.Bottom;
            instance = new SplashScreen();
            instance.Controls.Add(LRunMsg);
            instance.Show();
        }

        private delegate void del_ShowRuningMessage(string text);

        public static void ShowRuningMessage(string Info)
        {
            if (LRunMsg.InvokeRequired)
            {
                LRunMsg.Invoke(new del_ShowRuningMessage(ShowRuningMessage), Info);
            }
            else
            {
                LRunMsg.Text = Info;
                Application.DoEvents();
            }
        }
    }
}
