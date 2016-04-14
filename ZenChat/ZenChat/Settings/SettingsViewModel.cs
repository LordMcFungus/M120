// Copyright (c) 2016 
// All rights reserved

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Practices.Prism.Commands;
using ZenChat.Annotations;
using ZenChat.Models;
using ZenChat.ZenChatService;

namespace ZenChat.Settings
{
	internal class SettingsViewModel : INotifyPropertyChanged
	{
		private string _phonenumber;
		private string _username;
		private bool _isActive;


		public SettingsViewModel()
		{
			Username = Session.Username;
			Phonenumber = Session.PhoneNumber;
			Change = new DelegateCommand(SaveChanges);
		}

		public string Username
		{
			get { return _username; }
			set
			{
				_username = value;
				OnPropertyChanged();
			}
		}

		public string Phonenumber
		{
			get { return _phonenumber; }
			set
			{
				_phonenumber = value;
				OnPropertyChanged();
			}
		}


		public DelegateCommand Change { get; }

		public bool IsActive
		{
			get { return _isActive; }
			set
			{
				_isActive = value;
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private async void SaveChanges()
		{
			IsActive = true;
			var client = new ZenChatServiceClient(ZenChatServiceClient.EndpointConfiguration.BasicHttpsBinding_ZenChatService);
			await client.ChangeUsernameAsync(Session.UserID, Username);
			var user = await client.ChangePhoneNumberAsync(Session.UserID, Phonenumber);
			Session.PhoneNumber = Phonenumber = user.PhoneNumber;
			Session.Username = Username = user.Name;
			IsActive = false;
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}