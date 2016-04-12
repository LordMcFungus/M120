// Copyright (c) 2016 
// All rights reserved
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using ZenChat.Annotations;
using ZenChat.ZenChatService;

namespace ZenChat.Friends
{
	class AllFriendsViewModel : INotifyPropertyChanged
	{
		private ObservableCollection<User> _myFriends;
		private string _newFriendPhoneNumber;

		public ObservableCollection<User> MyFriends
		{
			get
			{
				return _myFriends;
			}
			set
			{
				_myFriends = value;
			}
		}

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

		public AllFriendsViewModel()
		{
			//LoadFriends();
			AddFriendCommand = new DelegateCommand(AddFriend, CanAddFriend);
		}

		private bool CanAddFriend()
		{
			return !string.IsNullOrEmpty(NewFriendPhoneNumber);
		}

		private async void AddFriend()
		{

			var client = new ZenChatServiceClient(ZenChatServiceClient.EndpointConfiguration.BasicHttpsBinding_ZenChatService);
			var id = Windows.Storage.ApplicationData.Current.LocalSettings.Values["UID"] as int?;
			try
			{
				if (id.HasValue)
				{
					await client.AddFriendAsync(id.Value, NewFriendPhoneNumber);
					var friend = await client.GetUserAsync(NewFriendPhoneNumber);
					_myFriends.Add(friend);
				}
			}
			catch (Exception)
			{
				
				
			}
		}

		private async void LoadFriends()
		{
			var client = new ZenChatServiceClient(ZenChatServiceClient.EndpointConfiguration.BasicHttpsBinding_ZenChatService);
			var id = Windows.Storage.ApplicationData.Current.LocalSettings.Values["UID"] as int?;
			var user = await client.GetFriendsAsync(id.Value);
			foreach (var friend in user)
			{
				_myFriends.Add(friend);
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
