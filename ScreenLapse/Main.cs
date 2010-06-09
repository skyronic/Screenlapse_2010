using System;
using Gtk;

namespace ScreenLapse
{
	class MainClass
	{
		private static StatusIcon trayIcon;
		
		/// <summary>
		/// Use this to run testing code instead of staring the whole gui
		/// </summary>
		public static void TestBed()
		{
			
			
			
		}
		public static void Main (string[] args)
		{
			if (true) {
				// Execute the main program
				Application.Init ();
				MainWindow win = new MainWindow ();
				Preferences.Initialize();
				
				// Set the tray icon properties and delegates
				trayIcon = new StatusIcon(new Gdk.Pixbuf("ScreenlapseIcon.png"));
				trayIcon.Visible = true;
				trayIcon.Activate += delegate(object sender, EventArgs e) {
					// Toggle the visibility of the main window
					if(win.Visible)
					{
						win.HideAll();
					}
					else
					{
						win.ShowAll();
					}
				};
				trayIcon.PopupMenu += HandleTrayIconPopupMenu;
				trayIcon.Tooltip = "ScreenLapse";
				
				// Start the timer class
				ScrotDaemon.Instance.Activate();
				win.Show ();
				Application.Run ();
			}
			else {
				TestBed();
			}
		}
		

		static void HandleTrayIconPopupMenu (object o, PopupMenuArgs args)
		{
			Menu popupMenu = new Menu ();
			ImageMenuItem menuItemQuit = new ImageMenuItem ("Quit");
			
			ImageMenuItem menuItemEnabled;
			if(Preferences.Enabled)
			{
				menuItemEnabled = new ImageMenuItem ("Disable");
				menuItemEnabled.Image = new Gtk.Image (Gtk.Stock.Yes, IconSize.Menu);
			}
			else
			{
				// hopefully prevent any major mem leaks
				
				menuItemEnabled = new ImageMenuItem ("Enable");				
				menuItemEnabled.Image = new Gtk.Image(Gtk.Stock.Yes, IconSize.Menu);
			}
			
			menuItemQuit.Image = new Gtk.Image (Gtk.Stock.Quit, IconSize.Menu);
			popupMenu.Add(menuItemQuit);
			popupMenu.Add(menuItemEnabled);
			
			menuItemQuit.Activated += delegate(object sender, EventArgs e) {
				Application.Quit();				
			};
			menuItemEnabled.Activated += HandleMenuItemEnabledActivated;
			
			popupMenu.ShowAll();
			popupMenu.Popup();			
		}

		static void HandleMenuItemEnabledActivated (object sender, EventArgs e)
		{
			if(Preferences.Enabled)
			{
				ScrotDaemon.Instance.Deactivate();
			}
			else
				ScrotDaemon.Instance.Activate();
		}
	}
}
