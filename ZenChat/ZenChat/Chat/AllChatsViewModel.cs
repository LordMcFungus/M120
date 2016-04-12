// Copyright (c) 2016 
// All rights reserved

using System.Collections.ObjectModel;
using ZenChat.ZenChatService;

namespace ZenChat.Chat
{
	internal class AllChatsViewModel
	{
		public ObservableCollection<ChatViewModel> MyChats { get; set; }

		public AllChatsViewModel()
		{
			GetChats();
		}

		public void GetChats()
		{
			var client = new ZenChatServiceClient(ZenChatServiceClient.EndpointConfiguration.BasicHttpsBinding_ZenChatService);
		}
	}
}