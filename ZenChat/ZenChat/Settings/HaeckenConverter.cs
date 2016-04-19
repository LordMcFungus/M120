using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using ZenChat.ZenChatService;

namespace ZenChat.Settings
{
	public class HaeckenConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var message = value as ChatMessage;
			if (message == null)
			{
				return string.Empty;
			}
			else
			{
				var sentTo = message.SentTo.Count;
				var arrivedAt = message.ArrivedAt.Count;
				var readBy = message.ReadBy.Count;
				if (readBy == sentTo)
				{
					return "✓✓✓";
				}
				else if (arrivedAt == sentTo)
				{
					return "✓✓";
				}
				else
				{
					return "✓";
				}
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
