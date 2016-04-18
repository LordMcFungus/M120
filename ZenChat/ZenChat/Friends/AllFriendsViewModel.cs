// Copyright (c) 2016 
// All rights reserved

using System;
using System.Collections.Generic;
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
	public sealed class AllFriendsViewModel : INotifyPropertyChanged
	{
		private string _newFriendPhoneNumber;

		public AllFriendsViewModel()
		{
			_removeUser = async user =>
			{
				var client = new ZenClient(ZenClient.EndpointConfiguration.BasicHttpBinding_Zen);
				await client.RemoveFriendAsync(Session.UserID, user.PhoneNumber);
				LoadFriends();
			};

			LoadFriends();
			AddFriendCommand = new DelegateCommand(AddFriend, CanAddFriend);
		}

		public AllFriendsViewModel(Action add, Func<bool> canAdd, Action<User> onXClicked, IEnumerable<User> usersToDisplay)
		{
			_removeUser = onXClicked;

			foreach (var user in usersToDisplay)
			{
				MyFriends.Add(new FriendViewModel(user, onXClicked));
			}
			AddFriendCommand = new DelegateCommand(add, canAdd);
		}

		public ObservableCollection<FriendViewModel> MyFriends { get; } = new ObservableCollection<FriendViewModel>();

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

		private Action<User> _removeUser;

		private async void AddFriend()
		{
			var client = new ZenClient(ZenClient.EndpointConfiguration.BasicHttpBinding_Zen);
			try
			{
				await client.AddFriendAsync(Session.UserID, NewFriendPhoneNumber);
				var friend = await client.GetUserAsync(NewFriendPhoneNumber);
				MyFriends.Add(new FriendViewModel(friend, _removeUser));
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
			MyFriends.Clear();
			var user = await client.GetFriendsAsync(Session.UserID);
			foreach (var friend in user)
			{
				MyFriends.Add(new FriendViewModel(friend, _removeUser));
			}
		}

		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}