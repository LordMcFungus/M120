using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.Prism.Commands;
using ZenChat.Annotations;
using ZenChat.Models;
using ZenChat.ZenChatService;

namespace ZenChat.Login
{
	public class LoginViewModel : INotifyPropertyChanged
	{
		private string _phoneNumber;
		private string _username;

		public LoginViewModel()
		{
			LoginCommand = new DelegateCommand(Login, CanLogin);
		}

		private bool CanLogin()
		{
			return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(PhoneNumber);
		}

		public string Username
		{
			get { return _username; }
			set
			{
				_username = value; 
				OnPropertyChanged();
				LoginCommand.RaiseCanExecuteChanged();
			}
		}

		public string PhoneNumber
		{
			get { return _phoneNumber; }
			set
			{
				_phoneNumber = value; 
				OnPropertyChanged();
				LoginCommand.RaiseCanExecuteChanged();
			}
		}

		public DelegateCommand LoginCommand { get; }

		private void Login()
		{
			var Client = new ZenChatServiceClient(ZenChatServiceClient.EndpointConfiguration.BasicHttpsBinding_ZenChatService);
			var User = Client.LoginAsync(PhoneNumber, Username);
			Windows.Storage.ApplicationData.Current.LocalSettings.Values["UID"] = User.Result.Item1;
			
			Session.UserID = User.Result.Item1;
			Session.PhoneNumber = User.Result.Item2.PhoneNumber;
			Session.Username = User.Result.Item2.Name;
			ChangeWindow();
		}

		private void ChangeWindow()
		{
			((Frame)Window.Current.Content).Navigate(typeof(MainPage));
		}


		/// <summary>
		/// 
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="propertyName"></param>
		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
