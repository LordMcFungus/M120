// Copyright (c) 2016 
// All rights reserved

using System;
using ZenChat.ZenChatService;

namespace ZenChat.Chat
{
	internal class ChatViewModel
	{
		public DateTime LastSentMessage { get; set; }

		public User User { get; set; }
	}
}