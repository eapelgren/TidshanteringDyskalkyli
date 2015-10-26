using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using View = Xamarin.Forms.View;

namespace TidshanteringDyskalkyli.Droid
{
    class PickerEventHandlerSetterDroid : IPickerEventHandlerSetter
    {
        public IEnumerable<View> PickerList; 

        public event PropertyChangedEventHandler PropertyChanged;
        public void setEventHandler(IEnumerable<View> pickers)
        {

            PickerList = pickers;

            foreach (var view in PickerList)
            {
                view.PropertyChanged += ViewOnPropertyChanged;
            }
            
        }

        private void ViewOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("update"));
        }
    }
}