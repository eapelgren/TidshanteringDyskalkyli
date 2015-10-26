using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using View = Xamarin.Forms.View;

namespace TidshanteringDyskalkyli.iOS
{
    class PickerEventHandlerSetteriOS : IPickerEventHandlerSetter
    {
        public IEnumerable<View> PickerList;

        public event PropertyChangedEventHandler PropertyChanged;
        public void setEventHandler(IEnumerable<View> pickers)
        {

            PickerList = pickers;

            foreach (var view in PickerList)
            {
                view.Unfocused += ViewOnUnfocused;
            }

        }

        private void ViewOnUnfocused(object sender, FocusEventArgs focusEventArgs)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("update"));
        }

    }
}