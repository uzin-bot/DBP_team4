using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kyg
{
    public partial class TempForm : Form
    {
        public TempForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string startup = Application.StartupPath;

            int binIndex = startup.IndexOf(@"\DBP_WinformChat\", StringComparison.OrdinalIgnoreCase);
            string projectRoot = startup.Substring(0, binIndex);

            string exePath = Path.Combine(
                projectRoot,
                @"DBP_WinformChat\kyg_chatServer\bin\Debug\net8.0\kyg_chatServer.exe"
            );
            Process.Start(exePath);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string startup = Application.StartupPath;

            int binIndex = startup.IndexOf(@"\DBP_WinformChat\", StringComparison.OrdinalIgnoreCase);
            string projectRoot = startup.Substring(0, binIndex);

            string exePath = Path.Combine(
                projectRoot,
                @"DBP_WinformChat\kyg_chat\bin\Debug\net8.0-windows\kyg_chat.exe"
            );
            Process.Start(exePath);
        }
    }
}
