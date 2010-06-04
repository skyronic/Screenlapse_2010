// 
// ScrotViewer.cs
//  
// Author:
//       Anirudh Sanjeev <anirudh@anirudhsanjeev.org>
// 
// Copyright (c) 2009 Anirudh Sanjeev
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.IO;
using Gtk;
using System.Collections.Generic;
using Gdk;

namespace ScreenLapse
{

	public partial class ScrotViewer : Gtk.Window
	{
		public ScrotViewer () : base(Gtk.WindowType.Toplevel)
		{
			// Initialize members
			
			validDirectories = new List<string> ();
			this.Build ();
			// Build the treeview
			TreeViewColumn dayColumn = new TreeViewColumn ();
			dayColumn.Title = "Day";
			CellRendererText textRenderer = new CellRendererText ();
			dayColumn.PackStart (textRenderer, true);
			dayColumn.AddAttribute (textRenderer, "text", 0);
			
			dayListStore = new ListStore (typeof(string));
			
			dayAvailTreeView.AppendColumn (dayColumn);
			dayAvailTreeView.Model = dayListStore;
			
			string tempFileName = "11-26-2009/011307.png";
			if (File.Exists (tempFileName)) {
				Console.WriteLine ("File exists");
				int height = scrotDisplayArea.HeightRequest;
				int width = scrotDisplayArea.WidthRequest;
				Console.WriteLine ("{0}x{1}", height, width);
				
				
				
				// The file
				Gdk.Pixbuf displayPic = new Gdk.Pixbuf (tempFileName, width, height, true);
				// Preserve the aspect ratio
				scrotDisplayArea.Pixbuf = displayPic;
				
			}
			this.ResizeMode = ResizeMode.Immediate;
			ExtractDayPaths ();
			
			// handle tree activate
			dayAvailTreeView.RowActivated += DayAvailRowActivated;
			
			// handle the time slider
			timeSlider.ValueChanged += RedrawImage;
			ImageArmed = false;
		}
		bool ImageArmed;

		/// <summary>
		/// Callback from the time slider widget. Redraws the appropriate image on the screen
		/// 
		/// Also will make playback easier, because I can just increment the value on the
		/// timeslider and the object will change
		/// </summary>
		/// <param name="sender">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <param name="e">
		/// A <see cref="EventArgs"/>
		/// </param>
		void RedrawImage (object sender, EventArgs e)
		{
			if (ImageArmed) {
				// get the index of the slider
				int index = (int)timeSlider.Value;
				
				// check if the currentfilenames has an index which corresponds to this
				if (currentFileNames == null)
					return;
				if (currentFileNames.Count < index)
					return;
				
				try {
					string filename = currentFileNames[index];
					if (File.Exists (filename)) {
						
						int height, width;
						scrotDisplayArea.GetSizeRequest (out width, out height);
						// make sure we store a reference to the old image so we can dispose it after
						// the image has changed, rather than relying on the GC to come, which can result in
						// memory leaks
						Pixbuf oldImage = null;
						if (scrotDisplayArea.Pixbuf != null) {
							oldImage = scrotDisplayArea.Pixbuf;
						}
						Console.WriteLine ("The h, w are : {0}, {1}", height, width);
						string fullpath = System.IO.Path.GetFullPath (filename);
						Console.WriteLine ("displaying {0}", fullpath);
						// The file
						Gdk.Pixbuf displayPic = new Gdk.Pixbuf (fullpath);
						// Preserve the aspect ratio
						scrotDisplayArea.Pixbuf = displayPic;
						
						//if(oldImage != null)
						//	oldImage.Dispose();
					}
				} catch (Exception ex) {
					Console.WriteLine ("Something broke with exception: " + ex.Message);
					return;
				}
			}
		}

		List<string> currentFileNames;

		void DayAvailRowActivated (object o, RowActivatedArgs args)
		{
			// find the current day path			
			if (currentFileNames != null) {
				currentFileNames.Clear ();
			} else {
				currentFileNames = new List<string> ();
			}
			currentDayPath = validDirectories[args.Path.Indices[0]];
			
			// Iterate over all the files in currentDayPath
			foreach (string filename in Directory.GetFiles (currentDayPath)) {
				Console.WriteLine (filename);
				if (System.IO.Path.GetExtension (filename) == ".png" && System.IO.Path.GetFileNameWithoutExtension (filename).Length == 6) {
					currentFileNames.Add (filename);
				}
			}
			Console.WriteLine ("The count of currentfilenames is: " + currentFileNames.Count.ToString ());
			// set up the slider
			if (currentFileNames.Count != 0) {
				timeSlider.SetRange (0, currentFileNames.Count);
				timeSlider.Value = 1;
				
				ImageArmed = true;
			} else {
				timeSlider.SetRange (0, 1);
				ImageArmed = false;
				timeSlider.Value = 0;
			}
			
		}

		ListStore dayListStore;

		List<string> validDirectories;
		string currentDayPath;

		/// <summary>
		/// Since each day is stored as a different path, this function extracts
		/// all the existing day paths in the applicatoins running path
		/// </summary>
		public void ExtractDayPaths ()
		{
			foreach (string dir in Directory.GetDirectories (".")) {
				int x;
				// TODO: Should we do this by Regexes?
				// our format uses 12 characters
				if (dir.Length == 12) {
					Console.WriteLine ("Directory: " + dir);
					string dirTrimmed = dir.TrimStart ("./".ToCharArray ());
					DateTime dirDate;
					Console.WriteLine ("The final dir is:" + dirTrimmed);
					try {
						dirDate = DateTime.Parse (dirTrimmed);
					} catch {
						Console.WriteLine ("Datetime parsing failed");
						continue;
						// Don't process ahead
					}
					Console.WriteLine ("The date is {0}", dirDate.ToString ());
					dayListStore.AppendValues (dirDate.ToShortDateString ());
					
					validDirectories.Add (dir);
				}
			}
		}
	}
	
}
