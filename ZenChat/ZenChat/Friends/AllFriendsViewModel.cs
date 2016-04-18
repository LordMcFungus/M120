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
		private readonly Action<User> _removeUser;
		private string _newFriendPhoneNumber;

		public AllFriendsViewModel()
		{
			_removeUser = async user =>
			{
				await Session.Client.RemoveFriendAsync(Session.UserID, user.PhoneNumber);
				LoadFriends();
			};

			LoadFriends();
			AddFriendCommand = new DelegateCommand(AddFriend, CanAddFriend);
		}

		/// <summary>
		///     Constructor for the advanced mode
		/// </summary>
		/// <param name="add"></param>
		/// <param name="canAdd"></param>
		/// <param name="onXClicked"></param>
		/// <param name="usersToDisplay"></param>
		/// <param name="allowSelection"></param>
		/// <param name="displayAddButton"></param>
		/// <param name="displayAddTextbox"></param>
		/// <param name="displayX"></param>
		public AllFriendsViewModel(Action add, Func<bool> canAdd, Action<User> onXClicked, IEnumerable<User> usersToDisplay,
			bool allowSelection, bool displayAddButton, bool displayAddTextbox, bool displayX)
		{
			DisplayAddButton = displayAddButton;
			DisplayAddTextbox = displayAddTextbox;

			_removeUser = onXClicked;

			foreach (var user in usersToDisplay)
			{
				MyFriends.Add(new FriendViewModel(user, onXClicked, allowSelection, displayX));
			}
			AddFriendCommand = new DelegateCommand(add, canAdd);
		}

		public string Title { get; set; }

		public bool DisplayAddTextbox { get; private set; } = true;

		public bool DisplayAddButton { get; private set; } = true;

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

		private async void AddFriend()
		{
			try
			{
				await Session.Client.AddFriendAsync(Session.UserID, NewFriendPhoneNumber);
				var friend = await Session.Client.GetUserAsync(NewFriendPhoneNumber);
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
			MyFriends.Clear();
			var user = await Session.Client.GetFriendsAsync(Session.UserID);
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