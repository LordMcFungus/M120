// Copyright (c) 2016 
// All rights reserved

using System;
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
			var sentTo = message.SentTo.Count;
			var arrivedAt = message.ArrivedAt.Count;
			var readBy = message.ReadBy.Count;
			if (readBy == sentTo)
			{
				return "✓✓✓";
			}
			if (arrivedAt == sentTo)
			{
				return "✓✓";
			}
			return "✓";
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}