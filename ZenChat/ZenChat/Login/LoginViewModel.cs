using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Storage;
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


		/// <summary>
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		private bool CanLogin()
		{
			return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(PhoneNumber);
		}

		private void Login()
		{
			var client = new ZenChatServiceClient(ZenChatServiceClient.EndpointConfiguration.BasicHttpsBinding_ZenChatService);
			var user = client.LoginAsync(PhoneNumber, Username);
			ApplicationData.Current.LocalSettings.Values["UID"] = user.Result.Item1;

			Session.UserID = user.Result.Item1;
			Session.PhoneNumber = user.Result.Item2.PhoneNumber;
			Session.Username = user.Result.Item2.Name;
			ChangeWindow();
		}

		private void ChangeWindow()
		{
			((Frame) Window.Current.Content).Navigate(typeof(MainPage));
		}

		/// <summary>
		/// </summary>
		/// <param name="propertyName"></param>
		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}