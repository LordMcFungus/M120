// Copyright (c) 2016 
// All rights reserved

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Microsoft.Practices.Prism.Commands;
using ZenChat.Annotations;
using ZenChat.Models;
using ZenChat.ZenChatService;

namespace ZenChat.Chat
{
	internal class AllChatsViewModel : INotifyPropertyChanged
	{
		private ChatViewModel _selectedChat;
		private string _topic = string.Empty;
		public DelegateCommand CreateGroupChatCommand { get; }

		public AllChatsViewModel()
		{
			LoadChats();
			CreateGroupChatCommand = new DelegateCommand(CreateGroupChatMethod);
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

		private async void CreateGroupChatMethod()
		{
			var panel = new StackPanel();

			var textBox = new TextBox();

			var binding = new Binding {Path = new PropertyPath(nameof(Topic)) };

			textBox.SetBinding(TextBox.TextProperty, binding);

			panel.Children.Add(textBox);

			var dialog = new ContentDialog
			{
				Title = "Neuen Chat erstellen",
				Content = panel,
				PrimaryButtonCommand = new DelegateCommand(DoOnOkClicked),
				PrimaryButtonText = "Erstellen"
			};

			await dialog.ShowAsync();
		}

		private async void DoOnOkClicked()
		{
			var client = new ZenClient(ZenClient.EndpointConfiguration.BasicHttpBinding_Zen);
			var createdChat = await client.CreateChatRoomAsync(Session.UserID, Topic);

			var rootFrame = Window.Current.Content as Frame;
			rootFrame?.Navigate(typeof(EditGroupChat), createdChat);
		}

		public string Topic
		{
			get { return _topic; }
			set
			{
				_topic = value;
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}