using System;
using System.Diagnostics;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;

namespace TidshanteringDyskalkyli.Pages
{
    public class TimeEstimatorPage : ContentPage
    {
        private TimePicker _endTimePicker;
        private Picker _hourDurationPicker;
        private Picker _minuteDurationPicker;
        private TimePicker _timePicker;
        private Label _totalTimeLabel;

        public TimeEstimatorPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            Debug.WriteLine(DateTime.Now.TimeOfDay);
            for (var i = 0; i < 25; i++)
            {
                HourDurationPicker.Items.Add(i + " Timmar");
            }

            for (var i = 0; i < 61; i++)
            {
                MinuteDurationPicker.Items.Add(i + " Minuter");
            }


            StartTimePicker.Unfocused += (sender, args) =>
            {
                UpdateTotalTime();
            };

            HourDurationPicker.Unfocused += (sender, args) =>
            {
                UpdateTotalTime();
            };

            MinuteDurationPicker.Unfocused += (sender, args) =>
            {
                UpdateTotalTime();
            };

            EndTimePicker.PropertyChanged += (sender, args) =>
            {
                UpdateTotalTime();
            };

            Padding = new Thickness(0, 20, 0, 0);

            Content = new StackLayout
            {
                Children =
                {
                    StartTimeLayout,
                    DurationTimePickerStackLayout,
                    TotalTimeLabelStackLayout,
                    ResetButton
                },
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = Resolver.Resolve<IDevice>().Display.Width
            };

            BackgroundColor = Colors.SoftGray;

            Title = "Tidsestimering";
            Icon = "128/resizedimagepie.png";
        }

        public TimePicker EndTimePicker
        {
            get
            {
                return _endTimePicker ?? (_endTimePicker = new TimePicker
                {
                    //Time = DateTime.Now.TimeOfDay,
                    Format = "HH:mm",
                    HorizontalOptions = LayoutOptions.StartAndExpand
                });
            }
            set { _endTimePicker = value; }
        }

        public Picker HourDurationPicker
        {
            get
            {
                return _hourDurationPicker ?? (_hourDurationPicker = new Picker
                {
                    Title = "Timmar",
                    WidthRequest = 100,
                });
            }
            set { _hourDurationPicker = value; }
        }

        public Picker MinuteDurationPicker
        {
            get
            {
                return _minuteDurationPicker ?? (_minuteDurationPicker = new Picker
                {
                    Title = "Minuter",
                    WidthRequest = 100,
                });
            }
            set { _minuteDurationPicker = value; }
        }

        public TimePicker StartTimePicker
        {
            get
            {
                return _timePicker ?? (_timePicker = new TimePicker
                {
                    //Time = DateTime.Now.TimeOfDay,
                    Format = "HH:mm",
                    HorizontalOptions = LayoutOptions.StartAndExpand
                });
            }
            set { _timePicker = value; }
        }

        public StackLayout TotalTimeLabelStackLayout
        {
            get
            {
                return new StackLayout
                {
                    Children =
                    {
                        new Label
                        {
                            Text = "Slut tid: ",
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center
                        },
                        EndTimePicker
                    },
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Spacing = 5,
                    Padding = new Thickness(5, 5, 5, 5)
                };
            }
        }

