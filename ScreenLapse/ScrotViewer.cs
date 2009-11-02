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

namespace ScreenLapse
{

	public partial class ScrotViewer : Gtk.Window
	{

		public ScrotViewer () : base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
			// Build the treeview
			TreeViewColumn dayColumn = new TreeViewColumn();
			dayColumn.Title = "Day";
			dayColumn.PackStart(new CellRendererText(), true);
			
			dayListStore = new ListStore(typeof(string));
			
			dayAvailTreeView.AppendColumn(dayColumn);
			dayAvailTreeView.Model = dayListStore;
			
			ExtractDayPaths();			
		}
		
		ListStore dayListStore;
		
		/// <summary>
		/// Since each day is stored as a different path, this function extracts
		/// all the existing day paths in the applicatoins running path
		/// </summary>
		private void ExtractDayPaths()
		{
			foreach(string dir in Directory.GetDirectories("."))
			{
				Console.WriteLine("Directory: " + dir);
				// TODO: Should we do this by Regexes?
				if ( dir.Length == 8 ) // our format uses 8 characters
				{
					DateTime dirDate;
					try
					{
						
						dirDate = DateTime.Parse(dir);
					}
					catch
					{
						continue; // Don't process ahead
					}
					
					dayListStore.AppendValues(dirDate.ToShortDateString());
				}
			}
		}
	}
}