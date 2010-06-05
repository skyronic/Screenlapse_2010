
using System;

namespace ScreenLapse
{

	public partial class PreferencesDialog : Gtk.Dialog
	{
		
		public PreferencesDialog ()
		{
			this.Build ();
			
			// Load the settings into the GUI
			filechooserbutton2.CurrentFolder = Preferences.SavePath;
			spinbutton1.Value = Preferences.Interval;
			spinbutton2.Value = Preferences.ScalePercentage;
			spinbutton3.Value = Preferences.PlaybackDelay;
		}
		protected virtual void ClosePref (object sender, System.EventArgs e)
		{
			this.Hide();
			this.Dispose();
		}
		
		protected virtual void SavePrefAndClose (object sender, System.EventArgs e)
		{
			Preferences.SavePath = filechooserbutton2.CurrentFolder;
			Preferences.Interval = spinbutton1.Value;
			Preferences.ScalePercentage = spinbutton2.Value;
			Preferences.PlaybackDelay = spinbutton3.Value;
			
			Preferences.WriteToGConf();
			this.Hide();
			this.Dispose();
		}
	}
}
