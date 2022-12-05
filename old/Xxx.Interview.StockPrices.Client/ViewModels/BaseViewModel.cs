using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Cibc.StockPrices.Client.ViewModels
{
    public abstract class BaseViewModel : IViewModel
    {
        private static readonly PropertyChangedEventArgs EmptyChangeArgs = new PropertyChangedEventArgs(string.Empty);

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged()
        {
            RaisePropertyChanged(string.Empty);
        }

        protected virtual void RaisePropertiesChanged(params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
                RaisePropertyChanged(propertyName);
        }

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                if (string.IsNullOrEmpty(propertyName))
                {
                    handler(this, EmptyChangeArgs);
                }
                else
                {
                    var args = new PropertyChangedEventArgs(propertyName);
                    handler(this, args);
                }
            }
        }

        protected virtual bool SetProperty<T>(ref T existingValue, T newValue,
            [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(existingValue, newValue)) return false;

            existingValue = newValue;
            RaisePropertyChanged(propertyName);

            return true;
        }
    }
}