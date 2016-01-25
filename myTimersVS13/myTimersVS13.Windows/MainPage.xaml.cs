using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace myTimersVS13
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        bool bIsStopwatchRunning;

        public MainPage()
        {
            this.InitializeComponent();
            this.SizeChanged += MainPage_SizeChanged;
        }

        void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // run this when the size of the window changes
            // change state when applicable
            // this is just a general example and would need further work.
            if( ApplicationView.GetForCurrentView().IsFullScreen )
            {
                VisualStateManager.GoToState(this, "DefaultScreen", false);
            }
            else
            {
                if( (Window.Current.Bounds.Width <= 500) && 
                    (bIsStopwatchRunning == true))
                {
                    VisualStateManager.GoToState(this, "StopWatchRunningState", true);
                }
                else if(Window.Current.Bounds.Width <= 500)
                {
                    VisualStateManager.GoToState(this, "NarrowScreen", false);
                }

            }
        }

        DispatcherTimer myStopwatchTimer;
        Stopwatch stopWatch;
        private long ms, ss, mm, hh, dd, lastLapTime;
        List<long> myLapTimes;

        private void btnLapReset_Click(object sender, RoutedEventArgs e)
        {
            long timeRightNow;
            TextBlock tblLapTime;

            if (btnLapReset.Content.ToString() == "Reset")
            {
                // rezero all timers
                stopWatch.Reset();
                dd = hh = mm = ss = ms = 0;
                tblTimeDisplay.Text = "00:00:00:00:000";

            }
            else     // Text = "Lap"
            {
                // save the current time, add to list
                if (myLapTimes == null)
                {
                    myLapTimes = new List<long>();
                    lastLapTime = 0;
                }
                // get the ellapsed milliseconde
                // subtract the last one and then store the difference
                timeRightNow = stopWatch.ElapsedMilliseconds;
                myLapTimes.Add(timeRightNow - lastLapTime);
                myLapTimes.Reverse();
                lastLapTime = timeRightNow;

                tblLapTime = new TextBlock();
                tblLapTime.Text = myLapTimes.Last().ToString();
                tblLapTime.HorizontalAlignment = HorizontalAlignment.Center;

                spLapTimes.Children.Add(tblLapTime);
                
            }
        }
         
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if( stopWatch == null)
            {
                stopWatch = new Stopwatch();
            }
            // check for the timer and then set up.
            if( myStopwatchTimer == null)
            {
                ms = ss = mm = hh = dd = 0;
                myStopwatchTimer = new DispatcherTimer();
                myStopwatchTimer.Tick += MyStopwatchTimer_Tick;
                myStopwatchTimer.Interval = new TimeSpan(0, 0, 0, 0, 1); // 1 millisecond
            }
            base.OnNavigatedTo(e);
        }
        
        private void MyStopwatchTimer_Tick(object sender, object e)
        {
            // update the textblock with the time elapsed
            // figure out the elapsed time using the timer properties
            // some maths division and modulus

            ms = stopWatch.ElapsedMilliseconds;

            tblTimeDisplay.Text = convertMsToString(ms);

        }

        private string convertMsToString(long milliSeconds)
        {
            ss = milliSeconds / 1000;
            milliSeconds = milliSeconds % 1000;

            mm = ss / 60;
            ss = ss % 60;

            hh = mm % 60;
            mm = mm % 60;

            dd = hh / 24;
            hh = hh % 24;

            return( hh.ToString("00") + ":" +
                                   mm.ToString("00") + ":" +
                                   ss.ToString("00") + ":" +
                                   milliSeconds.ToString("000"));

        }

        private void btnStartStop_Click(object sender, RoutedEventArgs e)
        {
            // start the stop watch
            // kick off a timer.
            if( btnStartStop.Content.ToString() == "Start" )
            {
                // start the timer, change the text
                myStopwatchTimer.Start();
                stopWatch.Start();
                btnLapReset.Content = "Lap";
                btnStartStop.Content = "Stop";
                btnStartStop.Background = new SolidColorBrush(Colors.Red);
                bIsStopwatchRunning = true;
            }
            else   //equal to stop
            {
                myStopwatchTimer.Stop();
                stopWatch.Stop();
                btnLapReset.Content = "Reset";
                btnStartStop.Content = "Start";
                btnStartStop.Background = new SolidColorBrush(Colors.Green);
                bIsStopwatchRunning = false;
            }
        }

    
    }
}

