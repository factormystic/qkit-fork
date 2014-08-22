using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace QKitSampleApp.InputAwarePanelSample
{
    public class ChatItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ReceivedMessageTemplate { get; set; }
        public DataTemplate SentMessageTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var message = item as MessageItem;
            if (message != null)
            {
                if (message.Sender == "Self")
                    return SentMessageTemplate;
                else
                    return ReceivedMessageTemplate;
            }
            return base.SelectTemplateCore(item, container);
        }
    }

}
