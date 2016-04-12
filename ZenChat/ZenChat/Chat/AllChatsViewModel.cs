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

		public async void GetChats()
		{
			var client = new ZenChatServiceClient(ZenChatServiceClient.EndpointConfiguration.BasicHttpsBinding_ZenChatService);
			var id = Windows.Storage.ApplicationData.Current.LocalSettings.Values["UID"] as int?;
			if (id.HasValue)
			{
				var friends = await client.GetFriendsAsync(id.Value);
				foreach (var friend in friends)
				{
					var chat = await client.GetPrivateConversationAsync(id.Value, friend.PhoneNumber);
					var chatModel = new ChatViewModel();
					chatModel.Chat = chat;
					MyChats.Add(chatModel);
				}
			}
		}
	}
}