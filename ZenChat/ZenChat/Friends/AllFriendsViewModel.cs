// Copyright (c) 2016 
// All rights reserved

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Popups;
using Microsoft.Practices.Prism.Commands;
using ZenChat.Annotations;
using ZenChat.Models;
using ZenChat.ZenChatService;

namespace ZenChat.Friends
{
	internal sealed class AllFriendsViewModel : INotifyPropertyChanged
	{
		private string _newFriendPhoneNumber;

		public AllFriendsViewModel()
		{
			LoadFriends();
			AddFriendCommand = new DelegateCommand(AddFriend, CanAddFriend);
		}

		public ObservableCollection<User> MyFriends { get; } = new ObservableCollection<User>();

		public string NewFriendPhoneNumber
		{
			get { return _newFriendPhoneNumber; }
			set
			{
				_newFriendPhoneNumber = value;
				OnPropertyChanged();
				AddFriendCommand.RaiseCanExecuteChanged();
			}
		}

		public DelegateCommand AddFriendCommand { get; }
		public event PropertyChangedEventHandler PropertyChanged;

		private bool CanAddFriend()
		{
			return !string.IsNullOrEmpty(NewFriendPhoneNumber);
		}

		private async void AddFriend()
		{
			var client = new ZenChatServiceClient(ZenChatServiceClient.EndpointConfiguration.BasicHttpsBinding_ZenChatService);
			try
			{
				await client.AddFriendAsync(Session.UserID, NewFriendPhoneNumber);
				var friend = await client.GetUserAsync(NewFriendPhoneNumber);
				MyFriends.Add(friend);
			}
			catch (Exception e)
			{
				var dialog = new MessageDialog(e.Message);
				await dialog.ShowAsync();
			}
		}

		private async void LoadFriends()
		{
			var client = new ZenChatServiceClient(ZenChatServiceClient.EndpointConfiguration.BasicHttpsBinding_ZenChatService);

			var user = await client.GetFriendsAsync(Session.UserID);
			foreach (var friend in user)
			{
				MyFriends.Add(friend);
			}
		}

		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}