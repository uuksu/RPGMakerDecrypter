using System;
using System.Linq;
using System.Reflection;
using Gtk;

namespace RPGMakerDecrypter.Gui.Gtk
{
    class AboutWindow : AboutDialog
    {
        public AboutWindow() : this(new Builder("AboutWindow.glade")) { }
        private AboutWindow(Builder builder) : base(builder.GetRawOwnedObject("AboutWindow"))
        {
            builder.Autoconnect(this);

            AppDomain currentDomain = AppDomain.CurrentDomain;
            var assemblies = currentDomain.GetAssemblies();

            Assembly guiAssembly = assemblies.First(a => a.GetName().Name == "RPGMakerDecrypter.Gui.Gtk");
            Assembly libraryAssembly = assemblies.First(a => a.GetName().Name == "RPGMakerDecrypter.Decrypter");

            this.Version = String.Format("Version: GUI: {0}, Library: {1}",
                guiAssembly.GetName().Version,
                libraryAssembly.GetName().Version);

            this.Close += (sender, e) => this.Dispose();            
        }
    }
}
