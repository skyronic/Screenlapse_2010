using System;
using Gtk;

namespace ScreenLapse
{
	class MainClass
	{
		private static StatusIcon trayIcon;
		
		public static void Main (string[] args)
		{
			Application.Init ();
			MainWindow win = new MainWindow ();
			
			// Set the tray icon properties and delegates
			trayIcon = new StatusIcon(new Gdk.Pixbuf("ScreenlapseIcon.png"));
			trayIcon.Visible = true;
			trayIcon.Activate += delegate(object sender, EventArgs e) {
				// Toggle the visibility of the main window
				win.Visible = !win.Visible;
			};
			trayIcon.PopupMenu += HandleTrayIconPopupMenu;
			trayIcon.Tooltip = "ScreenLapse";
			
			// Start the timer class
			ScrotDaemon.Instance.Activate();
			win.Show ();
			Application.Run ();			
		}

		static void HandleTrayIconPopupMenu (object o, PopupMenuArgs args)
		{
			Menu popupMenu = new Menu();
			ImageMenuItem menuItemQuit = new ImageMenuItem ("Quit");
			
			
			menuItemQuit.Image = new Gtk.Image (Gtk.Stock.Quit, IconSize.Menu);
			popupMenu.Add(menuItemQuit);
			
			menuItemQuit.Activated += delegate(object sender, EventArgs e) {
				Application.Quit();				
			};
			
			popupMenu.ShowAll();
			popupMenu.Popup();			
		}
	}
}
