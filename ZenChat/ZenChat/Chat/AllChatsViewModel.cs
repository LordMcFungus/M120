// Copyright (c) 2016 
// All rights reserved

using System.Collections.ObjectModel;
using ZenChat.Models;
using ZenChat.ZenChatService;

namespace ZenChat.Chat
{
	internal class AllChatsViewModel
	{
		public AllChatsViewModel()
		{
			GetChats();
		}

		public ObservableCollection<ChatViewModel> MyChats { get; } = new ObservableCollection<ChatViewModel>();

		public async void GetChats()
		{
			var client = new ZenChatServiceClient(ZenChatServiceClient.EndpointConfiguration.BasicHttpsBinding_ZenChatService);
			var friends = await client.GetFriendsAsync(Session.UserID);
			foreach (var friend in friends)
			{
				var chat = await client.GetPrivateConversationAsync(Session.UserID, friend.PhoneNumber);
				var chatModel = new ChatViewModel {Chat = chat};
				MyChats.Add(chatModel);
			}
		}
	}
}