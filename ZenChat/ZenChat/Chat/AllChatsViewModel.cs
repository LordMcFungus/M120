// Copyright (c) 2016 
// All rights reserved

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Microsoft.Practices.Prism.Commands;
using ZenChat.Annotations;
using ZenChat.Models;

namespace ZenChat.Chat
{
	internal class AllChatsViewModel : INotifyPropertyChanged
	{
		private ChatViewModel _selectedChat;
		private string _topic = string.Empty;
		private int _amountOfWorkers;

		public AllChatsViewModel()
		{
			LoadPrivateChats();
			LoadGroupChats();
			CreateGroupChatCommand = new DelegateCommand(CreateGroupChatMethod);
		}

		private async void LoadGroupChats()
		{
			AmountOfWorkers++;
			var chatrooms = await Session.Client.GetAllChatRoomsAsync(Session.UserID);
			foreach (var chat in chatrooms)
			{
				MyChats.Add(new ChatViewModel { Chatroom = chat });
			}
			AmountOfWorkers--;
		}

		private async void LoadPrivateChats()
		{
			AmountOfWorkers++;
			var friends = await Session.Client.GetFriendsAsync(Session.UserID);

			foreach (var friend in friends)
			{
				LoadChatForFriend(friend.PhoneNumber);
			}
			AmountOfWorkers--;
		}

		private async void LoadChatForFriend(string phone)
		{
			AmountOfWorkers++;
			var chat = await Session.Client.GetPrivateConversationAsync(Session.UserID, phone);
			var viewModel = new ChatViewModel { PrivateChat = chat };
			MyChats.Add(viewModel);
			AmountOfWorkers--;
		}

		public DelegateCommand CreateGroupChatCommand { get; }

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

		private int AmountOfWorkers
		{
			get { return _amountOfWorkers; }
			set
			{
				_amountOfWorkers = value;
				OnPropertyChanged(nameof(Working));
			}
		}

		public bool Working => AmountOfWorkers > 0;

		private async void CreateGroupChatMethod()
		{
			var panel = new StackPanel();

			var textBox = new TextBox();

			var binding = new Binding {Path = new PropertyPath(nameof(Topic))};

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
			var createdChat = await Session.Client.CreateChatRoomAsync(Session.UserID, Topic);

			var rootFrame = Window.Current.Content as Frame;
			rootFrame?.Navigate(typeof(EditGroupChat), createdChat);

			LoadPrivateChats();
			LoadGroupChats();

		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}