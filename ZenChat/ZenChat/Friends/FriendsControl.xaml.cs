using System.Linq;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ZenChat.Friends
{
	public sealed partial class FriendsControl : UserControl
	{
		public FriendsControl()
		{
			InitializeComponent();
		}

		private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			e.AddedItems.Cast<FriendViewModel>().ToList().ForEach(f => f.IsSelected = true);
			e.RemovedItems.Cast<FriendViewModel>().ToList().ForEach(f => f.IsSelected = false);
		}
	}
}