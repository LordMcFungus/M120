// Copyright (c) 2016 
// All rights reserved

using System.Collections.ObjectModel;
using Windows.Storage;
using ZenChat.ZenChatService;

namespace ZenChat.Chat
{
	internal class AllChatsViewModel
	{
		public AllChatsViewModel()
		{
			GetChats();
		}

		public ObservableCollection<ChatViewModel> MyChats { get; set; }

		public async void GetChats()
		{
			var client = new ZenChatServiceClient(ZenChatServiceClient.EndpointConfiguration.BasicHttpsBinding_ZenChatService);
			var id = ApplicationData.Current.LocalSettings.Values["UID"] as int?;
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