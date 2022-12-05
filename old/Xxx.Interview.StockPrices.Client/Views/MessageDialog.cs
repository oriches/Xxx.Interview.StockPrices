using System.Windows.Markup;
using Cibc.StockPrices.Client.Models;
using Cibc.StockPrices.Client.ViewModels;
using MahApps.Metro.Controls.Dialogs;

namespace Cibc.StockPrices.Client.Views
{
    [ContentProperty("DialogBody")]
    public sealed class MessageDialog : BaseMetroDialog
    {
        private readonly Message _message;

        public MessageDialog(Message message)
        {
            _message = message;

            Title = _message.Header;
            Content = _message.ViewModel;
        }

        public ICloseableViewModel CloseableContent => _message.ViewModel;
    }
}