
using System;

namespace ScreenLapse
{

	public partial class PreferencesDialog : Gtk.Dialog
	{
		
		public PreferencesDialog ()
		{
			this.Build ();
			
			// Load the settings into the GUI
			filechooserbutton2.SetCurrentFolder(Preferences.SavePath);
			spinbutton1.Value = Preferences.Interval;
			spinbutton2.Value = Preferences.ScalePercentage;
			spinbutton3.Value = Preferences.PlaybackDelay;
		}
		protected virtual void ClosePref (object sender, System.EventArgs e)
		{
			Console.WriteLine ("Closing");
			this.Hide();
			this.Destroy();
		}
		
		protected virtual void SavePrefAndClose (object sender, System.EventArgs e)
		{
			Console.WriteLine ("Saving preferences");
			Preferences.SavePath = filechooserbutton2.CurrentFolder;
			Preferences.Interval = (int)spinbutton1.Value;
			Preferences.ScalePercentage = (int)spinbutton2.Value;
			Preferences.PlaybackDelay = (int)spinbutton3.Value;
			
			Preferences.WriteToGConf();
			this.Hide();
			this.Destroy();
		}
	}
}
