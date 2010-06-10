using System;
using Gtk;
namespace ScreenLapse
{
public partial class MainWindow : Gtk.Window
{
	public MainWindow () : base(Gtk.WindowType.Toplevel)
	{
		Build ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		this.Visible = false;
	}
	
	// Playback
	protected virtual void OnButton55Clicked (object sender, System.EventArgs e)
	{
		ScrotViewer playBackWindow = new ScrotViewer();
			playBackWindow.ShowAll();
	}
	
	protected virtual void ShowPreferencesWindow (object sender, System.EventArgs e)
	{
			PreferencesDialog prefWindow = new PreferencesDialog();
			prefWindow.ShowAll();
	}
	
	protected virtual void StartRecordingButton (object sender, System.EventArgs e)
	{
			ScrotDaemon.Instance.Activate();
			this.HideAll();
	}
	
	
}
}