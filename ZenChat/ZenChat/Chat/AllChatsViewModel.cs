﻿// Copyright (c) 2016 
// All rights reserved

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel.Description;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.Prism.Commands;
using ZenChat.Models;
using ZenChat.ZenChatService;

namespace ZenChat.Chat
{
	internal class AllChatsViewModel
	{
		private ChatViewModel _selectedChat;
		public DelegateCommand CreateGroupChat { get; }

		public AllChatsViewModel()
		{
			LoadChats();
			CreateGroupChat = new DelegateCommand(CreateGroupChatMethod);
		}

		public ObservableCollection<ChatViewModel> MyChats { get; } = new ObservableCollection<ChatViewModel>();

		public ChatViewModel SelectedChat
		{
			get { return _selectedChat; }
			set
			{
				_selectedChat = value;
				_selectedChat.ReadMessages();

				var rootFrame = Window.Current.Content as Frame;
				rootFrame?.Navigate(typeof(ChatView), _selectedChat);
			}
		}

		private async void LoadChats()
		{
			var client = new ZenClient(ZenClient.EndpointConfiguration.BasicHttpBinding_Zen);
			var friends = await client.GetFriendsAsync(Session.UserID);

			var chats = new List<ChatViewModel>();
			foreach (var friend in friends)
			{
				var chat = await client.GetPrivateConversationAsync(Session.UserID, friend.PhoneNumber);
				var viewModel = new ChatViewModel {PrivateChat = chat};
				chats.Add(viewModel);
			}

			var chatrooms = await client.GetAllChatRoomsAsync(Session.UserID);
			chats.AddRange(chatrooms.Select(c => new ChatViewModel {Chatroom = c}));

			foreach (var chat in chats.OrderByDescending(c => c.LastSentMessage))
			{
				MyChats.Add(chat);
			}
		}

		private void CreateGroupChatMethod()
		{
			/*var client = new ZenClient(ZenClient.EndpointConfiguration.BasicHttpBinding_Zen);
			 var chatroom = client.CreateChatRoomAsync(Session.UserID, Title);
			foreach (var user in MyFriends.Where(n => n.IsSelectet == true))
			{
				client.InviteToChatRoomAsync(Session.UserID, user.User.PhoneNumber, chatroom.Id);
			} */
			var rootFrame = Window.Current.Content as Frame;
			rootFrame?.Navigate(typeof(EditGroupChat));
		}
	}
}