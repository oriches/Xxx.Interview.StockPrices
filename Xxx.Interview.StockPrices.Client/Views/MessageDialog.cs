using System.Windows.Markup;
using MahApps.Metro.Controls.Dialogs;
using Xxx.Interview.StockPrices.Client.Models;
using Xxx.Interview.StockPrices.Client.ViewModels;

namespace Xxx.Interview.StockPrices.Client.Views;

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