// Copyright (c) 2016 
// All rights reserved

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Storage;
using Microsoft.Practices.Prism.Commands;
using ZenChat.Annotations;
using ZenChat.ZenChatService;

namespace ZenChat.Friends
{
	internal class AllFriendsViewModel : INotifyPropertyChanged
	{
		private string _newFriendPhoneNumber;

		public AllFriendsViewModel()
		{
			//LoadFriends();
			AddFriendCommand = new DelegateCommand(AddFriend, CanAddFriend);
		}

		public ObservableCollection<User> MyFriends { get; set; }

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
			var id = ApplicationData.Current.LocalSettings.Values["UID"] as int?;
			try
			{
				if (id.HasValue)
				{
					await client.AddFriendAsync(id.Value, NewFriendPhoneNumber);
					var friend = await client.GetUserAsync(NewFriendPhoneNumber);
					MyFriends.Add(friend);
				}
			}
			catch (Exception)
			{
			}
		}

		private async void LoadFriends()
		{
			var client = new ZenChatServiceClient(ZenChatServiceClient.EndpointConfiguration.BasicHttpsBinding_ZenChatService);
			var id = ApplicationData.Current.LocalSettings.Values["UID"] as int?;
			var user = await client.GetFriendsAsync(id.Value);
			foreach (var friend in user)
			{
				MyFriends.Add(friend);
			}
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}