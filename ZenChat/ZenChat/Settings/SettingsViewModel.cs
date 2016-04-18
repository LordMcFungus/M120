// Copyright (c) 2016 
// All rights reserved

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using Windows.UI.Popups;
using Microsoft.Practices.Prism.Commands;
using ZenChat.Annotations;
using ZenChat.Models;
using ZenChat.ZenChatService;

namespace ZenChat.Settings
{
	internal class SettingsViewModel : INotifyPropertyChanged
	{
		private bool _isActive;
		private string _phonenumber;
		private string _username;


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
			User user;

			if (Phonenumber == Session.PhoneNumber && Username == Session.Username)
			{
				IsActive = false;
				var dialog = new MessageDialog("Es gibt keine Änderungen");
				await dialog.ShowAsync();
				return;
			}

			if (!Equals(Phonenumber, Session.PhoneNumber))
			{
				try
				{
					user = await Session.Client.ChangePhoneNumberAsync(Session.UserID, Phonenumber);
					Session.PhoneNumber = Phonenumber = user.PhoneNumber;
				}
				catch (FaultException e)
				{
					var dialog = new MessageDialog(e.Message);
					await dialog.ShowAsync();
					Phonenumber = Session.PhoneNumber;
				}
			}
			if (!Equals(Username, Session.Username))
			{
				user = await Session.Client.ChangeUsernameAsync(Session.UserID, Username);
				Session.Username = Username = user.Name;
			}

			IsActive = false;
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}