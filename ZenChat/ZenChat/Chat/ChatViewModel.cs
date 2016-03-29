// Copyright (c) 2016 
// All rights reserved
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZenChat.ZenChatService;

namespace ZenChat.Chat
{
	class ChatViewModel
	{
		private DateTime _lastSentMessage;

		public DateTime LastSentMessage
		{
			get
			{
				//TODO Calculate last sent message
				return _lastSentMessage;
			}
			set
			{
				_lastSentMessage = value;
			}
		}

		public User User { get; set; }
	}
}
