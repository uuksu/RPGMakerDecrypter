using System;
using Gtk;

namespace RPGMakerDecrypter.Gui.Gtk
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();

            var app = new Application("org.RPGMakerDecrypter.Gui.Gtk.RPGMakerDecrypter.Gui.Gtk", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);
            
            var win = new MainWindow();
            app.AddWindow(win);

            win.Show();
            Application.Run();
        }
    }
}
