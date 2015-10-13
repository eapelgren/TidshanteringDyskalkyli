using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TidshanteringDyskalkyli.ViewModel;
using Xamarin.Forms;

namespace TidshanteringDyskalkyli.Pages
{
    public class TimeTranslatePage : ContentPage
    {
        private Button _calculateButton;
        private Label _calculatedTimeLabel;
        private Entry _timeEntry;
        private Picker _timePicker;

        public TimeTranslatePage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            vm = new ClockViewModel();
            TimePicker.SelectedIndex = 0;
            vm.AM = true;
            TimePicker.SelectedIndexChanged += (sender, args) =>
            {
                if (TimePicker.SelectedIndex == 0)
                {
                    vm.AM = true;
                }
                else if (TimePicker.SelectedIndex == 1)
                {
                    vm.AM = false;
                }
            };


            Padding = new Thickness(0, 20, 0, 0);

            var clockView = new ClockView(vm)
            {
                MinimumHeightRequest = 100,
                MinimumWidthRequest = 100
            };
            //Content = clockView;

            SizeChanged += clockView.OnPageSizeChanged;

            TimeEntry.TextChanged += (sender, args) =>
            {
                if (TimeEntry.Text.Length > 0)
                {
                    CalculateButton.IsEnabled = true;
                }
            };
            BackgroundColor = Colors.SoftGray;
            Title = "Tidsöversättning";
            Icon = "128/resizedimagelanguage.png";

        Content = new ContentView
            {
                Content = new StackLayout
                {
                    Children =
                    {
                        TimeEnterStackLayout,
                        TotalTimeStackLayout,
                        //clockView
                    },
                    Orientation = StackOrientation.Vertical,
                    Spacing = 10,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                }
            };
        }

        public Entry TimeEntry
        {
            get
            {
                return _timeEntry ?? (_timeEntry = new Entry
                {
                    Placeholder = "kvart i tre",
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                });
            }
            set { _timeEntry = value; }
        }

        public Picker TimePicker
        {
            get
            {
                return _timePicker ?? (_timePicker = new Picker
                {
                    Items =
                    {
                        "Förmiddag  ",
                        "Eftermiddag"
                    },
                    Title = "Tid",
                    HorizontalOptions = LayoutOptions.Start,
                    WidthRequest = 150,
                    
                });
            }
            set { _timePicker = value; }
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

        public StackLayout TimeEnterStackLayout
        {
            get
            {
                return new StackLayout()
                {
                    Children =
                    {
                        new Label()
                        {
                            Text = "Input",
                            FontSize = Device.GetNamedSize(NamedSize.Micro, typeof (Label)),
                        },
                        TimeEntry,
                        AmpmStackLayout
                    },
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.Center
                };
            }
        }

        public StackLayout AmpmStackLayout
        {
            get
            {
                return new StackLayout()
                {
                    Children =
                    {
                        TimePicker,
                        CalculateButton
                    },
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 8,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };

            }
        }

        public ICommand OKClickedCommand
        {
            get
            {
                return new Command(() =>
                {
                    var timeToParse = TimeEntry.Text;

                    try
                    {
                        
                    var timeObject = TimeParser.ParseTime(timeToParse, vm.AM);
                        CalculatedTimeLabel.Text = timeObject.DispalyString;
                        if (CalculatedTimeLabel.Text != "HH:MM")
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                CalculatedTimeLabel.TextColor = Color.Black;
                                CalculatedTimeLabel.Opacity = 1;
                                ShowClockButton.IsEnabled = true;
                            });
                    
                        }
                        else
                        {
                            CalculatedTimeLabel = null;
                            CalculateButton = null;
                            ShowClockButton = null;
                        }
                        vm.Hour = int.Parse(timeObject.Hour);
                        vm.Minute = int.Parse(timeObject.Minute);
                    }
                    catch (Exception)
                    {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Dictionary<int, string> inputs = new Dictionary<int, string>()
                                {
                                    [1] = "fem i halv sju",
                                    [2] = "kvart i nio",
                                    [3] = "tio över fyra",
                                    [4] = "kvart över 12"
                                };
                                
                                var number = new Random().Next(1,4);
                                var collectedPair = inputs.FirstOrDefault(pair => pair.Key == number);


                                DisplayAlert("Kunde inte förstå texten",
                                    "Testa skriv: " + collectedPair.Value, "OK");

                                TimeEntry.Text = "";
                                TimeEntry.Placeholder = collectedPair.Value;
                            });
                    }
                    
                });
            }
        }

        public Label CalculatedTimeLabel
        {
            get { return _calculatedTimeLabel ?? (_calculatedTimeLabel = new Label()
            {
                Text = "HH:MM",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Opacity = 0.5,
                TextColor = Color.Gray,
                VerticalOptions = LayoutOptions.Center
            }); }
            set { _calculatedTimeLabel = value; }
        }

        public StackLayout TotalTimeStackLayout
        {
            get
            {
                return new StackLayout()
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
                    Padding = new Thickness(0,30,0,0),
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
                    Spacing = 10,
                };
            }
        }


        private Button _showClockButton;

        public Button ShowClockButton
        {
            get
            {
                return _showClockButton ??( _showClockButton = new Button
                {
                    Text = "Klockur",
                    Image = "128/resizedimage.png",
                    HorizontalOptions = LayoutOptions.Center,
                    //MinimumWidthRequest = 200,
                    WidthRequest = 100,
                    IsEnabled = false,
                    Command = new Command(async () =>
                    {
                       await DisplayAlert("Inte implementerat", "Kommer senare", "Cancel");
                    })
                });
            }
            set { _showClockButton = value; }
        }

        public ClockViewModel vm { get; set; }
    }
}