        public StackLayout DurationTimePickerStackLayout
        {
            get
            {
                return new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {
                        HourDurationPicker,
                        MinuteDurationPicker
                    },
                    Spacing = 10,
                    HorizontalOptions = LayoutOptions.Center
                };
            }
        }

        public Button ShowClockButton
        {
            get
            {
                return new Button
                {
                    Text = "Klockur",
                    Image = "128/resizedimage.png",
                    HorizontalOptions = LayoutOptions.Center,
                    //MinimumWidthRequest = 200,
                    WidthRequest = 100
                };
            }
        }

        public Button ResetButton
        {
            get
            {
                return new Button()
                {
                    Text = "Reset",
                    Command = new Command(() =>
                    {


                        HourDurationPicker.SelectedIndex = -1;
                        MinuteDurationPicker.SelectedIndex = -1;
                        StartTimePicker.Time = TimeSpan.Zero;
                        EndTimePicker.Time = TimeSpan.Zero;

                        HourDurationPicker.IsEnabled = true;
                        MinuteDurationPicker.IsEnabled = true;
                        EndTimePicker.IsEnabled = true;
                        StartTimePicker.IsEnabled = true;

                        HourDurationPicker.BackgroundColor = Color.White;
                        MinuteDurationPicker.BackgroundColor = Color.White;
                        StartTimePicker.BackgroundColor = Color.White;
                        EndTimePicker.BackgroundColor = Color.White;
                    })
                };
            }
        }

        public Layout StartTimeLayout
        {
            get
            {
                return new StackLayout
                {
                    Children =
                    {
                        new Label
                        {
                            Text = "Start tid: ",
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center
                        },
                        StartTimePicker
                    },
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Spacing = 5,
                    Padding = new Thickness(5, 5, 5, 5)
                };
            }
        }

        public Label TotalTimeLabel
        {
            get
            {
                return _totalTimeLabel ?? (_totalTimeLabel = new Label
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                });
            }
            set { _totalTimeLabel = value; }
        }

        private TimeSpan CalculateEndDateTime()
        {
            var totalTime = StartTimePicker.Time;


            if (HourDurationPicker.SelectedIndex != -1)
            {
                var hourTimeSpan = new TimeSpan(0, HourDurationPicker.SelectedIndex, 0, 0);
                totalTime = totalTime.Add(hourTimeSpan);
            }
            if (MinuteDurationPicker.SelectedIndex != -1)
            {
                var minuteTimeSpan = new TimeSpan(0, 0, MinuteDurationPicker.SelectedIndex, 0);
                totalTime = totalTime.Add(minuteTimeSpan);
            }
            return totalTime;
        }

        private TimeSpan CalculateTimeBetweenTime()
        {
            var startTime = StartTimePicker.Time;

            if (HourDurationPicker.SelectedIndex != -1)
            {
                HourDurationPicker.SelectedIndex = -1;
            }
            if (MinuteDurationPicker.SelectedIndex != -1)
            {
                MinuteDurationPicker.SelectedIndex = -1;
            }

            var endTime = EndTimePicker.Time;

            if (startTime > endTime)
            {

                var totalTime2 = endTime - startTime;
                Debug.WriteLine(totalTime2.ToString());
                startTime = startTime.Add(new TimeSpan(1, 0, 0, 0));
                //throw new Exception("illegal time exception");
            }


            var totalTime = startTime.Subtract(endTime);

            totalTime = totalTime.Duration();
            return totalTime;
        }

        private TimeSpan CalculateStartTime()
        {
            var endTime = EndTimePicker.Time;

            if (HourDurationPicker.SelectedIndex != -1)
            {
                var hourTimeSpan = new TimeSpan(0, HourDurationPicker.SelectedIndex, 0, 0);
                endTime = endTime.Subtract(hourTimeSpan);
            }
            if (MinuteDurationPicker.SelectedIndex != -1)
            {
                var minuteTimeSpan = new TimeSpan(0, 0, MinuteDurationPicker.SelectedIndex, 0);
                endTime = endTime.Subtract(minuteTimeSpan);
            }
            var startTime = endTime;

            return startTime;
        }

        private void UpdateFinalTimePicker()
        {
            var timeEstimate = CalculateEndDateTime();
            EndTimePicker.Time = timeEstimate;
            //EndTimePicker.IsEnabled = false;

            //    TotalTimeLabel.Text = "Slut tid: " +
            //                          timeEstimate.ToString("hh") + ":" + timeEstimate.ToString("mm");
        }

        private void UpdateTimeBetweenPickers()
        {
            var timeEstimate = CalculateTimeBetweenTime();
            HourDurationPicker.SelectedIndex = int.Parse(timeEstimate.ToString("hh"));
            MinuteDurationPicker.SelectedIndex = int.Parse(timeEstimate.ToString("mm"));
        }

        private void UpdateStartTime()
        {
            var timeEstimate = CalculateStartTime();
            StartTimePicker.Time = timeEstimate;
        }

        private void UpdateTotalTime()
        {
            if (StartTimePicker.Time != TimeSpan.Zero && HourDurationPicker.SelectedIndex != -1 && MinuteDurationPicker.SelectedIndex != -1 && StartTimePicker.IsEnabled && HourDurationPicker.IsEnabled && MinuteDurationPicker.IsEnabled)
            {
                EndTimePicker.IsEnabled = false;
                StartTimePicker.IsEnabled = false;
                HourDurationPicker.IsEnabled = false;
                MinuteDurationPicker.IsEnabled = false;
                UpdateFinalTimePicker();
                HourDurationPicker.BackgroundColor = Color.Silver;
                MinuteDurationPicker.BackgroundColor = Color.Silver;
                StartTimePicker.BackgroundColor = Color.Silver;
                EndTimePicker.BackgroundColor = Color.Silver;
            }
            else if (StartTimePicker.Time != TimeSpan.Zero && EndTimePicker.Time != TimeSpan.Zero && StartTimePicker.IsEnabled && EndTimePicker.IsEnabled)
            {
                HourDurationPicker.IsEnabled = false;
                MinuteDurationPicker.IsEnabled = false;
                EndTimePicker.IsEnabled = false;
                StartTimePicker.IsEnabled = false;
                UpdateTimeBetweenPickers();
                HourDurationPicker.BackgroundColor = Color.Silver;
                MinuteDurationPicker.BackgroundColor = Color.Silver;
                StartTimePicker.BackgroundColor = Color.Silver;
                EndTimePicker.BackgroundColor = Color.Silver;
            }
            else if ((HourDurationPicker.SelectedIndex != -1 && MinuteDurationPicker.SelectedIndex != -1) && EndTimePicker.Time != TimeSpan.Zero && HourDurationPicker.IsEnabled && MinuteDurationPicker.IsEnabled && EndTimePicker.IsEnabled)
            {
                HourDurationPicker.IsEnabled = false;
                MinuteDurationPicker.IsEnabled = false;
                EndTimePicker.IsEnabled = false;
                StartTimePicker.IsEnabled = false;
                UpdateStartTime();
                HourDurationPicker.BackgroundColor = Color.Silver;
                MinuteDurationPicker.BackgroundColor = Color.Silver;
                StartTimePicker.BackgroundColor = Color.Silver;
                EndTimePicker.BackgroundColor = Color.Silver;
            }

            
        }
    }
}