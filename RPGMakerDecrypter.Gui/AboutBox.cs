using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using RPGMakerDecrypter.Decrypter;

namespace RPGMakerDecrypter.Gui
{
    partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
        }

        private void AboutBox_Load(object sender, EventArgs e)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            var assemblies = currentDomain.GetAssemblies();

            Assembly guiAssembly = assemblies.First(a => a.GetName().Name == "RPGMakerDecrypter");
            Assembly libraryAssembly = assemblies.First(a => a.GetName().Name == "RPGMakerDecrypter.Decrypter");

            versionLabel.Text = String.Format("Version: GUI: {0}, Library: {1}",
                guiAssembly.GetName().Version,
                libraryAssembly.GetName().Version);
        }
    }
}
