using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace TidshanteringDyskalkyli
{
    public interface IPickerEventHandlerSetter : INotifyPropertyChanged
    {
        void setEventHandler(IEnumerable<View> pickers);


    }
}