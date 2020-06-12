using MimeKit;

namespace Mailer.Entities
{
    public class MessageText : TextPart
    {
        public MessageText() 
        {
        }

        public MessageText(string text) 
        {
            Text = text;
        }
    }
}
