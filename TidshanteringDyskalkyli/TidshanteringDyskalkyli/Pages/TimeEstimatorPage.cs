using System;
using System.Diagnostics;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;

namespace TidshanteringDyskalkyli.Pages
{
    public class TimeEstimatorPage : ContentPage
    {


        public StackLayout TotalTimeLabelStackLayout
        {
            get
            {
                return new StackLayout
                {
                    Children =
                    {
                        TotalTimeLabel,
                        ShowClockButton
                    },
                    Padding = new Thickness(5, 5, 5, 5),
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Spacing = 10,
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

        private TimeSpan CalculateTotalDateTime()
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

        private TimePicker _timePicker;

        public TimePicker StartTimePicker
        {
            get
            {
                return _timePicker ?? (_timePicker = new TimePicker
                {
                    Time = DateTime.Now.TimeOfDay,
                    Format = "HH:mm",
                    HorizontalOptions = LayoutOptions.StartAndExpand
                });
            }
            set { _timePicker = value; }
        }

        private Picker _hourDurationPicker;

        public Picker HourDurationPicker
        {
            get
            {
                return _hourDurationPicker ?? (_hourDurationPicker = new Picker
                {
                    Title = "0 Timmar "
                });
            }
            set { _hourDurationPicker = value; }
        }

        private Picker _minuteDurationPicker;

        public Picker MinuteDurationPicker
        {
            get
            {
                return _minuteDurationPicker ?? (_minuteDurationPicker = new Picker
                {
                    Title = "0 Minuter "
                });
            }
            set { _minuteDurationPicker = value; }
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

        private Label _totalTimeLabel;

        public Label TotalTimeLabel
        {
            get
            {
                return _totalTimeLabel ?? (_totalTimeLabel = new Label()
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,    
                });
            }
            set { _totalTimeLabel = value; }
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
                    Padding = new Thickness(5, 5, 5, 5),
                };
            }
        }

        private void UpdateTimeLabel()
        {
            var timeEstimate = CalculateTotalDateTime();
            TotalTimeLabel.Text = "Slut tid: " +
                                  timeEstimate.ToString("hh") + ":" + timeEstimate.ToString("mm");
        }

        public TimeEstimatorPage()
        {
            Debug.WriteLine(DateTime.Now.TimeOfDay);
            for (var i = 0; i < 25; i++)
            {
                HourDurationPicker.Items.Add(i + " Timmar");
            }

            for (var i = 0; i < 61; i++)
            {
                MinuteDurationPicker.Items.Add(i + " Minuter");
            }


            StartTimePicker.PropertyChanged += (sender, args) => { UpdateTimeLabel(); };

            HourDurationPicker.PropertyChanged += (sender, args) => { UpdateTimeLabel(); };

            MinuteDurationPicker.PropertyChanged += (sender, args) => { UpdateTimeLabel(); };

            Padding = new Thickness(0, 20, 0, 0);

            Content = new StackLayout
            {
                Children =
                {
                    StartTimeLayout,
                    DurationTimePickerStackLayout,
                    TotalTimeLabelStackLayout
                },
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = Resolver.Resolve<IDevice>().Display.Width
            };

            BackgroundColor = Colors.SoftGray;

            Title = "Tidsestimering";
            Icon = "128/resizedimagepie.png";

        }
    }
}
