using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TidshanteringDyskalkyli.Annotations;

namespace TidshanteringDyskalkyli.ViewModel
{

    public class ClockViewModel : INotifyPropertyChanged
    {

        private int _minute;

        public int Minute
        {

            get { return _minute; }
            set
            {
                _minute = value; 
                OnPropertyChanged();
            }
        }

        private int _hour;
        public bool AM;

        public int Hour
        {

            get
            {
                return _hour;
            }
            set
            {
                _hour = value;
                OnPropertyChanged();
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
