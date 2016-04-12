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
		private int _unreadMessages;
		private PrivateConversation _chat;

		public PrivateConversation Chat
		{
			get { return _chat; }
			set { _chat = value; }
		}

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

		public int UnreadMessages
		{
			get
			{
				// TODO GET AMMOUNT OF UNREAD MESSAGES
				return _unreadMessages;
			}
			set
			{
				_unreadMessages = value;
			}
		}
	}
}
