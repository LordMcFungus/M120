using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZenChat.Chat
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class EditGroupChat : Page
	{

		private readonly EditGroupChatViewModel ViewModel;

		public EditGroupChat()
		{
			InitializeComponent();
			ViewModel = new EditGroupChatViewModel();
			DataContext = ViewModel;

			var currentView = SystemNavigationManager.GetForCurrentView();
			currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
			currentView.BackRequested += (sender, e) =>
			{
				var rootFrame = Window.Current.Content as Frame;

				if (rootFrame != null && rootFrame.CanGoBack && !e.Handled)
				{
					rootFrame.GoBack();
					SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
					e.Handled = true;
				}
			};
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
		}
	}
}
