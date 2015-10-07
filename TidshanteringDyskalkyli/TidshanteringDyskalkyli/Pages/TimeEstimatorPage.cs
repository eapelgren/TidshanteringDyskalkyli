using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace TidshanteringDyskalkyli.Pages
{
    public class TimeEstimatorPage : ContentPage
    {
        private Label _totalTimeLabel;

        public Label TotalTimeLabel
        {
            get { return _totalTimeLabel ?? (_totalTimeLabel = new Label()); }
            set { _totalTimeLabel = value; }
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
                var minuteTimeSpan = new TimeSpan(0,0,MinuteDurationPicker.SelectedIndex,0);
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
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    
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
                    Title = "Timmar"
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
                    Title = "Minuter"
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
                    Spacing = 10
                };
            }
        }


        public TimeEstimatorPage()
        {
            Debug.WriteLine(DateTime.Now.TimeOfDay);
            for (var i = 0; i < 25; i++)
            {
                HourDurationPicker.Items.Add(i.ToString() + " Timmar");
            }

            for (var i = 1; i < 61; i++)
            {
                MinuteDurationPicker.Items.Add(i.ToString() + " Minuter");
            }


            StartTimePicker.PropertyChanged += (sender, args) => { UpdateTimeLabel(); };

            HourDurationPicker.PropertyChanged += (sender, args) => { UpdateTimeLabel(); };

            MinuteDurationPicker.PropertyChanged += (sender, args) => { UpdateTimeLabel(); };

            Padding = new Thickness(0, 20, 0, 0);

            Content = new StackLayout
            {
                Children =
                {
                    StartTimePicker,
                    DurationTimePickerStackLayout,
                    TotalTimeLabel

                }
            };
        }

        private void UpdateTimeLabel()
        {
            var timeEstimate = CalculateTotalDateTime();
            TotalTimeLabel.Text = "Slut tid: " +
                                  timeEstimate.ToString("hh") + ":" +timeEstimate.ToString("mm");
        }
    }
}
