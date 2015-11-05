using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using TidshanteringDyskalkyli.Annotations;
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
            BackgroundColor = Colors.SoftGray;
            Title = "Översättning";
            Icon = "128/resizedimagelanguage.png";

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

            TimePicker.Unfocused += (sender, args) =>
            {
                OKClickedCommand.Execute(null);
            };


            Padding = new Thickness(0, 20, 0, 0);

            //var clockView = new ClockView(vm)
            //{
            //    MinimumHeightRequest = 100,
            //    MinimumWidthRequest = 100
            //};

            //SizeChanged += clockView.OnPageSizeChanged;

            TimeEntry.TextChanged += (sender, args) =>
            {
                if (TimeEntry.Text.Length > 0)
                {
                    CalculateButton.IsEnabled = true;
                }
            };


            Content = new ContentView
            {
                Content = new StackLayout
                {
                    Children =
                    {
                        TimeEnterStackLayout,
                        TotalTimeStackLayout,
                        
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
                    BackgroundColor = Color.White
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
                    BackgroundColor = Color.White
                    
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
                return new Command(async () =>
                {

                    var timeToParse = TimeEntry.Text;

                    if (timeToParse == null)
                    {
                      await DisplayAlert("Du har inte skrivit något i textfältet", "Skrriv in en text", "Cancel");
                      TimeEntry.Focus();
                      return;
                    }

                    try
                    {
                        
                    TimeReturnObject timeObject = new TimeParser().ParseTime(timeToParse, vm.AM);
                        Debug.WriteLine(timeObject.Hour);
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
                        Debug.WriteLine(timeObject.Hour);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            vm.Hour = int.Parse(timeObject.Hour);
                            vm.Minute = int.Parse(timeObject.Minute);
                        });
        
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
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
                                CalculatedTimeLabel.Text = "HH:MM";
                                TimeEntry.Placeholder = collectedPair.Value;
                                
                                throw e;
                            });
                     
                    }

                    TimePicker.Unfocus();
                    TimeEntry.Unfocus();

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
                TextColor = Color.Black,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
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
        private Picker _halfScroll;
        private View _hourScroll;

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