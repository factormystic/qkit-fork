using QKitSampleApp.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QKitSampleApp.InputAwarePanelSample
{
    public class ChatViewModel : BaseModel
    {
        private ObservableCollection<MessageItem> threadContent;
        private string message;

        public ChatViewModel()
        {
            ThreadContent.Add(new MessageItem { Sender = "Other", Content = "Hello." });
            ThreadContent.Add(new MessageItem { Sender = "Self", Content = "Hey. I hooked up to the InputAwarePanel's LayoutChangeStarted event to scroll to the bottom of the list to ensure the TextBox is visible." });
            ThreadContent.Add(new MessageItem { Sender = "Other", Content = "That's good. This is currently using the Independent animation mode for performance." });
            ThreadContent.Add(new MessageItem { Sender = "Self", Content = "That animation mode works in this case because there's no element at the top of this InputAwarePanel." });
            ThreadContent.Add(new MessageItem { Sender = "Other", Content = "Yes, it's recommended that one should test different animation modes for best results." });
            ThreadContent.Add(new MessageItem { Sender = "Self", Content = "Cool." });
        }

        public ObservableCollection<MessageItem> ThreadContent
        {
            get
            {
                if (threadContent == null)
                    threadContent = new ObservableCollection<MessageItem>();
                return threadContent;
            }
            set
            {
                threadContent = value;
                NotifyPropertyChanged();
            }
        }

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                NotifyPropertyChanged();
            }
        }

        public void SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(Message))
                ThreadContent.Add(new MessageItem { Sender = "Self", Content = Message });
        }
    }

    public class MessageItem
    {
        public string Sender { get; set; }
        public string Content { get; set; }
    }
}
