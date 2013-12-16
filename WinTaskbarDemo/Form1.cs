using System;
using DevExpress.Utils.Taskbar;
using WinTaskbarDemo.Properties;

namespace WinTaskbarDemo
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
            ToggleCheckValue();
        }

        private void busyCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            ToggleCheckValue();
        }

        private void ToggleCheckValue()
        {
            if (busyCheckEdit.Checked)
                taskbarAssistant1.OverlayIcon = Resources.delete;
            else
                taskbarAssistant1.OverlayIcon = Resources.check;
        }

        private void whatsNewTaskbarButton_Click(object sender, ThumbButtonClickEventArgs e)
        {
            webBrowser1.Navigate(@"https://www.devexpress.com/Subscriptions/New-2013.xml");
        }

        private void helpTaskbarButton_Click(object sender, ThumbButtonClickEventArgs e)
        {
            webBrowser1.Navigate(@"http://documentation.devexpress.com/#HomePage/CustomDocument9453");
        }
    }
}
