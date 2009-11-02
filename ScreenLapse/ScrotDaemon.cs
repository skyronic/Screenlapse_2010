// 
// ScrotDaemon.cs
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
using Gnome;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using Gtk;

namespace ScreenLapse
{

	public class ScrotDaemon
	{
		
		static readonly ScrotDaemon instance = new ScrotDaemon();
		
		private System.Timers.Timer timer;
		
		public static ScrotDaemon Instance
		{
			get { return instance; }
		}
		
		
		/// <summary>
		/// Property to check whether screenshots can be taken
		/// 
		/// use Activate() and Deactivate() to modify behaviour
		/// </summary>
		public bool IsActive
		{
			get; private set;
		}
		
		/// <summary>
		/// Activate the screenshot timer
		/// </summary>
		public void Activate()
		{
			IsActive = true;
			timer.Enabled = true;			
		}
		
		/// <summary>
		/// Deactivate the screenshot timer
		/// </summary>
		public void Deactivate()
		{
			IsActive = false;
			timer.Enabled = false;
		}

		private ScrotDaemon ()
		{			
			timer = new System.Timers.Timer();
			timer.Enabled = false;
			timer.Elapsed += OnTimerTick;
			
			timer.Interval = Preferences.Interval * 1000;
			
			GC.KeepAlive(timer); // Prevent the GC from collecting this.
		}

		void OnTimerTick (object sender, System.Timers.ElapsedEventArgs e)
		{
			// Set the directory name and file name as [MMDDYYYY/HHMMSS.png]
			string specifier = "D2"; // the format of the integer to be printed out
			
			//string dirName = String.Format("{0}{1}{2}", e.SignalTime.Date.Month.ToString(specifier), e.SignalTime.Date.Day.ToString(specifier), e.SignalTime.Date.Year.ToString(specifier));
			//string fileName = String.Format("{0}{1}{2}.png", e.SignalTime.Hour.ToString(specifier), e.SignalTime.Minute.ToString(specifier), e.SignalTime.Second.ToString(specifier));
			
			string dirName = e.SignalTime.ToString("MMddYYYY");
			string fileName = e.SignalTime.ToString("hhmmss");
			Console.WriteLine("Timer ticked");
			
			// create directory if it doesn't exist
			if(!Directory.Exists(dirName))
			{
				try
				{
					Directory.CreateDirectory(dirName);
				}
				catch
				{
					// handle error
				}
			}
			string filePath = Path.Combine(dirName, fileName);
			
			if(File.Exists(filePath))
			{
				try
				{
					File.Delete(filePath);
				}
				catch
				{
					
				}
			}
			
			Console.WriteLine(filePath);
			
			// Take the screenshot			
			int screenWidth = Gdk.Screen.Default.Width;
			int screenHeight = Gdk.Screen.Default.Height;
			
			Console.WriteLine("Scroting for {0}x{1}", screenWidth, screenHeight);
			
			Bitmap bmpScreenShot = new Bitmap (screenWidth, screenHeight);
			Graphics gfx = Graphics.FromImage ((System.Drawing.Image)bmpScreenShot);
			gfx.CopyFromScreen (0, 0, 0, 0, new Size (screenWidth, screenHeight));
			
			// Create a thumbnailed version of the screenshot
			Preferences.ScalePercentage = 80;
			int thumbWidth = (int)((double)screenWidth * ((double)Preferences.ScalePercentage / (double)100));
			int thumbHeight = (int)((double)screenHeight * ((double)Preferences.ScalePercentage / (double)100));
			
			Console.WriteLine("Scroting for {0}x{1}", thumbWidth, thumbHeight);
			System.Drawing.Image thumb = bmpScreenShot.GetThumbnailImage(thumbWidth, thumbHeight, null, IntPtr.Zero);
			thumb.Save (filePath, ImageFormat.Png);
			
			// Clean up
			bmpScreenShot.Dispose();
			gfx.Dispose();
			thumb.Dispose();
		}
	}
}
