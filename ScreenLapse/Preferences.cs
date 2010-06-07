// 
// Preferences.cs
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
using GConf;


namespace ScreenLapse
{

	public static class Preferences
	{

		static Client client;

		static string appName = "/apps/screenlapse/";

		static string intervalKey = appName + "interval";
		public static int Interval { get; set; }

		static string scaleKey = appName + "scale";
		public static int ScalePercentage { get; set; }

		static string playbackDelayKey = appName + "playbackDelay";
		public static int PlaybackDelay { get; set; }

		static string savePathKey = appName + "savePath";
		public static string SavePath { get; set; }

		public static bool Enabled { get; set; }

		public static void ReadFromGConf ()
		{
			try {
				Interval = (int)client.Get (intervalKey);
				ScalePercentage = (int)client.Get (scaleKey);
				PlaybackDelay = (int)client.Get (playbackDelayKey);
				SavePath = (string)client.Get (savePathKey);
				
				return;
			} catch (GConf.NoSuchKeyException e) {
				Log.Error("Error: A key with that name doesn't exist.");
			} catch (System.InvalidCastException e) {
				Log.Error("Error: Cannot typecast.");
			} catch (Exception ex) {
				Log.Error("Some other error - " + ex.Message);
			}
				Enabled = false;
				ScalePercentage = 50;
				Interval = 5000;
				PlaybackDelay = 500;
			SavePath = "/tmp";
			WriteToGConf();
		}
		
		public static void WriteToGConf()
		{
			try {
				
				client.Set(intervalKey, Interval);
				client.Set(scaleKey, ScalePercentage);
				client.Set(playbackDelayKey, PlaybackDelay);
				client.Set(savePathKey, SavePath);
			} catch (Exception ex) {
				Log.Error("Error in writeToConf" + ex.Message);
			}
		}

		public static void Initialize ()
		{
			// Initialize the gconf listener.
			client = new Client ();
			
			ReadFromGConf();
		}
	}
}
