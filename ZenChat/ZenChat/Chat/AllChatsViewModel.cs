// Copyright (c) 2016 
// All rights reserved

using System.Collections.ObjectModel;
using System.ServiceModel.Description;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ZenChat.Models;
using ZenChat.ZenChatService;

namespace ZenChat.Chat
{
	internal class AllChatsViewModel
	{
		private ChatViewModel _selectedChat;

		public AllChatsViewModel()
		{
			GetChats();
		}

		public ObservableCollection<ChatViewModel> MyChats { get; } = new ObservableCollection<ChatViewModel>();

		public ChatViewModel SelectedChat
		{
			get { return _selectedChat; }
			set
			{
				_selectedChat = value;

				var rootFrame = Window.Current.Content as Frame;
				rootFrame?.Navigate(typeof(ChatView), _selectedChat);
			}
		}

		public async void GetChats()
		{
			var client = new ZenClient(ZenClient.EndpointConfiguration.BasicHttpBinding_Zen);
			var friends = await client.GetFriendsAsync(Session.UserID);
			foreach (var friend in friends)
			{
				var chat = await client.GetPrivateConversationAsync(Session.UserID, friend.PhoneNumber);
				var chatModel = new ChatViewModel {PrivateChat = chat};
				MyChats.Add(chatModel);
			}
		}
	}
}