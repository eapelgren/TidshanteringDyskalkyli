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

            Content = new ContentView
            {
                Content = new StackLayout
                {
                    Children =
                    {
                        TimeEnterStackLayout,
                        CalculatedTimeLabel,
                        clockView
                    },
                    Orientation = StackOrientation.Vertical,
                    Spacing = 10
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
                    MinimumWidthRequest = 20
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
                    Command = OKClickedCommand
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
                        TimeEntry,
                        TimePicker,
                        CalculateButton
                    },
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 8
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
                HorizontalOptions = LayoutOptions.CenterAndExpand
            }); }
            set { _calculatedTimeLabel = value; }
        }

        public ClockViewModel vm { get; set; }
    }
}