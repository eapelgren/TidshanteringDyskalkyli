using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using XLabs;

namespace TidshanteringDyskalkyli.Pages
{
    internal class ScrollTranslatePage : ContentPage
    {
        private Picker _hourScroll;
        private Button _showClockButton;
        private Picker _halfScroll;
        private Label _calculatedTimeLabel;
        private Picker _amPmPicker;
        private Button _calculateButton;
        private Picker _minuteScroll;
        private Picker _pastPresentScroll;


        public ScrollTranslatePage()
        {

            Title = "Skapare";
            Icon = "128/resizedimageullist.png";

            Content = new StackLayout
            {
                Children =
                {
                    ScrollerStackLayout,
                    AmpmStackLayout,
                    TotalTimeStackLayout
                },
                Orientation = StackOrientation.Vertical,
                Spacing = 10,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,  
                BackgroundColor = Colors.SoftGray
            };

            BackgroundColor = Colors.SoftGray;

            MinutesScroll.Unfocused += (sender, args) => { CalculateTime(); };

            HourScroll.Unfocused += (sender, args) => { CalculateTime(); };

            PastPresentScroll.Unfocused += (sender, args) => { CalculateTime(); };

            AmPmPicker.Unfocused += (sender, args) => { CalculateTime(); };
        }

        private void CalculateTime()
        {
            string minute = null;
            string hour = null;
            string preposition = null;
            string amPm = null;

            var times = new string[4];


            try
            {

                if (MinutesScroll.SelectedIndex != -1)
                {
                minute = MinutesScroll.Items[MinutesScroll.SelectedIndex];
                    times[0] = minute;
                }

                if (HourScroll.SelectedIndex != -1)
                {
                hour = HourScroll.Items[HourScroll.SelectedIndex];
                    times[2] = hour;
                }

                if (PastPresentScroll.SelectedIndex != -1)
                {
                preposition = PastPresentScroll.Items[PastPresentScroll.SelectedIndex];
                    times[1] = preposition;
                }

                if (AmPmPicker.SelectedIndex != -1)
                {
                amPm = AmPmPicker.Items[AmPmPicker.SelectedIndex];
                    times[3] = amPm;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            //CHECK IF HALV
            PastPresentScroll.IsEnabled = true;
            PastPresentScroll.BackgroundColor = Color.White;
            if (minute != null && minute.ToLower().Equals("halv"))
            {
                PastPresentScroll.IsEnabled = false;
                PastPresentScroll.BackgroundColor = Color.Silver;
                PastPresentScroll.SelectedIndex = -1;
                times[1] = "";
            }

            if (hour == null)
            {
                return;
            }


            try
            {
                string totalTime = "";
                for (int i = 0; i < times.Count() - 1; i++)
                {
                    if (times[i] != null)
                    {
                        totalTime += times[i];
                        //Dont add an empty space on last number
                        if (i != times.Count() - 2)
                        totalTime += " ";
                    }    
                }
                bool am = !amPm.ToLower().Equals("eftermiddag");
                
                var timeObject = TimeParser.ParseTime(totalTime.ToLower(), am);
                CalculatedTimeLabel.Text = timeObject.DispalyString;
                CalculatedTimeLabel.TextColor = Color.Black;
            }
            catch
            {
            }
        }

        public Picker AmPmPicker
        {
            get
            {
                return _amPmPicker ?? (_amPmPicker = new Picker
                {
                    Items =
                    {
                        "Förmiddag  ",
                        "Eftermiddag"
                    },
                    Title = "Förmiddah/EfterMiddag",
                    HorizontalOptions = LayoutOptions.Start,
                    WidthRequest = 150,
                    BackgroundColor = Color.White,
                    SelectedIndex = 0
                });
            }
            set { _amPmPicker = value; }
        }

        public Button CalculateButton
        {
            get
            {
                return _calculateButton ?? (_calculateButton = new Button
                {
                    Text = "OK",
                    Command = OKClickedCommand,
                    HorizontalOptions = LayoutOptions.Start,
                    IsEnabled = false
                });
            }
            set { _calculateButton = value; }
        }

        public ICommand OKClickedCommand
        {
            get
            {
                return new Command((() => { })
                    );
            }
        }

        public StackLayout AmpmStackLayout
        {
            get
            {
                return new StackLayout
                {
                    Children =
                    {
                        AmPmPicker,
                        //CalculateButton
                    },
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 8,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
            }
        }


        public StackLayout TotalTimeStackLayout
        {
            get
            {
                return new StackLayout
                {
                    Children =
                    {
                        //new Label()
                        //{
                        //    Text = "Resultat",
                        //    FontSize = Device.GetNamedSize(NamedSize.Micro, typeof (Label)),
                        //},
                        TotalTimeLabelSubLabelAndButtonStackLayout
                    },
                    HorizontalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Vertical,
                    Padding = new Thickness(0, 30, 0, 0),
                    VerticalOptions = LayoutOptions.Center
                };
            }
        }

        public StackLayout TotalTimeLabelSubLabelAndButtonStackLayout
        {
            get
            {
                return new StackLayout
                {
                    Children =
                    {
                        CalculatedTimeLabel,
                        ShowClockButton
                    },
                    Padding = new Thickness(5, 5, 5, 5),
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Spacing = 10
                };
            }
        }

        public Label CalculatedTimeLabel
        {
            get
            {
                return _calculatedTimeLabel ?? (_calculatedTimeLabel = new Label
                {
                    Text = "HH:MM",
                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Opacity = 0.5,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center
                });
            }
            set { _calculatedTimeLabel = value; }
        }

        public StackLayout ScrollerStackLayout
        {
            get
            {
                return new StackLayout
                {
                    Children =
                    {
                        MinutesScroll,
                        PastPresentScroll,
                        HourScroll
                    },
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 5
                };
            }
        }

        public Picker HourScroll
        {
            get
            {
                if (_hourScroll != null)
                {
                    return _hourScroll;
                }


                var hourScroll = new Picker
                {
                    Title = "Timmar",
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    BackgroundColor =  Color.White,
                };

                for (var i = 0; i < 12; i++)
                {
                    var refinedNumberList = TimeParser.TimeDictionary.Select(pair => pair.Key).ToArray();
                    hourScroll.Items.Add(refinedNumberList[i]);
                }
                _hourScroll = hourScroll;

                return _hourScroll;
            }
            set { _hourScroll = value; }
        }

        public Picker PastPresentScroll
        {
            get
            {
                if (_pastPresentScroll != null)
                    return _pastPresentScroll;
                        
               _pastPresentScroll = new Picker
                {
                    Title = "i/över",
                    BackgroundColor = Color.White
                };

                _pastPresentScroll.Items.Add("i");
                _pastPresentScroll.Items.Add("över");
                return _pastPresentScroll;
            }
            set { _pastPresentScroll = value; }
        }

        public Picker MinutesScroll
        {
            get
            {
                if (_minuteScroll != null)
                {
                    return _minuteScroll;
                }
                var items = TimeParser.TimeDictionary.Select(pair => pair.Key);
                var picker = new Picker
                {
                    Title = "Minuter",
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    BackgroundColor = Color.White,
                    
                };

                foreach (var item in items)
                {
                    picker.Items.Add(item);
                }

                _minuteScroll = picker;
                return _minuteScroll;
            }
            set { _minuteScroll = value; }
        }

        public Button ShowClockButton
        {
            get
            {
                return _showClockButton ?? (_showClockButton = new Button
                {
                    Text = "Klockur",
                    Image = "128/resizedimage.png",
                    HorizontalOptions = LayoutOptions.Center,
                    //MinimumWidthRequest = 200,
                    WidthRequest = 100,
                    IsEnabled = false,
                    Command =
                        new Command(async () => { await DisplayAlert("Inte implementerat", "Kommer senare", "Cancel"); })
                });
            }
            set { _showClockButton = value; }
        }
    }
}
