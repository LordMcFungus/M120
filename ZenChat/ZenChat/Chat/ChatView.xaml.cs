using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZenChat.Chat
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class ChatView : Page
	{
		public ChatView()
		{
			this.InitializeComponent();

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
			DataContext = e.Parameter;
		}
	}
}
