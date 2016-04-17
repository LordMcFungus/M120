﻿// Copyright (c) 2016 
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

		public ObservableCollection<UserViewModel> MyFriends { get; } = new ObservableCollection<UserViewModel>();

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

		private void RemoveUser(User user)
		{
			
		}

		private async void AddFriend()
		{
			var client = new ZenClient(ZenClient.EndpointConfiguration.BasicHttpBinding_Zen);
			try
			{
				await client.AddFriendAsync(Session.UserID, NewFriendPhoneNumber);
				var friend = await client.GetUserAsync(NewFriendPhoneNumber);
				MyFriends.Add(new UserViewModel(friend, RemoveUser));
			}
			catch (Exception e)
			{
				var dialog = new MessageDialog(e.Message);
				await dialog.ShowAsync();
			}
		}

		private async void LoadFriends()
		{
			var client = new ZenClient(ZenClient.EndpointConfiguration.BasicHttpBinding_Zen);

			var user = await client.GetFriendsAsync(Session.UserID);
			foreach (var friend in user)
			{
				MyFriends.Add(new UserViewModel(friend, RemoveUser));
			}
		}

		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}