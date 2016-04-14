// Copyright (c) 2016 
// All rights reserved

using System;
using ZenChat.ZenChatService;

namespace ZenChat.Chat
{
	internal class ChatViewModel
	{
		public PrivateConversation Chat { get; set; }

		public DateTime LastSentMessage { get; set; }

		public User User { get; set; }

		public int UnreadMessages { get; set; }
	}
}