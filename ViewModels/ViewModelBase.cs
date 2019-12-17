using FootballApp.Classes;
using FootballApp.ErrorHandling;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FootballApp.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;
            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        public ErrorHandler errorHandler = new ErrorHandler();

        /// <summary>
        /// stores the country selected info
        /// </summary>
        private Country _currentCountry;

        public Country CurrentCountry
        {
            get { return _currentCountry; }
            set { SetProperty(ref _currentCountry, value); }
        }
    }
}
