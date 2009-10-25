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


namespace ScreenLapse
{

	public class ScrotDaemon
	{
		
		static readonly ScrotDaemon instance = new ScrotDaemon();
		
		private System.Timers.Timer timer;
		
		public ScrotDaemon Instance
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
		}
		
		/// <summary>
		/// Deactivate the screenshot timer
		/// </summary>
		public void Deactivate()
		{
			
		}

		private ScrotDaemon ()
		{			
			timer = new System.Timers.Timer();
			timer.Enabled = false;
			timer.Elapsed += OnTimerTick;
		}

		void OnTimerTick (object sender, Timers.ElapsedEventArgs e)
		{
			
		}
	}
}
