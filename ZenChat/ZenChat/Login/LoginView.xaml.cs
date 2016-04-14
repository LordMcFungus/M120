using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZenChat.Login
{
	/// <summary>
	///     An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class LoginView : Page
	{
		private readonly LoginViewModel ViewModel;

		public LoginView()
		{
			InitializeComponent();
			ViewModel = new LoginViewModel();
			DataContext = ViewModel;
		}
	}
}